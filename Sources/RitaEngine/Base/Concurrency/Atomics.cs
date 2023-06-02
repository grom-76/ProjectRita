



namespace RitaEngine.Base.Concurrency;
    
using System.Threading;
//https://github.com/StirlingLabs/Utilities.Net/blob/main/StirlingLabs.Utilities.Extensions/AtomicExtensions.cs

[SkipLocalsInit, StructLayout(LayoutKind.Sequential )]
public static class Atomics
{
    [SkipLocalsInit, StructLayout(LayoutKind.Sequential )]
    internal static class MemoryStats
    {
        private static int _allocations;

        internal static int Allocations => _allocations;

        internal static void Allocated()=>	Interlocked.Increment(ref _allocations);

        internal static void Freed() => Interlocked.Decrement(ref _allocations);
    }
}

// /*

// https://github.com/Egodystonic/Atomics cree le 19/05/2023 GROM76
// date github + 4an 

// Atomics is a C#/.NET Standard 2.0 library aimed at providing thread-safe wrappers for mutable shared state variables.

// Please note: This library is currently in alpha. There is no inline documentation yet and the API is liable to minor changes.

// ## Library Features

// * Provides built-in mechanisms for operating on mutable variables in discrete, atomic operations; including 'basics' like compare-and-swap/increment etc. but also more complex or arbitrary routines, making it easier to reason about potential concurrent accesses and eliminate accidental race conditions.
// * Helps ensure that all accesses to wrapped variables are done in a threadsafe manner. Unlike with other synchronization primitives, mutable variables wrapped in an `Atomic<T>` wrapper are much harder to accidentally alter in an unsafe way.
// * Almost all operations are "lock-free", resulting in high-scalability for almost any contention level and any number of threads; with a suite of [benchmarks](https://github.com/Egodystonic/Atomics/tree/master/Egodystonic.Atomics.Benchmarks) used to measure performance and guide implementation.
// * A full suite of [unit tests](https://github.com/Egodystonic/Atomics/tree/master/Egodystonic.Atomics.Tests) with a custom-built harness for testing the entire library with multiple concurrent threads.
// * Support for custom equality (e.g. compare-and-swap with two `IEquatable<>` objects will use their `Equals()` function for comparison).
// * Library is targeted to .NET Standard 2.0 which is [supported by most modern .NET platforms](https://github.com/dotnet/standard/blob/master/docs/versions.md).
// * MIT licensed.


// ## Library Advantages

// Although the best design for threadsafe code is to have no mutable state at all, sometimes it is a necessity for performance or reasons of complexity.

// As [growth in single-core computing power is slowing](https://www.technologyreview.com/s/601441/moores-law-is-dead-now-what/), scaling via parallelism with higher CPU core counts is becoming an increasingly important way to increase application responsiveness. The latest generation of desktop CPUs have more threads than ever before, with the flagship specs currently being Intel's i9 9900k (16 threads) and AMD's Threadripper 2950X (32 threads).

// Currently the .NET FCL/BCL offers a great suite of tools for writing parellized/concurrent code with `async/await`, concurrent & immutable collections, `Task`s and the TPL, and more fundamental constructs like locks, semaphores, reader-writer locks, and more. 

// However, one potentially missing element is the provision for threadsafe single variables, often used as part of a larger concurrent algorithm or data structure. Many other languages provide a similar library, including [C++](https://en.cppreference.com/w/cpp/atomic/atomic), [Java](https://docs.oracle.com/javase/tutorial/essential/concurrency/atomicvars.html), [Rust](https://doc.rust-lang.org/nomicon/atomics.html), and [Go](https://golang.org/pkg/sync/atomic/).


// ## Installation

// Simply install `Egodystonic.Atomics` via NuGet.

// # Examples

// Currently the library is in alpha and has no inline documentation. However, the following examples demonstrate common use-cases.

// ## Atomic Types

// * `AtomicRef<T>`: Represents an atomic reference (class instance).
// * `AtomicVal<T>`: Represents an atomic value (struct instance).
// * `AtomicInt`: Represents an atomic 32-bit signed integer value.
// * `AtomicLong`: Represents an atomic 64-bit signed integer value.
// * `AtomicFloat`: Represents an atomic 32-bit floating-point value.
// * `AtomicDouble`: Represents an atomic 64-bit floating-point value.
// * `CopyOnReadRef<T>`: Represents an atomic reference (class instance) where the current value is always copied before being returned from any operation.
// * `AtomicDelegate<T>`: Represents an atomic delegate value (e.g. `Action<>`, `Func<>`, or any custom delegate type).
// * `AtomicValUnmanaged<T>`: Faster alternative to `AtomicVal<T>` for [unmanaged](https://blogs.msdn.microsoft.com/seteplia/2018/06/12/dissecting-new-generics-constraints-in-c-7-3/) value types. `sizeof(T)` must be <= `sizeof(long)`.
// * `AtomicPtr<T>`: Represents an atomic pointer to type T.
// * `AtomicBool<T>`: Represents an atomic boolean value (true/false).
// * `AtomicEnumVal<T>`: Represents an atomic enum value.

// ## Common Operations

// The following operations are supported on all `Atomic` types.

// ### Get()/Set()/Value

// Atomically get or set the value on the atomic object.
	
// 	var currentValue = _atomic.Get(); // Atomically get the current value.
// 	_atomic.Set(newValue); // Atomically set a new value.
	
// 	var currentValue = _atomic.Value; // Atomically get the current value.
// 	_atomic.Value = newValue; // Atomically set a new value.
	
// ### Exchange()

// Atomically set a new value and return the previous one.

// 	var exchangeResult = _atomic.Exchange(newValue); // Set a new value and return the value that was previously set as a single atomic operation.
	
// 	var exchangeResult = _atomic.Exchange(v => v.Frobnicate()); // Set a new value via a map function that uses the current value, and return that value, as an atomic operation.
	
// * `exchangeResult.PreviousValue` returns the value that was set before the exchange operation completed.
// * `exchangeResult.CurrentValue` returns the value that is now currently set (after the exchange operation completed).
	
// ### TryExchange()

// Atomically set a new value and return the previous one, depending on the current value.

// 	var tryExchangeResult = _atomic.TryExchange(newValue, expectedValue); // Set a new value if and only if the current value is equal to "expectedValue". Returns the previous and current values (i.e. the values before/after the operation executes) and whether or not the exchange actually occurred.
	
// 	var tryExchangeResult = _atomic.TryExchange(v => v.Frobnicate(), expectedValue); // Set a new value via a map function that uses the current value, if and only if the current value is equal to "expectedValue". Returns the previous and current values (i.e. the values before/after the operation executes) and whether or not the exchange actually occurred.
	
// 	var tryExchangeResult = _atomic.TryExchange(newValue, (vCurrent, vNext) => vNext.SomeProperty > vCurrent.SomeProperty); // Set a new value if and only if the given predicate function returns true. The predicate function takes the currently set value and the new value as inputs. Returns the previous and current values (i.e. the values before/after the operation executes) and whether or not the exchange actually occurred.
	
// 	var tryExchangeResult = _atomic.TryExchange(v => v.Frobnicate(), (vCurrent, vNext) => vNext.SomeProperty > vCurrent.SomeProperty); // Set a new value via a map function that uses the current value, if and only if the given predicate function returns true. The predicate function takes the currently set value and the potential new value (calculated via the map function) as inputs. Returns the previous and current values (i.e. the values before/after the operation executes) and whether or not the exchange actually occurred.
	
// * `tryExchangeResult.ValueWasSet` returns whether or not the exchange actually occurred.
// * `tryExchangeResult.PreviousValue` returns the value that was set before the exchange operation was attempted.
// * `tryExchangeResult.CurrentValue` returns the value that is currently set, after the exchange operation ended. If `ValueWasSet` is `false`, this is the same as `PreviousValue`. If `ValueWasSet` is `true`, this is the new value that was passed to the method call (or created via the map func).

	
// ## Numeric Operations

// The following operations are supported on all numeric types (e.g. `AtomicInt`, `AtomicLong`, `AtomicFloat`, `AtomicDouble`).

// ### Increment()/Decrement()

// Add or remove 1 from the current value.

// 	var incResult = _atomic.Increment(); // Atomically raise the value by 1, and return the current and previous value.
// 	var decResult = _atomic.Decrement(); // Atomically lower the value by 1, and return the current and previous value.
	
// * `incResult.PreviousValue`/`decResult.PreviousValue` is the value that was set before the Increment/Decrement call.
// * `incResult.CurrentValue`/`decResult.CurrentValue` is the value that is now set after the Increment/Decrement call.

// ### Add()/Subtract()/MultiplyBy()/DivideBy()

// Add to, subtract from, multiply, or divide the current value by a given operand.

// 	var addResult = _atomic.Add(n); // Atomically add n, and return the current and previous value.
// 	var subResult = _atomic.Sub(n); // Atomically subtract n, and return the current and previous value.
// 	var mulResult = _atomic.MultiplyBy(n); // Atomically multiply by n, and return the current and previous value.
// 	var divResult = _atomic.DivideBy(n); // Atomically divide by n, and return the current and previous value.
	
// * `addResult.PreviousValue` (etc.) is the value that was set before the arithmetic operation.
// * `addResult.CurrentValue` (etc.) is the value that is now set after the arithmetic operation.

// ### TryMinimumExchange()/TryMaximumExchange()

// Atomically set a new value and return the previous one, depending on the current value.

// 	var tryMinExchangeResult = _atomic.TryMinimumExchange(newValue, minValue); // Set a new value if and only if the current value is greater than or equal to "minValue". Returns the previous and current values (i.e. the values before/after the operation executes) and whether or not the exchange actually occurred.
	
// 	var tryMaxExchangeResult = _atomic.TryMaximumExchange(newValue, maxValue); // Set a new value if and only if the current value is less than or equal to "maxValue". Returns the previous and current values (i.e. the values before/after the operation executes) and whether or not the exchange actually occurred.

// * `tryMinExchangeResult.ValueWasSet` (etc.) returns whether or not the exchange actually occurred.
// * `tryMinExchangeResult.PreviousValue` (etc.) returns the value that was set before the exchange operation was attempted.
// * `tryMinExchangeResult.CurrentValue` (etc.) returns the value that is currently set, after the exchange operation ended. If `ValueWasSet` is `false`, this is the same as `PreviousValue`. If `ValueWasSet` is `true`, this is the new value that was passed to the method call (or created via the map func).

// ### TryBoundedExchange()

// Atomically set a new value and return the previous one, depending on the current value.

// 	var tryBoundedExchangeResult = _atomic.TryBoundedExchange(newValue, lowerBound, upperBound); // Set a new value if and only if lowerBound <= the current value < upperBound. Returns the previous and current values (i.e. the values before/after the operation executes) and whether or not the exchange actually occurred.
	
// * `tryBoundedExchangeResult.ValueWasSet` returns whether or not the exchange actually occurred.
// * `tryBoundedExchangeResult.PreviousValue` returns the value that was set before the exchange operation was attempted.
// * `tryBoundedExchangeResult.CurrentValue` returns the value that is currently set, after the exchange operation ended. If `ValueWasSet` is `false`, this is the same as `PreviousValue`. If `ValueWasSet` is `true`, this is the new value that was passed to the method call (or created via the map func).

// ### TryExchangeWithMaxDelta() (`AtomicFloat`/`AtomicDouble` only)

// Atomically set a new value and return the previous one, depending on the current value.

// 	var tryExchangeMaxDeltaResult = _atomic.TryExchangeWithMaxDelta(newValue, comparand, maxDelta); // Set a new value if and only if the current value is equal to the given comparand, +/- the given maxDelta. Returns the previous and current values (i.e. the values before/after the operation executes) and whether or not the exchange actually occurred.
	
// * `tryExchangeMaxDeltaResult.ValueWasSet` returns whether or not the exchange actually occurred.
// * `tryExchangeMaxDeltaResult.PreviousValue` returns the value that was set before the exchange operation was attempted.
// * `tryExchangeMaxDeltaResult.CurrentValue` returns the value that is currently set, after the exchange operation ended. If `ValueWasSet` is `false`, this is the same as `PreviousValue`. If `ValueWasSet` is `true`, this is the new value that was passed to the method call (or created via the map func).

// ## Additional Operations

// ### `AtomicDelegate<T>.Combine()`/`AtomicDelegate<T>.Remove()`/`AtomicDelegate<T>.RemoveAll()`

// Atomically [Combine](https://docs.microsoft.com/en-us/dotnet/api/system.delegate.combine?view=netframework-4.7.2), [Remove](https://docs.microsoft.com/en-us/dotnet/api/system.delegate.remove?view=netframework-4.7.2), or [RemoveAll](https://docs.microsoft.com/en-us/dotnet/api/system.delegate.removeall?view=netframework-4.7.2) the currently set delegate value.

// ### `AtomicDelegate<T>.TryDynamicInvoke()`

// Invoke the delegate value with the given args (via [DynamicInvoke](https://docs.microsoft.com/en-us/dotnet/api/system.delegate.dynamicinvoke?view=netframework-4.7.2)) if it's not null. Returns a tuple containing whether an invocation was made, and if so the result of that invocation.

// ### `AtomicDelegate<T>.TryInvoke()`

// Only supported when `T` is a `Func<>` type or `Action<>` type. Directly invoke the delegate value with the given args if it's not null.

// * When `T` is a `Func<>` deriviative, returns a tuple containing whether an invocation was made, and if so the result of that invocation.
// * When `T` is an `Action<>` deriviative, returns a boolean indicating whether or not an invocation was made.

// ### `AtomicDelegate<T>.TryWrappedInvoke()`

// Provide a function/lambda to directly (i.e. not dynamically) invoke the delegate value if it is not null. Provided for when dynamic invoke is too slow. Return type/value is the same as `TryDynamicInvoke()`.

// ### `AtomicBool.Negate()`

// Negate the current value of the atomic boolean. Returns the previous value and the new current value.

// ### `CopyOnReadRef<T>.GetWithoutCopy()`

// Return the currently set value without making a copy of it.

// ## Advanced Operations

// The following operations are provided for advanced usage scenarios. Most everyday use of the atomic types won't require these functions.

// ### `Fast...()`

// There are various `Fast...()` versions of methods (e.g. `FastExchange()`) that return less verbose data and in some cases allow circumventing custom equality checks when reference-equality is acceptable, etc. These functions are only necessary in extreme cases, most users are recommended to use the standard versions.

// ### `Exchange()`/`TryExchange()` variants that consume context objects

// These methods can take optional generic context objects to be used in the corresponding map/predicate functions; making it easier to pass context objects in to those functions without the implicit generation of GC pressure/garbage that comes from closure capture in lambdas. For most users, simply providing contextual arguments to map/predicate lambdas as closed-over variables is recommended, as it's simpler and not much slower.

// ### `GetUnsafe()`/`SetUnsafe()`

// These methods are identical to `Get()`/`Set()` but elide any fence instructions or atomic/Interlocked operations; instead just directly reading/writing the internal value in a non-threadsafe way. Useful only for extremely-high-performance algorithm authors who understand the implications.

// ### `SpinWaitForValue()`

// Forces the calling thread to busy-spin in an extremely tight loop waiting for the given value to be set (or predicate to be fulfilled). This is not an alternative for proper cross-thread synchronization, and is intended to be used by lock-free algorithm writers only. Internally uses a [SpinWait](https://docs.microsoft.com/en-us/dotnet/api/system.threading.spinwait?view=netframework-4.7.2) object to ensure correct busy-spin behaviour on any target architecture.

// ### `SpinWaitForExchange()`

// Similar to `SpinWaitForValue()`, but additionally sets a new value once the target value/predicate has been met.

// # Threading Model

// The following paragraph details the threading guarantees made by this library; and is useful for experts wishing to write lock-free algorithms or data structures.

// * All reads (except those marked as `Unsafe` such as `GetUnsafe()`) emit acquire fences or full fences. This includes spin-wait operations such as `SpinWaitForValue()`.
// * All writes (including compound read-writes such as `Exchange` operations) emit release fences or full fences (except those marked as `Unsafe` such as `SetUnsafe()`).
// * The library does not make any assumptions about need for value stability or 'freshness'. If you require a 'fresh' read (as opposed to a volatile read, which is what this library provides), you are expected to emit the relevant fences yourself or use the `GetUnsafe()` methods (which are inlined) in a spinloop to wait for a stable value.
// * For target variables whose size exceeds the native word size (e.g. structs larger than 4 or 8 bytes); `AtomicVal<T>` currently uses a locked write model. Reads are still lock-free. 
// * One invariant guarantee in the lib is that writes can not be 'lost' (i.e. when a concurrent `Set()` and `Exchange()` operation occur, either the `Set()` value will eventually be propagated as the current value to all threads, or it will be returned as the `PreviousValue` from the `Exchange()` operation).
// * All the methods that take map or predicate functions may call those functions multiple times with different values while attempting to atomically alter the target variable.
// */

// namespace RitaEngine.Core.Base.Atomics;

// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Runtime.CompilerServices;
// using System.Threading;
// using IntBool = RitaEngine.Core.Base.Atomics.AtomicUtils.Union<int, bool>;

// #pragma warning disable 

// #region Extensions

// public static class LockingAtomicDelegateExtensions {
// 		#region Action
// 		public static bool TryInvoke(this ILockingAtomic<Action> @this) {
// 			var valueLocal = @this.Value;
// 			if (valueLocal == null) return false;
// 			valueLocal();
// 			return true;
// 		}

// 		public static bool TryInvoke<T1>(this ILockingAtomic<Action<T1>> @this, T1 arg1) {
// 			var valueLocal = @this.Value;
// 			if (valueLocal == null) return false;
// 			valueLocal(arg1);
// 			return true;
// 		}

// 		public static bool TryInvoke<T1, T2>(this ILockingAtomic<Action<T1, T2>> @this, T1 arg1, T2 arg2) {
// 			var valueLocal = @this.Value;
// 			if (valueLocal == null) return false;
// 			valueLocal(arg1, arg2);
// 			return true;
// 		}

// 		public static bool TryInvoke<T1, T2, T3>(this ILockingAtomic<Action<T1, T2, T3>> @this, T1 arg1, T2 arg2, T3 arg3) {
// 			var valueLocal = @this.Value;
// 			if (valueLocal is null) return false;
// 			valueLocal(arg1, arg2, arg3);
// 			return true;
// 		}

// 		public static bool TryInvoke<T1, T2, T3, T4>(this ILockingAtomic<Action<T1, T2, T3, T4>> @this, T1 arg1, T2 arg2, T3 arg3, T4 arg4) {
// 			var valueLocal = @this.Value;
// 			if (valueLocal is null) return false;
// 			valueLocal(arg1, arg2, arg3, arg4);
// 			return true;
// 		}

// 		public static bool TryInvoke<T1, T2, T3, T4, T5>(this ILockingAtomic<Action<T1, T2, T3, T4, T5>> @this, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) {
// 			var valueLocal = @this.Value;
// 			if (valueLocal is null) return false;
// 			valueLocal(arg1, arg2, arg3, arg4, arg5);
// 			return true;
// 		}

// 		public static bool TryInvoke<T1, T2, T3, T4, T5, T6>(this ILockingAtomic<Action<T1, T2, T3, T4, T5, T6>> @this, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) {
// 			var valueLocal = @this.Value;
// 			if (valueLocal is null) return false;
// 			valueLocal(arg1, arg2, arg3, arg4, arg5, arg6);
// 			return true;
// 		}

// 		public static bool TryInvoke<T1, T2, T3, T4, T5, T6, T7>(this ILockingAtomic<Action<T1, T2, T3, T4, T5, T6, T7>> @this, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) {
// 			var valueLocal = @this.Value;
// 			if (valueLocal is null) return false;
// 			valueLocal(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
// 			return true;
// 		}

// 		public static bool TryInvoke<T1, T2, T3, T4, T5, T6, T7, T8>(this ILockingAtomic<Action<T1, T2, T3, T4, T5, T6, T7, T8>> @this, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8) {
// 			var valueLocal = @this.Value;
// 			if (valueLocal is null) return false;
// 			valueLocal(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
// 			return true;
// 		}

// 		public static bool TryInvoke<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this ILockingAtomic<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>> @this, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9) {
// 			var valueLocal = @this.Value;
// 			if (valueLocal is null) return false;
// 			valueLocal(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
// 			return true;
// 		}

// 		public static bool TryInvoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this ILockingAtomic<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>> @this, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10) {
// 			var valueLocal = @this.Value;
// 			if (valueLocal is null) return false;
// 			valueLocal(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
// 			return true;
// 		}

// 		public static bool TryInvoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this ILockingAtomic<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>> @this, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11) {
// 			var valueLocal = @this.Value;
// 			if (valueLocal is null) return false;
// 			valueLocal(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
// 			return true;
// 		}

// 		public static bool TryInvoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this ILockingAtomic<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>> @this, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12) {
// 			var valueLocal = @this.Value;
// 			if (valueLocal is null) return false;
// 			valueLocal(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
// 			return true;
// 		}

// 		public static bool TryInvoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this ILockingAtomic<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>> @this, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13) {
// 			var valueLocal = @this.Value;
// 			if (valueLocal is null) return false;
// 			valueLocal(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
// 			return true;
// 		}

// 		public static bool TryInvoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this ILockingAtomic<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>> @this, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14) {
// 			var valueLocal = @this.Value;
// 			if (valueLocal is null) return false;
// 			valueLocal(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
// 			return true;
// 		}

// 		public static bool TryInvoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this ILockingAtomic<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>> @this, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15) {
// 			var valueLocal = @this.Value;
// 			if (valueLocal is null) return false;
// 			valueLocal(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
// 			return true;
// 		}

// 		public static bool TryInvoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this ILockingAtomic<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>> @this, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16) {
// 			var valueLocal = @this.Value;
// 			if (valueLocal is null) return false;
// 			valueLocal(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
// 			return true;
// 		}
// 		#endregion

// 		#region Func
// 		public static bool TryInvoke<TOut>(this ILockingAtomic<Func<TOut>> @this, out TOut returnValue) {
// 			var valueLocal = @this.Value;
// 			if (valueLocal is null) {
// #pragma warning disable CS8653 // "A default expression introduces a null value for a type parameter." -- That's fine here, we just can't mark TOut as nullable
// 				returnValue = default;
// #pragma warning restore CS8653
// 				return false;
// 			}
// 			returnValue = valueLocal();
// 			return true;
// 		}

// 		public static bool TryInvoke<T1, T2, TOut>(this ILockingAtomic<Func<T1, T2, TOut>> @this, T1 arg1, T2 arg2, out TOut returnValue) {
// 			var valueLocal = @this.Value;
// 			if (valueLocal is null) {
// #pragma warning disable CS8653 // "A default expression introduces a null value for a type parameter." -- That's fine here, we just can't mark TOut as nullable
// 				returnValue = default;
// #pragma warning restore CS8653
// 				return false;
// 			}
// 			returnValue = valueLocal(arg1, arg2);
// 			return true;
// 		}

// 		public static bool TryInvoke<T1, T2, T3, TOut>(this ILockingAtomic<Func<T1, T2, T3, TOut>> @this, T1 arg1, T2 arg2, T3 arg3, out TOut returnValue) {
// 			var valueLocal = @this.Value;
// 			if (valueLocal is null) {
// #pragma warning disable CS8653 // "A default expression introduces a null value for a type parameter." -- That's fine here, we just can't mark TOut as nullable
// 				returnValue = default;
// #pragma warning restore CS8653
// 				return false;
// 			}
// 			returnValue = valueLocal(arg1, arg2, arg3);
// 			return true;
// 		}

// 		public static bool TryInvoke<T1, T2, T3, T4, TOut>(this ILockingAtomic<Func<T1, T2, T3, T4, TOut>> @this, T1 arg1, T2 arg2, T3 arg3, T4 arg4, out TOut returnValue) {
// 			var valueLocal = @this.Value;
// 			if (valueLocal is null) {
// #pragma warning disable CS8653 // "A default expression introduces a null value for a type parameter." -- That's fine here, we just can't mark TOut as nullable
// 				returnValue = default;
// #pragma warning restore CS8653
// 				return false;
// 			}
// 			returnValue = valueLocal(arg1, arg2, arg3, arg4);
// 			return true;
// 		}

// 		public static bool TryInvoke<T1, T2, T3, T4, T5, TOut>(this ILockingAtomic<Func<T1, T2, T3, T4, T5, TOut>> @this, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, out TOut returnValue) {
// 			var valueLocal = @this.Value;
// 			if (valueLocal is null) {
// #pragma warning disable CS8653 // "A default expression introduces a null value for a type parameter." -- That's fine here, we just can't mark TOut as nullable
// 				returnValue = default;
// #pragma warning restore CS8653
// 				return false;
// 			}
// 			returnValue = valueLocal(arg1, arg2, arg3, arg4, arg5);
// 			return true;
// 		}

// 		public static bool TryInvoke<T1, T2, T3, T4, T5, T6, TOut>(this ILockingAtomic<Func<T1, T2, T3, T4, T5, T6, TOut>> @this, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, out TOut returnValue) {
// 			var valueLocal = @this.Value;
// 			if (valueLocal is null) {
// #pragma warning disable CS8653 // "A default expression introduces a null value for a type parameter." -- That's fine here, we just can't mark TOut as nullable
// 				returnValue = default;
// #pragma warning restore CS8653
// 				return false;
// 			}
// 			returnValue = valueLocal(arg1, arg2, arg3, arg4, arg5, arg6);
// 			return true;
// 		}

// 		public static bool TryInvoke<T1, T2, T3, T4, T5, T6, T7, TOut>(this ILockingAtomic<Func<T1, T2, T3, T4, T5, T6, T7, TOut>> @this, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, out TOut returnValue) {
// 			var valueLocal = @this.Value;
// 			if (valueLocal is null) {
// #pragma warning disable CS8653 // "A default expression introduces a null value for a type parameter." -- That's fine here, we just can't mark TOut as nullable
// 				returnValue = default;
// #pragma warning restore CS8653
// 				return false;
// 			}
// 			returnValue = valueLocal(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
// 			return true;
// 		}

// 		public static bool TryInvoke<T1, T2, T3, T4, T5, T6, T7, T8, TOut>(this ILockingAtomic<Func<T1, T2, T3, T4, T5, T6, T7, T8, TOut>> @this, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, out TOut returnValue) {
// 			var valueLocal = @this.Value;
// 			if (valueLocal is null) {
// #pragma warning disable CS8653 // "A default expression introduces a null value for a type parameter." -- That's fine here, we just can't mark TOut as nullable
// 				returnValue = default;
// #pragma warning restore CS8653
// 				return false;
// 			}
// 			returnValue = valueLocal(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
// 			return true;
// 		}

// 		public static bool TryInvoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOut>(this ILockingAtomic<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOut>> @this, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, out TOut returnValue) {
// 			var valueLocal = @this.Value;
// 			if (valueLocal is null) {
// #pragma warning disable CS8653 // "A default expression introduces a null value for a type parameter." -- That's fine here, we just can't mark TOut as nullable
// 				returnValue = default;
// #pragma warning restore CS8653
// 				return false;
// 			}
// 			returnValue = valueLocal(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
// 			return true;
// 		}

// 		public static bool TryInvoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOut>(this ILockingAtomic<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOut>> @this, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, out TOut returnValue) {
// 			var valueLocal = @this.Value;
// 			if (valueLocal is null) {
// #pragma warning disable CS8653 // "A default expression introduces a null value for a type parameter." -- That's fine here, we just can't mark TOut as nullable
// 				returnValue = default;
// #pragma warning restore CS8653
// 				return false;
// 			}
// 			returnValue = valueLocal(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
// 			return true;
// 		}

// 		public static bool TryInvoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOut>(this ILockingAtomic<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOut>> @this, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, out TOut returnValue) {
// 			var valueLocal = @this.Value;
// 			if (valueLocal is null) {
// #pragma warning disable CS8653 // "A default expression introduces a null value for a type parameter." -- That's fine here, we just can't mark TOut as nullable
// 				returnValue = default;
// #pragma warning restore CS8653
// 				return false;
// 			}
// 			returnValue = valueLocal(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
// 			return true;
// 		}

// 		public static bool TryInvoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOut>(this ILockingAtomic<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOut>> @this, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, out TOut returnValue) {
// 			var valueLocal = @this.Value;
// 			if (valueLocal is null) {
// #pragma warning disable CS8653 // "A default expression introduces a null value for a type parameter." -- That's fine here, we just can't mark TOut as nullable
// 				returnValue = default;
// #pragma warning restore CS8653
// 				return false;
// 			}
// 			returnValue = valueLocal(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
// 			return true;
// 		}

// 		public static bool TryInvoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOut>(this ILockingAtomic<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOut>> @this, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, out TOut returnValue) {
// 			var valueLocal = @this.Value;
// 			if (valueLocal is null) {
// #pragma warning disable CS8653 // "A default expression introduces a null value for a type parameter." -- That's fine here, we just can't mark TOut as nullable
// 				returnValue = default;
// #pragma warning restore CS8653
// 				return false;
// 			}
// 			returnValue = valueLocal(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
// 			return true;
// 		}

// 		public static bool TryInvoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOut>(this ILockingAtomic<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOut>> @this, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, out TOut returnValue) {
// 			var valueLocal = @this.Value;
// 			if (valueLocal is null) {
// #pragma warning disable CS8653 // "A default expression introduces a null value for a type parameter." -- That's fine here, we just can't mark TOut as nullable
// 				returnValue = default;
// #pragma warning restore CS8653
// 				return false;
// 			}
// 			returnValue = valueLocal(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
// 			return true;
// 		}

// 		public static bool TryInvoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOut>(this ILockingAtomic<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOut>> @this, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, out TOut returnValue) {
// 			var valueLocal = @this.Value;
// 			if (valueLocal is null) {
// #pragma warning disable CS8653 // "A default expression introduces a null value for a type parameter." -- That's fine here, we just can't mark TOut as nullable
// 				returnValue = default;
// #pragma warning restore CS8653
// 				return false;
// 			}
// 			returnValue = valueLocal(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
// 			return true;
// 		}

// 		public static bool TryInvoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOut>(this ILockingAtomic<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOut>> @this, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, out TOut returnValue) {
// 			var valueLocal = @this.Value;
// 			if (valueLocal is null) {
// #pragma warning disable CS8653 // "A default expression introduces a null value for a type parameter." -- That's fine here, we just can't mark TOut as nullable
// 				returnValue = default;
// #pragma warning restore CS8653
// 				return false;
// 			}
// 			returnValue = valueLocal(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
// 			return true;
// 		}
// 		#endregion

// 		public static bool TryDynamicInvoke<T>(this ILockingAtomic<T> @this, object?[] args, out object? returnValue) where T : MulticastDelegate {
// 			var valueLocal = @this.Value;
// 			if (valueLocal == null) {
// 				returnValue = default;
// 				return false;
// 			}
// 			returnValue = valueLocal.DynamicInvoke(args);
// 			return true;
// 		}

// 		public static void Combine<T>(this ILockingAtomic<T> @this, T delegateToCombine) where T : MulticastDelegate {
// 			@this.Set(v => (T) Delegate.Combine(v, delegateToCombine));
// 		}

// 		public static void Remove<T>(this ILockingAtomic<T> @this, T delegateToRemove) where T : MulticastDelegate {
// 			@this.Set(v => (T) Delegate.Remove(v, delegateToRemove));
// 		}

// 		public static void RemoveAll<T>(this ILockingAtomic<T> @this, T delegateToRemove) where T : MulticastDelegate {
// 			@this.Set(v => (T) Delegate.RemoveAll(v, delegateToRemove));
// 		}
// 	}

// public static class LockingAtomicNumberExtensions {
// 		#region SByte
// 		public static SByte Increment(this ILockingAtomic<SByte> @this) => @this.Set(i => ++i);
// 		public static SByte Increment(this ILockingAtomic<SByte> @this, out SByte previousValue) => @this.Set(i => ++i, out previousValue);

// 		public static SByte Decrement(this ILockingAtomic<SByte> @this) => @this.Set(i => --i);
// 		public static SByte Decrement(this ILockingAtomic<SByte> @this, out SByte previousValue) => @this.Set(i => --i, out previousValue);

// 		public static SByte Add(this ILockingAtomic<SByte> @this, SByte operand) => @this.Set(i => (SByte) (i + operand));
// 		public static SByte Add(this ILockingAtomic<SByte> @this, SByte operand, out SByte previousValue) => @this.Set(i => (SByte) (i + operand), out previousValue);

// 		public static SByte Subtract(this ILockingAtomic<SByte> @this, SByte operand) => @this.Set(i => (SByte) (i - operand));
// 		public static SByte Subtract(this ILockingAtomic<SByte> @this, SByte operand, out SByte previousValue) => @this.Set(i => (SByte) (i - operand), out previousValue);

// 		public static SByte Multiply(this ILockingAtomic<SByte> @this, SByte operand) => @this.Set(i => (SByte) (i * operand));
// 		public static SByte Multiply(this ILockingAtomic<SByte> @this, SByte operand, out SByte previousValue) => @this.Set(i => (SByte) (i * operand), out previousValue);

// 		public static SByte Divide(this ILockingAtomic<SByte> @this, SByte operand) => @this.Set(i => (SByte) (i / operand));
// 		public static SByte Divide(this ILockingAtomic<SByte> @this, SByte operand, out SByte previousValue) => @this.Set(i => (SByte) (i / operand), out previousValue);
// 		#endregion

// 		#region Byte
// 		public static Byte Increment(this ILockingAtomic<Byte> @this) => @this.Set(i => ++i);
// 		public static Byte Increment(this ILockingAtomic<Byte> @this, out Byte previousValue) => @this.Set(i => ++i, out previousValue);

// 		public static Byte Decrement(this ILockingAtomic<Byte> @this) => @this.Set(i => --i);
// 		public static Byte Decrement(this ILockingAtomic<Byte> @this, out Byte previousValue) => @this.Set(i => --i, out previousValue);

// 		public static Byte Add(this ILockingAtomic<Byte> @this, Byte operand) => @this.Set(i => (Byte) (i + operand));
// 		public static Byte Add(this ILockingAtomic<Byte> @this, Byte operand, out Byte previousValue) => @this.Set(i => (Byte) (i + operand), out previousValue);

// 		public static Byte Subtract(this ILockingAtomic<Byte> @this, Byte operand) => @this.Set(i => (Byte) (i - operand));
// 		public static Byte Subtract(this ILockingAtomic<Byte> @this, Byte operand, out Byte previousValue) => @this.Set(i => (Byte) (i - operand), out previousValue);

// 		public static Byte Multiply(this ILockingAtomic<Byte> @this, Byte operand) => @this.Set(i => (Byte) (i * operand));
// 		public static Byte Multiply(this ILockingAtomic<Byte> @this, Byte operand, out Byte previousValue) => @this.Set(i => (Byte) (i * operand), out previousValue);

// 		public static Byte Divide(this ILockingAtomic<Byte> @this, Byte operand) => @this.Set(i => (Byte) (i / operand));
// 		public static Byte Divide(this ILockingAtomic<Byte> @this, Byte operand, out Byte previousValue) => @this.Set(i => (Byte) (i / operand), out previousValue);
// 		#endregion

// 		#region Int16
// 		public static Int16 Increment(this ILockingAtomic<Int16> @this) => @this.Set(i => ++i);
// 		public static Int16 Increment(this ILockingAtomic<Int16> @this, out Int16 previousValue) => @this.Set(i => ++i, out previousValue);

// 		public static Int16 Decrement(this ILockingAtomic<Int16> @this) => @this.Set(i => --i);
// 		public static Int16 Decrement(this ILockingAtomic<Int16> @this, out Int16 previousValue) => @this.Set(i => --i, out previousValue);

// 		public static Int16 Add(this ILockingAtomic<Int16> @this, Int16 operand) => @this.Set(i => (Int16) (i + operand));
// 		public static Int16 Add(this ILockingAtomic<Int16> @this, Int16 operand, out Int16 previousValue) => @this.Set(i => (Int16) (i + operand), out previousValue);

// 		public static Int16 Subtract(this ILockingAtomic<Int16> @this, Int16 operand) => @this.Set(i => (Int16) (i - operand));
// 		public static Int16 Subtract(this ILockingAtomic<Int16> @this, Int16 operand, out Int16 previousValue) => @this.Set(i => (Int16) (i - operand), out previousValue);

// 		public static Int16 Multiply(this ILockingAtomic<Int16> @this, Int16 operand) => @this.Set(i => (Int16) (i * operand));
// 		public static Int16 Multiply(this ILockingAtomic<Int16> @this, Int16 operand, out Int16 previousValue) => @this.Set(i => (Int16) (i * operand), out previousValue);

// 		public static Int16 Divide(this ILockingAtomic<Int16> @this, Int16 operand) => @this.Set(i => (Int16) (i / operand));
// 		public static Int16 Divide(this ILockingAtomic<Int16> @this, Int16 operand, out Int16 previousValue) => @this.Set(i => (Int16) (i / operand), out previousValue);
// 		#endregion

// 		#region UInt16
// 		public static UInt16 Increment(this ILockingAtomic<UInt16> @this) => @this.Set(i => ++i);
// 		public static UInt16 Increment(this ILockingAtomic<UInt16> @this, out UInt16 previousValue) => @this.Set(i => ++i, out previousValue);

// 		public static UInt16 Decrement(this ILockingAtomic<UInt16> @this) => @this.Set(i => --i);
// 		public static UInt16 Decrement(this ILockingAtomic<UInt16> @this, out UInt16 previousValue) => @this.Set(i => --i, out previousValue);

// 		public static UInt16 Add(this ILockingAtomic<UInt16> @this, UInt16 operand) => @this.Set(i => (UInt16) (i + operand));
// 		public static UInt16 Add(this ILockingAtomic<UInt16> @this, UInt16 operand, out UInt16 previousValue) => @this.Set(i => (UInt16) (i + operand), out previousValue);

// 		public static UInt16 Subtract(this ILockingAtomic<UInt16> @this, UInt16 operand) => @this.Set(i => (UInt16) (i - operand));
// 		public static UInt16 Subtract(this ILockingAtomic<UInt16> @this, UInt16 operand, out UInt16 previousValue) => @this.Set(i => (UInt16) (i - operand), out previousValue);

// 		public static UInt16 Multiply(this ILockingAtomic<UInt16> @this, UInt16 operand) => @this.Set(i => (UInt16) (i * operand));
// 		public static UInt16 Multiply(this ILockingAtomic<UInt16> @this, UInt16 operand, out UInt16 previousValue) => @this.Set(i => (UInt16) (i * operand), out previousValue);

// 		public static UInt16 Divide(this ILockingAtomic<UInt16> @this, UInt16 operand) => @this.Set(i => (UInt16) (i / operand));
// 		public static UInt16 Divide(this ILockingAtomic<UInt16> @this, UInt16 operand, out UInt16 previousValue) => @this.Set(i => (UInt16) (i / operand), out previousValue);
// 		#endregion

// 		#region Int32
// 		public static Int32 Increment(this ILockingAtomic<Int32> @this) => @this.Set(i => ++i);
// 		public static Int32 Increment(this ILockingAtomic<Int32> @this, out Int32 previousValue) => @this.Set(i => ++i, out previousValue);

// 		public static Int32 Decrement(this ILockingAtomic<Int32> @this) => @this.Set(i => --i);
// 		public static Int32 Decrement(this ILockingAtomic<Int32> @this, out Int32 previousValue) => @this.Set(i => --i, out previousValue);

// 		public static Int32 Add(this ILockingAtomic<Int32> @this, Int32 operand) => @this.Set(i => i + operand);
// 		public static Int32 Add(this ILockingAtomic<Int32> @this, Int32 operand, out Int32 previousValue) => @this.Set(i => i + operand, out previousValue);

// 		public static Int32 Subtract(this ILockingAtomic<Int32> @this, Int32 operand) => @this.Set(i => i - operand);
// 		public static Int32 Subtract(this ILockingAtomic<Int32> @this, Int32 operand, out Int32 previousValue) => @this.Set(i => i - operand, out previousValue);

// 		public static Int32 Multiply(this ILockingAtomic<Int32> @this, Int32 operand) => @this.Set(i => i * operand);
// 		public static Int32 Multiply(this ILockingAtomic<Int32> @this, Int32 operand, out Int32 previousValue) => @this.Set(i => i * operand, out previousValue);

// 		public static Int32 Divide(this ILockingAtomic<Int32> @this, Int32 operand) => @this.Set(i => i / operand);
// 		public static Int32 Divide(this ILockingAtomic<Int32> @this, Int32 operand, out Int32 previousValue) => @this.Set(i => i / operand, out previousValue);
// 		#endregion

// 		#region UInt32
// 		public static UInt32 Increment(this ILockingAtomic<UInt32> @this) => @this.Set(i => ++i);
// 		public static UInt32 Increment(this ILockingAtomic<UInt32> @this, out UInt32 previousValue) => @this.Set(i => ++i, out previousValue);

// 		public static UInt32 Decrement(this ILockingAtomic<UInt32> @this) => @this.Set(i => --i);
// 		public static UInt32 Decrement(this ILockingAtomic<UInt32> @this, out UInt32 previousValue) => @this.Set(i => --i, out previousValue);

// 		public static UInt32 Add(this ILockingAtomic<UInt32> @this, UInt32 operand) => @this.Set(i => i + operand);
// 		public static UInt32 Add(this ILockingAtomic<UInt32> @this, UInt32 operand, out UInt32 previousValue) => @this.Set(i => i + operand, out previousValue);

// 		public static UInt32 Subtract(this ILockingAtomic<UInt32> @this, UInt32 operand) => @this.Set(i => i - operand);
// 		public static UInt32 Subtract(this ILockingAtomic<UInt32> @this, UInt32 operand, out UInt32 previousValue) => @this.Set(i => i - operand, out previousValue);

// 		public static UInt32 Multiply(this ILockingAtomic<UInt32> @this, UInt32 operand) => @this.Set(i => i * operand);
// 		public static UInt32 Multiply(this ILockingAtomic<UInt32> @this, UInt32 operand, out UInt32 previousValue) => @this.Set(i => i * operand, out previousValue);

// 		public static UInt32 Divide(this ILockingAtomic<UInt32> @this, UInt32 operand) => @this.Set(i => i / operand);
// 		public static UInt32 Divide(this ILockingAtomic<UInt32> @this, UInt32 operand, out UInt32 previousValue) => @this.Set(i => i / operand, out previousValue);
// 		#endregion

// 		#region Int64
// 		public static Int64 Increment(this ILockingAtomic<Int64> @this) => @this.Set(i => ++i);
// 		public static Int64 Increment(this ILockingAtomic<Int64> @this, out Int64 previousValue) => @this.Set(i => ++i, out previousValue);

// 		public static Int64 Decrement(this ILockingAtomic<Int64> @this) => @this.Set(i => --i);
// 		public static Int64 Decrement(this ILockingAtomic<Int64> @this, out Int64 previousValue) => @this.Set(i => --i, out previousValue);

// 		public static Int64 Add(this ILockingAtomic<Int64> @this, Int64 operand) => @this.Set(i => i + operand);
// 		public static Int64 Add(this ILockingAtomic<Int64> @this, Int64 operand, out Int64 previousValue) => @this.Set(i => i + operand, out previousValue);

// 		public static Int64 Subtract(this ILockingAtomic<Int64> @this, Int64 operand) => @this.Set(i => i - operand);
// 		public static Int64 Subtract(this ILockingAtomic<Int64> @this, Int64 operand, out Int64 previousValue) => @this.Set(i => i - operand, out previousValue);

// 		public static Int64 Multiply(this ILockingAtomic<Int64> @this, Int64 operand) => @this.Set(i => i * operand);
// 		public static Int64 Multiply(this ILockingAtomic<Int64> @this, Int64 operand, out Int64 previousValue) => @this.Set(i => i * operand, out previousValue);

// 		public static Int64 Divide(this ILockingAtomic<Int64> @this, Int64 operand) => @this.Set(i => i / operand);
// 		public static Int64 Divide(this ILockingAtomic<Int64> @this, Int64 operand, out Int64 previousValue) => @this.Set(i => i / operand, out previousValue);
// 		#endregion

// 		#region UInt64
// 		public static UInt64 Increment(this ILockingAtomic<UInt64> @this) => @this.Set(i => ++i);
// 		public static UInt64 Increment(this ILockingAtomic<UInt64> @this, out UInt64 previousValue) => @this.Set(i => ++i, out previousValue);

// 		public static UInt64 Decrement(this ILockingAtomic<UInt64> @this) => @this.Set(i => --i);
// 		public static UInt64 Decrement(this ILockingAtomic<UInt64> @this, out UInt64 previousValue) => @this.Set(i => --i, out previousValue);

// 		public static UInt64 Add(this ILockingAtomic<UInt64> @this, UInt64 operand) => @this.Set(i => i + operand);
// 		public static UInt64 Add(this ILockingAtomic<UInt64> @this, UInt64 operand, out UInt64 previousValue) => @this.Set(i => i + operand, out previousValue);

// 		public static UInt64 Subtract(this ILockingAtomic<UInt64> @this, UInt64 operand) => @this.Set(i => i - operand);
// 		public static UInt64 Subtract(this ILockingAtomic<UInt64> @this, UInt64 operand, out UInt64 previousValue) => @this.Set(i => i - operand, out previousValue);

// 		public static UInt64 Multiply(this ILockingAtomic<UInt64> @this, UInt64 operand) => @this.Set(i => i * operand);
// 		public static UInt64 Multiply(this ILockingAtomic<UInt64> @this, UInt64 operand, out UInt64 previousValue) => @this.Set(i => i * operand, out previousValue);

// 		public static UInt64 Divide(this ILockingAtomic<UInt64> @this, UInt64 operand) => @this.Set(i => i / operand);
// 		public static UInt64 Divide(this ILockingAtomic<UInt64> @this, UInt64 operand, out UInt64 previousValue) => @this.Set(i => i / operand, out previousValue);
// 		#endregion

// 		#region Single
// 		public static Single Increment(this ILockingAtomic<Single> @this) => @this.Set(i => ++i);
// 		public static Single Increment(this ILockingAtomic<Single> @this, out Single previousValue) => @this.Set(i => ++i, out previousValue);

// 		public static Single Decrement(this ILockingAtomic<Single> @this) => @this.Set(i => --i);
// 		public static Single Decrement(this ILockingAtomic<Single> @this, out Single previousValue) => @this.Set(i => --i, out previousValue);

// 		public static Single Add(this ILockingAtomic<Single> @this, Single operand) => @this.Set(i => i + operand);
// 		public static Single Add(this ILockingAtomic<Single> @this, Single operand, out Single previousValue) => @this.Set(i => i + operand, out previousValue);

// 		public static Single Subtract(this ILockingAtomic<Single> @this, Single operand) => @this.Set(i => i - operand);
// 		public static Single Subtract(this ILockingAtomic<Single> @this, Single operand, out Single previousValue) => @this.Set(i => i - operand, out previousValue);

// 		public static Single Multiply(this ILockingAtomic<Single> @this, Single operand) => @this.Set(i => i * operand);
// 		public static Single Multiply(this ILockingAtomic<Single> @this, Single operand, out Single previousValue) => @this.Set(i => i * operand, out previousValue);

// 		public static Single Divide(this ILockingAtomic<Single> @this, Single operand) => @this.Set(i => i / operand);
// 		public static Single Divide(this ILockingAtomic<Single> @this, Single operand, out Single previousValue) => @this.Set(i => i / operand, out previousValue);
// 		#endregion

// 		#region Double
// 		public static Double Increment(this ILockingAtomic<Double> @this) => @this.Set(i => ++i);
// 		public static Double Increment(this ILockingAtomic<Double> @this, out Double previousValue) => @this.Set(i => ++i, out previousValue);

// 		public static Double Decrement(this ILockingAtomic<Double> @this) => @this.Set(i => --i);
// 		public static Double Decrement(this ILockingAtomic<Double> @this, out Double previousValue) => @this.Set(i => --i, out previousValue);

// 		public static Double Add(this ILockingAtomic<Double> @this, Double operand) => @this.Set(i => i + operand);
// 		public static Double Add(this ILockingAtomic<Double> @this, Double operand, out Double previousValue) => @this.Set(i => i + operand, out previousValue);

// 		public static Double Subtract(this ILockingAtomic<Double> @this, Double operand) => @this.Set(i => i - operand);
// 		public static Double Subtract(this ILockingAtomic<Double> @this, Double operand, out Double previousValue) => @this.Set(i => i - operand, out previousValue);

// 		public static Double Multiply(this ILockingAtomic<Double> @this, Double operand) => @this.Set(i => i * operand);
// 		public static Double Multiply(this ILockingAtomic<Double> @this, Double operand, out Double previousValue) => @this.Set(i => i * operand, out previousValue);

// 		public static Double Divide(this ILockingAtomic<Double> @this, Double operand) => @this.Set(i => i / operand);
// 		public static Double Divide(this ILockingAtomic<Double> @this, Double operand, out Double previousValue) => @this.Set(i => i / operand, out previousValue);
// 		#endregion
// 	}

// #endregion

// #region Numerics

// // Notes on this interface:
// 	// Increment, Decrement, Add and Subtract are all reversible operations
// 	// Therefore offering only the fast path (XAndGet) makes sense. If the user
// 	// needs the 'previous' value they can just reverse the operation manually
// 	// (which is all we'd be doing for them anyway, it's not like we have some
// 	// 'faster' API internally to do that).
// 	// On the other hand, multiply and divide can be irreversible (multiplication
// 	// that overflows and any division that has a remainder > 0). So knowing the
// 	// previous value is not always possible for the caller of this API, I'm not
// 	// too bothered about offering overloads that provide a single return value
// 	// because the multiply/divide op is probably going to overshadow the performance
// 	// loss from returning 2 values anyway. And if they're desperate they can use
// 	// GetUnsafeRef and do it themselves.
// 	public interface INonLockingNumericAtomic<T> : INonLockingAtomic<T> {
// 		T IncrementAndGet();
// 		T DecrementAndGet();
// 		T AddAndGet(T operand);
// 		T SubtractAndGet(T operand);

// 		ExchangeResult<T> Multiply(T operand);
// 		ExchangeResult<T> Divide(T operand);
// 	}

// 	// I did consider putting BitwiseXyzAndGet() overloads but the actual implementations for
// 	// these operations tend to be CAS loops with at least one conditional so the overhead of
// 	// returning a slightly larger type tends to be overshadowed anyway.
// 	public interface INonLockingIntegerAtomic<T> : INonLockingNumericAtomic<T> {
// 		ExchangeResult<T> BitwiseAnd(T operand);

// 		ExchangeResult<T> BitwiseOr(T operand);

// 		ExchangeResult<T> BitwiseExclusiveOr(T operand);

// 		ExchangeResult<T> BitwiseNegate();

// 		ExchangeResult<T> BitwiseLeftShift(int operand);

// 		ExchangeResult<T> BitwiseRightShift(int operand);
// 	}

// 	public interface INonLockingFloatingPointAtomic<T> : INonLockingNumericAtomic<T> {
// 		TryExchangeResult<T> TrySwap(T newValue, T comparand, T maxDelta);
// 	}

// public sealed class LockFreeDouble : INonLockingFloatingPointAtomic<double>, IFormattable {
// 		double _value;

// 		public double Value {
// 			[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 			get => Get();
// 			[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 			set => Set(value);
// 		}

// 		public LockFreeDouble() : this(default) { }
// 		public LockFreeDouble(double initialValue) => Set(initialValue);

// 		[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 		public double Get() {
// 			if (IntPtr.Size == sizeof(long)) return Volatile.Read(ref _value);
// 			else return Interlocked.CompareExchange(ref _value, default, default);
// 		}

// 		[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 		public double GetUnsafe() => _value;

// 		[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 		public ref double GetUnsafeRef() => ref _value;

// 		[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 		public void Set(double newValue) {
// 			if (IntPtr.Size == sizeof(long)) Volatile.Write(ref _value, newValue);
// 			else Interlocked.Exchange(ref _value, newValue);
// 		}

// 		[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 		public void SetUnsafe(double newValue) => _value = newValue;

// 		[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 		public double Swap(double newValue) => Interlocked.Exchange(ref _value, newValue);

// 		[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 		public double TrySwap(double newValue, double comparand) => Interlocked.CompareExchange(ref _value, newValue, comparand);

// 		[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 		public TryExchangeResult<double> TrySwap(double newValue, double comparand, double maxDelta) {
// 			var spinner = new SpinWait();
// 			var previousValue = Get();

// 			while (true) {
// 				if (Math.Abs(previousValue - comparand) > maxDelta) return new TryExchangeResult<double>(false, previousValue, previousValue);
// 				var updatedPreviousValue = Interlocked.CompareExchange(ref _value, newValue, previousValue);
// 				if (updatedPreviousValue == previousValue) return new TryExchangeResult<double>(true, previousValue, newValue);
// 				previousValue = updatedPreviousValue;
// 				spinner.SpinOnce();
// 			}
// 		}

// 		public double IncrementAndGet() {
// 			var spinner = new SpinWait();
// 			var previousValue = Get();

// 			while (true) {
// 				var updatedPreviousValue = Interlocked.CompareExchange(ref _value, previousValue + 1f, previousValue);
// 				if (updatedPreviousValue == previousValue) return previousValue;
// 				previousValue = updatedPreviousValue;
// 				spinner.SpinOnce();
// 			}
// 		}

// 		public double DecrementAndGet() {
// 			var spinner = new SpinWait();
// 			var previousValue = Get();

// 			while (true) {
// 				var updatedPreviousValue = Interlocked.CompareExchange(ref _value, previousValue - 1f, previousValue);
// 				if (updatedPreviousValue == previousValue) return previousValue;
// 				previousValue = updatedPreviousValue;
// 				spinner.SpinOnce();
// 			}
// 		}

// 		public double AddAndGet(double operand) {
// 			var spinner = new SpinWait();
// 			var previousValue = Get();

// 			while (true) {
// 				var updatedPreviousValue = Interlocked.CompareExchange(ref _value, previousValue + operand, previousValue);
// 				if (updatedPreviousValue == previousValue) return previousValue;
// 				previousValue = updatedPreviousValue;
// 				spinner.SpinOnce();
// 			}
// 		}

// 		public double SubtractAndGet(double operand) {
// 			var spinner = new SpinWait();
// 			var previousValue = Get();

// 			while (true) {
// 				var updatedPreviousValue = Interlocked.CompareExchange(ref _value, previousValue - operand, previousValue);
// 				if (updatedPreviousValue == previousValue) return previousValue;
// 				previousValue = updatedPreviousValue;
// 				spinner.SpinOnce();
// 			}
// 		}

// 		public ExchangeResult<double> Multiply(double operand) {
// 			var spinner = new SpinWait();
// 			var previousValue = Get();

// 			while (true) {
// 				var newValue = previousValue * operand;
// 				var updatedPreviousValue = Interlocked.CompareExchange(ref _value, newValue, previousValue);
// 				if (updatedPreviousValue == previousValue) return new ExchangeResult<double>(previousValue, newValue);
// 				previousValue = updatedPreviousValue;
// 				spinner.SpinOnce();
// 			}
// 		}

// 		public ExchangeResult<double> Divide(double operand) {
// 			var spinner = new SpinWait();
// 			var previousValue = Get();

// 			while (true) {
// 				var newValue = previousValue / operand;
// 				var updatedPreviousValue = Interlocked.CompareExchange(ref _value, newValue, previousValue);
// 				if (updatedPreviousValue == previousValue) return new ExchangeResult<double>(previousValue, newValue);
// 				previousValue = updatedPreviousValue;
// 				spinner.SpinOnce();
// 			}
// 		}

// 		// ReSharper disable once SpecifyACultureInStringConversionExplicitly It's up to the user of our API to pick the overload they prefer (but they should probably specify a culture indeed).
// 		public override string ToString() => Get().ToString();
// 		public string ToString(string format, IFormatProvider formatProvider) => Get().ToString(format, formatProvider);

// 		#region Equality
// 		[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 		public bool Equals(double other) => other == Value;

// 		public override bool Equals(object obj) {
// 			if (obj is double value) return Equals(value);
// 			return ReferenceEquals(this, obj);
// 		}

// 		// ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode Base GetHashCode() is appropriate here.
// 		public override int GetHashCode() => base.GetHashCode();

// 		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool operator ==(LockFreeDouble left, double right) => left?.Equals(right) ?? false;
// 		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool operator !=(LockFreeDouble left, double right) => !(left == right);
// 		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool operator ==(double left, LockFreeDouble right) => right?.Equals(left) ?? false;
// 		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool operator !=(double left, LockFreeDouble right) => !(right == left);
// 		#endregion
// 	}

//     public sealed class LockFreeInt32 : INonLockingIntegerAtomic<int>, IFormattable {
// 		int _value;

// 		public int Value {
// 			[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 			get => Get();
// 			[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 			set => Set(value);
// 		}

// 		public LockFreeInt32() : this(default) { }
// 		public LockFreeInt32(int initialValue) => Set(initialValue);

// 		[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 		public int Get() => Volatile.Read(ref _value);

// 		[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 		public int GetUnsafe() => _value;

// 		[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 		public ref int GetUnsafeRef() => ref _value;

// 		[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 		public void Set(int newValue) => Volatile.Write(ref _value, newValue);

// 		[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 		public void SetUnsafe(int newValue) => _value = newValue;

// 		[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 		public int Swap(int newValue) => Interlocked.Exchange(ref _value, newValue);

// 		[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 		public int TrySwap(int newValue, int comparand) => Interlocked.CompareExchange(ref _value, newValue, comparand);

// 		[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 		public int IncrementAndGet() => Interlocked.Increment(ref _value);

// 		[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 		public int DecrementAndGet() => Interlocked.Decrement(ref _value);

// 		[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 		public int AddAndGet(int operand) => Interlocked.Add(ref _value, operand);

// 		[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 		public int SubtractAndGet(int operand) => Interlocked.Add(ref _value, -operand);

// 		public ExchangeResult<int> Multiply(int operand) {
// 			var spinner = new SpinWait();
// 			var previousValue = Get();

// 			while (true) {
// 				var newValue = previousValue * operand;
// 				var updatedPreviousValue = Interlocked.CompareExchange(ref _value, newValue, previousValue);
// 				if (updatedPreviousValue == previousValue) return new ExchangeResult<int>(previousValue, newValue);
// 				previousValue = updatedPreviousValue;
// 				spinner.SpinOnce();
// 			}
// 		}

// 		public ExchangeResult<int> Divide(int operand) {
// 			var spinner = new SpinWait();
// 			var previousValue = Get();

// 			while (true) {
// 				var newValue = previousValue / operand;
// 				var updatedPreviousValue = Interlocked.CompareExchange(ref _value, newValue, previousValue);
// 				if (updatedPreviousValue == previousValue) return new ExchangeResult<int>(previousValue, newValue);
// 				previousValue = updatedPreviousValue;
// 				spinner.SpinOnce();
// 			}
// 		}

// 		public ExchangeResult<int> BitwiseAnd(int operand) {
// 			var spinner = new SpinWait();
// 			var previousValue = Get();

// 			while (true) {
// 				var newValue = previousValue & operand;
// 				var updatedPreviousValue = Interlocked.CompareExchange(ref _value, newValue, previousValue);
// 				if (updatedPreviousValue == previousValue) return new ExchangeResult<int>(previousValue, newValue);
// 				previousValue = updatedPreviousValue;
// 				spinner.SpinOnce();
// 			}
// 		}

// 		public ExchangeResult<int> BitwiseOr(int operand) {
// 			var spinner = new SpinWait();
// 			var previousValue = Get();

// 			while (true) {
// 				var newValue = previousValue | operand;
// 				var updatedPreviousValue = Interlocked.CompareExchange(ref _value, newValue, previousValue);
// 				if (updatedPreviousValue == previousValue) return new ExchangeResult<int>(previousValue, newValue);
// 				previousValue = updatedPreviousValue;
// 				spinner.SpinOnce();
// 			}
// 		}

// 		public ExchangeResult<int> BitwiseExclusiveOr(int operand) {
// 			var spinner = new SpinWait();
// 			var previousValue = Get();

// 			while (true) {
// 				var newValue = previousValue ^ operand;
// 				var updatedPreviousValue = Interlocked.CompareExchange(ref _value, newValue, previousValue);
// 				if (updatedPreviousValue == previousValue) return new ExchangeResult<int>(previousValue, newValue);
// 				previousValue = updatedPreviousValue;
// 				spinner.SpinOnce();
// 			}
// 		}

// 		public ExchangeResult<int> BitwiseNegate() {
// 			var spinner = new SpinWait();
// 			var previousValue = Get();

// 			while (true) {
// 				var newValue = ~previousValue;
// 				var updatedPreviousValue = Interlocked.CompareExchange(ref _value, newValue, previousValue);
// 				if (updatedPreviousValue == previousValue) return new ExchangeResult<int>(previousValue, newValue);
// 				previousValue = updatedPreviousValue;
// 				spinner.SpinOnce();
// 			}
// 		}

// 		public ExchangeResult<int> BitwiseLeftShift(int operand) {
// 			var spinner = new SpinWait();
// 			var previousValue = Get();

// 			while (true) {
// 				var newValue = previousValue << operand;
// 				var updatedPreviousValue = Interlocked.CompareExchange(ref _value, newValue, previousValue);
// 				if (updatedPreviousValue == previousValue) return new ExchangeResult<int>(previousValue, newValue);
// 				previousValue = updatedPreviousValue;
// 				spinner.SpinOnce();
// 			}
// 		}

// 		public ExchangeResult<int> BitwiseRightShift(int operand) {
// 			var spinner = new SpinWait();
// 			var previousValue = Get();

// 			while (true) {
// 				var newValue = previousValue >> operand;
// 				var updatedPreviousValue = Interlocked.CompareExchange(ref _value, newValue, previousValue);
// 				if (updatedPreviousValue == previousValue) return new ExchangeResult<int>(previousValue, newValue);
// 				previousValue = updatedPreviousValue;
// 				spinner.SpinOnce();
// 			}
// 		}

// 		public override string ToString() => Get().ToString();
// 		public string ToString(string format, IFormatProvider formatProvider) => Get().ToString(format, formatProvider);

// 		#region Equality
// 		[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 		public bool Equals(int other) => Value == other;

// 		public override bool Equals(object obj) {
// 			if (obj is int value) return Equals(value);
// 			return ReferenceEquals(this, obj);
// 		}

// 		// ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode Base GetHashCode() is appropriate here.
// 		public override int GetHashCode() => base.GetHashCode();

// 		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool operator ==(LockFreeInt32 left, int right) => left?.Equals(right) ?? false;
// 		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool operator !=(LockFreeInt32 left, int right) => !(left == right);
// 		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool operator ==(int left, LockFreeInt32 right) => right?.Equals(left) ?? false;
// 		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool operator !=(int left, LockFreeInt32 right) => !(right == left);
// 		#endregion
// 	}

//     public sealed class LockFreeInt64 : INonLockingIntegerAtomic<long>, IFormattable {
// 		long _value;

// 		public long Value {
// 			[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 			get => Get();
// 			[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 			set => Set(value);
// 		}

// 		public LockFreeInt64() : this(default) { }
// 		public LockFreeInt64(long initialValue) => Set(initialValue);

// 		[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 		public long Get() {
// 			if (IntPtr.Size == sizeof(long)) return Volatile.Read(ref _value);
// 			else return Interlocked.Read(ref _value);
// 		}

// 		[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 		public long GetUnsafe() => _value;

// 		[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 		public ref long GetUnsafeRef() => ref _value;

// 		[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 		public void Set(long newValue) {
// 			if (IntPtr.Size == sizeof(long)) Volatile.Write(ref _value, newValue);
// 			else Interlocked.Exchange(ref _value, newValue);
// 		}

// 		[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 		public void SetUnsafe(long newValue) => _value = newValue;

// 		[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 		public long Swap(long newValue) => Interlocked.Exchange(ref _value, newValue);

// 		[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 		public long TrySwap(long newValue, long comparand) => Interlocked.CompareExchange(ref _value, newValue, comparand);

// 		[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 		public long IncrementAndGet() => Interlocked.Increment(ref _value);

// 		[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 		public long DecrementAndGet() => Interlocked.Decrement(ref _value);

// 		[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 		public long AddAndGet(long operand) => Interlocked.Add(ref _value, operand);

// 		[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 		public long SubtractAndGet(long operand) => Interlocked.Add(ref _value, -operand);

// 		public ExchangeResult<long> Multiply(long operand) {
// 			var spinner = new SpinWait();
// 			var previousValue = Get();

// 			while (true) {
// 				var newValue = previousValue * operand;
// 				var updatedPreviousValue = Interlocked.CompareExchange(ref _value, newValue, previousValue);
// 				if (updatedPreviousValue == previousValue) return new ExchangeResult<long>(previousValue, newValue);
// 				previousValue = updatedPreviousValue;
// 				spinner.SpinOnce();
// 			}
// 		}

// 		public ExchangeResult<long> Divide(long operand) {
// 			var spinner = new SpinWait();
// 			var previousValue = Get();

// 			while (true) {
// 				var newValue = previousValue / operand;
// 				var updatedPreviousValue = Interlocked.CompareExchange(ref _value, newValue, previousValue);
// 				if (updatedPreviousValue == previousValue) return new ExchangeResult<long>(previousValue, newValue);
// 				previousValue = updatedPreviousValue;
// 				spinner.SpinOnce();
// 			}
// 		}

// 		public ExchangeResult<long> BitwiseAnd(long operand) {
// 			var spinner = new SpinWait();
// 			var previousValue = Get();

// 			while (true) {
// 				var newValue = previousValue & operand;
// 				var updatedPreviousValue = Interlocked.CompareExchange(ref _value, newValue, previousValue);
// 				if (updatedPreviousValue == previousValue) return new ExchangeResult<long>(previousValue, newValue);
// 				previousValue = updatedPreviousValue;
// 				spinner.SpinOnce();
// 			}
// 		}

// 		public ExchangeResult<long> BitwiseOr(long operand) {
// 			var spinner = new SpinWait();
// 			var previousValue = Get();

// 			while (true) {
// 				var newValue = previousValue | operand;
// 				var updatedPreviousValue = Interlocked.CompareExchange(ref _value, newValue, previousValue);
// 				if (updatedPreviousValue == previousValue) return new ExchangeResult<long>(previousValue, newValue);
// 				previousValue = updatedPreviousValue;
// 				spinner.SpinOnce();
// 			}
// 		}

// 		public ExchangeResult<long> BitwiseExclusiveOr(long operand) {
// 			var spinner = new SpinWait();
// 			var previousValue = Get();

// 			while (true) {
// 				var newValue = previousValue ^ operand;
// 				var updatedPreviousValue = Interlocked.CompareExchange(ref _value, newValue, previousValue);
// 				if (updatedPreviousValue == previousValue) return new ExchangeResult<long>(previousValue, newValue);
// 				previousValue = updatedPreviousValue;
// 				spinner.SpinOnce();
// 			}
// 		}

// 		public ExchangeResult<long> BitwiseNegate() {
// 			var spinner = new SpinWait();
// 			var previousValue = Get();

// 			while (true) {
// 				var newValue = ~previousValue;
// 				var updatedPreviousValue = Interlocked.CompareExchange(ref _value, newValue, previousValue);
// 				if (updatedPreviousValue == previousValue) return new ExchangeResult<long>(previousValue, newValue);
// 				previousValue = updatedPreviousValue;
// 				spinner.SpinOnce();
// 			}
// 		}

// 		public ExchangeResult<long> BitwiseLeftShift(int operand) {
// 			var spinner = new SpinWait();
// 			var previousValue = Get();

// 			while (true) {
// 				var newValue = previousValue << operand;
// 				var updatedPreviousValue = Interlocked.CompareExchange(ref _value, newValue, previousValue);
// 				if (updatedPreviousValue == previousValue) return new ExchangeResult<long>(previousValue, newValue);
// 				previousValue = updatedPreviousValue;
// 				spinner.SpinOnce();
// 			}
// 		}

// 		public ExchangeResult<long> BitwiseRightShift(int operand) {
// 			var spinner = new SpinWait();
// 			var previousValue = Get();

// 			while (true) {
// 				var newValue = previousValue >> operand;
// 				var updatedPreviousValue = Interlocked.CompareExchange(ref _value, newValue, previousValue);
// 				if (updatedPreviousValue == previousValue) return new ExchangeResult<long>(previousValue, newValue);
// 				previousValue = updatedPreviousValue;
// 				spinner.SpinOnce();
// 			}
// 		}

// 		public override string ToString() => Get().ToString();
// 		public string ToString(string format, IFormatProvider formatProvider) => Get().ToString(format, formatProvider);

// 		#region Equality
// 		[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 		public bool Equals(long other) => Value == other;

// 		public override bool Equals(object obj) {
// 			if (obj is long value) return Equals(value);
// 			return ReferenceEquals(this, obj);
// 		}

// 		// ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode Base GetHashCode() is appropriate here.
// 		public override int GetHashCode() => base.GetHashCode();

// 		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool operator ==(LockFreeInt64 left, long right) => left?.Equals(right) ?? false;
// 		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool operator !=(LockFreeInt64 left, long right) => !(left == right);
// 		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool operator ==(long left, LockFreeInt64 right) => right?.Equals(left) ?? false;
// 		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool operator !=(long left, LockFreeInt64 right) => !(right == left);
// 		#endregion
// 	}

//     public sealed class LockFreeSingle : INonLockingFloatingPointAtomic<float>, IFormattable {
// 		float _value;

// 		public float Value {
// 			[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 			get => Get();
// 			[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 			set => Set(value);
// 		}

// 		public LockFreeSingle() : this(default) { }
// 		public LockFreeSingle(float initialValue) => Set(initialValue);

// 		[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 		public float Get() => Volatile.Read(ref _value);

// 		[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 		public float GetUnsafe() => _value;

// 		[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 		public ref float GetUnsafeRef() => ref _value;

// 		[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 		public void Set(float newValue) => Volatile.Write(ref _value, newValue);

// 		[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 		public void SetUnsafe(float newValue) => _value = newValue;

// 		[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 		public float Swap(float newValue) => Interlocked.Exchange(ref _value, newValue);

// 		[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 		public float TrySwap(float newValue, float comparand) => Interlocked.CompareExchange(ref _value, newValue, comparand);

// 		[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 		public TryExchangeResult<float> TrySwap(float newValue, float comparand, float maxDelta) {
// 			var spinner = new SpinWait();
// 			var previousValue = Get();

// 			while (true) {
// 				if (Math.Abs(previousValue - comparand) > maxDelta) return new TryExchangeResult<float>(false, previousValue, previousValue);
// 				var updatedPreviousValue = Interlocked.CompareExchange(ref _value, newValue, previousValue);
// 				if (updatedPreviousValue == previousValue) return new TryExchangeResult<float>(true, previousValue, newValue);
// 				previousValue = updatedPreviousValue;
// 				spinner.SpinOnce();
// 			}
// 		}

// 		public float IncrementAndGet() {
// 			var spinner = new SpinWait();
// 			var previousValue = Get();

// 			while (true) {
// 				var updatedPreviousValue = Interlocked.CompareExchange(ref _value, previousValue + 1f, previousValue);
// 				if (updatedPreviousValue == previousValue) return previousValue;
// 				previousValue = updatedPreviousValue;
// 				spinner.SpinOnce();
// 			}
// 		}

// 		public float DecrementAndGet() {
// 			var spinner = new SpinWait();
// 			var previousValue = Get();

// 			while (true) {
// 				var updatedPreviousValue = Interlocked.CompareExchange(ref _value, previousValue - 1f, previousValue);
// 				if (updatedPreviousValue == previousValue) return previousValue;
// 				previousValue = updatedPreviousValue;
// 				spinner.SpinOnce();
// 			}
// 		}

// 		public float AddAndGet(float operand) {
// 			var spinner = new SpinWait();
// 			var previousValue = Get();

// 			while (true) {
// 				var updatedPreviousValue = Interlocked.CompareExchange(ref _value, previousValue + operand, previousValue);
// 				if (updatedPreviousValue == previousValue) return previousValue;
// 				previousValue = updatedPreviousValue;
// 				spinner.SpinOnce();
// 			}
// 		}

// 		public float SubtractAndGet(float operand) {
// 			var spinner = new SpinWait();
// 			var previousValue = Get();

// 			while (true) {
// 				var updatedPreviousValue = Interlocked.CompareExchange(ref _value, previousValue - operand, previousValue);
// 				if (updatedPreviousValue == previousValue) return previousValue;
// 				previousValue = updatedPreviousValue;
// 				spinner.SpinOnce();
// 			}
// 		}

// 		public ExchangeResult<float> Multiply(float operand) {
// 			var spinner = new SpinWait();
// 			var previousValue = Get();

// 			while (true) {
// 				var newValue = previousValue * operand;
// 				var updatedPreviousValue = Interlocked.CompareExchange(ref _value, newValue, previousValue);
// 				if (updatedPreviousValue == previousValue) return new ExchangeResult<float>(previousValue, newValue);
// 				previousValue = updatedPreviousValue;
// 				spinner.SpinOnce();
// 			}
// 		}

// 		public ExchangeResult<float> Divide(float operand) {
// 			var spinner = new SpinWait();
// 			var previousValue = Get();

// 			while (true) {
// 				var newValue = previousValue / operand;
// 				var updatedPreviousValue = Interlocked.CompareExchange(ref _value, newValue, previousValue);
// 				if (updatedPreviousValue == previousValue) return new ExchangeResult<float>(previousValue, newValue);
// 				previousValue = updatedPreviousValue;
// 				spinner.SpinOnce();
// 			}
// 		}

// 		// ReSharper disable once SpecifyACultureInStringConversionExplicitly It's up to the user of our API to pick the overload they prefer (but they should probably specify a culture indeed).
// 		public override string ToString() => Get().ToString();
// 		public string ToString(string format, IFormatProvider formatProvider) => Get().ToString(format, formatProvider);

// 		#region Equality
// 		[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 		public bool Equals(float other) => other == Value;

// 		public override bool Equals(object obj) {
// 			if (obj is float value) return Equals(value);
// 			return ReferenceEquals(this, obj);
// 		}

// 		// ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode Base GetHashCode() is appropriate here.
// 		public override int GetHashCode() => base.GetHashCode();

// 		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool operator ==(LockFreeSingle left, float right) => left?.Equals(right) ?? false;
// 		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool operator !=(LockFreeSingle left, float right) => !(left == right);
// 		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool operator ==(float left, LockFreeSingle right) => right?.Equals(left) ?? false;
// 		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool operator !=(float left, LockFreeSingle right) => !(right == left);
// 		#endregion
// 	}

// #endregion

// public struct TryExchangeResult<T> : IEquatable<TryExchangeResult<T>> {
// 		public readonly bool ExchangeSuccess;
// 		public readonly T PreviousValue;
// 		public readonly T NewValue;

// 		public TryExchangeResult(bool exchangeSuccess, T previousValue, T newValue) {
// 			ExchangeSuccess = exchangeSuccess;
// 			PreviousValue = previousValue;
// 			NewValue = newValue;
// 		}

// 		public static TryExchangeResult<T> Success(T previousValue, T newValue) => new TryExchangeResult<T>(true, previousValue, newValue);

// 		public static TryExchangeResult<T> Failure(T persistingValue) => new TryExchangeResult<T>(false, persistingValue, persistingValue);

// 		public override string ToString() {
// 			if (!ExchangeSuccess) return $"{(PreviousValue != null ? PreviousValue.ToString() : "<null>")} (unchanged)";
			
// 			return $"{(PreviousValue != null ? PreviousValue.ToString() : "<null>")} => " +
// 				   $"{(NewValue != null ? NewValue.ToString() : "<null>")}";
// 		}

// 		public bool Equals(TryExchangeResult<T> other) {
// 			return ExchangeSuccess == other.ExchangeSuccess && EqualityComparer<T>.Default.Equals(PreviousValue, other.PreviousValue) && EqualityComparer<T>.Default.Equals(NewValue, other.NewValue);
// 		}

// 		public override bool Equals(object obj) {
// 			return obj is TryExchangeResult<T> other && Equals(other);
// 		}

// 		public override int GetHashCode() {
// 			unchecked {
// 				var hashCode = ExchangeSuccess.GetHashCode();
// 				hashCode = (hashCode * 397) ^ EqualityComparer<T>.Default.GetHashCode(PreviousValue);
// 				hashCode = (hashCode * 397) ^ EqualityComparer<T>.Default.GetHashCode(NewValue);
// 				return hashCode;
// 			}
// 		}

// 		public static bool operator ==(TryExchangeResult<T> left, TryExchangeResult<T> right) => left.Equals(right);
// 		public static bool operator !=(TryExchangeResult<T> left, TryExchangeResult<T> right) => !left.Equals(right);
// 	}

// public sealed unsafe class LockFreeValue<T> : INonLockingAtomic<T> where T : unmanaged {
// static readonly bool TargetTypeIsEquatable = typeof(IEquatable<T>).IsAssignableFrom(typeof(T));
// long _valueAsLong;

// public T Value {
//     [MethodImpl(MethodImplOptions.AggressiveInlining)] get => Get();
//     [MethodImpl(MethodImplOptions.AggressiveInlining)] set => Set(value);
// }

// public LockFreeValue() : this(default) { }
// public LockFreeValue(T initialValue) {
//     if (sizeof(T) > sizeof(long)) {
//         throw new ArgumentException($"Generic type parameter in {typeof(LockFreeValue<>).Name} must not exceed {sizeof(long)} bytes. " +
//                                     $"Given type '{typeof(T)}' has a size of {sizeof(T)} bytes. " +
//                                     $"Use {typeof(LockFreeReadsValue<>).Name} instead for large unmanaged types.");
//     }
//     Set(initialValue);
// }

// [MethodImpl(MethodImplOptions.AggressiveInlining)]
// public T Get() {
//     var valueCopy = SafeGetAsLong();
//     return ReadFromLong(&valueCopy);
// }

// [MethodImpl(MethodImplOptions.AggressiveInlining)]
// long SafeGetAsLong() {
//     if (IntPtr.Size == sizeof(long)) return Volatile.Read(ref _valueAsLong);
//     else return Interlocked.Read(ref _valueAsLong);
// }

// [MethodImpl(MethodImplOptions.AggressiveInlining)]
// public T GetUnsafe() {
//     var valueCopy = _valueAsLong;
//     return ReadFromLong(&valueCopy);
// }

// [MethodImpl(MethodImplOptions.AggressiveInlining)]
// public void Set(T newValue) {
//     long newValueAsLong;
//     WriteToLong(&newValueAsLong, newValue);
//     SafeSetAsLong(newValueAsLong);
// }

// [MethodImpl(MethodImplOptions.AggressiveInlining)]
// void SafeSetAsLong(long newValueAsLong) {
//     if (IntPtr.Size == sizeof(long)) Volatile.Write(ref _valueAsLong, newValueAsLong);
//     else Interlocked.Exchange(ref _valueAsLong, newValueAsLong);
// }

// [MethodImpl(MethodImplOptions.AggressiveInlining)]
// public void SetUnsafe(T newValue) {
//     long newValueAsLong;
//     WriteToLong(&newValueAsLong, newValue);
//     _valueAsLong = newValueAsLong;
// }

// [MethodImpl(MethodImplOptions.AggressiveInlining)]
// public ref T GetUnsafeRef() => ref Unsafe.As<long, T>(ref _valueAsLong);

// [MethodImpl(MethodImplOptions.AggressiveInlining)]
// public T Swap(T newValue) {
//     long newValueAsLong;
//     WriteToLong(&newValueAsLong, newValue);
//     var previousValueAsLong = Interlocked.Exchange(ref _valueAsLong, newValueAsLong);
//     return ReadFromLong(&previousValueAsLong);
// }

// [MethodImpl(MethodImplOptions.AggressiveInlining)]
// public T TrySwap(T newValue, T comparand) {
//     if (!TargetTypeIsEquatable) return TrySwapByValueOnly(newValue, comparand);

//     long newValueAsLong;
//     WriteToLong(&newValueAsLong, newValue);

//     var spinner = new SpinWait();
//     while (true) {
//         var currentValue = Value;
//         long currentValueAsLong;
//         WriteToLong(&currentValueAsLong, currentValue);
//         if (!EquatableEquals(currentValue, comparand) || Interlocked.CompareExchange(ref _valueAsLong, newValueAsLong, currentValueAsLong) == currentValueAsLong) return currentValue;
//         spinner.SpinOnce();
//     }
// }

// [MethodImpl(MethodImplOptions.AggressiveInlining)]
// public T TrySwapByValueOnly(T newValue, T comparand) {
//     long newValueAsLong, comparandAsLong;
//     WriteToLong(&newValueAsLong, newValue);
//     WriteToLong(&comparandAsLong, comparand);
//     var previousValueAsLong = Interlocked.CompareExchange(ref _valueAsLong, newValueAsLong, comparandAsLong);
//     return ReadFromLong(&previousValueAsLong);
// }

// [MethodImpl(MethodImplOptions.AggressiveInlining)]
// static void WriteToLong(long* target, T val) {
//     *((T*) target) = val;
// }

// [MethodImpl(MethodImplOptions.AggressiveInlining)]
// static T ReadFromLong(long* src) {
//     return *((T*) src);
// }

// [MethodImpl(MethodImplOptions.AggressiveInlining)]
// static bool EquatableEquals(T lhs, T rhs) => ((IEquatable<T>) lhs).Equals(rhs);

// public override string ToString() => Get().ToString();

// #region Equality
// [MethodImpl(MethodImplOptions.AggressiveInlining)]
// public bool Equals(T other) {
//     if (TargetTypeIsEquatable) return EquatableEquals(Value, other);

//     long otherAsLong;
//     WriteToLong(&otherAsLong, other);
//     return otherAsLong == SafeGetAsLong();
// }

// public override bool Equals(object obj) {
//     if (obj is T value) return Equals(value);
//     return ReferenceEquals(this, obj);
// }

// // ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode Base GetHashCode() is appropriate here.
// public override int GetHashCode() => base.GetHashCode();

// [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool operator ==(LockFreeValue<T> left, T right) => left?.Equals(right) ?? false;
// [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool operator !=(LockFreeValue<T> left, T right) => !(left == right);
// [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool operator ==(T left, LockFreeValue<T> right) => right?.Equals(left) ?? false;
// [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool operator !=(T left, LockFreeValue<T> right) => !(right == left);
// #endregion
// }

// public sealed class LockFreeReference<T> : INonLockingAtomic<T> where T : class {
// 		static readonly bool TargetTypeIsEquatable = typeof(IEquatable<T>).IsAssignableFrom(typeof(T));
// 		T? _value;

// 		public T? Value {
// 			[MethodImpl(MethodImplOptions.AggressiveInlining)] get => Get();
// 			[MethodImpl(MethodImplOptions.AggressiveInlining)] set => Set(value);
// 		}

// 		public LockFreeReference() : this(default) { }
// 		public LockFreeReference(T? initialValue) => Set(initialValue);

// 		[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 		public T? Get() => Volatile.Read(ref _value);

// 		[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 		public T? GetUnsafe() => _value;

// 		[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 		public void Set(T? newValue) => Volatile.Write(ref _value, newValue);

// 		[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 		public void SetUnsafe(T? newValue) => _value = newValue;

// 		[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 		public ref T? GetUnsafeRef() => ref _value;

// 		[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 		public T? Swap(T? newValue) => Interlocked.Exchange(ref _value, newValue);

// 		public T? TrySwap(T? newValue, T? comparand) {
// 			if (!TargetTypeIsEquatable) return TrySwapByRefOnly(newValue, comparand);

// 			var spinner = new SpinWait();
// 			while (true) {
// 				var currentValue = Value;
// 				if (!EquatableEquals(currentValue, comparand) || Interlocked.CompareExchange(ref _value, newValue, currentValue) == currentValue) return currentValue;
// 				spinner.SpinOnce();
// 			}
// 		}

// 		[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 		public T? TrySwapByRefOnly(T? newValue, T? comparand) => Interlocked.CompareExchange(ref _value, newValue, comparand);

// 		static bool EquatableEquals(T? lhs, T? rhs) {
// 			if (lhs is null || rhs is null) return lhs is null && rhs is null;
// 			return ((IEquatable<T>) lhs).Equals(rhs);
// 		}

// 		public override string ToString() => Get()?.ToString() ?? AtomicUtils.NullValueString;

// 		#region Equality
// 		[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 		public bool Equals(T? other) => TargetTypeIsEquatable ? EquatableEquals(Value, other) : ReferenceEquals(Value, other);

// 		public override bool Equals(object obj) {
// 			if (obj is T value) return Equals(value);
// 			return ReferenceEquals(this, obj);
// 		}

// 		// ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode Base GetHashCode() is appropriate here.
// 		public override int GetHashCode() => base.GetHashCode();

// 		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool operator ==(LockFreeReference<T> left, T? right) => left?.Equals(right) ?? false;
// 		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool operator !=(LockFreeReference<T> left, T? right) => !(left == right);
// 		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool operator ==(T? left, LockFreeReference<T> right) => right?.Equals(left) ?? false;
// 		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool operator !=(T? left, LockFreeReference<T> right) => !(right == left);
// 		#endregion
// 	}

// public sealed class LockFreeReadsValue<T> : INonLockingAtomic<T> where T : struct, IEquatable<T> {
//     struct BufferSlot<TSlot> where TSlot : struct {
//         public long WriteCount;
//         public TSlot Value;
//     }

//     const int NumSlots = 32;
//     const int SlotMask = NumSlots - 1;
//     readonly object _writeLock = new object();
//     readonly BufferSlot<T>[] _slots = new BufferSlot<T>[NumSlots];
//     long _lastWriteID;

//     public T Value {
//         [MethodImpl(MethodImplOptions.AggressiveInlining)] get => Get();
//         [MethodImpl(MethodImplOptions.AggressiveInlining)] set => Set(value);
//     }

//     public LockFreeReadsValue() : this(default) { }
//     public LockFreeReadsValue(T initialValue) => Set(initialValue);

//     public T Get() {
//         if (IntPtr.Size != sizeof(long)) return Get32(); // All hail the branch predictor

//         var spinner = new SpinWait();

//         while (true) {
//             var lastWriteID = Volatile.Read(ref _lastWriteID);
//             var index = lastWriteID & SlotMask;

//             var expectedWriteCount = Volatile.Read(ref _slots[index].WriteCount);
//             var result = _slots[index].Value;
//             Thread.MemoryBarrier();
//             var actualWriteCount = _slots[index].WriteCount;

//             if (expectedWriteCount == actualWriteCount) return result;

//             spinner.SpinOnce();
//         }
//     }

//     T Get32() {
//         var spinner = new SpinWait();

//         while (true) {
//             var lastWriteID = Interlocked.Read(ref _lastWriteID);
//             var index = lastWriteID & SlotMask;

//             var expectedWriteCount = Interlocked.Read(ref _slots[index].WriteCount);
//             var result = _slots[index].Value;
//             var actualWriteCount = Interlocked.Read(ref _slots[index].WriteCount);

//             if (expectedWriteCount == actualWriteCount) return result;

//             spinner.SpinOnce();
//         }
//     }

//     [MethodImpl(MethodImplOptions.AggressiveInlining)]
//     // ReSharper disable once InconsistentlySynchronizedField Unsafe method deliberately foregoes synchronization
//     public T GetUnsafe() => _slots[_lastWriteID & SlotMask].Value;

//     public ref T GetUnsafeRef() => ref _slots[_lastWriteID & SlotMask].Value;

//     public void Set(T newValue) {
//         lock (_writeLock) {
//             var nextWriteID = _lastWriteID + 1;
//             var index = nextWriteID & SlotMask;
//             _slots[index].WriteCount++;
//             Thread.MemoryBarrier(); // Ensure that we increment the WriteCount BEFORE we start copying over the new value. This lets readers detect changes
//             _slots[index].Value = newValue;
//             Thread.MemoryBarrier(); // Ensure the copy of the new value is completed before we propagate the new write ID
//             _lastWriteID = nextWriteID;
//         }
//     }

//     [MethodImpl(MethodImplOptions.AggressiveInlining)]
//     // ReSharper disable once InconsistentlySynchronizedField Unsafe method deliberately foregoes synchronization
//     public void SetUnsafe(T newValue) => _slots[_lastWriteID & SlotMask].Value = newValue;

//     [MethodImpl(MethodImplOptions.AggressiveInlining)]
//     public T Swap(T newValue) {
//         lock (_writeLock) {
//             var oldValue = GetUnsafe();
//             Set(newValue);
//             return oldValue;
//         }
//     }

//     [MethodImpl(MethodImplOptions.AggressiveInlining)]
//     public T TrySwap(T newValue, T comparand) {
//         lock (_writeLock) {
//             var oldValue = GetUnsafe();
//             if (oldValue.Equals(comparand)) Set(newValue);
//             return oldValue;
//         }
//     }

//     public override string ToString() => Get().ToString();

//     #region Equality
//     [MethodImpl(MethodImplOptions.AggressiveInlining)]
//     public bool Equals(T other) => Get().Equals(other);

//     public override bool Equals(object obj) {
//         if (obj is T value) return Equals(value);
//         return ReferenceEquals(this, obj);
//     }

//     // ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode Base GetHashCode() is appropriate here.
//     public override int GetHashCode() => base.GetHashCode();

//     [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool operator ==(LockFreeReadsValue<T> left, T right) => left?.Equals(right) ?? false;
//     [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool operator !=(LockFreeReadsValue<T> left, T right) => !(left == right);
//     [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool operator ==(T left, LockFreeReadsValue<T> right) => right?.Equals(left) ?? false;
//     [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool operator !=(T left, LockFreeReadsValue<T> right) => !(right == left);
//     #endregion
// }

// public interface INonLockingAtomic<T> : IAtomic<T> {
// 		T GetUnsafe();
// 		void SetUnsafe(T newValue);
// 		ref T GetUnsafeRef();

// 		T Swap(T newValue);
// 		T TrySwap(T newValue, T comparand);
// 	}

// public interface ILockingAtomic<T> : IAtomic<T> {
// 		void Set(T newValue, out T previousValue);
// 		T Set(Func<T, T> valueMapFunc);
// 		T Set(Func<T, T> valueMapFunc, out T previousValue);

// 		// May be overkill in some situations, but I think its presence
// 		// may help protect against accidental check-then-get mistakes a bit
// 		// e.g. if (myAtomic.Value.X) DoStuff(myAtomic.Value)
// 		bool TryGet(Func<T, bool> valueComparisonPredicate, out T currentValue);

// 		bool TrySet(T newValue, Func<T, bool> setPredicate);
// 		bool TrySet(T newValue, Func<T, bool> setPredicate, out T previousValue);
// 		bool TrySet(Func<T, T> valueMapFunc, Func<T, bool> setPredicate);
// 		bool TrySet(Func<T, T> valueMapFunc, Func<T, bool> setPredicate, out T previousValue);
// 		bool TrySet(Func<T, T> valueMapFunc, Func<T, bool> setPredicate, out T previousValue, out T newValue);
// 		bool TrySet(Func<T, T> valueMapFunc, Func<T, T, bool> setPredicate);
// 		bool TrySet(Func<T, T> valueMapFunc, Func<T, T, bool> setPredicate, out T previousValue);
// 		bool TrySet(Func<T, T> valueMapFunc, Func<T, T, bool> setPredicate, out T previousValue, out T newValue);
// 	}

// public interface IAtomic<T> : IEquatable<T> {
//     T Value { get; set; }

//     T Get();
//     void Set(T newValue);
// }

// public struct ExchangeResult<T> : IEquatable<ExchangeResult<T>> {
// 		public readonly T PreviousValue;
// 		public readonly T NewValue;

// 		public ExchangeResult(T previousValue, T newValue) {
// 			PreviousValue = previousValue;
// 			NewValue = newValue;
// 		}

// 		public override string ToString() {
// 			return $"{(PreviousValue != null ? PreviousValue.ToString() : "<null>")} => " +
// 				   $"{(NewValue != null ? NewValue.ToString() : "<null>")}";
// 		}

// 		public bool Equals(ExchangeResult<T> other) {
// 			return EqualityComparer<T>.Default.Equals(PreviousValue, other.PreviousValue) && EqualityComparer<T>.Default.Equals(NewValue, other.NewValue);
// 		}

// 		public override bool Equals(object obj) {
// 			return obj is ExchangeResult<T> other && Equals(other);
// 		}

// 		public override int GetHashCode() {
// 			unchecked {
// 				return (EqualityComparer<T>.Default.GetHashCode(PreviousValue) * 397) ^ EqualityComparer<T>.Default.GetHashCode(NewValue);
// 			}
// 		}

// 		public static bool operator ==(ExchangeResult<T> left, ExchangeResult<T> right) => left.Equals(right);
// 		public static bool operator !=(ExchangeResult<T> left, ExchangeResult<T> right) => !left.Equals(right);
// 	}

// static class AtomicUtils {
//     public const string NullValueString = "<null>";

//     [StructLayout(LayoutKind.Explicit)]
//     public struct Union<T1, T2> where T1 : unmanaged where T2 : unmanaged {
//         [FieldOffset(0)]
//         public T1 AsTypeOne;

//         [FieldOffset(0)]
//         public T2 AsTypeTwo;

//         [MethodImpl(MethodImplOptions.AggressiveInlining)]
//         public Union(T1 asTypeOne) : this() => AsTypeOne = asTypeOne;

//         [MethodImpl(MethodImplOptions.AggressiveInlining)]
//         public Union(T2 asTypeTwo) : this() => AsTypeTwo = asTypeTwo;

//         [MethodImpl(MethodImplOptions.AggressiveInlining)]
//         public static implicit operator T1(Union<T1, T2> operand) => operand.AsTypeOne;

//         [MethodImpl(MethodImplOptions.AggressiveInlining)]
//         public static implicit operator T2(Union<T1, T2> operand) => operand.AsTypeTwo;

//         [MethodImpl(MethodImplOptions.AggressiveInlining)]
//         public static implicit operator Union<T1, T2>(T1 operand) => new Union<T1, T2>(operand);

//         [MethodImpl(MethodImplOptions.AggressiveInlining)]
//         public static implicit operator Union<T1, T2>(T2 operand) => new Union<T1, T2>(operand);
//     }
// }

// public sealed class AtomicSwitch : INonLockingAtomic<bool> {
//     IntBool _value;

//     public bool IsFlipped {
//         [MethodImpl(MethodImplOptions.AggressiveInlining)]
//         get => VolatileRead();
//         [MethodImpl(MethodImplOptions.AggressiveInlining)]
//         set => VolatileWrite(value);
//     }

//     bool IAtomic<bool>.Value {
//         [MethodImpl(MethodImplOptions.AggressiveInlining)]
//         get => IsFlipped;
//         [MethodImpl(MethodImplOptions.AggressiveInlining)]
//         set => IsFlipped = value;
//     }

//     [MethodImpl(MethodImplOptions.AggressiveInlining)]
//     public AtomicSwitch() : this(default) { }

//     [MethodImpl(MethodImplOptions.AggressiveInlining)]
//     public AtomicSwitch(bool isFlipped) => VolatileWrite(isFlipped);

//     [MethodImpl(MethodImplOptions.AggressiveInlining)]
//     bool VolatileRead() => (IntBool) Volatile.Read(ref _value.AsTypeOne);

//     [MethodImpl(MethodImplOptions.AggressiveInlining)]
//     void VolatileWrite(bool newValue) => Volatile.Write(ref _value.AsTypeOne, (IntBool) newValue);

//     [MethodImpl(MethodImplOptions.AggressiveInlining)]
//     bool CompareExchange(bool newValue, bool comparand) => (IntBool) Interlocked.CompareExchange(ref _value.AsTypeOne, (IntBool) newValue, (IntBool) comparand);

//     [MethodImpl(MethodImplOptions.AggressiveInlining)]
//     bool Exchange(bool newValue) => (IntBool) Interlocked.Exchange(ref _value.AsTypeOne, (IntBool) newValue);

//     // AtomicSwitch implementation
//     public bool FlipAndGet() {
//         var spinner = new SpinWait();
//         while (true) {
//             if (TryFlip(true)) return true;
//             if (TryFlip(false)) return false;
//             spinner.SpinOnce();
//         }
//     }

//     [MethodImpl(MethodImplOptions.AggressiveInlining)]
//     public bool TryFlip(bool desiredFlipState) => CompareExchange(desiredFlipState, !desiredFlipState);

//     [MethodImpl(MethodImplOptions.AggressiveInlining)]
//     public bool SetAndGetPreviousFlipState(bool newFlipState) => Exchange(newFlipState);

//     // Interface implementation
//     [MethodImpl(MethodImplOptions.AggressiveInlining)]
//     public bool Get() => VolatileRead();

//     [MethodImpl(MethodImplOptions.AggressiveInlining)]
//     public void Set(bool isFlipped) => VolatileWrite(isFlipped);

//     [MethodImpl(MethodImplOptions.AggressiveInlining)]
//     public bool GetUnsafe() => _value;

//     [MethodImpl(MethodImplOptions.AggressiveInlining)]
//     public void SetUnsafe(bool isFlipped) => _value = isFlipped;

//     [MethodImpl(MethodImplOptions.AggressiveInlining)]
//     public ref bool GetUnsafeRef() => ref _value.AsTypeTwo;

//     // Note: Hidden because it's slightly confusing in this class. "Swap" clashes too strongly with "flip", but does something different.
//     [MethodImpl(MethodImplOptions.AggressiveInlining)]
//     bool INonLockingAtomic<bool>.Swap(bool isFlipped) => Exchange(isFlipped);

//     // Note: Hidden because it's confusing as fuck. The return value is the previously set value (like usual) but on this class in particular
//     // it's easy to misunderstand and assume it's some kind of "was set successfully" return value.
//     // Also... I'm not really sure what you'd ever want this for (watch someone complain now...).
//     [MethodImpl(MethodImplOptions.AggressiveInlining)]
//     bool INonLockingAtomic<bool>.TrySwap(bool newValue, bool comparand) => CompareExchange(newValue, comparand);

//     [MethodImpl(MethodImplOptions.AggressiveInlining)]
//     public override string ToString() => IsFlipped ? "Switch (Flipped)" : "Switch (Not flipped)";

//     #region Equality
//     [MethodImpl(MethodImplOptions.AggressiveInlining)]
//     public bool Equals(bool other) => other == Get();

//     public override bool Equals(object obj) {
//         if (obj is bool value) return Equals(value);
//         return ReferenceEquals(this, obj);
//     }

//     // ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode Base GetHashCode() is appropriate here.
//     public override int GetHashCode() => base.GetHashCode();

//     [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool operator ==(AtomicSwitch left, bool right) => left?.Equals(right) ?? false;
//     [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool operator !=(AtomicSwitch left, bool right) => !(left == right);
//     [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool operator ==(bool left, AtomicSwitch right) => right?.Equals(left) ?? false;
//     [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool operator !=(bool left, AtomicSwitch right) => !(right == left);
//     #endregion
// }

// public sealed unsafe class AtomicPtr<T> : INonLockingAtomic<IntPtr> where T : unmanaged {
//     [StructLayout(LayoutKind.Explicit)]
//     struct PtrUnion {
//         // Note: Don't use AsTypedPtr except for the unsafe methods. It's only necessary to allow creating an unsafe ref
//         // to a variable of type T*. The memory barriers only sync up with each other when accessing the same variable
//         // (from the point of view of the compiler). The CPU will see these two union vars as the same memory address but I'm not
//         // sure that C# will. I'm also fairly sure that C# orders around memfences according to their emission in program order
//         // and not the variable they access... But why risk it?
//         [FieldOffset(0)]
//         public T* AsTypedPtr; 
//         [FieldOffset(0)]
//         public IntPtr AsIntPtr;
//     }
//     PtrUnion _value;

//     public T* Value {
//         [MethodImpl(MethodImplOptions.AggressiveInlining)] get => Get();
//         [MethodImpl(MethodImplOptions.AggressiveInlining)] set => Set(value);
//     }
//     IntPtr IAtomic<IntPtr>.Value {
//         [MethodImpl(MethodImplOptions.AggressiveInlining)] get => GetAsIntPtr();
//         [MethodImpl(MethodImplOptions.AggressiveInlining)] set => SetAsIntPtr(value);
//     }

//     [MethodImpl(MethodImplOptions.AggressiveInlining)] public AtomicPtr() : this(default(T*)) { }
//     [MethodImpl(MethodImplOptions.AggressiveInlining)] public AtomicPtr(IntPtr initialValue) => SetAsIntPtr(initialValue);
//     [MethodImpl(MethodImplOptions.AggressiveInlining)] public AtomicPtr(T* initialValue) => Set(initialValue);

//     [MethodImpl(MethodImplOptions.AggressiveInlining)] IntPtr GetAsIntPtr() => Volatile.Read(ref _value.AsIntPtr);
//     [MethodImpl(MethodImplOptions.AggressiveInlining)] void SetAsIntPtr(IntPtr v) => Volatile.Write(ref _value.AsIntPtr, v);

//     [MethodImpl(MethodImplOptions.AggressiveInlining)] public T* Get() => (T*) GetAsIntPtr();
//     [MethodImpl(MethodImplOptions.AggressiveInlining)] public void Set(T* newValue) => SetAsIntPtr((IntPtr) newValue);
//     [MethodImpl(MethodImplOptions.AggressiveInlining)] IntPtr IAtomic<IntPtr>.Get() => GetAsIntPtr();
//     [MethodImpl(MethodImplOptions.AggressiveInlining)] void IAtomic<IntPtr>.Set(IntPtr newValue) => SetAsIntPtr(newValue);

//     [MethodImpl(MethodImplOptions.AggressiveInlining)] public T* GetUnsafe() => _value.AsTypedPtr;
//     [MethodImpl(MethodImplOptions.AggressiveInlining)] public void SetUnsafe(T* newValue) => _value.AsTypedPtr = newValue;
//     [MethodImpl(MethodImplOptions.AggressiveInlining)] IntPtr INonLockingAtomic<IntPtr>.GetUnsafe() => _value.AsIntPtr;
//     [MethodImpl(MethodImplOptions.AggressiveInlining)] void INonLockingAtomic<IntPtr>.SetUnsafe(IntPtr newValue) => _value.AsIntPtr = newValue;

//     [MethodImpl(MethodImplOptions.AggressiveInlining)] public ref T* GetUnsafeRef() => ref _value.AsTypedPtr;
//     [MethodImpl(MethodImplOptions.AggressiveInlining)] ref IntPtr INonLockingAtomic<IntPtr>.GetUnsafeRef() => ref _value.AsIntPtr;

//     [MethodImpl(MethodImplOptions.AggressiveInlining)] public T* Swap(T* newValue) => (T*) Interlocked.Exchange(ref _value.AsIntPtr, (IntPtr) newValue);
//     [MethodImpl(MethodImplOptions.AggressiveInlining)] IntPtr INonLockingAtomic<IntPtr>.Swap(IntPtr newValue) => Interlocked.Exchange(ref _value.AsIntPtr, newValue);

//     [MethodImpl(MethodImplOptions.AggressiveInlining)] public T* TrySwap(T* newValue, T* comparand) => (T*) Interlocked.CompareExchange(ref _value.AsIntPtr, (IntPtr) newValue, (IntPtr) comparand);
//     [MethodImpl(MethodImplOptions.AggressiveInlining)] IntPtr INonLockingAtomic<IntPtr>.TrySwap(IntPtr newValue, IntPtr comparand) => Interlocked.CompareExchange(ref _value.AsIntPtr, newValue, comparand);

//     public override string ToString() => GetAsIntPtr().ToString("x");

//     #region Equality
//     [MethodImpl(MethodImplOptions.AggressiveInlining)]
//     public bool Equals(T* other) => Value == other;

//     [MethodImpl(MethodImplOptions.AggressiveInlining)]
//     public bool Equals(void* other) => Value == other;

//     [MethodImpl(MethodImplOptions.AggressiveInlining)]
//     public bool Equals(IntPtr other) => GetAsIntPtr() == other;

//     public override bool Equals(object obj) {
//         if (obj is IntPtr value) return Equals(value);
//         return ReferenceEquals(this, obj);
//     }

//     // ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode Base GetHashCode() is appropriate here.
//     public override int GetHashCode() => base.GetHashCode();

//     [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool operator ==(AtomicPtr<T> left, T* right) => left?.Equals(right) ?? false;
//     [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool operator !=(AtomicPtr<T> left, T* right) => !(left == right);
//     [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool operator ==(T* left, AtomicPtr<T> right) => right?.Equals(left) ?? false;
//     [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool operator !=(T* left, AtomicPtr<T> right) => !(right == left);
//     [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool operator ==(AtomicPtr<T> left, IntPtr right) => left?.Equals(right) ?? false;
//     [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool operator !=(AtomicPtr<T> left, IntPtr right) => !(left == right);
//     [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool operator ==(IntPtr left, AtomicPtr<T> right) => right?.Equals(left) ?? false;
//     [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool operator !=(IntPtr left, AtomicPtr<T> right) => !(right == left);
//     [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool operator ==(AtomicPtr<T> left, void* right) => left?.Equals(right) ?? false;
//     [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool operator !=(AtomicPtr<T> left, void* right) => !(left == right);
//     [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool operator ==(void* left, AtomicPtr<T> right) => right?.Equals(left) ?? false;
//     [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool operator !=(void* left, AtomicPtr<T> right) => !(right == left);
//     #endregion

// }

// public sealed class Atomic<T> : ILockingAtomic<T> {
// 		readonly object _instanceMutationLock = new object();
// 		T _value;

// 		public T Value {
// 			get {
// 				lock (_instanceMutationLock) return _value;
// 			}
// 			set {
// 				lock (_instanceMutationLock) _value = value;
// 			}
// 		}

// 		public Atomic() : this(default) { }
// 		public Atomic(T value) => _value = value;

// 		[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 		public T Get() => Value;

// 		[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 		public void Set(T newValue) => Value = newValue;

// 		public void Set(T newValue, out T previousValue) {
// 			lock (_instanceMutationLock) {
// 				previousValue = _value;
// 				_value = newValue;
// 			}
// 		}

// 		public T Set(Func<T, T> valueMapFunc) {
// 			lock (_instanceMutationLock) {
// 				return _value = valueMapFunc(_value);
// 			}
// 		}

// 		public T Set(Func<T, T> valueMapFunc, out T previousValue) {
// 			lock (_instanceMutationLock) {
// 				previousValue = _value;
// 				return _value = valueMapFunc(_value);
// 			}
// 		}

// 		public bool TryGet(Func<T, bool> valueComparisonPredicate, out T currentValue) {
// 			lock (_instanceMutationLock) {
// 				currentValue = _value;
// 			}
// 			return valueComparisonPredicate(currentValue);
// 		}

// 		[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 		public bool TrySet(T newValue, Func<T, bool> setPredicate) => TrySet(newValue, setPredicate, out _);
// 		public bool TrySet(T newValue, Func<T, bool> setPredicate, out T previousValue) {
// 			lock (_instanceMutationLock) {
// 				previousValue = _value;
// 				if (setPredicate(_value)) {
// 					_value = newValue;
// 					return true;
// 				}
// 				else return false;
// 			}
// 		}

// 		[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 		public bool TrySet(Func<T, T> valueMapFunc, Func<T, bool> setPredicate) => TrySet(valueMapFunc, setPredicate, out _);
// 		[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 		public bool TrySet(Func<T, T> valueMapFunc, Func<T, bool> setPredicate, out T previousValue) => TrySet(valueMapFunc, setPredicate, out previousValue, out _);
// 		public bool TrySet(Func<T, T> valueMapFunc, Func<T, bool> setPredicate, out T previousValue, out T newValue) {
// 			lock (_instanceMutationLock) {
// 				previousValue = _value;
// 				if (setPredicate(_value)) {
// 					newValue = _value = valueMapFunc(_value);
// 					return true;
// 				}
// 				else {
// 					newValue = previousValue;
// 					return false;
// 				}
// 			}
// 		}

// 		[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 		public bool TrySet(Func<T, T> valueMapFunc, Func<T, T, bool> setPredicate) => TrySet(valueMapFunc, setPredicate, out _);
// 		[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 		public bool TrySet(Func<T, T> valueMapFunc, Func<T, T, bool> setPredicate, out T previousValue) => TrySet(valueMapFunc, setPredicate, out previousValue, out _);
// 		public bool TrySet(Func<T, T> valueMapFunc, Func<T, T, bool> setPredicate, out T previousValue, out T newValue) {
// 			lock (_instanceMutationLock) {
// 				previousValue = _value;
// 				var potentialNewValue = valueMapFunc(previousValue);
// 				if (setPredicate(_value, potentialNewValue)) {
// 					newValue = _value = potentialNewValue;
// 					return true;
// 				}
// 				else {
// 					newValue = previousValue;
// 					return false;
// 				}
// 			}
// 		}

// 		public override string ToString() => Value?.ToString() ?? AtomicUtils.NullValueString;

// 		#region Equality
// 		[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 		public bool Equals(T other) => EqualityComparer<T>.Default.Equals(Value, other);

// 		public override bool Equals(object obj) {
// 			if (obj is T value) return Equals(value);
// 			return ReferenceEquals(this, obj);
// 		}

// 		// ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode Base GetHashCode() is appropriate here.
// 		public override int GetHashCode() => base.GetHashCode();

// 		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool operator ==(Atomic<T> left, T right) => left?.Equals(right) ?? false;
// 		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool operator !=(Atomic<T> left, T right) => !(left == right);
// 		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool operator ==(T left, Atomic<T> right) => right?.Equals(left) ?? false;
// 		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool operator !=(T left, Atomic<T> right) => !(right == left);
// 		#endregion
// 	}

// #pragma warning restore
