using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis;

namespace Convenient.Razor
{
    public class RazorCompilationException : Exception
    {
        public RazorCompilationException(IEnumerable<RazorDiagnostic> diagnostics) : base(Format(diagnostics))
        {
        }

        public RazorCompilationException(IEnumerable<Diagnostic> diagnostics) : base(Format(diagnostics))
        {
        }

        private static string Format(IEnumerable<Diagnostic> diagnostics)
        {
            var builder = new StringBuilder("Compilation failed").AppendLine();
            foreach (var diagnostic in diagnostics)
            {
                builder.AppendLine(diagnostic.GetMessage());
            }
            return builder.ToString();
        }


        private static string Format(IEnumerable<RazorDiagnostic> diagnostics)
        {
            var builder = new StringBuilder("Compilation failed").AppendLine();
            foreach (var diagnostic in diagnostics)
            {
                builder.AppendLine(diagnostic.GetMessage());
            }
            return builder.ToString();
        }
    }
}