using AutoMapper;
using Application.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Application.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using WorkflowAction = Domain.Entities.Action;



namespace Application.Services
{
    public class WorkflowService : IWorkflowService
    {
        private readonly IWorkflowDefinitionRepository _defs;
        private readonly IWorkflowInstanceRepository _insts;
        private readonly IMapper _mapper;

        public WorkflowService(IWorkflowDefinitionRepository defs, IWorkflowInstanceRepository insts, IMapper mapper)
        {
            _defs = defs;
            _insts = insts;
            _mapper = mapper;
        }

        public async Task<Guid> CreateDefinitionAsync(string name, IEnumerable<(string name, bool isStart, bool isEnd)> states, IEnumerable<(string name, Guid from, Guid to)> actions)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required", nameof(name));

            var definition = new WorkflowDefinition(Guid.NewGuid(), name);

            foreach (var (stateName, isStart, isEnd) in states)
            {
                var state = new State(Guid.NewGuid(), stateName, isStart, isEnd);
                definition.AddState(state);
            }

            foreach (var (actionName, fromStateId, toStateId) in actions)
            {
                var action = new WorkflowAction(Guid.NewGuid(), actionName, fromStateId, toStateId);
                definition.AddAction(action);
            }

            await _defs.AddAsync(definition);
            return definition.Id;
        }

        public async Task<Guid> StartInstanceAsync(Guid definitionId)
        {
            var definition = await _defs.GetByIdAsync(definitionId)
                ?? throw new KeyNotFoundException($"Definition '{definitionId}' not found.");
            var instance = new WorkflowInstance(definition);
            await _insts.AddAsync(instance);
            return instance.Id;
        }

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

        public async Task<WorkflowInstanceDto> GetInstanceAsync(Guid instanceId)
        {
            var instance = await _insts.GetByIdAsync(instanceId)
                ?? throw new KeyNotFoundException($"Instance '{instanceId}' not found.");
            return _mapper.Map<WorkflowInstanceDto>(instance);
        }

        public async Task<IEnumerable<WorkflowDefinitionDto>> ListDefinitionsAsync()
        {
            var defs = await _defs.ListAllAsync();
            return _mapper.Map<IEnumerable<WorkflowDefinitionDto>>(defs);
        }

        public async Task<WorkflowDefinitionDto> GetDefinitionAsync(Guid definitionId)
        {
            var def = await _defs.GetByIdAsync(definitionId)
                ?? throw new KeyNotFoundException($"Definition '{definitionId}' not found.");
            return _mapper.Map<WorkflowDefinitionDto>(def);
        }

        public async Task<IEnumerable<InstanceSummaryDto>> ListInstancesAsync()
        {
            var insts = await _insts.ListAllAsync();
            return _mapper.Map<IEnumerable<InstanceSummaryDto>>(insts);
        }
    }
}
