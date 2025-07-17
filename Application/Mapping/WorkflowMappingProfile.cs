using AutoMapper;
using Application.DTOs;
using Domain.Entities;

namespace Application.Mapping
{
    public class WorkflowMappingProfile : Profile
    {
        public WorkflowMappingProfile()
        {
            // Definition → DTO
            CreateMap<WorkflowDefinition, WorkflowDefinitionDto>()
                .ForMember(d => d.DefinitionId, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.States, o => o.MapFrom(s => s.States))
                .ForMember(d => d.Actions, o => o.MapFrom(s => s.Actions));

            // State → DefinitionStateDto
            CreateMap<State, DefinitionStateDto>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.IsStart, o => o.MapFrom(s => s.IsStart))
                .ForMember(d => d.IsEnd, o => o.MapFrom(s => s.IsEnd))
                .ForMember(d => d.Enabled, o => o.MapFrom(s => s.Enabled));

            // Action → DefinitionActionDto
            CreateMap<Domain.Entities.Action, DefinitionActionDto>()
                .ForMember(d => d.Id, o => o.MapFrom(a => a.Id))
                .ForMember(d => d.Name, o => o.MapFrom(a => a.Name))
                .ForMember(d => d.FromStateIds, o => o.MapFrom(a => a.FromStateIds))
                .ForMember(d => d.ToStateId, o => o.MapFrom(a => a.ToStateId))
                .ForMember(d => d.Enabled, o => o.MapFrom(a => a.Enabled));

            // Instance → summary / detail
            CreateMap<WorkflowInstance, InstanceSummaryDto>()
                .ForMember(d => d.InstanceId, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.DefinitionId, o => o.MapFrom(s => s.DefinitionId))
                .ForMember(d => d.CurrentStateId, o => o.MapFrom(s => s.CurrentStateId))
                .ForMember(d => d.CreatedAt, o => o.MapFrom(s => s.CreatedAt));

            CreateMap<WorkflowInstance, WorkflowInstanceDto>()
                .ForMember(d => d.InstanceId, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.CurrentStateId, o => o.MapFrom(s => s.CurrentStateId))
                .ForMember(d => d.CreatedAt, o => o.MapFrom(s => s.CreatedAt))
                .ForMember(d => d.History, o => o.MapFrom(s => s.History));

            CreateMap<InstanceHistoryEntry, HistoryEntryDto>();
        }
    }
}
