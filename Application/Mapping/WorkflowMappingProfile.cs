
using System;
using System.Linq;
using AutoMapper;
using Application.Requests;
using Application.DTOs;
using Domain.Entities;

using AutoMapper;
using Application.Requests;
using Application.DTOs;
using Domain.Entities;

namespace Application.Mapping
{
    public class WorkflowMappingProfile : Profile
    {
        public WorkflowMappingProfile()
        {

            CreateMap<WorkflowDefinition, WorkflowDefinitionDto>()
                .ForMember(d => d.DefinitionId, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.States, o => o.MapFrom(s => s.States))
                .ForMember(d => d.Actions, o => o.MapFrom(s => s.Actions));

            CreateMap<State, DefinitionStateDto>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.IsStart, o => o.MapFrom(s => s.IsStart))
                .ForMember(d => d.IsEnd, o => o.MapFrom(s => s.IsEnd));

            CreateMap<Domain.Entities.Action, DefinitionActionDto>()
                .ForMember(d => d.Id, o => o.MapFrom(a => a.Id))
                .ForMember(d => d.Name, o => o.MapFrom(a => a.Name))
                .ForMember(d => d.FromStateId, o => o.MapFrom(a => a.FromStateId))
                .ForMember(d => d.ToStateId, o => o.MapFrom(a => a.ToStateId));

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
