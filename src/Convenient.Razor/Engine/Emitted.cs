using System.Reflection;

namespace Convenient.Razor.Engine
{
    public struct Emitted
    {
        public Compiled Compiled { get; }
        public Assembly Assembly { get; }

        public Emitted(Compiled compiled, Assembly assembly)
        {
            Compiled = compiled;
            Assembly = assembly;
        }
    }
}
