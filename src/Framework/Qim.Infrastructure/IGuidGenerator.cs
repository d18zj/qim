using System;

namespace Qim
{
    /// <summary>
    /// Used to generate Ids.
    /// </summary>
    public interface IGuidGenerator
    {
        /// <summary>
        /// new a GUID.
        /// </summary>
        Guid NewGuid();
    }
}