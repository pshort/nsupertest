# nsupertest [![NuGet](https://badge.fury.io/nu/nsupertest.svg)](https://www.nuget.org/packages/NSuperTest) [![License](https://img.shields.io/github/license/mashape/apistatus.svg)](https://en.wikipedia.org/wiki/MIT_License)

Super Test style testing for .net Core projects. (With possible support for OWIN if a server provider is written).

## About

A small focused library for ergonomic functional testing of .net web api projects. Inspiration taken from the excellent [Super Test](https://github.com/visionmedia/supertest) libaray for testing nodejs http servers. 

The goal of the library is twofold:


1) Make it simple to setup tests against either an in memory web api server in .net or a remote one. Make it easy to switch between different servers so the same tests can be used against multiple deployment stages.
2) Provide a nice syntax for writing assertions against http response messages.


The assertions part of the library are extensions on HttpResponseMessage, so you can even only take half, if the server part doenst interest you you are still able to take advantage of the assertions part of the library.


## Why "In Memory" testing?

The big win with in memory testing is the debug story; being able to debug the test code and the server code in 1 process is very valuable and helps you quickly diagnose the reason for a test failing using the debug tools.

This is especially valuable if you are working against a pre-written set of functional tests and using them as a validation that you are fulfilling the contract of behavior specified in the suite. If your test is still red after completing what you beleive to be the feature, you can simply run through the test in debug and see whats what.

## Getting started

The library is available as a [nuget](https://www.nuget.org/packages/NSuperTest) package.
```
dotnet add package nsupertest
```

### Register servers

In order to setup a server, or target, for nsupertest to test against you need to register them with the testing library. This is done using an IRegisterServers implementation. The reason for this is that in order to test against an in memory server you want nsupertest to manage that server for you, you might not like a new server with each test or have multiple servers compete over available ports.

```C#
    public class ServerRegistrar : IRegisterServers
    {
        public void Register(ServerRegistry reg)
        {
            // use reg to register servers
        }
    }
```

In this example 2 servers are registered for use in the test project. A .net core webapi is being registered in memory 

### In memory testing

In order to test against in memory .net core servers you need to register a net core server:

```C#
    reg.RegisterNetCoreServer<Startup>("TestServer");
```

This registers a net core server using its startup class which is then supplied to a pretty generic web host builder. Nsupertest will manage the server in memory and when you need it you simply create a TestClient and give it the name of your server:

```C#
    TestClient client = new TestClient("TestServer");
```

This client can be then used to perform tests and assert against the API. 

#### Configuration

You can also provide a configuration builder so that your in memory server can be setup for testing. In this case a configuration that uses a different json configuration file is being used.

```C#
    var config = new ConfigurationBuilder().AddJsonFile("appsettings.json");
    reg.RegisterNetCoreServer<Startup>("TestServer", config);
```

### Proxy testing

Setting up a proxy server is very simple:

```C#
    reg.RegisterProxyServer("TestServer", "https://myserver/api/v1");
```

## The assertion API

Once you have a TestClient in your test its quite simple to use it to make http requests and perform assertions:

```C#
    var message = await client
        .GetAsync("/values")
        .ExpectStatus(200);
```
This test performs a GET request against the values resource in the api and performs a status assertion, expecting a http status response code of 200.

The client part has support for the following verbs:

- GET
- PUT
- POST
- DELETE
- PATCH

You can also use MakeRequestAsync() and pass your own request object to utilize other more obscure verbs.

### Post / Put
Post allows you to post a body to the server

```C#
    await client
        .PostAsync("/persons", new {
            Age = 200,
            Name = "t1000"
        })
        .ExpectStatus(400);
```

You can post anonymous objects if you wish, or if you are in memory you can use the actual post model from your web api project, meaning less boilerplate code in the functional test lib.

### Headers + Query

Each request method allows you to set optional parameters to set header and query variables. These are both just Dictionary<string, string> objects.

```C#
    await client
        .PostAsync("/persons", 
            body: new {
                Age = 10,
                Name = "Test"
            },
            headers: new Headers { { "nameOverride", "peter" } },
            query: new Query { { "setId", "9" } }
        )
        .ExpectStatus(200);
```

## Assertions

The library has a set of assertions that can be used to test the http response to your call:

### ExpectStatus

Expect a specific http status result:

```C#
.ExpectStatus(200); // expect OK
.ExpectStatus(HttpStatusCode.BadRequest); // expect failure
```

### ExpectBody

Expect a string body:

```C#
.ExpectBody("I expect this");
```

### ExpectBody (Object)

Expect a C# object

```C#
var object = new { Name = "Pete" };

// request
.ExpectBody(object); // expects the declared object.
```

If the library cannot parse the content and create the above object the test fails. You can pass an optional parameter to this method to change the camel case behavior of the Json serializer.

### ExpectResponse

Provides a callback allowing you to perform assertions on the raw http response object:

```C#
.ExpectResponse(resp => {
    // assert against resp here
});
```

### ExpectHeader

Assert the existence of a header value pair:

```C#
.ExpectHeader("TestHeader", "TestValue");
```

## Attribution

The JSON schema validation is provided by the awesome [NJsonSchema](https://github.com/RicoSuter/NJsonSchema) library.