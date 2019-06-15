# JOS.Multiple.HostedServices

Basic example of how to use multiple IHostedService implementations in the same solution.
Also demonstrates how to use the CancellationToken correct so that the IHostedServices are killed when the application is killed.

MyService1 and MyService2 simulates that they are listening for messages from a queue and does some work.
MyService3 are triggered by a timer.

The key to get ```while (!cancellationToken.IsCancellationRequested)``` to work and don't block the whole application is to await another task in it.
Only having the while loop like below will **not** work

```csharp
while (!cancellationToken.IsCancellationRequested)
{
    DoWork();
}
```

MyService3 is using a timer, but it could have been implemented like the other handlers as well together with a delay, but I wanted to show different solutions.
