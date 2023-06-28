

namespace RitaEngine.Platform;

using System.Runtime;


public enum GarbageCollectionPriority
{
    Normal =1 ,
    High =3,
    Desactive=0,
    Default = GCLatencyMode.Interactive ,
    WhenUseMemoryPressureActivate = GCLatencyMode.SustainedLowLatency,
    ///Disables garbage collection concurrency and reclaims objects in a batch call. This is the most intrusive mode. This mode is designed for maximum throughput at the expense of responsiveness.
    Batch = 0,
    ///Enables garbage collection concurrency and reclaims objects while the application is running. This is the default mode for garbage collection on a workstation and is less intrusive than Batch. It balances responsiveness with throughput. This mode is equivalent to garbage collection on a workstation that is concurrent.
    Interactive	=1,
    ///Enables garbage collection that is more conservative in reclaiming objects. Full collections occur only if the system is under memory pressure, whereas generation 0 and generation 1 collections might occur more frequently. This mode is not available for the server garbage collector.
    LowLatency =2,
    ///Indicates that garbage collection is suspended while the app is executing a critical path.
    ///NoGCRegion is a read-only value; that is, you cannot assign the NoGCRegion value to the LatencyMode property. You specify the no GC region latency mode by calling the TryStartNoGCRegion method and terminate it by calling the EndNoGCRegion() method.
    NoGCRegion =4,
    ///Enables garbage collection that tries to minimize latency over an extended period. The collector tries to perform only generation 0, generation 1, and concurrent generation 2 collections. Full blocking collections may still occur if the system is under memory pressure.
    SustainedLowLatency	=3,
}

