using OpenAI;
using OpenAI.Chat;
using OpenAI.Responses;
using Sentinel.Domain.Constants;
using Sentinel.Domain.Interfaces;
using Sentinel.Infrastructure.Llm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#pragma warning disable OPENAI001

namespace Sentinel.Infrastructure.Llm
{
    public class OpenAiLlmService : ILlmService
    {
        private readonly string _apiKey;
        private readonly ChatClient _chatClient;
        
        public OpenAiLlmService()
        {
            _apiKey = "sk-...b-0A";
            _chatClient = new ChatClient("gpt-5", _apiKey) ;
        }
        public async Task<string> AnalyzeCodeAsync(string code, string prompt)
        {
            var fullPrompt = prompt + "\n" + code;
            var responseFormat = ChatResponseFormat.CreateJsonSchemaFormat(
                    jsonSchemaFormatName: "code_analysis",
                    jsonSchema: BinaryData.FromString(LlmConstants.AnalysisResultSchema),
                    jsonSchemaIsStrict: false
                );
            var options = new ChatCompletionOptions { ResponseFormat = responseFormat };
            var completion = await _chatClient.CompleteChatAsync([new UserChatMessage(fullPrompt)], options);
            string jsonResponse = completion.Value.Content[0].Text;
            return jsonResponse;
        }
    }
}
public class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("--- Starting Test Runner ---");

        // 1. Create an instance of your service
        // (This will use the hardcoded key/model in your constructor)
        var llmService = new OpenAiLlmService();

        // 2. Define your test inputs
        string testCode = """

            public string SayHello(string name)
            {
                return "Hello " + name;
            }
        """;

        string testPrompt = LlmConstants.ReadabilityAnalysisPromptCS;

        // 3. Call your function and print the result
        try
        {
            string jsonResult = await llmService.AnalyzeCodeAsync(testCode, testPrompt);

            Console.WriteLine("\n--- API Call Successful ---");
            Console.WriteLine("Raw JSON Response:");
            Console.WriteLine(jsonResult);
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n--- API Call Failed ---");
            // This will probably be an "AuthenticationError" if you didn't change the key
            Console.WriteLine(ex.Message);
            Console.ResetColor();
        }

        Console.WriteLine("\n--- Test Finished ---");
    }
}