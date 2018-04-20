using System.IO;
using System.Reflection;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;

namespace Convenient.Razor.Engine
{
    public struct Compiled
    {
        private readonly EmitOptions _emitOptions;
        public RazorCSharp Razor { get; }
        public CSharpCompilation Compilation { get; }

        public Compiled(RazorCSharp razor, CSharpCompilation compilation, EmitOptions emitOptions)
        {
            Razor = razor;
            Compilation = compilation;
            _emitOptions = emitOptions;
        }

        public Emitted Emit()
        {
            using (var assemblyStream = new MemoryStream())
            {
                using (var pdbStream = new MemoryStream())
                {
                    var result = Compilation.Emit(assemblyStream, pdbStream, options: _emitOptions);
                    if (!result.Success)
                    {
                        throw new RazorCompilationException(result.Diagnostics);
                    }
                    assemblyStream.Seek(0, SeekOrigin.Begin);
                    pdbStream.Seek(0, SeekOrigin.Begin);
                    var assembly = Assembly.Load(assemblyStream.ToArray(), pdbStream.ToArray());
                    return new Emitted(this, assembly);
                }
            }
        }
    }
}