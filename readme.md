![graph icon](https://raw.githubusercontent.com/wikibus/Nancy.RDF/master/assets/icon_21532.png)

# Nancy.Rdf [![Build status][av-badge]][build] [![NuGet version][nuget-badge]][nuget-link] [![codecov.io][cov-badge]][cov-link]

[Nancy](http://github.org/nancyFx/nancy) serializers for RDF media types

## Introduction

Nancy.Rdf is a set of [Nancy][nancy] components, which allow working with POCO models as usual and serving them as RDF media types:

* JSON-LD
* RDF/XML
* Turtle
* Notation3
* n-triples

Under the hood [dotNetRDF][dnr] is used. Thus in the future I plan to add the possibility to serve dotNetRDF graphs and datasets directly.

## Installation

```
PM> Install-Package Nancy.Rdf
```

You may also want to install [Rdf.Vocabularies][vocab], so that you don't have to remember all of those common URIs.

```
PM> Install-Package Rdf.Vocabularies
```

## Basic usage

With Nancy.Rdf I struggle to follow the [Super-Duper-Happy-Path][sdhp]. Nancy has an unparalelled extensibility model and thanks to
SDHP there is a minimal number of steps required to get RDF up and running in your API. It just works!

**You can try out the code below by running the [sample project][sample]**

### Create a POCO model class

This is done with [JsonLD.Entites][entities]. A simples possible model is an ordinary .NET type with a `Context` and `Type` 
properties which are mapped to JSON-LD's `@context` and `@type` keys respectively:

``` C#
using JsonLD.Entities.Context;
using Newtonsoft.Json;
using Vocab;

public class Person
{
  public string Id { get; set; }

  [JsonProperty("givenName")]
  public string Name { get; set; }
  
  public string LastName { get ;set; }
  
  public DateTime DateOfBirth { get; set; }
  
  [JsonProperty]
  private string Type 
  {
    get 
    {
      return Foaf.Person;
    }
  }
  
  private static JObject Context
  {
    get 
    {
      return new JObject(
        "givenName".IsProperty(Foaf.givenName),
        "lastName".IsProperty(Foaf.lastName),
        "dateOfBirth".IsProperty(Schema.birthDate)
      );
    }
  }
}
```

The `Context` property defines how model properties map to RDF terms. By default properties are serialized in camelCase, but the
standard Newtonsoft.Json `[JsonProperty]` can be used to overwrite that. `Id` is seriaized as `@id`; `Type` is serialized as `@type`.

For more information on serialization see JsonLD.Entities [documentation/samples][entities-samples].

### Create a NancyModule as usual

``` c#
public class PersonModule : NancyModule
{
  public PersonModule()
  {
    Get["person/{id}"] = _ => {
      return new Person {
        Id = "http://api.guru/person/" + _.id,
        Name = "John",
        LastName = "Doe",
        DateOfBirth = new DateTime(1967, 8, 2)
      };
    };
  }
}
```

### Request RDF

```
curl http://my.host/person/2 -H Accept:application/ld+json
```

will return JSON-LD

``` json
{
  "@context": {
    "givenName": "http://xmlns.com/foaf/0.1/givenName",
    "lastName": "http://xmlns.com/foaf/0.1/lastName",
    "dateOfBirth": "http://schema.org/birthDate"
  },
  "@id": "http://api.guru/person/2",
  "@type": "http://xmlns.com/foaf/0.1/Person",
  "givenName": "John",
  "lastName": "Doe",
  "dateOfBirth": "1967-08-02T00:00:00"
}
```

Other RDF media types are served by serializing to JSON-LD first and then converting to their respective graph format.

### (Optional) Get rid of inline `@context`

The JSON-LD context can become quite lengthy and Nancy.Rdf makes it trivialy to serve remote contexts and replace the `@context`
object on serialized models. By implementing `IContextPathMapper` it is possible to make Nancy.Rdf replace context of serialized
models and have it served from a dedicated module. For convenience there is a base class to set that up.

``` c#
public class ContextPathMapper : Contexts.DefaultContextPathMapper
{
  public ContextPathMapper()
  {
    ServeContextOf<Person>();
  }
}
```

The above class will expose a `/_contexts/Person` resource and use it as context whenever an instance of `Person` is serialized.
The `_contexts` part can be changed by overriding the `BasePath` property. The `/Person` part can be changed by passing a parameter
to the `ServeContextOf` method call.

## Credits

Great thanks to the [Nancy][nancy] team for creating this great framework.
Thanks to the [NuGet][nuget] team for implementing the [JSON-LD API][ld-api].

[The icon](http://thenounproject.com/term/graph/21532/) desiged by [Piotrek Chuchla](http://thenounproject.com/pchuchla/) from [The Noun Project](http://thenounproject.com/)

[nancy]: https://github.com/NancyFx/Nancy/
[av-badge]: https://ci.appveyor.com/api/projects/status/utu3rrmadr1p2p3v?svg=true
[build]: https://ci.appveyor.com/project/tpluscode78631/nancy-rdf/branch/master
[nuget-badge]: https://badge.fury.io/nu/nancy.rdf.svg
[nuget-link]: https://badge.fury.io/nu/nancy.rdf
[entities]: https://github.com/wikibus/JsonLD.Entities/
[entities-samples]: https://github.com/wikibus/JsonLD.Entities/tree/master/src/Documentation
[nuget]: https://github.com/nuget/json-ld.net
[ld-api]: http://json-ld.org/spec/latest/json-ld-api/
[sdhp]: https://github.com/NancyFx/Nancy/wiki/Introduction#the-super-duper-happy-path
[sample]: https://github.com/wikibus/Nancy.Rdf/tree/master/src/Example/Nancy.Rdf.Sample
[dnr]: http://dotnetrdf.org
[vocab]: https://github.com/wikibus/Rdf.Vocabularies
[cov-badge]: https://codecov.io/github/wikibus/Nancy.Rdf/coverage.svg?branch=master
[cov-link]: https://codecov.io/github/wikibus/Nancy.Rdf?branch=master
