# nsupertest [![Build Status](https://travis-ci.org/pshort/nsupertest.svg?branch=master)](https://travis-ci.org/pshort/nsupertest)

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
