using System;
using Qim.Configuration;
using Qim.Ioc.Autofac;
using Qim.Ioc.DryIoc;

namespace Qim.Ioc.Tests
{
    public abstract class TestBase :IDisposable
    {
        private void CreateIocAppConfiguration()
        {

            var config = new TestAppConfiguration()
                .UseDryIoc();
                //.UseAutofac();
           

            Resolver = config.Resolver;
            Registrar = config.Registrar;
        }

        protected void Reset()
        {
            Dispose();
            CreateIocAppConfiguration();
        }

        protected IIocResolver Resolver { get; private set; }

        protected IIocRegistrar Registrar { get; private set; }

        public void Dispose()
        {
            TryDispose(Resolver);
            TryDispose(Registrar);
        }

        private void TryDispose(object obj)
        {
            var dispose = obj as IDisposable;
            dispose?.Dispose();
        }

    }
}