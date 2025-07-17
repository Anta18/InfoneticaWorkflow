using Application.Mapping;
using Application.Services;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<WorkflowDbContext>(opts =>
                opts.UseSqlServer(config.GetConnectionString("DefaultConnection")));
            services.AddScoped<IWorkflowDefinitionRepository, WorkflowDefinitionRepository>();
            services.AddScoped<IWorkflowInstanceRepository, WorkflowInstanceRepository>();
            return services;
        }

        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IWorkflowService, WorkflowService>();
            services.AddAutoMapper(cfg => cfg.AddProfile<WorkflowMappingProfile>());
            return services;
        }

        public static IServiceCollection AddApi(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddCors(opts => opts.AddDefaultPolicy(pb => pb.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
            return services;
        }
    }
}
