# Autofac Sociable Testing

![.NET Core](https://github.com/SDavey149/SociableTesting/workflows/.NET%20Core/badge.svg)

The main reason for creating this package is to make writing [sociable unit tests](https://martinfowler.com/bliki/UnitTest.html) easier
which is often used in the Chicago style of TDD as oppose to mocking which is more heavily used in the London style. These are 
also known as classicist vs mockist testing styles. 

This package can also be used as a tool to help write integration tests. Please read the link above about sociable unit tests
to understand the difference between the two.

Note: This package only works with the Autofac container.

When using dependency injection writing a sociable unit test often ends up with something like this:

```cs
public UnitShould() {
    //common test setup
    sut = new Sut(
        new DependencyA(new DependencyB(), new DependencyC()),
        new DependencyD(),
        new DependencyE(new DependencyF())
        );    
}
```

whereas with test doubles it would look similar to:

```cs
public UnitShould() {
    //common test setup
    var mockA = new Mock<DependencyA>();
    var mockD = new Mock<DependencyD>();
    var mockE = new Mock<DependencyE>();
    
    sut = new Sut(mockA, mockD, mockE);    
}
```

Mocking means you only have to worry about the immediate set of dependencies rather than all of the layers. 

## Installation

This package can be installed via Nuget: https://www.nuget.org/packages/SociableTesting.Autofac/

## Usage

With this package sociable unit tests can be setup without having to worry about initialising any of the dependencies. Instead,
dependencies are initialised with the container just like they are in production. The examples below are for use
with XUnit.

```cs
public class SociableTestShould
{
    [Fact]
    public void InitialiseRegisteredClass()
    {
        var setup = new SociableTest<Sut>(new MyModule());
        setup.Sut.Should().BeOfType<Sut>();
    }
}
```

### Using Test Doubles (Mocks)

Test doubles can still be used for some dependencies if you wish by using the ```ProvideDependency``` method. ```ProvideDependency```
must be used **before** any access to either the ```Sut``` or ```ContainerBuilder``` properties. Once either of these properties
is accessed the container is built and ```ProvideDependency``` will no longer have any effect.

```cs
public class SociableTestShould
{
    [Fact]
    public void InitialiseRegisteredClass()
    {
        var setup = new SociableTest<Class1, MyModule>();
        var mock = new Mock<IMockedDependency>();
        ProvideDependency<IMockedDependency>(mock.Object);
        setup.Sut.Should().BeOfType<Class1>();
    }
}
```

### Register on ContainerBuilder

The ```ContainerBuilder``` is accessible via a public property on ```SociableTest``` so you can make use of the full range of registration types Autofac provides.
