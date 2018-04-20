using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis.Text;

namespace Convenient.Razor.Engine
{
    public struct RazorCSharp
    {
        private readonly CSharpCompilationOptions _compilationOptions;
        private readonly IList<MetadataReference> _references;
        private readonly EmitOptions _emitOptions;
        public RazorCSharpDocument CSharp { get; }

        public RazorCSharp(RazorCSharpDocument cSharp, CSharpCompilationOptions compilationOptions, IList<MetadataReference> references, EmitOptions emitOptions)
        {
            CSharp = cSharp;
            _compilationOptions = compilationOptions;
            _references = references;
            _emitOptions = emitOptions;
        }

        public Compiled Compile()
        {
            if (CSharp.Diagnostics.Any())
            {
                throw new RazorCompilationException(CSharp.Diagnostics);
            }
            var assemblyName = Path.GetRandomFileName();
            var sourceText = SourceText.From(CSharp.GeneratedCode, Encoding.UTF8);
            var syntaxTree = CSharpSyntaxTree.ParseText(sourceText).WithFilePath(assemblyName);
            var compilation = CSharpCompilation.Create(assemblyName, options: _compilationOptions, references: _references).AddSyntaxTrees(syntaxTree);
            return new Compiled(this, compilation, _emitOptions);
        }
    }
}