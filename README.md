## C#/.NET NNG (pre-nanomsg v2)

Essentially, this repository is a continuation of my efforts to translate my [C++ NNG](http://github.com/mwpowellhtx/nngcpp/) wrapper of the same, but with couple of severe caveats. Choosing to proceed with NNG, never mind wrappers, is up to you. You have been warned.

In truth, a wiki is probably better to organize this, so I will try to keep it brief for reading purposes.

## Core technologies

I think that the core technology is basically solid. NNG supports multiple transports and multiple protocols, as has been demonstrated in both of these repositories during unit testing, which are both modeled after the C API unit tests.

I've gone to extra lengths to hide the nasty calling conventions from end users in favor of more natural language level patterns.

### Messages

Messages are the core of the API. You can also send or receive bytes or strings, but Message is the basic unit of work that is maintained during socket as well as asynchronous operations.

### Options

The API also supports a rich set of getters and setters for options found from several different object patterns.

### Transports

In Process and TCP IPv4 and IPv6 are all supported and are fairly strong. There are some problems with IPC, especially in a Windows environment, but I understand the author is working on that.

### Protocols

The supported patterns include Bus, Pair, Pipeline (Push/Pull), Pub/Sub, Request/Reply, and Survey.

### Asynchronous

Last but not least, asynrhconous operations are also supported via the C# language ``IAsyncService``, which packages the C API neatly for consumption. At the moment I have not gotten round to providing try language level ``async`` support.

*That's the good news. Now for a couple of severe caveats.*

## Severe caveats

### C DLL status

#### ***Single-thread*** test runs

In and of itself, the DLL packaging works just fine, *as long as you are running a **single** test fixture*.

For example, I choose to run *Pub/Sub* and *Req/Rep* unit tests individually, which each succeed.

```
    PubSubTests.Invalid_topics_are_not_received_by_Subscriber [0:00.185] Success
Current process Id: 10292
Managed thread Id: 9
And can create linked sockets.
  Running protocol tests for address family 'IPv4'.
  Testing using address 'tcp://127.0.0.1:12224'.
  And Subscriber can receive from publisher.
    And Invalid topics are not received by Subscriber.
      And Topic: '/somewhere/over/the/rainbow'.
      And Topic: '/something/aint/quite/right'.
```

```
    ReqRepTests.The_sockets_can_exchange_messages [0:00.004] Success
Current process Id: 4088
Managed thread Id: 9
Running protocol tests for address family 'IPv4'.
Testing using address 'tcp://127.0.0.1:11176'.
And given two fresh sockets.
  And we can create linked sockets.
    And the sockets can exchange messages.
```

#### *Running **2+** test fixtures* concurrently

When you try to run **any *2+* test fixtures** the test runner fails to start at all.

I have not tested this exhaustively, but it seems that the success rate for multi-threaded may be limited to IPv4, with failure rates being more the norm than the exception for the other transports, never mind heterogeneous operation. At least based on early testing, *"smoke tests"* if you will, that I've conducted manually.

```
    Subscribers_in_raw_receive [0:00.000] Aborted
Current process Id: 8380
Managed thread Id: 11
And can create linked sockets.
  Running protocol tests for address family 'InProcess'.
  Testing using address 'inproc://e0399d4f-654b-4aee-9195-7542525a0155'.
  And Subscribers in raw receive.
```

```
    The_sockets_can_exchange_messages [0:00.000] Aborted
Current process Id: 8380
Managed thread Id: 9
Running protocol tests for address family 'InProcess'.
Testing using address 'inproc://59067477-9912-47ef-8fd3-e1eddd0c7ba6'.
And given two fresh sockets.
```

With the Visual Studio test runner error code being:

```
2017.11.06 15:55:47.069   ERROR Process path\to\AppData\Local\JetBrains\Installations\ReSharperPlatformVs14\JetBrains.ReSharper.TaskRunner.CLR45.x64.exe:8380 exited with code '3'.
```

#### Visual Studio Test Runner versus xUnit console runner

This behavior is the same whether running from the *visual studio test runner*, or from the *xUnit console runner*.

I have not verified, but I imagine that sequentially run unit tests would work just fine. However, the caveat here is that this proves nothing about an otherwise multi-threaded client incorporating the DLL.

### Undue focus on ***the 'C' code***

The C code is the starting point, but tests in the C code fail to prove anything about integration opportunities based upon the C calling convention DLL entry points.

## Conclusions

I will let my repository stand for itself. I encourage you to fork and discover these caveats for yourself. I am also open to suggestions if there is something integration-wise that I should be doing else-wise or in addition to what I am already doing.

However, the *"owner"* of NNG reigns surpeme. *"Sieg Heil!"*, so to speak; on which issue alone you have been warned. Of course which runs counter to liberty and freedom, which I fully support.

That's his prerogative, of course, and I will reconsider whether NNG is the right fit for any of my distributed messaging concerns, knowing what I know now, or whether I wouldn't simply roll my own framework, to be perfectly honest with you.
