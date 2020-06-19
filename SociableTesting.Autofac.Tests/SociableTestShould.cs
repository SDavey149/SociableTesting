using Autofac.Core;
using DummyProject;
using FluentAssertions;
using Moq;
using Xunit;

namespace AutofacClassicalTesting.Tests
{
    public class SociableTestShould : SociableTest<Class1, MyModule>
    {
        [Fact]
        public void InitialiseRegisteredClass()
        {
            var mock = new Mock<IMockedDependency>();
            ProvideDependency<IMockedDependency>(mock.Object);
            Sut.Should().BeOfType<Class1>();
        }

        [Fact]
        public void ThrowExceptionIfTypeNotRegistered()
        {
            Assert.Throws<DependencyResolutionException>(() =>
            {
                Sut.GetType();
            });
        }
    }
}