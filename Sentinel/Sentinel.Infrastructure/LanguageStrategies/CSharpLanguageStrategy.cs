using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Sentinel.Domain.Entities;
using Sentinel.Domain.Enums;
using Sentinel.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                .Select(method => new CodeChunk
                {
                    MethodName = method.Identifier.Text,
                    Code = method.ToFullString(),
                    StartLine = method.GetLocation().GetLineSpan().StartLinePosition.Line + 1,
                    EndLine = method.GetLocation().GetLineSpan().EndLinePosition.Line + 1
                })
                .ToList();
        }
    }
}
