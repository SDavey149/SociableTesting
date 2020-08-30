using Autofac;

namespace DummyProject
{
    public class MyModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ClassDependingOnIMockedDependency>();
            builder.RegisterType<ClassWithNoDependencies>()
                .AsSelf()
                .As<IDependency>();
        }
    }
}