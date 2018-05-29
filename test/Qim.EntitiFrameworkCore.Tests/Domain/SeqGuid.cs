using System;
using Qim.Domain.Entity;

namespace Qim.EntitiFrameworkCore.Tests.Domain
{
    public class SeqGuid : BaseEntity<int>
    {
        /// <summary>
        ///     原生Guid,
        /// </summary>
        public Guid GuidKey { get; set; }

        public string StrKey { get; set; }

        public byte[] ArrKey { get; set; }
    }
}