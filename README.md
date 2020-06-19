# Autofac Sociable Testing

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

## Usage

With this package sociable unit tests can be setup without having to worry about initialising any of the dependencies. Instead,
dependencies are initialised with the container just like they are in production. The examples below are for use
with XUnit.

Inherit from ```SociableTest<TSut, TModule>``` where TSut is the type under test, and TModule is the Autofac module that
the type (and its dependencies) are registered. If some dependencies aren't registered in this module then you can provide 
a test double or fake instead using ```ProvideDependency```.

```cs
public class SociableTestShould : SociableTest<Sut, MyModule>
{
    [Fact]
    public void InitialiseRegisteredClass()
    {
        Sut.Should().BeOfType<Sut>();
    }
}
```

### Using Test Doubles (Mocks)

Test doubles can still be used for some dependencies if you wish by using the ```ProvideDependency``` method. ```ProvideDependency```
must be used **before** any access to either the ```Sut``` or ```Container``` properties. Once either of these properties
is accessed the container is built and ```ProvideDependency``` will no longer have any effect.

```cs
public class SociableTestShould : SociableTest<Class1, MyModule>
{
    [Fact]
    public void InitialiseRegisteredClass()
    {
        var mock = new Mock<IMockedDependency>();
        ProvideDependency<IMockedDependency>(mock.Object);
        Sut.Should().BeOfType<Class1>();
    }
}
```

```SociableTest``` doesn't have to be used with inheritance. It can be used in a composition style within a test
class which would suit unit testing packages that have separate methods for test setup like ```[TestInitialize]``` 
in MSTest or ```[Setup]``` in NUnit.

```cs
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
```