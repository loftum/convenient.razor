using System;
using System.Runtime.InteropServices;

namespace Convenient.Razor.Tests
{
    public static class We
    {
        private const string SymWriterGuid = "0AE2DEB0-F901-478b-BB9F-881EE8066788";

        public static bool SupportFullPdb()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return false;
            }

            if (Type.GetType("Mono.Runtime") != null)
            {
                return false;
            }

            try
            {
                var type = Marshal.GetTypeFromCLSID(new Guid(SymWriterGuid));
                if (type != null)
                {
                    Activator.CreateInstance(type);
                    return true;
                }
            }
            catch
            {
                //
            }
            return false;
        }
    }
}