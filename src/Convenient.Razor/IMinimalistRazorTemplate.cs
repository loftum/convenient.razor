using System.IO;
using System.Threading.Tasks;

namespace Convenient.Razor
{
    public interface IMinimalistRazorTemplate
    {
        TextWriter TextWriter { get; set; }
        Task ExecuteAsync();
        void WriteLiteral(object value);
        void Write(object value);
    }
}