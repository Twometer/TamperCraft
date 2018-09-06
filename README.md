# TamperCraft
## Introduction
TamperCraft is a Minecraft 1.8.X proxy written in C# that is designed
to modify the packets sent to and from the client. Using this proxy,
it is possible to circumvent client-sided anti cheat systems because
cheats can now be implemented on a separate computer and are therefore undetectable
to the anti cheat system.

## User interface (coming soon)
TamperCraft offers an easy-to-use UI for use without coding. Built into it are some
cheats and their configuration. To use this GUI it is not neccessary to know anything about
programming.

## API
TamperCraft uses a simple API that makes it possible to easily create code that modifies
the packets.

### Minecraft auth
To authenticate with the Mojang servers, you can use the `CredentialManager` that caches tokens, renews
them as neccessary and makes dealing with Mojang auth easy. Usage is as follows:

```csharp
var credentialManager = new CredentialManager();
var loginResult = credentialManager.LogIn(username, password, out SessionToken sessionToken);
if (loginResult != LoginResult.Success)
{
	Console.WriteLine("Error: Could not log in with the specified credentials (" + loginResult + ")");
	return;
}
else
{
	Console.WriteLine("Mojang authentication successful");
}
```

### Creating processors
A simple packet processor that cancels EntityVelocity packets could look like this:
```csharp
public class AntiVelocityProcessor : IPacketProcessor
{
	public void ProcessClientPacket(PacketEvent packetEvent)
	{
		
	}

	public void ProcessServerPacket(PacketEvent packetEvent)
	{
		if(packetEvent.Id == 0x12)
		{
			packetEvent.Cancelled = true;
		}
	}
}
```

### Registering processors
You have to register the processor like so:
```csharp
...
var server = new TamperServer(sessionToken, targetServer);
server.PacketProcessors.Add(new AntiVelocityProcessor());
server.Start();
...
```