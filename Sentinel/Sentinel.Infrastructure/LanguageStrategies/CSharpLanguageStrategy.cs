using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Sentinel.Domain.Entities;
using Sentinel.Domain.Enums;
using Sentinel.Domain.Interfaces;

namespace Sentinel.Infrastructure.LanguageStrategies
{
    public class CSharpLanguageStrategy : ILanguageStrategy
    {
        public Language LanguageName => Language.CSharp;

        public List<CodeChunk> ExtractMethods(string code)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(code);
            var root = syntaxTree.GetRoot();

            return root.DescendantNodes()
                .OfType<MethodDeclarationSyntax>()
                .Select(method =>
                {
                    var span = method.Span;
                    var lineSpan = syntaxTree.GetLineSpan(span);

                    return new CodeChunk
                    {
                        MethodName = method.Identifier.Text,
                        Code = method.ToString().TrimEnd() + "\n",
                        
                    };
                })
                .ToList();
        }
    }
}