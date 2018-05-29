namespace Qim.Ioc
{
    public interface IIocScopeResolverFactory
    {
        IIocScopeResolver CreateScopeResolver(string name = null);
    }
}