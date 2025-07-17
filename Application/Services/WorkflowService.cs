using Application.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using AutoMapper;
using WorkflowAction = Domain.Entities.Action;

namespace Application.Services
{
    public class WorkflowService : IWorkflowService
    {
        private readonly IWorkflowDefinitionRepository _defs;
        private readonly IWorkflowInstanceRepository _insts;
        private readonly IMapper _mapper;

        public WorkflowService(
            IWorkflowDefinitionRepository defs,
            IWorkflowInstanceRepository insts,
            IMapper mapper)
        {
            _defs = defs;
            _insts = insts;
            _mapper = mapper;
        }

        // 1) Create definition
        public async Task<Guid> CreateDefinitionAsync(
            string name,
            IEnumerable<(string name, bool isStart, bool isEnd)> states,
            IEnumerable<(string name, IEnumerable<Guid> fromStates, Guid toState)> actions)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required", nameof(name));

            var definition = new WorkflowDefinition(Guid.NewGuid(), name);

            foreach (var (stateName, isStart, isEnd) in states)
            {
                var state = new State(Guid.NewGuid(), stateName, isStart, isEnd);
                definition.AddState(state);
            }

            foreach (var (actionName, fromStateIds, toStateId) in actions)
            {
                var action = new WorkflowAction(
                    Guid.NewGuid(),
                    actionName,
                    fromStateIds,
                    toStateId);
                definition.AddAction(action);
            }

            await _defs.AddAsync(definition);
            return definition.Id;
        }

        // 2) Start instance
        public async Task<Guid> StartInstanceAsync(Guid definitionId)
        {
            var definition = await _defs.GetByIdAsync(definitionId)
                ?? throw new KeyNotFoundException($"Definition '{definitionId}' not found.");
            var instance = new WorkflowInstance(definition);
            await _insts.AddAsync(instance);
            return instance.Id;
        }

        // 3) Perform action
        public async Task PerformActionAsync(Guid instanceId, Guid actionId)
        {
            var instance = await _insts.GetByIdAsync(instanceId)
                ?? throw new KeyNotFoundException($"Instance '{instanceId}' not found.");
            var definition = await _defs.GetByIdAsync(instance.DefinitionId)
                ?? throw new KeyNotFoundException($"Definition '{instance.DefinitionId}' not found.");
            var action = definition.Actions.SingleOrDefault(a => a.Id == actionId)
                ?? throw new InvalidOperationException($"Action '{actionId}' not in definition.");

            instance.ExecuteAction(action, definition.States);
            await _insts.UpdateAsync(instance);
        }

        // 4) Get one instance
        public async Task<WorkflowInstanceDto> GetInstanceAsync(Guid instanceId)
        {
            var instance = await _insts.GetByIdAsync(instanceId)
                ?? throw new KeyNotFoundException($"Instance '{instanceId}' not found.");
            return _mapper.Map<WorkflowInstanceDto>(instance);
        }

        // 5) List definitions
        public async Task<IEnumerable<WorkflowDefinitionDto>> ListDefinitionsAsync()
        {
            var defs = await _defs.ListAllAsync();
            return _mapper.Map<IEnumerable<WorkflowDefinitionDto>>(defs);
        }

        // 6) Get definition
        public async Task<WorkflowDefinitionDto> GetDefinitionAsync(Guid definitionId)
        {
            var def = await _defs.GetByIdAsync(definitionId)
                ?? throw new KeyNotFoundException($"Definition '{definitionId}' not found.");
            return _mapper.Map<WorkflowDefinitionDto>(def);
        }

        // 7) List instances
        public async Task<IEnumerable<InstanceSummaryDto>> ListInstancesAsync()
        {
            var insts = await _insts.ListAllAsync();
            return _mapper.Map<IEnumerable<InstanceSummaryDto>>(insts);
        }

        // 8) Disable state
        public async Task DisableStateAsync(Guid defId, Guid stateId)
        {
            var def = await _defs.GetByIdAsync(defId)
                ?? throw new KeyNotFoundException($"Definition '{defId}' not found.");
            var st = def.States.SingleOrDefault(s => s.Id == stateId)
                ?? throw new KeyNotFoundException($"State '{stateId}' not in definition.");
            st.Disable();
            await _defs.UpdateAsync(def);
        }

        // 9) Enable state
        public async Task EnableStateAsync(Guid defId, Guid stateId)
        {
            var def = await _defs.GetByIdAsync(defId)
                ?? throw new KeyNotFoundException($"Definition '{defId}' not found.");
            var st = def.States.SingleOrDefault(s => s.Id == stateId)
                ?? throw new KeyNotFoundException($"State '{stateId}' not in definition.");
            st.Enable();
            await _defs.UpdateAsync(def);
        }

        // 10) Disable action
        public async Task DisableActionAsync(Guid defId, Guid actionId)
        {
            var def = await _defs.GetByIdAsync(defId)
                ?? throw new KeyNotFoundException($"Definition '{defId}' not found.");
            var ac = def.Actions.SingleOrDefault(a => a.Id == actionId)
                ?? throw new KeyNotFoundException($"Action '{actionId}' not in definition.");
            ac.Disable();
            await _defs.UpdateAsync(def);
        }

        // 11) Enable action
        public async Task EnableActionAsync(Guid defId, Guid actionId)
        {
            var def = await _defs.GetByIdAsync(defId)
                ?? throw new KeyNotFoundException($"Definition '{defId}' not found.");
            var ac = def.Actions.SingleOrDefault(a => a.Id == actionId)
                ?? throw new KeyNotFoundException($"Action '{actionId}' not in definition.");
            ac.Enable();
            await _defs.UpdateAsync(def);
        }
    }
}
