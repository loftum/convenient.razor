using System.IO;
using System.Threading.Tasks;

namespace Convenient.Razor
{
    public abstract class MinimalistRazorTemplate : IMinimalistRazorTemplate
    {
        public TextWriter TextWriter { get; set; }

        public abstract Task ExecuteAsync();

        public virtual void WriteLiteral(object value)
        {
            TextWriter?.Write(value);
        }

        public void Write(object value)
        {
            TextWriter?.Write(value);
        }
    }
}