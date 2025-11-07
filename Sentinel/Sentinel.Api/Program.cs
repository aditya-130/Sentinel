
using OpenAI.Chat;
using Sentinel.Application.Handlers;
using Sentinel.Application.Helpers;
using Sentinel.Domain.Interfaces;
using Sentinel.Infrastructure.LanguageStrategies;
using Sentinel.Infrastructure.Llm;
using Sentinel.Infrastructure.Resolvers;

namespace Sentinel.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddSingleton(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                var apiKey = configuration["OpenAi:ApiKey"] ?? throw new InvalidOperationException("OpenAI API key not configured");
                var model = configuration["OpenAi:Model"] ?? "gpt-4o";
                return new ChatClient(model, apiKey);
            });

            builder.Services.AddScoped<ILlmServiceResolver, LlmServiceResolver>();
            builder.Services.AddScoped<ILanguageStrategyResolver, LanguageStrategyResolver>();

            builder.Services.AddScoped<ILanguageStrategy, CSharpLanguageStrategy>();

            builder.Services.AddScoped<ILlmService, OpenAiService>();

            builder.Services.AddScoped<AnalyzeCodeDiffHandler>();
            builder.Services.AddScoped<SchemaProvider>();
            builder.Services.AddScoped<PromptBuilder>();
            builder.Services.AddScoped<CodeAnalysisResponseParser>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
