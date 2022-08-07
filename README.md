<div align=center>

![mnemo-banner](https://user-images.githubusercontent.com/35552782/183278976-525d8f99-ba6d-425e-9125-1565194e0aeb.png)

## The Memory DSL

</div>

> **Warning**:
> This project is still under development so none of this is actually working yet.

Mnemo, short for Mnemosyne, is a statically typed [DSL](https://en.wikipedia.org/wiki/Domain-specific_language) for memory reading. It exists for the purposes of making it easier and more straightforward to read other processes memory.


```js
// This is a comment
const PLAYER_ADDRESS: uint64_t = 0x21000000;
const PLAYER_OFFSETS: vec<uint32_t> => [0x10, 0x20, 0x30];

// This can be initialized later on by the Mnemo Context
var LATE_INIT_PTR: uint64_t;

// Mnemo also supports dynamic offsets calculation
const getDynamicOffsets(rax: int32_t) => [
    0x10,
    0x20,
    (rax * 8) + 0x20,
    0xA8
];

const PlayerId => Read<int32_t>(PLAYER_ADDRESS, getDynamicOffsets(2));
```

## Mnemo Context

Mnemo runs on its own "Context", you can talk to the Mnemo context using the C# API Mnemo provides you.

### Creating a context

You can create a context by calling the `Mnemo.Create(string path, IntPtr pHandle)` function, where `path` is the path to the script file and `pHandle` is the Process handle you got from calling the `OpenProcess` function.

```cs
Mnemo context = Mnemo.Create("script.mn", pHandle);
int playerId = context.Get<int>("PlayerId");
int[] playerOffsets = context.Get<int[]>("PLAYER_OFFSETS");
long playerAddress = context.Get<long>("PLAYER_ADDRESS");

// You can set variables within the context
context.Set<long>("LATE_INIT_PTR", 0x1412345678);

long playerPointer = context.Read<long>("PLAYER_ADDRESS", "DYNAMIC_DEPENDENT_OFFSETS");
```
