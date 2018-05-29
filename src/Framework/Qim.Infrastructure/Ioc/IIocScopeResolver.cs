using System;

namespace Qim.Ioc
{
    public interface IIocScopeResolver : IDisposable
    {
        IIocResolver Resolver { get; }
    }
}