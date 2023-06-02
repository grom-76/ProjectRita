namespace RitaEngine.Base.Concurrency;


public static class Threads
{
    
}
      
/*

https://github.com/stride3d/stride/tree/master/sources/core/Stride.Core/Threading

*/


// namespace RITAENGINE.CORE.CONCURRENCY;


// using System;
// using System.Collections.Generic;
// using System.Reflection;
// using System.Runtime.CompilerServices;
// using System.Runtime.InteropServices;
// using System.Threading;
// using RITAENGINE.CORE.MEMORY;


//  /// <summary>
//     /// Interface implemented by pooled closure types through the AssemblyProcessor.
//     /// Enables <see cref="PooledDelegateHelper"/> to keep closures and delegates alive.
//     /// </summary>
//     public interface IPooledClosure
//     {
//         void AddReference();

//         void Release();
//     }

// /// <summary>
//     /// Thread pool for scheduling sub-millisecond actions, do not schedule long-running tasks.
//     /// Can be instantiated and generates less garbage than dotnet's.
//     /// </summary>
//     public sealed partial class ThreadPool : IDisposable
//     {
//         /// <summary>
//         /// The default instance that the whole process shares, use this one to avoid wasting process memory.
//         /// </summary>
//         public static ThreadPool Instance = new ThreadPool();
        
//         private static readonly bool SingleCore;
//         [ThreadStatic]
//         private static bool isWorkedThread;
//         /// <summary> Is the thread reading this property a worker thread </summary>
//         public static bool IsWorkedThread => isWorkedThread;
        
//         private readonly ConcurrentQueue<Action> workItems = new ConcurrentQueue<Action>();
//         private readonly SemaphoreW semaphore;
        
//         private long completionCounter;
//         private int workScheduled, threadsBusy;
//         private int disposing;
//         private int leftToDispose;

//         /// <summary> Amount of threads within this pool </summary>
//         public readonly int WorkerThreadsCount;
//         /// <summary> Amount of work waiting to be taken care of </summary>
//         public int WorkScheduled => Volatile.Read(ref workScheduled);
//         /// <summary> Amount of work completed </summary>
//         public ulong CompletedWork => (ulong)Volatile.Read(ref completionCounter);
//         /// <summary> Amount of threads currently executing work items </summary>
//         public int ThreadsBusy => Volatile.Read(ref threadsBusy);

//         public ThreadPool(int? threadCount = null)
//         {
//             semaphore = new SemaphoreW(spinCountParam:70);
            
//             WorkerThreadsCount = threadCount ?? (Environment.ProcessorCount == 1 ? 1 : Environment.ProcessorCount - 1);
//             leftToDispose = WorkerThreadsCount;
//             for (int i = 0; i < WorkerThreadsCount; i++)
//             {
//                 NewWorker();
//             }
//         }

//         static ThreadPool()
//         {
//             SingleCore = Environment.ProcessorCount < 2;
//         }

//         /// <summary>
//         /// Queue an action to run on one of the available threads,
//         /// it is strongly recommended that the action takes less than a millisecond.
//         /// </summary>
//         public void QueueWorkItem([NotNull] Action workItem, int amount = 1)
//         {
//             // Throw right here to help debugging
//             if (workItem == null)
//             {
//                 throw new NullReferenceException(nameof(workItem));
//             }

//             if (amount < 1)
//             {
//                 throw new ArgumentOutOfRangeException(nameof(amount));
//             }

//             if (disposing > 0)
//             {
//                 throw new ObjectDisposedException(ToString());
//             }

//             Interlocked.Add(ref workScheduled, amount);
//             for (int i = 0; i < amount; i++)
//             {
//                 PooledDelegateHelper.AddReference(workItem);
//                 workItems.Enqueue(workItem);
//             }
//             semaphore.Release(amount);
//         }

//         /// <summary>
//         /// Attempt to steal work from the threadpool to execute it from the calling thread.
//         /// If you absolutely have to block inside one of the threadpool's thread for whatever
//         /// reason do a busy loop over this function.
//         /// </summary>
//         public bool TryCooperate()
//         {
//             if (workItems.TryDequeue(out var workItem))
//             {
//                 Interlocked.Increment(ref threadsBusy);
//                 Interlocked.Decrement(ref workScheduled);
//                 try
//                 {
//                     workItem.Invoke();
//                 }
//                 finally
//                 {
//                     PooledDelegateHelper.Release(workItem);
//                     Interlocked.Decrement(ref threadsBusy);
//                     Interlocked.Increment(ref completionCounter);
//                 }
//                 return true;
//             }

//             return false;
//         }

//         private void NewWorker()
//         {
//             new Thread(WorkerThreadScope)
//             {
//                 Name = $"{GetType().FullName} thread",
//                 IsBackground = true,
//                 Priority = ThreadPriority.Highest,
//             }.Start();
//         }

//         private void WorkerThreadScope()
//         {
//             isWorkedThread = true;
//             try
//             {
//                 do
//                 {
//                     while (TryCooperate())
//                     {
                        
//                     }
                    
//                     if (disposing > 0)
//                     {
//                         return;
//                     }

//                     semaphore.Wait();
//                 } while (true);
//             }
//             finally
//             {
//                 if (disposing == 0)
//                 {
//                     NewWorker();
//                 }
//                 else
//                 {
//                     Interlocked.Decrement(ref leftToDispose);
//                 }
//             }
//         }



//         public void Dispose()
//         {
//             if (Interlocked.CompareExchange(ref disposing, 1, 0) == 1)
//             {
//                 return;
//             }
            
//             semaphore.Release(WorkerThreadsCount);
//             while (Volatile.Read(ref leftToDispose) != 0)
//             {
//                 if (semaphore.SignalCount == 0)
//                 {
//                     semaphore.Release(1);
//                 }
//                 Thread.Yield();
//             }

//             // Finish any work left
//             while (TryCooperate())
//             {
                
//             }
//         }


//          /// <summary>
//         /// Mostly lifted from dotnet's LowLevelLifoSemaphore
//         /// </summary>
//         private class SemaphoreW
//         {
//             private const int SpinSleep0Threshold = 10;
            
//             private static readonly int OptimalMaxSpinWaitsPerSpinIteration;
//             private static readonly int SpinMult;
            
//             /// <summary>
//             /// Eideren: Is not actually lifo, standard 2.0 doesn't have such constructs right now
//             /// </summary>
//             private readonly Semaphore lifoSemaphore;
//             private readonly int spinCount;
//             private Internals internals;
//             public uint SignalCount => internals.SignalCount;
            
            
            
//             static SemaphoreW()
//             {
//                 // Workaround as Thread.OptimalMaxSpinWaitsPerSpinIteration is internal and only implemented in core
//                 BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static;
//                 var f = typeof(Thread).GetProperty("OptimalMaxSpinWaitsPerSpinIteration", flags);
//                 int opti = 7;
//                 if (f != null)
//                 {
//                     opti = (int)f.GetValue(null);
//                 }
//                 OptimalMaxSpinWaitsPerSpinIteration = opti;
//                 SpinMult = Environment.OSVersion.Platform == PlatformID.Unix ? 2 : 1;
//             }



//             public SemaphoreW(int spinCountParam)
//             {
//                 Debug.Assert(spinCountParam >= 0);

//                 internals = default;
//                 spinCount = spinCountParam;

//                 lifoSemaphore = new Semaphore(0, int.MaxValue);
//             }

//             public void Wait(int timeout = -1) => internals.Wait(spinCount, lifoSemaphore, timeout);

//             public void Release(int releaseCount) => internals.Release(releaseCount, lifoSemaphore);

//             [StructLayout(LayoutKind.Explicit)]
//             private struct Counts
//             {
//                 [FieldOffset(0)] public long AsLong;
//                 [FieldOffset(0)] public uint SignalCount;
//                 [FieldOffset(4)] public ushort WaiterCount;
//                 [FieldOffset(6)] public byte SpinnerCount;
//                 [FieldOffset(7)] public byte CountOfWaitersSignaledToWake;
//             }
            
//             [StructLayout(LayoutKind.Sequential)]
//             private struct Internals
//             {
//                 public uint SignalCount => _counts.SignalCount;
                
//                 private readonly PaddingFalseSharing _pad1;
//                 private Counts _counts;
//                 private readonly PaddingFalseSharing _pad2;
                
//                 public bool Wait(int spinCount, Semaphore lifoSemaphore, int timeoutMs)
//                 {
//                     Debug.Assert(timeoutMs >= -1);

//                     // Try to acquire the semaphore or
//                     // a) register as a spinner if spinCount > 0 and timeoutMs > 0
//                     // b) register as a waiter if there's already too many spinners or spinCount == 0 and timeoutMs > 0
//                     // c) bail out if timeoutMs == 0 and return false

//                     Counts counts = _counts;
//                     do
//                     {
//                         Counts newCounts = counts;

//                         if (counts.SignalCount != 0)
//                         {
//                             newCounts.SignalCount--;
//                         }
//                         else if (timeoutMs != 0)
//                         {
//                             if (spinCount > 0 && newCounts.SpinnerCount < byte.MaxValue)
//                             {
//                                 newCounts.SpinnerCount++;
//                             }
//                             else
//                             {
//                                 // Maximum number of spinners reached, register as a waiter instead
//                                 newCounts.WaiterCount++;
//                                 Debug.Assert(newCounts.WaiterCount != 0); // overflow check, this many waiters is currently not supported
//                             }
//                         }

//                         Counts countsBeforeUpdate = CompareExchange(newCounts, counts);
//                         if (countsBeforeUpdate.AsLong == counts.AsLong)
//                         {
//                             if (counts.SignalCount != 0)
//                             {
//                                 return true;
//                             }

//                             if (newCounts.WaiterCount != counts.WaiterCount)
//                             {
//                                 return WaitForSignal(timeoutMs, lifoSemaphore);
//                             }

//                             if (timeoutMs == 0)
//                             {
//                                 return false;
//                             }

//                             break;
//                         }

//                         counts = countsBeforeUpdate;
//                     } while (true);

//                     spinCount *= SpinMult;

//                     // Waiting for signal as a spinner
//                     int spinIndex = SingleCore == false ? 0 : SpinSleep0Threshold;
//                     while (spinIndex < spinCount)
//                     {
//                         Spin(spinIndex, 10);
//                         spinIndex++;

//                         // Try to acquire the semaphore and unregister as a spinner
//                         counts = _counts;
//                         while (counts.SignalCount > 0)
//                         {
//                             Counts newCounts = counts;
//                             newCounts.SignalCount--;
//                             newCounts.SpinnerCount--;

//                             Counts countsBeforeUpdate = CompareExchange(newCounts, counts);
//                             if (countsBeforeUpdate.AsLong == counts.AsLong)
//                             {
//                                 return true;
//                             }

//                             counts = countsBeforeUpdate;
//                         }
//                     }

//                     // Swap to waiter
//                     counts = _counts;
//                     do
//                     {
//                         Counts newCounts = counts;
//                         newCounts.SpinnerCount--;
//                         if (counts.SignalCount != 0)
//                         {
//                             newCounts.SignalCount--;
//                         }
//                         else
//                         {
//                             newCounts.WaiterCount++;
//                             Debug.Assert(newCounts.WaiterCount != 0); // overflow check, this many waiters is currently not supported
//                         }

//                         Counts countsBeforeUpdate = CompareExchange(newCounts, counts);
//                         if (countsBeforeUpdate.AsLong == counts.AsLong)
//                         {
//                             return counts.SignalCount != 0 || WaitForSignal(timeoutMs, lifoSemaphore);
//                         }

//                         counts = countsBeforeUpdate;
//                     } while (true);
//                 }
                
//                 public void Release(int releaseCount, Semaphore lifoSemaphore)
//                 {
//                     Debug.Assert(releaseCount > 0);

//                     int countOfWaitersToWake;
//                     Counts counts = _counts;
//                     do
//                     {
//                         Counts newCounts = counts;

//                         // Increase the signal count. The addition doesn't overflow because of the limit on the max signal count in constructor.
//                         newCounts.SignalCount += (uint)releaseCount;
//                         Debug.Assert(newCounts.SignalCount > counts.SignalCount);

//                         // Determine how many waiters to wake, taking into account how many spinners and waiters there are and how many waiters
//                         // have previously been signaled to wake but have not yet woken
//                         countOfWaitersToWake = (int)System.Math.Min(newCounts.SignalCount, (uint)counts.WaiterCount + counts.SpinnerCount) -
//                                                counts.SpinnerCount -
//                                                counts.CountOfWaitersSignaledToWake;
//                         if (countOfWaitersToWake > 0)
//                         {
//                             // Ideally, limiting to a maximum of releaseCount would not be necessary and could be an assert instead, but since
//                             // WaitForSignal() does not have enough information to tell whether a woken thread was signaled, and due to the cap
//                             // below, it's possible for countOfWaitersSignaledToWake to be less than the number of threads that have actually
//                             // been signaled to wake.
//                             if (countOfWaitersToWake > releaseCount)
//                             {
//                                 countOfWaitersToWake = releaseCount;
//                             }

//                             // Cap countOfWaitersSignaledToWake to its max value. It's ok to ignore some woken threads in this count, it just
//                             // means some more threads will be woken next time. Typically, it won't reach the max anyway.

//                             uint value = (uint)countOfWaitersToWake;
//                             uint availableCount = (uint)(byte.MaxValue - newCounts.CountOfWaitersSignaledToWake);
//                             if (value > availableCount)
//                             {
//                                 value = availableCount;
//                             }
//                             newCounts.CountOfWaitersSignaledToWake += (byte)value;
//                         }

//                         Counts countsBeforeUpdate = CompareExchange(newCounts, counts);
//                         if (countsBeforeUpdate.AsLong == counts.AsLong)
//                         {
//                             if (countOfWaitersToWake > 0)
//                                 lifoSemaphore.Release(countOfWaitersToWake);
//                             return;
//                         }

//                         counts = countsBeforeUpdate;
//                     } while (true);
//                 }
                
//                 public bool WaitForSignal(int timeoutMs, Semaphore lifoSemaphore)
//                 {
//                     Debug.Assert(timeoutMs > 0 || timeoutMs == -1);

//                     while (true)
//                     {
//                         if (!lifoSemaphore.WaitOne(timeoutMs))
//                         {
//                             // Unregister the waiter. The wait subsystem used above guarantees that a thread that wakes due to a timeout does
//                             // not observe a signal to the object being waited upon.
//                             Counts toSubtract = default;
//                             toSubtract.WaiterCount++;
//                             Counts newCounts = Subtract(toSubtract);
//                             Debug.Assert(newCounts.WaiterCount != ushort.MaxValue); // Check for underflow
//                             return false;
//                         }

//                         // Unregister the waiter if this thread will not be waiting anymore, and try to acquire the semaphore
//                         Counts counts = _counts;
//                         while (true)
//                         {
//                             Debug.Assert(counts.WaiterCount != 0);
//                             Counts newCounts = counts;
//                             if (counts.SignalCount != 0)
//                             {
//                                 --newCounts.SignalCount;
//                                 --newCounts.WaiterCount;
//                             }

//                             // This waiter has woken up and this needs to be reflected in the count of waiters signaled to wake
//                             if (counts.CountOfWaitersSignaledToWake != 0)
//                             {
//                                 --newCounts.CountOfWaitersSignaledToWake;
//                             }

//                             Counts countsBeforeUpdate = CompareExchange(newCounts, counts);
//                             if (countsBeforeUpdate.AsLong == counts.AsLong)
//                             {
//                                 if (counts.SignalCount != 0)
//                                 {
//                                     return true;
//                                 }

//                                 break;
//                             }

//                             counts = countsBeforeUpdate;
//                         }
//                     }
//                 }
            
//                 private static void Spin(int spinIndex, int sleep0Threshold)
//                 {
//                     Debug.Assert(spinIndex >= 0);

//                     // Wait
//                     //
//                     // (spinIndex - Sleep0Threshold) % 2 != 0: The purpose of this check is to interleave Thread.Yield/Sleep(0) with
//                     // Thread.SpinWait. Otherwise, the following issues occur:
//                     //   - When there are no threads to switch to, Yield and Sleep(0) become no-op and it turns the spin loop into a
//                     //     busy-spin that may quickly reach the max spin count and cause the thread to enter a wait state. Completing the
//                     //     spin loop too early can cause excessive context switcing from the wait.
//                     //   - If there are multiple threads doing Yield and Sleep(0) (typically from the same spin loop due to contention),
//                     //     they may switch between one another, delaying work that can make progress.
//                     if (SingleCore == false && (spinIndex < sleep0Threshold || (spinIndex - sleep0Threshold) % 2 != 0))
//                     {
//                         // Cap the maximum spin count to a value such that many thousands of CPU cycles would not be wasted doing
//                         // the equivalent of YieldProcessor(), as that that point SwitchToThread/Sleep(0) are more likely to be able to
//                         // allow other useful work to run. Long YieldProcessor() loops can help to reduce contention, but Sleep(1) is
//                         // usually better for that.
//                         //
//                         // Thread.OptimalMaxSpinWaitsPerSpinIteration:
//                         //   - See Thread::InitializeYieldProcessorNormalized(), which describes and calculates this value.
//                         //
//                         int n = OptimalMaxSpinWaitsPerSpinIteration;
//                         if (spinIndex <= 30 && (1 << spinIndex) < n)
//                         {
//                             n = 1 << spinIndex;
//                         }
//                         Thread.SpinWait(n);
//                         return;
//                     }

//                     // Thread.Sleep(int) is interruptible. The current operation may not allow thread interrupt
//                     // (for instance, LowLevelLock.Acquire as part of EventWaitHandle.Set). Use the
//                     // uninterruptible version of Sleep(0). Not doing Thread.Yield, it does not seem to have any
//                     // benefit over Sleep(0).
//                     Thread.Sleep(0);
//                     /*Thread.UninterruptibleSleep0();*/ // Eideren: Not a thing on standard 2.0 and pointless since our implementation doesn't have area preventing thread interrupts

//                     // Don't want to Sleep(1) in this spin wait:
//                     //   - Don't want to spin for that long, since a proper wait will follow when the spin wait fails
//                     //   - Sleep(1) would put the thread into a wait state, and a proper wait will follow when the spin wait fails
//                     //     anyway (the intended use for this class), so it's preferable to put the thread into the proper wait state
//                 }
                
//                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
//                 Counts CompareExchange(Counts newCounts, Counts oldCounts)
//                 {
//                     return new Counts { AsLong = Interlocked.CompareExchange(ref _counts.AsLong, newCounts.AsLong, oldCounts.AsLong) };
//                 }

//                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
//                 Counts Subtract(Counts subtractCounts)
//                 {
//                     return new Counts { AsLong = Interlocked.Add(ref _counts.AsLong, -subtractCounts.AsLong) };
//                 }
//             }

//             /// <summary>Padding structure used to minimize false sharing</summary>
//             [StructLayout(LayoutKind.Explicit, Size = CACHE_LINE_SIZE - sizeof(int))]
//             private struct PaddingFalseSharing
//             {
//             }

//             /// <summary>A size greater than or equal to the size of the most common CPU cache lines.</summary>
// #if TARGET_ARM64
//             private const int CACHE_LINE_SIZE = 128;
// #else
//             private const int CACHE_LINE_SIZE = 64;
// #endif
//         }
//     }

//      /// <summary>
//     /// Helper class to add and remove references to pooled delegates, passed as parameters with <see cref="PooledAttribute"/>>.
//     /// </summary>
//     internal static class PooledDelegateHelper
//     {
//         /// <summary>
//         /// Adds a reference to a delegate, keeping it from being recycled. Does nothing if the delegate is not drawn from a pool.
//         /// </summary>
//         /// <param name="pooledDelegate">The pooled delegate</param>
//         public static void AddReference([NotNull] Delegate pooledDelegate)
//         {
//             var closure = pooledDelegate.Target as IPooledClosure;
//             closure?.AddReference();
//         }

//         /// <summary>
//         /// Removes a reference from a delegate, allowing it to be recycled. Does nothing if the delegate is not drawn from a pool.
//         /// </summary>
//         /// <param name="pooledDelegate">The pooled delegate</param>
//         public static void Release([NotNull] Delegate pooledDelegate)
//         {
//             var closure = pooledDelegate.Target as IPooledClosure;
//             closure?.Release();
//         }
//     }


//       /// <summary>
//     /// A concurrent object pool.
//     /// </summary>
//     /// <remarks>
//     /// Circular buffer segments are used as storage. When full, new segments are added as tail. Items are only appended to the tail segment.
//     /// When the head segment is empty, it will be discarded. After stabilizing, only a single segment exists at a time, causing no further segment allocations or locking.
//     /// </remarks>
//     /// <typeparam name="T">The pooled item type</typeparam>
//     public class ConcurrentPool<T>
//         where T : class
//     {
//         private class Segment
//         {
//             /// <summary>
//             /// The array of items. Length must be a power of two.
//             /// </summary>
//             public readonly T[] Items;

//             /// <summary>
//             /// A bit mask for calculation of (Low % Items.Length) and (High % Items.Length)
//             /// </summary>
//             public readonly int Mask;

//             /// <summary>
//             /// The read index for Release. It is only ever incremented and safe to overflow.
//             /// </summary>
//             public int Low;

//             /// <summary>
//             /// The write index for Acquire. It is only ever incremented and safe to overflow.
//             /// </summary>
//             public int High;

//             /// <summary>
//             /// The current number of stored items, used to check when to change head and tail segments.
//             /// When it reaches zero, the segment can be safely discarded.
//             /// </summary>
//             public int Count;

//             /// <summary>
//             /// The next segment to draw from, after this one is emptied.
//             /// </summary>
//             public Segment Next;

//             public Segment(int size)
//             {
//                 // Size must be a power of two for modulo and overflow of read/write indices to behave correctly
//                 if (size <= 0 || ((size & (size - 1)) != 0))
//                     throw new ArgumentOutOfRangeException(nameof(size), "Must be power of two");

//                 Items = new T[size];
//                 Mask = size - 1;
//             }
//         }

//         private const int DefaultCapacity = 4;

//         private readonly object resizeLock = new object();
//         private readonly Func<T> factory;
//         private Segment head;
//         private Segment tail;

//         /// <summary>
//         /// Initializes a new instance of the <see cref="ConcurrentPool{T}"/> class.
//         /// </summary>
//         /// <param name="factory">The factory method for creating new items, should the pool be empty.</param>
//         public ConcurrentPool(Func<T> factory)
//         {
//             head = tail = new Segment(DefaultCapacity);
//             this.factory = factory;
//         }

//         /// <summary>
//         /// Draws an item from the pool.
//         /// </summary>
//         public T Acquire()
//         {
//             while (true)
//             {
//                 var localHead = head;
//                 var count = localHead.Count;

//                 if (count == 0)
//                 {
//                     // If first segment is empty, but there is at least one other, move the head forward.
//                     if (localHead.Next != null)
//                     {
//                         lock (resizeLock)
//                         {
//                             if (head.Next != null && head.Count == 0)
//                             {
//                                 head = head.Next;
//                             }
//                         }
//                     }
//                     else
//                     {
//                         // If there was only one segment and it was empty, create a new item.
//                         return factory();
//                     }
//                 }
//                 else if (Interlocked.CompareExchange(ref localHead.Count, count - 1, count) == count)
//                 {
//                     // If there were any items and we could reserve one of them, move the
//                     // read index forward and get the index of the item we can acquire.
//                     var localLow = Interlocked.Increment(ref localHead.Low) - 1;

//                     // Modulo Items.Length to calculate the actual index.
//                     var index = localLow & localHead.Mask;

//                     // Take the item. Spin until the slot has been written by pending calls to Release.
//                     T item;
//                     var spinWait = new SpinWait();
//                     while ((item = Interlocked.Exchange(ref localHead.Items[index], null)) == null)
//                     {
//                         spinWait.SpinOnce();
//                     }

//                     return item;
//                 }
//             }
//         }

//         /// <summary>
//         /// Releases an item back to the pool.
//         /// </summary>
//         /// <param name="item">The item to release to the pool.</param>
//         public void Release(T item)
//         {
//             while (true)
//             {
//                 var localTail = tail;
//                 var count = localTail.Count;

//                 // If the segment was full, allocate and append a new, bigger one.
//                 if (count == localTail.Items.Length)
//                 {
//                     lock (resizeLock)
//                     {
//                         if (tail.Next == null && count == tail.Items.Length)
//                         {
//                             tail = tail.Next = new Segment(tail.Items.Length << 1);
//                         }
//                     }
//                 }
//                 else if (Interlocked.CompareExchange(ref localTail.Count, count + 1, count) == count)
//                 {
//                     // If there was space for another item and we were able to reserve it, move the
//                     // write index forward and get the index of the slot we can write into.
//                     var localHigh = Interlocked.Increment(ref localTail.High) - 1;

//                     // Modulo Items.Length to calculate the actual index.
//                     var index = localHigh & localTail.Mask;

//                     // Write the item. Spin until the slot has been cleared by pending calls to Acquire.
//                     var spinWait = new SpinWait();
//                     while (Interlocked.CompareExchange(ref localTail.Items[index], item, null) != null)
//                     {
//                         spinWait.SpinOnce();
//                     }

//                     return;
//                 }
//             }
//         }
//     }




//     public class ConcurrentCollectorCache<T>
//     {
//         private readonly int capacity;
//         private readonly List<T> cache = new List<T>();
//         private ConcurrentCollector<T> currentCollection;

//         public ConcurrentCollectorCache(int capacity)
//         {
//             this.capacity = capacity;
//         }

//         public void Add([NotNull] ConcurrentCollector<T> collection, T item)
//         {
//             if (collection == null) throw new ArgumentNullException(nameof(collection));

//             if (currentCollection != collection || cache.Count > capacity)
//             {
//                 if (currentCollection != null)
//                 {
//                     currentCollection.AddRange(cache);
//                     cache.Clear();
//                 }
//                 currentCollection = collection;
//             }

//             cache.Add(item);
//         }

//         public void Flush()
//         {
//             if (currentCollection != null)
//             {
//                 currentCollection.AddRange(cache);
//                 cache.Clear();
//             }
//             currentCollection = null;
//         }
//     }

//     public static class ConcurrentCollectorExtensions
//     {
//         public static void Add<T>([NotNull] this ConcurrentCollector<T> collection, T item, [NotNull] ConcurrentCollectorCache<T> cache)
//         {
//             cache.Add(collection, item);
//         }
//     }

//     /// <summary>
//     /// A collector that allows for concurrent adding of items, as well as non-thread-safe clearing and accessing of the underlying colletion.
//     /// </summary>
//     /// <typeparam name="T">The element type in the collection.</typeparam>
//     public class ConcurrentCollector<T> : IReadOnlyList<T>
//     {
//         private const int DefaultCapacity = 16;

//         private class Segment
//         {
//             public T[] Items;
//             public int Offset;
//             public Segment Previous;
//             public Segment Next;
//         }

//         private readonly object resizeLock = new object();
//         private readonly Segment head;
//         private Segment tail;
//         private int count;

//         public ConcurrentCollector(int capacity = DefaultCapacity)
//         {
//             tail = head = new Segment { Items = new T[capacity] };
//         }

//         public T[] Items
//         {
//             get
//             {
//                 if (head != tail)
//                     throw new InvalidOperationException();

//                 return head.Items;
//             }
//         }

//         /// <summary>
//         /// Consolidates all added items into a single consecutive array. It is an error to access Items after adding elements, but before closing.
//         /// </summary>
//         public void Close()
//         {
//             if (head.Next != null)
//             {
//                 var newItems = new T[tail.Offset + tail.Items.Length];

//                 var segment = head;
//                 while (segment != null)
//                 {
//                     Array.Copy(segment.Items, 0, newItems, segment.Offset, segment.Items.Length);
//                     segment = segment.Next;
//                 }

//                 head.Items = newItems;
//                 head.Next = null;

//                 tail = head;
//             }
//         }

//         public int Add(T item)
//         {
//             var index = Interlocked.Increment(ref count) - 1;

//             var segment = tail;
//             if (index >= segment.Offset + segment.Items.Length)
//             {
//                 lock (resizeLock)
//                 {
//                     if (index >= tail.Offset + tail.Items.Length)
//                     {
//                         tail.Next = new Segment
//                         {
//                             Items = new T[segment.Items.Length * 2],
//                             Offset = segment.Offset + segment.Items.Length,
//                             Previous = tail,
//                         };

//                         tail = tail.Next;
//                     }

//                     segment = tail;
//                 }
//             }

//             while (index < segment.Offset)
//             {
//                 segment = segment.Previous;
//             }

//             segment.Items[index - segment.Offset] = item;

//             return index;
//         }

//         public void AddRange([NotNull] IReadOnlyList<T> collection)
//         {
//             var newCount = Interlocked.Add(ref count, collection.Count);

//             var segment = tail;
//             if (newCount >= segment.Offset + segment.Items.Length)
//             {
//                 lock (resizeLock)
//                 {
//                     if (newCount >= tail.Offset + tail.Items.Length)
//                     {
//                         var capacity = tail.Offset + tail.Items.Length;
//                         var size = System.Math.Max(capacity, newCount - capacity);

//                         tail.Next = new Segment
//                         {
//                             Items = new T[size],
//                             Offset = capacity,
//                             Previous = tail,
//                         };

//                         tail = tail.Next;
//                     }

//                     segment = tail;
//                 }
//             }

//             // Find the segment containing the last index
//             while (newCount <= segment.Offset)
//                 segment = segment.Previous;

//             var destinationIndex = newCount - segment.Offset - 1;
//             for (var sourceIndex = collection.Count - 1; sourceIndex >= 0; sourceIndex--)
//             {
//                 if (destinationIndex < 0)
//                 {
//                     segment = segment.Previous;
//                     destinationIndex = segment.Items.Length - 1;
//                 }

//                 segment.Items[destinationIndex] = collection[sourceIndex];
//                 destinationIndex--;
//             }
//         }

//         public void Clear(bool fastClear)
//         {
//             Close();
//             if (!fastClear && count > 0)
//             {
//                 Array.Clear(Items, 0, count);
//             }
//             count = 0;
//         }

//         IEnumerator IEnumerable.GetEnumerator()
//         {
//             return GetEnumerator();
//         }

//         IEnumerator<T> IEnumerable<T>.GetEnumerator()
//         {
//             return new Enumerator(this);
//         }

//         public Enumerator GetEnumerator()
//         {
//             return new Enumerator(this);
//         }

//         public int Count => count;

//         public T this[int index]
//         {
//             get
//             {
//                 return Items[index];
//             }
//             set
//             {
//                 Items[index] = value;
//             }
//         }

//         public struct Enumerator : IEnumerator<T>, IDisposable, IEnumerator
//         {
//             private readonly ConcurrentCollector<T> list;
//             private int index;
//             private T current;

//             internal Enumerator(ConcurrentCollector<T> list)
//             {
//                 this.list = list;
//                 index = 0;
//                 current = default(T);
//             }

//             public void Dispose()
//             {
//             }

//             public bool MoveNext()
//             {
//                 var list = this.list;
//                 if (index < list.count)
//                 {
//                     current = list.Items[index];
//                     index++;
//                     return true;
//                 }
//                 return MoveNextRare();
//             }

//             private bool MoveNextRare()
//             {
//                 index = list.count + 1;
//                 current = default(T);
//                 return false;
//             }

//             public T Current => current;

//             object IEnumerator.Current => Current;

//             void IEnumerator.Reset()
//             {
//                 index = 0;
//                 current = default(T);
//             }
//         }
//     }


//      public class Dispatcher
//     {
// #if STRIDE_PLATFORM_IOS || STRIDE_PLATFORM_ANDROID
//         public static int MaxDegreeOfParallelism = 1;
// #else
//         public static int MaxDegreeOfParallelism = Environment.ProcessorCount;
// #endif

//         public delegate void ValueAction<T>(ref T obj);

//         public static void For(int fromInclusive, int toExclusive, [Pooled] Action<int> action)
//         {
//             using (Profile(action))
//             {
//                 if (fromInclusive > toExclusive)
//                 {
//                     var temp = fromInclusive;
//                     fromInclusive = toExclusive + 1;
//                     toExclusive = temp + 1;
//                 }

//                 var count = toExclusive - fromInclusive;
//                 if (count == 0)
//                     return;

//                 if (MaxDegreeOfParallelism <= 1 || count == 1)
//                 {
//                     ExecuteBatch(fromInclusive, toExclusive, action);
//                 }
//                 else
//                 {
//                     var state = BatchState.Acquire();
//                     state.WorkDone = state.StartInclusive = fromInclusive;

//                     try
//                     {
//                         var batchCount = Math.Min(MaxDegreeOfParallelism, count);
//                         var batchSize = (count + (batchCount - 1)) / batchCount;

//                         // Kick off a worker, then perform work synchronously
//                         state.AddReference();
//                         Fork(toExclusive, batchSize, MaxDegreeOfParallelism, action, state);

//                         // Wait for all workers to finish
//                         state.WaitCompletion(toExclusive);

//                         var ex = Interlocked.Exchange(ref state.ExceptionThrown, null);
//                         if (ex != null)
//                             throw ex;
//                     }
//                     finally
//                     {
//                         state.Release();
//                     }
//                 }
//             }
//         }

//         public static void For<TLocal>(int fromInclusive, int toExclusive, [Pooled] Func<TLocal> initializeLocal, [Pooled] Action<int, TLocal> action, [Pooled] Action<TLocal> finalizeLocal = null)
//         {
//             using (Profile(action))
//             {
//                 if (fromInclusive > toExclusive)
//                 {
//                     var temp = fromInclusive;
//                     fromInclusive = toExclusive + 1;
//                     toExclusive = temp + 1;
//                 }

//                 var count = toExclusive - fromInclusive;
//                 if (count == 0)
//                     return;

//                 if (MaxDegreeOfParallelism <= 1 || count == 1)
//                 {
//                     ExecuteBatch(fromInclusive, toExclusive, initializeLocal, action, finalizeLocal);
//                 }
//                 else
//                 {
//                     var state = BatchState.Acquire();
//                     state.WorkDone = state.StartInclusive = fromInclusive;

//                     try
//                     {
//                         var batchCount = Math.Min(MaxDegreeOfParallelism, count);
//                         var batchSize = (count + (batchCount - 1)) / batchCount;

//                         // Kick off a worker, then perform work synchronously
//                         state.AddReference();
//                         Fork(toExclusive, batchSize, MaxDegreeOfParallelism, initializeLocal, action, finalizeLocal, state);

//                         // Wait for all workers to finish
//                         state.WaitCompletion(toExclusive);

//                         var ex = Interlocked.Exchange(ref state.ExceptionThrown, null);
//                         if (ex != null)
//                             throw ex;
//                     }
//                     finally
//                     {
//                         state.Release();
//                     }
//                 }
//             }
//         }
        
//         public static void ForEach<TItem, TLocal>([NotNull] IReadOnlyList<TItem> collection, [Pooled] Func<TLocal> initializeLocal, [Pooled] Action<TItem, TLocal> action, [Pooled] Action<TLocal> finalizeLocal = null)
//         {
//             For(0, collection.Count, initializeLocal, (i, local) => action(collection[i], local), finalizeLocal);
//         }

//         public static void ForEach<T>([NotNull] IReadOnlyList<T> collection, [Pooled] Action<T> action)
//         {
//             For(0, collection.Count, i => action(collection[i]));
//         }

//         public static void ForEach<T>([NotNull] List<T> collection, [Pooled] Action<T> action)
//         {
//             For(0, collection.Count, i => action(collection[i]));
//         }

//         public static void ForEach<TKey, TValue>([NotNull] Dictionary<TKey, TValue> collection, [Pooled] Action<KeyValuePair<TKey, TValue>> action)
//         {
//             if (MaxDegreeOfParallelism <= 1 || collection.Count <= 1)
//             {
//                 ExecuteBatch(collection, 0, collection.Count, action);
//             }
//             else
//             {
//                 var state = BatchState.Acquire();

//                 try
//                 {
//                     var batchCount = Math.Min(MaxDegreeOfParallelism, collection.Count);
//                     var batchSize = (collection.Count + (batchCount - 1)) / batchCount;

//                     // Kick off a worker, then perform work synchronously
//                     state.AddReference();
//                     Fork(collection, batchSize, MaxDegreeOfParallelism, action, state);

//                     // Wait for all workers to finish
//                     state.WaitCompletion(collection.Count);

//                     var ex = Interlocked.Exchange(ref state.ExceptionThrown, null);
//                     if (ex != null)
//                         throw ex;
//                 }
//                 finally
//                 {
//                     state.Release();
//                 }
//             }
//         }

//         public static void ForEach<TKey, TValue, TLocal>([NotNull] Dictionary<TKey, TValue> collection, [Pooled] Func<TLocal> initializeLocal, [Pooled] Action<KeyValuePair<TKey, TValue>, TLocal> action, [Pooled] Action<TLocal> finalizeLocal = null)
//         {
//             if (MaxDegreeOfParallelism <= 1 || collection.Count <= 1)
//             {
//                 ExecuteBatch(collection, 0, collection.Count, initializeLocal, action, finalizeLocal);
//             }
//             else
//             {
//                 var state = BatchState.Acquire();

//                 try
//                 {
//                     var batchCount = Math.Min(MaxDegreeOfParallelism, collection.Count);
//                     var batchSize = (collection.Count + (batchCount - 1)) / batchCount;

//                     // Kick off a worker, then perform work synchronously
//                     state.AddReference();
//                     Fork(collection, batchSize, MaxDegreeOfParallelism, initializeLocal, action, finalizeLocal, state);

//                     // Wait for all workers to finish
//                     state.WaitCompletion(collection.Count);

//                     var ex = Interlocked.Exchange(ref state.ExceptionThrown, null);
//                     if (ex != null)
//                         throw ex;
//                 }
//                 finally
//                 {
//                     state.Release();
//                 }
//             }
//         }

//         public static void ForEach<T>([NotNull] FastCollection<T> collection, [Pooled] Action<T> action)
//         {
//             For(0, collection.Count, i => action(collection[i]));
//         }

//         public static void ForEach<T>([NotNull] FastList<T> collection, [Pooled] Action<T> action)
//         {
//             For(0, collection.Count, i => action(collection.Items[i]));
//         }

//         public static void ForEach<T>([NotNull] ConcurrentCollector<T> collection, [Pooled] Action<T> action)
//         {
//             For(0, collection.Count, i => action(collection.Items[i]));
//         }

//         public static void ForEach<T>([NotNull] FastList<T> collection, [Pooled] ValueAction<T> action)
//         {
//             For(0, collection.Count, i => action(ref collection.Items[i]));
//         }

//         public static void ForEach<T>([NotNull] ConcurrentCollector<T> collection, [Pooled] ValueAction<T> action)
//         {
//             For(0, collection.Count, i => action(ref collection.Items[i]));
//         }

//         private static void Fork<TKey, TValue>([NotNull] Dictionary<TKey, TValue> collection, int batchSize, int maxDegreeOfParallelism, [Pooled] Action<KeyValuePair<TKey, TValue>> action, [NotNull] BatchState state)
//         {
//             // Other threads already processed all work before this one started.
//             if (state.StartInclusive >= collection.Count)
//             {
//                 state.Release();
//                 return;
//             }

//             // Kick off another worker if there's any work left
//             if (maxDegreeOfParallelism > 1 && state.StartInclusive + batchSize < collection.Count)
//             {
//                 int workToSchedule = maxDegreeOfParallelism - 1;
//                 for (int i = 0; i < workToSchedule; i++)
//                 {
//                     state.AddReference();
//                 }
//                 ThreadPool.Instance.QueueWorkItem(() => Fork(collection, batchSize, 0, action, state), workToSchedule);
//             }

//             try
//             {
//                 // Process batches synchronously as long as there are any
//                 int newStart;
//                 while ((newStart = Interlocked.Add(ref state.StartInclusive, batchSize)) - batchSize < collection.Count)
//                 {
//                     try
//                     {
//                         // TODO: Reuse enumerator when processing multiple batches synchronously
//                         var start = newStart - batchSize;
//                         ExecuteBatch(collection, newStart - batchSize, Math.Min(collection.Count, newStart) - start, action);
//                     }
//                     finally
//                     {
//                         if (Interlocked.Add(ref state.WorkDone, batchSize) >= collection.Count)
//                         {
//                              // Don't wait for other threads to wake up and signal the BatchState, release as soon as work is finished
//                             state.Finished.Set();
//                         }
//                     }
//                 }
//             }
//             catch (Exception e)
//             {
//                 Interlocked.Exchange(ref state.ExceptionThrown, e);
//                 throw;
//             }
//             finally
//             {
//                 state.Release();
//             }
//         }

//         private static void Fork<TKey, TValue, TLocal>([NotNull] Dictionary<TKey, TValue> collection, int batchSize, int maxDegreeOfParallelism, [Pooled] Func<TLocal> initializeLocal, [Pooled] Action<KeyValuePair<TKey, TValue>, TLocal> action, [Pooled] Action<TLocal> finalizeLocal, [NotNull] BatchState state)
//         {
//             // Other threads already processed all work before this one started.
//             if (state.StartInclusive >= collection.Count)
//             {
//                 state.Release();
//                 return;
//             }

//             // Kick off another worker if there's any work left
//             if (maxDegreeOfParallelism > 1 && state.StartInclusive + batchSize < collection.Count)
//             {
//                 int workToSchedule = maxDegreeOfParallelism - 1;
//                 for (int i = 0; i < workToSchedule; i++)
//                 {
//                     state.AddReference();
//                 }
//                 ThreadPool.Instance.QueueWorkItem(() => Fork(collection, batchSize, 0, initializeLocal, action, finalizeLocal, state), workToSchedule);
//             }

//             try
//             {
//                 // Process batches synchronously as long as there are any
//                 int newStart;
//                 while ((newStart = Interlocked.Add(ref state.StartInclusive, batchSize)) - batchSize < collection.Count)
//                 {
//                     try
//                     {
//                         // TODO: Reuse enumerator when processing multiple batches synchronously
//                         var start = newStart - batchSize;
//                         ExecuteBatch(collection, newStart - batchSize, Math.Min(collection.Count, newStart) - start, initializeLocal, action, finalizeLocal);
//                     }
//                     finally
//                     {
//                         if (Interlocked.Add(ref state.WorkDone, batchSize) >= collection.Count)
//                         {
//                              // Don't wait for other threads to wake up and signal the BatchState, release as soon as work is finished
//                             state.Finished.Set();
//                         }
//                     }
//                 }
//             }
//             catch (Exception e)
//             {
//                 Interlocked.Exchange(ref state.ExceptionThrown, e);
//                 throw;
//             }
//             finally
//             {
//                 state.Release();
//             }
//         }

//         private static void ExecuteBatch(int fromInclusive, int toExclusive, [Pooled] Action<int> action)
//         {
//             for (var i = fromInclusive; i < toExclusive; i++)
//             {
//                 action(i);
//             }
//         }

//         private static void ExecuteBatch<TLocal>(int fromInclusive, int toExclusive, [Pooled] Func<TLocal> initializeLocal, [Pooled] Action<int, TLocal> action, [Pooled] Action<TLocal> finalizeLocal)
//         {
//             var local = default(TLocal);
//             try
//             {
//                 if (initializeLocal != null)
//                 {
//                     local = initializeLocal();
//                 }

//                 for (var i = fromInclusive; i < toExclusive; i++)
//                 {
//                     action(i, local);
//                 }
//             }
//             finally
//             {
//                 finalizeLocal?.Invoke(local);
//             }
//         }

//         private static void Fork(int endExclusive, int batchSize, int maxDegreeOfParallelism, [Pooled] Action<int> action, [NotNull] BatchState state)
//         {
//             // Other threads already processed all work before this one started.
//             if (state.StartInclusive >= endExclusive)
//             {
//                 state.Release();
//                 return;
//             }

//             // Kick off another worker if there's any work left
//             if (maxDegreeOfParallelism > 1 && state.StartInclusive + batchSize < endExclusive)
//             {
//                 int workToSchedule = maxDegreeOfParallelism - 1;
//                 for (int i = 0; i < workToSchedule; i++)
//                 {
//                     state.AddReference();
//                 }
//                 ThreadPool.Instance.QueueWorkItem(() => Fork(endExclusive, batchSize, 0, action, state), workToSchedule);
//             }

//             try
//             {
//                 // Process batches synchronously as long as there are any
//                 int newStart;
//                 while ((newStart = Interlocked.Add(ref state.StartInclusive, batchSize)) - batchSize < endExclusive)
//                 {
//                     try
//                     {
//                         ExecuteBatch(newStart - batchSize, Math.Min(endExclusive, newStart), action);
//                     }
//                     finally
//                     {
//                         if (Interlocked.Add(ref state.WorkDone, batchSize) >= endExclusive)
//                         {
//                              // Don't wait for other threads to wake up and signal the BatchState, release as soon as work is finished
//                             state.Finished.Set();
//                         }
//                     }
//                 }
//             }
//             catch (Exception e)
//             {
//                 Interlocked.Exchange(ref state.ExceptionThrown, e);
//                 throw;
//             }
//             finally
//             {
//                 state.Release();
//             }
//         }

//         private static void Fork<TLocal>(int endExclusive, int batchSize, int maxDegreeOfParallelism, [Pooled] Func<TLocal> initializeLocal, [Pooled] Action<int, TLocal> action, [Pooled] Action<TLocal> finalizeLocal, [NotNull] BatchState state)
//         {
//             // Other threads already processed all work before this one started.
//             if (state.StartInclusive >= endExclusive)
//             {
//                 state.Release();
//                 return;
//             }

//             // Kick off another worker if there's any work left
//             if (maxDegreeOfParallelism > 1 && state.StartInclusive + batchSize < endExclusive)
//             {
//                 int workToSchedule = maxDegreeOfParallelism - 1;
//                 for (int i = 0; i < workToSchedule; i++)
//                 {
//                     state.AddReference();
//                 }
//                 ThreadPool.Instance.QueueWorkItem(() => Fork(endExclusive, batchSize, 0, initializeLocal, action, finalizeLocal, state), workToSchedule);
//             }

//             try
//             {
//                 // Process batches synchronously as long as there are any
//                 int newStart;
//                 while ((newStart = Interlocked.Add(ref state.StartInclusive, batchSize)) - batchSize < endExclusive)
//                 {
//                     try
//                     {
//                         ExecuteBatch(newStart - batchSize, Math.Min(endExclusive, newStart), initializeLocal, action, finalizeLocal);
//                     }
//                     finally
//                     {
//                         if (Interlocked.Add(ref state.WorkDone, batchSize) >= endExclusive)
//                         {
//                             // Don't wait for other threads to wake up and signal the BatchState, release as soon as work is finished
//                             state.Finished.Set();
//                         }
//                     }
//                 }
//             }
//             catch (Exception e)
//             {
//                 Interlocked.Exchange(ref state.ExceptionThrown, e);
//                 throw;
//             }
//             finally
//             {
//                 state.Release();
//             }
//         }

//         private static void ExecuteBatch<TKey, TValue>([NotNull] Dictionary<TKey, TValue> dictionary, int offset, int count, [Pooled] Action<KeyValuePair<TKey, TValue>> action)
//         {
//             var enumerator = dictionary.GetEnumerator();
//             var index = 0;

//             // Skip to offset
//             while (index < offset && enumerator.MoveNext())
//             {
//                 index++;
//             }

//             // Process batch
//             while (index < offset + count && enumerator.MoveNext())
//             {
//                 action(enumerator.Current);
//                 index++;
//             }
//         }

//         private static void ExecuteBatch<TKey, TValue, TLocal>([NotNull] Dictionary<TKey, TValue> dictionary, int offset, int count, [Pooled] Func<TLocal> initializeLocal, [Pooled] Action<KeyValuePair<TKey, TValue>, TLocal> action, [Pooled] Action<TLocal> finalizeLocal)
//         {
//             var local = default(TLocal);
//             try
//             {
//                 if (initializeLocal != null)
//                 {
//                     local = initializeLocal();
//                 }

//                 var enumerator = dictionary.GetEnumerator();
//                 var index = 0;

//                 // Skip to offset
//                 while (index < offset && enumerator.MoveNext())
//                 {
//                     index++;
//                 }

//                 // Process batch
//                 while (index < offset + count && enumerator.MoveNext())
//                 {
//                     action(enumerator.Current, local);
//                     index++;
//                 }
//             }
//             finally
//             {
//                 finalizeLocal?.Invoke(local);
//             }
//         }

//         public static void Sort<T>([NotNull] ConcurrentCollector<T> collection, IComparer<T> comparer)
//         {
//             Sort(collection.Items, 0, collection.Count, comparer);
//         }

//         public static void Sort<T>([NotNull] FastList<T> collection, IComparer<T> comparer)
//         {
//             Sort(collection.Items, 0, collection.Count, comparer);
//         }

//         public static void Sort<T>(T[] collection, int index, int length, IComparer<T> comparer)
//         {
//             if (length <= 0)
//                 return;

//             var state = SortState.Acquire(MaxDegreeOfParallelism);

//             try
//             {
//                 // Initial partition
//                 Interlocked.Increment(ref state.OpLeft);
//                 state.Partitions.Enqueue(new SortRange(index, length - 1));

//                 // Sort recursively
//                 state.AddReference();
//                 SortOnThread(collection, comparer, state);

//                 // Wait for all work to finish
//                 state.WaitCompletion();
//             }
//             finally
//             {
//                 state.Release();
//             }
//         }

//         private static void SortOnThread<T>(T[] collection, IComparer<T> comparer, [NotNull] SortState state)
//         {
//             const int sequentialThreshold = 2048;

//             var hasChild = false;
//             try
//             {
//                 var sw = new SpinWait();
//                 while (Volatile.Read(ref state.OpLeft) != 0)
//                 {
//                     if (state.Partitions.TryDequeue(out var range) == false)
//                     {
//                         sw.SpinOnce();
//                         continue;
//                     }

//                     if (range.Right - range.Left < sequentialThreshold)
//                     {
//                         // Sort small collections sequentially
//                         Array.Sort(collection, range.Left, range.Right - range.Left + 1, comparer);
//                         Interlocked.Decrement(ref state.OpLeft);
//                     }
//                     else
//                     {
//                         var pivot = Partition(collection, range.Left, range.Right, comparer);
                        
//                         int delta = -1;
//                         // Add work items
//                         if (pivot - 1 > range.Left)
//                             delta++;

//                         if (range.Right > pivot + 1)
//                             delta++;
                        
//                         Interlocked.Add(ref state.OpLeft, delta);
                        
//                         if (pivot - 1 > range.Left)
//                             state.Partitions.Enqueue(new SortRange(range.Left, pivot - 1));
                        

//                         if (range.Right > pivot + 1)
//                             state.Partitions.Enqueue(new SortRange(pivot + 1, range.Right));
                        

//                         // Add a new worker if necessary
//                         if (!hasChild)
//                         {
//                             var w = Interlocked.Decrement(ref state.MaxWorkerCount);
//                             if (w >= 0)
//                             {
//                                 state.AddReference();
//                                 ThreadPool.Instance.QueueWorkItem(() => SortOnThread(collection, comparer, state));
//                             }
//                             hasChild = true;
//                         }
//                     }
//                 }
//             }
//             finally
//             {
//                 if(Volatile.Read(ref state.OpLeft) == 0)
//                     state.Finished.Set();
//                 state.Release();
//             }
//         }

//         private static int Partition<T>([NotNull] T[] collection, int left, int right, [NotNull] IComparer<T> comparer)
//         {
//             int i = left, j = right;
//             var mid = (left + right) / 2;

//             if (comparer.Compare(collection[right], collection[left]) < 0)
//                 Swap(collection, left, right);
//             if (comparer.Compare(collection[mid], collection[left]) < 0)
//                 Swap(collection, left, mid);
//             if (comparer.Compare(collection[right], collection[mid]) < 0)
//                 Swap(collection, mid, right);

//             while (i <= j)
//             {
//                 var pivot = collection[mid];

//                 while (comparer.Compare(collection[i], pivot) < 0)
//                 {
//                     i++;
//                 }

//                 while (comparer.Compare(collection[j], pivot) > 0)
//                 {
//                     j--;
//                 }

//                 if (i <= j)
//                 {
//                     Swap(collection, i++, j--);
//                 }
//             }

//             return mid;
//         }

//         [MethodImpl(MethodImplOptions.AggressiveInlining)]
//         private static void Swap<T>([NotNull] T[] collection, int i, int j)
//         {
//             var temp = collection[i];
//             collection[i] = collection[j];
//             collection[j] = temp;
//         }

//         private class BatchState
//         {
//             private static readonly ConcurrentPool<BatchState> Pool = new ConcurrentPool<BatchState>(() => new BatchState());

//             private int referenceCount;

//             public readonly ManualResetEvent Finished = new ManualResetEvent(false);

//             public int StartInclusive;

//             public int WorkDone;

//             public Exception ExceptionThrown;

//             [NotNull]
//             public static BatchState Acquire()
//             {
//                 var state = Pool.Acquire();
//                 state.referenceCount = 1;
//                 state.StartInclusive = 0;
//                 state.WorkDone = 0;
//                 state.ExceptionThrown = null;
//                 state.Finished.Reset();
//                 return state;
//             }

//             public void AddReference()
//             {
//                 Interlocked.Increment(ref referenceCount);
//             }

//             public void Release()
//             {
//                 if (Interlocked.Decrement(ref referenceCount) == 0)
//                 {
//                     Pool.Release(this);
//                 }
//             }
            
//             public void WaitCompletion(int end)
//             {
//                 // Might as well steal some work instead of just waiting,
//                 // also helps prevent potential deadlocks from badly threaded code
//                 while(WorkDone < end && Finished.WaitOne(0) == false)
//                     ThreadPool.Instance.TryCooperate();
//             }
//         }

//         private struct SortRange
//         {
//             public readonly int Left;

//             public readonly int Right;

//             public SortRange(int left, int right)
//             {
//                 Left = left;
//                 Right = right;
//             }
//         }

//         private class SortState
//         {
//             private static readonly ConcurrentPool<SortState> Pool = new ConcurrentPool<SortState>(() => new SortState());

//             private int referenceCount;

//             public readonly ManualResetEvent Finished = new ManualResetEvent(false);

//             public readonly ConcurrentQueue<SortRange> Partitions = new ConcurrentQueue<SortRange>();

//             public int MaxWorkerCount;

//             public int OpLeft;

//             [NotNull]
//             public static SortState Acquire(int MaxWorkerCount)
//             {
//                 var state = Pool.Acquire();
//                 state.referenceCount = 1;
//                 state.OpLeft = 0;
//                 state.MaxWorkerCount = MaxWorkerCount;
//                 state.Finished.Reset();
//                 return state;
//             }

//             public void AddReference()
//             {
//                 Interlocked.Increment(ref referenceCount);
//             }

//             public void Release()
//             {
//                 if (Interlocked.Decrement(ref referenceCount) == 0)
//                 {
//                     Pool.Release(this);
//                 }
//             }

//             public void WaitCompletion()
//             {
//                 // Might as well steal some work instead of just waiting,
//                 // also helps prevent potential deadlocks from badly threaded code
//                 while(Volatile.Read(ref OpLeft) != 0 && Finished.WaitOne(0) == false)
//                     ThreadPool.Instance.TryCooperate();
//             }
//         }

//         private class DispatcherNode
//         {
//             public MethodBase Caller;
//             public int Count;
//             public TimeSpan TotalTime;
//         }
// #if PROFILING_SCOPES
//         private static ConcurrentDictionary<MethodInfo, DispatcherNode> nodes = new ConcurrentDictionary<MethodInfo, DispatcherNode>();
// #endif
//         private struct ProfilingScope : IDisposable
//         {
// #if PROFILING_SCOPES
//             public Stopwatch Stopwatch;
//             public Delegate Action;
// #endif
//             public void Dispose()
//             {
// #if PROFILING_SCOPES
//                 Stopwatch.Stop();
//                 var elapsed = Stopwatch.Elapsed;

//                 DispatcherNode node;
//                 if (!nodes.TryGetValue(Action.Method, out node))
//                 {
//                     int skipFrames = 1;
//                     MethodBase caller = null;

//                     do
//                     {
//                         caller = new StackFrame(skipFrames++, true).GetMethod();
//                     }
//                     while (caller.DeclaringType == typeof(Dispatcher));
                    
//                     node = nodes.GetOrAdd(Action.Method, key => new DispatcherNode());
//                     node.Caller = caller;
//                 }

//                 node.Count++;
//                 node.TotalTime += elapsed;

//                 if (node.Count % 500 == 0)
//                 {
//                     Console.WriteLine($"[{node.Count}] {node.Caller.DeclaringType.Name}.{node.Caller.Name}: {node.TotalTime.TotalMilliseconds / node.Count}");
//                 }
// #endif
//             }
//         }

//         private static ProfilingScope Profile(Delegate action)
//         {
//             var result = new ProfilingScope();
// #if PROFILING_SCOPES
//             result.Action = action;
//             result.Stopwatch = new Stopwatch();
//             result.Stopwatch.Start();
// #endif
//             return result;
//         }
//     }