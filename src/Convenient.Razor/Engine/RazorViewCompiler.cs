using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;

namespace Convenient.Razor.Engine
{
    public class RazorViewCompiler
    {
        private readonly RazorTemplateEngine _templateEngine;
        private readonly IList<MetadataReference> _references;
        private readonly CSharpCompilationOptions _compilationOptions;
        private readonly EmitOptions _emitOptions;

        public RazorViewCompiler(RazorTemplateEngine templateEngine, IEnumerable<MetadataReference> references, EmitOptions emitOptions)
        {
            _templateEngine = templateEngine;
            _references = references.ToList();
            _emitOptions = emitOptions;
            _compilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
        }

        public RazorCSharp Generate(string path)
        {
            var razor = _templateEngine.CreateCodeDocument(path);
            var csharp = _templateEngine.GenerateCode(razor);
            return new RazorCSharp(csharp, _compilationOptions, _references, _emitOptions);
        }

        public Assembly Emit(string path)
        {
            return Generate(path).Compile().Emit().Assembly;
        }
    }
}
