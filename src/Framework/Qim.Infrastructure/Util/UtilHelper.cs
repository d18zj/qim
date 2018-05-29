using System.IO;
using System.Reflection;

namespace Qim.Util
{
    public static class UtilHelper
    {
        public static string GetCurrentBinDirectory()
        {
            var location = Assembly.GetEntryAssembly().Location;
            return Path.GetDirectoryName(location);
        }
    }
}