using System;

namespace Qim.Ioc.Tests.CaseServices
{
    public interface IBar
    {
        Guid Id { get; }
    }

    public interface ISingletonBar : IBar
    {
        
    }

    public interface IScopedBar : IBar
    {
        
    }

    public interface ITransientBar : IBar
    {
        
    }


    public class Bar : ISingletonBar, IScopedBar, ITransientBar
    {
        public Bar()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; }
    }
}