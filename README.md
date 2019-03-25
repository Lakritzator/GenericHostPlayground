# GenericHostPlayground
This is just a playground for the generic host, to see if I can add plugin loading

**This is work in progress**

I want to create a simple solution for adding plugins to an application.
This is my prototype: https://github.com/Lakritzator/GenericHostPlayground/blob/master/src/GenericHostSample.ConsoleDemo/Program.cs#L18
With this as the "loading" implementation: https://github.com/Lakritzator/GenericHostPlayground/blob/master/src/Dapplo.Extensions.Plugins/HostBuilderExtensions.cs#L42

I didn't check all possible combinations, but I have created 2 plugins:
1. The sample here: https://github.com/aspnet/Docs/tree/master/aspnetcore/fundamentals/host/generic-host/samples/2.x/GenericHostSample
2. And one plugin which just uses a dependency, to see if this works

There are a lot of things I want to try out, so this is really a prototype.

I've added a ForceSingleInstance, which uses a Mutex to prevent the application running twice, it will a shutdown of the application before it really starts.
(Still need to check if this works as expected)