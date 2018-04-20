using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Convenient.Razor.Engine;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Emit;
using NUnit.Framework;

namespace Convenient.Razor.Tests
{
    [TestFixture]
    public class EngineTest
    {
        private RazorViewCompiler _renderer;

        [OneTimeSetUp]
        public void Setup()
        {
            var razorProject = RazorProject.Create(AppDomain.CurrentDomain.BaseDirectory);
            var razorEngine = RazorEngine.Create(b =>
            {
                b.SetNamespace("Some.Namespace");
                b.SetBaseType(typeof(MinimalistRazorTemplate).FullName);
            });
            var razorTemplateEngine = new RazorTemplateEngine(razorEngine, razorProject);

            var references = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.IsDynamic)
                .Select(a => MetadataReference.CreateFromFile(a.Location))
                .ToList();
            var emitOptions = new EmitOptions(debugInformationFormat: We.SupportFullPdb() ? DebugInformationFormat.Pdb : DebugInformationFormat.PortablePdb);
            _renderer = new RazorViewCompiler(razorTemplateEngine, references, emitOptions);
        }

        [Test]
        public void ExecuteTemplate()
        {
            var assembly = _renderer.Emit("Test.cshtml");
            var page = (IMinimalistRazorTemplate) Activator.CreateInstance(assembly.GetType("Some.Namespace.Template"));
            page.TextWriter = Console.Out;
            page.ExecuteAsync();
        }

        [Test]
        public void ShowGeneratedCode()
        {
            var generated = _renderer.Generate("Test.cshtml");
            Console.WriteLine(generated.CSharp.GeneratedCode);
        }

        [Test]
        public void ShowRazorAssemblyInfo()
        {
            var assembly = _renderer.Emit("Test.cshtml");
            Console.WriteLine($"Assembly: {assembly.FullName}");
            Console.WriteLine("Attributes:");
            foreach (var attribute in assembly.CustomAttributes)
            {
                Console.WriteLine(attribute.AttributeType.FullName);
            }

            Console.WriteLine();
            Console.WriteLine("Types:");
            foreach (var type in assembly.GetTypes().Where(t => t.IsPublic))
            {
                Console.WriteLine($"Type: {type.FullName}");
                Console.WriteLine("Methods:");
                foreach (var method in type.GetMethods())
                {
                    var keywords = method.GetKeywords();
                    var parameters = method.GetParameters().Select(p => $"{p.ParameterType.Name} {p.Name}");

                    Console.WriteLine($"{string.Join(" ", keywords)} {method.ReturnType.Name} {method.Name}({string.Join(", ", parameters)})");
                }

                Console.WriteLine();
            }

            Console.WriteLine();
            
        }
    }
    public static class MethodInfoExtensions
    {
        public static IEnumerable<string> GetKeywords(this MethodInfo method)
        {
            if (method.IsPublic)
            {
                yield return "public";
            }
            else if (method.IsPrivate)
            {
                yield return "private";
            }
            else if (method.IsFamily)
            {
                yield return "protected";
            }
            else if (method.IsAssembly)
            {
                yield return "internal";
            }

            if (method.IsStatic)
            {
                yield return "static";
            }

            if (method.IsFinal)
            {
                yield return "final";
            }
            if (method.IsAbstract)
            {
                yield return "abstract";
            }
            else if (method.IsVirtual)
            {
                yield return "virtual";
            }
        }
    }
}
