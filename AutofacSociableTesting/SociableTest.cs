﻿using System;
using Autofac;
using Autofac.Core;

namespace AutofacClassicalTesting
{
    public class SociableTest<TSut, TModule> 
        where TSut : class
        where TModule : IModule, new()
    {
        private readonly Lazy<TSut> sut;
        private readonly Lazy<IContainer> container;
        private readonly ContainerBuilder containerBuilder;

        public IContainer Container => container.Value;

        public TSut Sut => sut.Value;

        public SociableTest()
        {
            containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule(new TModule());
            
            container = new Lazy<IContainer>(() => containerBuilder.Build());
            sut = new Lazy<TSut>(() => Container.Resolve<TSut>());
        }

        public void ProvideDependency<TDependencyRegistration>(TDependencyRegistration dependencyInstance) where TDependencyRegistration : class
        {
            containerBuilder.RegisterInstance(dependencyInstance).As<TDependencyRegistration>();
        }
    }
}