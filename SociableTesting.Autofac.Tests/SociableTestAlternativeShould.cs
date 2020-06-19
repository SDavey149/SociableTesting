﻿using DummyProject;
using FluentAssertions;
using Moq;
using Xunit;

namespace AutofacClassicalTesting.Tests
{
    //An alternative usage
    public class SociableTestAlternativeShould
    {
        private readonly Class1 sut;
        
        public SociableTestAlternativeShould()
        {
            var setup = new SociableTest<Class1, MyModule>();
            setup.ProvideDependency<IMockedDependency>(new Mock<IMockedDependency>().Object);
            sut = setup.Sut;
        }

        [Fact]
        public void InitialiseRegisteredClass()
        {
            sut.Should().BeOfType<Class1>();
        }
    }
}