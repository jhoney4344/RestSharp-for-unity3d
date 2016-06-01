# RestSharp - Simple .NET REST Client

## This is a fork of [RestSharp-for-unity3d](https://github.com/Cratesmith/RestSharp-for-unity3d)

The fork was required since the original project marked itself as needing .NET 3.5 or better.  This
fork simply adjusts that requirement, it doesn't appear the library actually requires anything that
the 3.5 revision of .NET introduced.

Additionally, this library has been refactored to provide iOS support.  Please see the release
notes for details on what was required to do this.

## NuGet

To build the NuGet package execute the following _after_ a build in the root directory of the project

	nuget pack restsharp.nuspec

In order to publish the package to the central NuGet repository you must first export the account
API key via:

	nuget setApiKey <API KEY>

And then push the package:

	nuget push <Package Name>


## Example:

```csharp
var client = new RestClient("http://example.com");
// client.Authenticator = new HttpBasicAuthenticator(username, password);

var request = new RestRequest("resource/{id}", Method.POST);
request.AddParameter("name", "value"); // adds to POST or URL querystring based on Method
request.AddUrlSegment("id", 123); // replaces matching token in request.Resource

// add parameters for all properties on an object
request.AddObject(object);

// or just whitelisted properties
request.AddObject(object, "PersonId", "Name", ...);

// easily add HTTP Headers
request.AddHeader("header", "value");

// add files to upload (works with compatible verbs)
request.AddFile(path);

// execute the request
RestResponse response = client.Execute(request);
var content = response.Content; // raw content as string

// or automatically deserialize result
// return content type is sniffed but can be explicitly set via RestClient.AddHandler();
RestResponse<Person> response2 = client.Execute<Person>(request);
var name = response2.Data.Name;

// or download and save file to disk
client.DownloadData(request).SaveAs(path);

// easy async support
client.ExecuteAsync(request, response => {
    Console.WriteLine(response.Content);
});

// async with deserialization
var asyncHandle = client.ExecuteAsync<Person>(request, response => {
    Console.WriteLine(response.Data.Name);
});

// abort the request on demand
asyncHandle.Abort();
```
