using System.Collections.Generic;
using System.Reflection;

namespace Qim.Reflection
{
    public interface IAssemblyProvider
    {
        IEnumerable<Assembly> GetAssemblies(string path = null);
    }
}