# GenericHostPlayground
This is just a playground for the generic host, to see if I can add plugin loading

**This is work in progress**

I wanted to use the generic host as a bootstrapper of more types of applications. The idea is a bit similar like that of Spring-Boot for java.
At this time I am evaluation the gap between both: https://gist.github.com/Lakritzator/d395f76dafbd51b03533af968982ff92

Current extensions:

AddPlugins
----------
will scan in the content root for matching globs, and load them as a plugin. A plugin can configure the host.
The following code is where the prototype extends the host: https://github.com/Lakritzator/GenericHostPlayground/blob/master/src/GenericHostSample.ConsoleDemo/Program.cs#L43
With this as the "loading" implementation: https://github.com/Lakritzator/GenericHostPlayground/blob/master/src/Dapplo.Extensions.Plugins/HostBuilderExtensions.cs#L47
I didn't check all possible combinations, but I have created 2 plugins:
1. The sample here: https://github.com/aspnet/Docs/tree/master/aspnetcore/fundamentals/host/generic-host/samples/2.x/GenericHostSample
2. And one plugin which just uses a dependency, to see if this works

There are a lot of things I want to try out, so this is really a prototype.

Mutex
-----
I've added a ForceSingleInstance, which uses a Mutex to prevent the application running twice, it will call an action and trigger a shutdown of the application before it really starts.
(Still need to check if this works as expected)
See example here: https://github.com/Lakritzator/GenericHostPlayground/blob/master/src/GenericHostSample.ConsoleDemo/Program.cs#L38
