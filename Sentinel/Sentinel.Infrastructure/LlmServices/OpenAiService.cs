using OpenAI;
using OpenAI.Chat;
using OpenAI.Responses;
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
    public class OpenAiService : ILlmService
    {
        private readonly ChatClient _chatClient;
        public string Provider => "OpenAI";

        public OpenAiService(ChatClient chatClient)
        {
            _chatClient = chatClient;
        }


        public async Task<string> AnalyzeCodeAsync(string prompt, string schema)
        {
            var responseFormat = ChatResponseFormat.CreateJsonSchemaFormat(
                    jsonSchemaFormatName: "code_analysis",
                    jsonSchema: BinaryData.FromString(schema),
                    jsonSchemaIsStrict: false
                );
            var options = new ChatCompletionOptions { ResponseFormat = responseFormat };
            var result = await _chatClient.CompleteChatAsync([new UserChatMessage(prompt)], options);
            string jsonResponse = result.Value.Content[0].Text;
            return jsonResponse;
        }
    }
}