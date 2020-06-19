﻿using Autofac;

namespace DummyProject
{
    public class MyModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Class1>();
            builder.RegisterType<Dependency>().As<IDependency>();
        }
    }
}