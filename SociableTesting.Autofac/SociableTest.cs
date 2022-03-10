using System;
using Autofac;
using Autofac.Core;

namespace AutofacClassicalTesting
{
    public sealed class SociableTest<TSut> 
        where TSut : class
    {
        private readonly Lazy<TSut> sut;
        private Lazy<IContainer> container;
        private readonly ContainerBuilder containerBuilder;
        
        public TSut Sut => sut.Value;

        public ContainerBuilder ContainerBuilder
        {
            get
            {
                if (container.IsValueCreated)
                {
                    throw new ContainerAlreadyBuiltException();
                }
                
                return containerBuilder;
            }
        }

        public IContainer Container => container.Value;

        public SociableTest()
        {
            containerBuilder = new ContainerBuilder();

            container = new Lazy<IContainer>(() => containerBuilder.Build());
            sut = new Lazy<TSut>(() => container.Value.Resolve<TSut>());
        }

        public SociableTest(IModule module) : this()
        {
            containerBuilder.RegisterModule(module);
        }

        public SociableTest(ContainerBuilder containerBuilder) : this()
        {
            this.containerBuilder = containerBuilder;
        }

        public void ProvideModule(Module module)
        {
            ContainerBuilder.RegisterModule(module);
        }
        
        public void ProvideDependency<TDependency>(TDependency instance) 
            where TDependency : class
        {
            ContainerBuilder.RegisterInstance(instance).As<TDependency>();
        }
    }
}