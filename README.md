# Mnemo

Mnemo, short for Mnemosyne, is a statically typed scripting language for memory reading, it supports dynamic offsets, static offsets, offsets calculation based on other pointers.

```
// This is a comment
Address PLAYER_ADDRESS = 0x21000000;
Offsets PLAYER_OFFSETS = [0x10, 0x20, 0x30];

Offsets DYNAMIC_OFFSETS = [
    0x10,
    0x20,
    (rbx * 8) + 0x20,
    0xA8
];

// Mnemo also supports dynamic offsets calculation based on other pointers
let PlayerId => Read<Int32>(PLAYER_ADDRESS, PLAYER_OFFSETS);

Offsets DYNAMIC_DEPENDENT_OFFSETS = [
    0x10,
    0x20,
    (PlayerId * 8) + 0x20,
    0xA8
];
```

## Mnemo Context

Mnemo runs on it's own "Context", you can talk to the Mnemo context using the C# API Mnemo provides you.

### Creating a context

You can create a context by calling the `Mnemo.Create(string path, IntPtr pHandle)` function, where `path` is the path to the script file and `pHandle` is the Process handle you got from calling the `OpenProcess` function.

```cs
Mnemo context = Mnemo.Create("script.mn", pHandle);
int playerId = context.Get<int>("PlayerId");
int[] playerOffsets = context.Get<int[]>("PLAYER_OFFSETS");
long playerAddress = context.Get<long>("PLAYER_ADDRESS");

long playerPointer = context.Read<long>("PLAYER_ADDRESS", "DYNAMIC_DEPENDENT_OFFSETS");
```

## Examples

### Example 1
Lets say you want to read a game's process memory looking for the player's save, you have the static address and the offsets:
Static address: Process.exe+0xDEADC0FFEE
Offsets: { 0xA0, 0xB1, SaveSlot * 8 + 0xC0 } 

A Mnemosyne script for that would look somewhat like this:
```
Address PlayerStaticAddress = 0xDEADC0FFEE;
Offsets PlayerOffsets = [ 0xA0, 0xB1, SaveSlot * 8 + 0xC0 ];

Address SaveSlotAddress = 0xFFFFFFFF;
Address SaveSlotOffsets = [0x10, 0x20, 0x0];
let SaveSlot => Read<Int32>(SaveSlotAddress, SaveSlotOffsets);
```

Now we can call the script values from Mnemo's C# Context:
```cs
private long GetCurrentPlayerSaveAddress() 
{
    Mnemo context = Mnemo.Create("save_slot.mn", pHandle);
    return context.Read<long>("PlayerStaticAddress", "PlayerOffsets");
}
```
