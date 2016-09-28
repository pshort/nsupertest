# nsupertest [![Build status](https://ci.appveyor.com/api/projects/status/a1htkm0xyg2wih6c?svg=true)](https://ci.appveyor.com/project/pshort/nsupertest) [![NuGet](https://img.shields.io/badge/Nuget-0.1.18-green.svg)](https://www.nuget.org/packages/NSuperTest) [![License](https://img.shields.io/github/license/mashape/apistatus.svg)](https://en.wikipedia.org/wiki/MIT_License)

Super Test style testing for .net [OWIN](http://owin.org/) web api projects

## About

A small focused library for in memory testing of .net web api projects. Inspiration taken from the excellent [Super Test](https://github.com/visionmedia/supertest) libaray for testing nodejs http servers.

## Getting started

The library is available as a [nuget](https://www.nuget.org/packages/NSuperTest) package.
```
Install-Package NSuperTest
```

## In memory testing
The library allows you to test web api http servers in memory by loading the Startup class using the OWIN self host. The basic object for testing is the ServerClass. You can load up the object using any testing tool and use it to automate requests against a http endpoint and do assertions against the response.

```C#
using NSuperTest;

[TestClass]
public void SomeTests
{
  static Server<Startup> server;
  
  [ClassInitialize]
  public static void Init(TestContext ctx) 
  {
    server = new Server();
  }
  
  [TestMethod]
  public void TestHelloWorldRoute()
  {
    server.Get("/hello")
      .Expect(200)
      .End();
  }
}
```

## In memory with config
You can also in memory test using a configuration setting. To use this, specify an app setting called nsupertest:appStartup and specify the fully qualified type name for your startup class.

```XML
  <appSettings>
    <add key="nsupertest:appStartup" value="MyLib.StartupMock, MyLib" />
  </appSettings
```

Once this is done you can just create a new server instance and you dont need to supply an standard or generic parameters

```C#
  static Server server;
  
  [ClassInitialize]
  public static void Init(TestContext ctx)
  {
    server = new Server();
  }
```

## Testing an external API
Sometimes you just want to run tests against an external API thats hosted somewhere else. The Service objects provides a non generic constructor that takes a Url as a target to help you do that.

```C#
  static Server server;
  
  [ClassInitialize]
  public static void Init(TestContext ctx) 
  {
    server = new Server("http://api.mysite.com:3002/myapi");
  }
```

## The API
The api is a fluent API that hangs off the server.verb chain where verb is a standard HTTP verb.

### Get
Get is pretty straight forward.

```C#
    server
      .Get("/products")
      .Expect(200)
      .End();
```

### Post
Post allows you to post a body to the server

```C#
    var product = new {
      Name = "Acme Thing",
      Price = 100
    };
    
    server
      .Post("/products")
      .Send(product)
      .Expect(201)
      .End();
```
### Put
Put allows you to send a body to the server

```C#
    var product = new {
      Name = "Acme Better Thing"
    };
    
    server
      .Put("/products")
      .Send(product)
      .Expect(200)
      .End();
```

### Delete
Does what it says on the tin

```C#
    server
      .Delete("/products/12")
      .Expect(204)
      .End();
```

### Headers
Headers can be set on requests using the Set method

```C#
    server
      .Get("/products")
      .Set("Accept", "application/json")
      .Expect(200)
      .End();
```
There is also a specific method to allow setting a bearer token
```C#
    server
      .Get("/products")
      .SetBearerToken("cC2340xcv")
      .Expect(200)
      .End();
      
    // ITestBuilder SetBearerToken(Func<string> generator);
    // ITestBuilder SetBearerToken(Func<Task<string>> generatorTask);
```
The SetBearerToken has 3 overloads allowing you to generate the token then and there or farm it off to a Task that will return the bearer token, allowing you to request it from an authentication server using client credentials if possible.

## Response Status

There are 3 ways to assert response status, either via int code, HttpStatusCode enum or via some convenience methods that have been added. 

```C#

.Expect(200);               // Expect ok

.Expect(HttpStatusCode.Ok); // Expect ok

.ExpectOk();                // Expect ok

```

### End
When you have completed the chain you use the end method to invoke the request and run the assertions. You can also use one of two overloads that allow you to place some custom assertions on the response using the HttpResponseMessage object returned, or a Generic version of End that tries to cast the body into an object.
```C#
    server
      .Get("/products/12")
      .Expect(200)
      .End<Product>((m, p) => {
        Assert.AreEqual(12, p.Id);
      });
```
