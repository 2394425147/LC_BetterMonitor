using System.IO;
using System.Reflection;

namespace BetterMonitor.Resources
{
    public static class ModResources
    {
        private static readonly Assembly Assembly = typeof(ModResources).GetTypeInfo().Assembly;

        public static Stream Get(string path)
        {
            return Assembly.GetManifestResourceStream($"BetterMonitor.Resources.{path}");
        }
    }
}
