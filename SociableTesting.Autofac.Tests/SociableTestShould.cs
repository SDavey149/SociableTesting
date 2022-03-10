using Autofac;
using Autofac.Core;
using DummyProject;
using FluentAssertions;
using Moq;
using Xunit;

namespace AutofacClassicalTesting.Tests
{
    public class SociableTestShould
    {
        [Fact]
        public void InitialiseRegisteredClass_When_InModule()
        {
            var setup = new SociableTest<ClassDependingOnIMockedDependency>(new MyModule());
            var mock = new Mock<IMockedDependency>();
            setup.ProvideDependency(mock.Object);
            AssertSutCreated(setup);
        }

        [Fact]
        public void InitialiseRegisteredClass_When_AlternativeModuleRegistrationUsed()
        {
            var setup = new SociableTest<ClassDependingOnIMockedDependency>();
            setup.ProvideModule(new MyModule());
            setup.ProvideDependency<IMockedDependency>(new MockedDependency());
            AssertSutCreated(setup);
        }

        [Fact]
        public void GetBuiltDependency()
        {
            var setup = new SociableTest<ClassDependingOnIMockedDependency>();
            var dependency = new Mock<IMockedDependency>().Object;

            setup.ProvideDependency(dependency);

            var result = setup.Container.Resolve<IMockedDependency>();
            result.Should().Be(dependency);
        }

        [Fact]
        public void AutomaticallyMockDependency()
        {
            var setup = new SociableTest<ClassDependingOnIMockedDependency>();
            AssertSutCreated(setup);
        }

        [Fact]
        public void ThrowException_When_AccessingContainerBuilderAfterSutPropertyAccessed()
        {
            var setup = new SociableTest<ClassWithNoDependencies>(new MyModule());
            var sut = setup.Sut;

            Assert.Throws<ContainerAlreadyBuiltException>(() =>
            {
                setup.ContainerBuilder.RegisterType<MockedDependency>();
            });
        }

        private static void AssertSutCreated(SociableTest<ClassDependingOnIMockedDependency> setup)
        {
            setup.Sut.Should().BeOfType<ClassDependingOnIMockedDependency>();
        }
    }
}