namespace RitaEngine.API.Web;

/*
Before Runing 

Install wasm-tools (root terminal): dotnet workload install wasm-tools --skip-manifest-update

dotnet add package Microsoft.AspNetCore.Components.WebAssembly 
dotnet add package Microsoft.AspNetCore.Components.WebAssembly.DevServer //--version 6.0.0-rc.2.21480.10

Exemple based on : https://github.com/EvergineTeam/dotnet-wasm-samples

https://github.com/EvergineTeam/wasm-native-libraries/tree/main/src

https://github.com/EvergineTeam/dotnet-wasm-samples/tree/main/src

https://github.com/EvergineTeam/WebGPU.NET
*/


// using Microsoft.JSInterop;
// using  System;
// using System.Collections.Generic;

// using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
// // using Microsoft.JSInterop;
// using Microsoft.JSInterop.WebAssembly;


// public class WebProgram
// {
//     private static WebAssemblyRuntime? wasm;

//     public static void Main()
//     {
//         wasm = WebAssemblyRuntime.GetInstance();
//         Console.WriteLine("Wasm Ready!");

//         var box = wasm.GetElementById("box");
//         var style = box?.GetObjectProperty<JSObject>("style");
//         style?.SetObjectProperty("backgroundColor", "green");

//         box?.AddSimpleEventListener("click", new Action(OnBoxClick), false);
//     }

//     public static void OnBoxClick()
//     {
//         wasm?.Invoke("alert", warn: true, "hello!");
//     }
// }

// /// <summary>
// /// Class to encapsulate web assembly operations.
// /// </summary>
// public class WebAssemblyRuntime : IDisposable
// {
//     private static WebAssemblyRuntime? instance = null;
//     private DotNetObjectReference<WebAssemblyRuntime>? dotnetRefrence = null;
//     private Action<double>? requestAnimationFrameAction = null;

//     /// <summary>
//     /// Gets the runtime for interact with JavaScript.
//     /// </summary>
//     public WebAssemblyJSRuntime Runtime { get; private set; }=null!;

//     /// <summary>
//     /// Gets or creates singleton instance.
//     /// </summary>
//     /// <returns>The singleton.</returns>
//     public static WebAssemblyRuntime GetInstance()
//     {
//         if (instance == null)
//         {
//             instance = new WebAssemblyRuntime();
//         }

//         return instance;
//     }

//     /// <summary>
//     /// Invokes the specified JavaScript function synchronously.
//     /// </summary>
//     /// <param name="identifier">JavaScript function.</param>
//     /// <param name="warn">Show warning if function is not defined.</param>
//     /// <param name="args">Function arguments.</param>
//     public void Invoke(string identifier, bool warn = true, params object[] args)
//     {
//         try
//         {
//             this.Runtime.InvokeVoid(identifier, args);
//         }
//         catch (Exception e)
//         {
//             if (warn)
//             {
//                 Console.WriteLine($"WARNING: function {identifier}({string.Join(",", args)}): {e.Message}");
//             }
//         }
//     }

//     /// <summary>
//     /// Invokes the specified JavaScript function synchronously.
//     /// </summary>
//     /// <typeparam name="T">Return type.</typeparam>
//     /// <param name="identifier">JavaScript function.</param>
//     /// <param name="warn">Show warning if function is not defined.</param>
//     /// <param name="args">Function arguments.</param>
//     /// <returns>Function result.</returns>
//     public T? Invoke<T>(string identifier, bool warn = true, params object[] args)
//     {
//         try
//         {
//             if (typeof(T) == typeof(JSObject))
//             {
//                 return (T)Convert.ChangeType(new JSObject(this.Runtime.Invoke<IJSInProcessObjectReference>(identifier, args)), typeof(T));
//             }

//             return this.Runtime.Invoke<T>(identifier, args);
//         }
//         catch (Exception e)
//         {
//             if (warn)
//             {
//                 Console.WriteLine($"WARNING: function {identifier}({string.Join(",", args)}): {e.Message}");
//             }

//             return default(T);
//         }
//     }

//     /// <summary>
//     /// Get global window object.
//     /// </summary>
//     /// <param name="warn">Show warning if property is not defined.</param>
//     /// <returns>window object.</returns>
//     public JSObject? GetGlobalObject(bool warn = true)
//     {
//         return this.Invoke<JSObject>("window._getGlobalObject", warn: warn);
//     }

//     /// <summary>
//     /// Gets an JSObject by its id.
//     /// </summary>
//     /// <param name="id">Id of the object.</param>
//     /// <param name="warn">Show warning if property is not defined.</param>
//     /// <returns>JSObject from the DOM.</returns>
//     public JSObject? GetElementById(string id, bool warn = true)
//     {
//         return this.Invoke<JSObject>("document.getElementById", warn: warn, id);
//     }

//     /// <summary>
//     /// Configures the callback for draw loop and starts it. If callback is null the loop is stopped.
//     /// </summary>
//     /// <param name="callback">Draw loop callback.</param>
//     public void SetRequestAnimationFrameCallback(Action<double> callback)
//     {
//         if (this.dotnetRefrence == null)
//         {
//             this.dotnetRefrence = DotNetObjectReference.Create<WebAssemblyRuntime>(this);
//         }

//         this.Invoke("window._setRequestAnimationFrameCallback", warn: true, this.dotnetRefrence, callback != null ? nameof(WebAssemblyRuntime.InvokeRequestAnimationFrameCallback) : string.Empty);

//         this.requestAnimationFrameAction = callback;
//     }

//     /// <summary>
//     /// Invoke draw call.
//     /// </summary>
//     /// <param name="d">Timestamp.</param>
//     [JSInvokable]
//     public void InvokeRequestAnimationFrameCallback(double d)
//     {
//         this.requestAnimationFrameAction?.Invoke(d);
//     }

//     /// <summary>
//     /// Dispose object.
//     /// </summary>
//     public void Dispose()
//     {
//         this.dotnetRefrence?.Dispose();

//         // TODO delete singleton instance?
//     }

//     private WebAssemblyRuntime()
//     {
//         var args = new string[] { "Web", "WebGL2" };
//         var builder = WebAssemblyHostBuilder.CreateDefault(args);

//         var host = builder.Build();
//         host.RunAsync();
//         var services = host.Services ;
//         // container.GetInstance<UserService>()
        
//         this.Runtime = (WebAssemblyJSRuntime) services.GetService(typeof(IJSRuntime))!;
//         // this.Runtime = (WebAssemblyJSRuntime)host.Services.GetRequiredService<IJSRuntime>();
//     }
// }
// /// <summary>
// /// Represents a reference to a JavaScript object.
// /// </summary>
// public class JSObject : IDisposable
// {
//     private Dictionary<string, Action<JSObject>> activeListeners = new Dictionary<string, Action<JSObject>>();
//     private Dictionary<string, Action> activeSimpleListeners = new Dictionary<string, Action>();
//     private DotNetObjectReference<JSObject>? dotnetRefrence = null;

//     /// <summary>
//     /// Gets the JavaScrtipt reference of the object.
//     /// </summary>
//     public IJSInProcessObjectReference Reference { get; private set; }

//     /// <summary>
//     /// Initializes a new instance of the <see cref="JSObject"/> class.
//     /// </summary>
//     /// <param name="reference">Internal object reference.</param>
//     public JSObject(IJSInProcessObjectReference reference)
//     {
//         this.Reference = reference;
//     }

//     /// <summary>
//     /// Initializes a new instance of the <see cref="JSObject"/> class.
//     /// </summary>
//     /// <param name="reference">Internal object reference.</param>
//     public JSObject(IJSObjectReference reference)
//     {
//         this.Reference = (IJSInProcessObjectReference)reference;
//     }

//     /// <summary>
//     /// Invokes the specified JavaScript function synchronously.
//     /// </summary>
//     /// <param name="identifier">JavaScript function.</param>
//     /// <param name="warn">Show warning if function is not defined.</param>
//     /// <param name="args">Function arguments.</param>
//     public void Invoke(string identifier, bool warn = true, params object[] args)
//     {
//         try
//         {
//             this.Reference.InvokeVoid(identifier, args);
//         }
//         catch (Exception e)
//         {
//             if (warn)
//             {
//                 Console.WriteLine($"WARNING: function {identifier}({string.Join(", ", args)}): {e.Message}");
//             }
//         }
//     }

//     /// <summary>
//     /// Invokes the specified JavaScript function synchronously.
//     /// </summary>
//     /// <typeparam name="T">Return type.</typeparam>
//     /// <param name="identifier">JavaScript function.</param>
//     /// <param name="warn">Show warning if function is not defined.</param>
//     /// <param name="args">Function arguments.</param>
//     /// <returns>Function result. default(T) if it does not exists.</returns>
//     public T? Invoke<T>(string identifier, bool warn = true, params object[] args)
//     {
//         try
//         {
//             if (typeof(T) == typeof(JSObject))
//             {
//                 return (T)Convert.ChangeType(new JSObject(this.Reference.Invoke<IJSInProcessObjectReference>(identifier, args)), typeof(T));
//             }

//             return this.Reference.Invoke<T>(identifier, args);
//         }
//         catch (Exception e)
//         {
//             if (warn)
//             {
//                 Console.WriteLine($"WARNING:function {identifier}({string.Join(",", args)}): {e.Message}");
//             }

//             return default;
//         }
//     }

//     /// <summary>
//     /// Access JSObject property.
//     /// </summary>
//     /// <typeparam name="T">Type of the property.</typeparam>
//     /// <param name="property">Name of the property.</param>
//     /// <param name="warn">Show warning if property is not defined.</param>
//     /// <returns>Property value. Null if it does not exists.</returns>
//     public T? GetObjectProperty<T>(string property, bool warn = true)
//     {
//         var wasm = WebAssemblyRuntime.GetInstance();

//         return wasm.Invoke<T>("window._getObjectProperty", warn: warn, this.Reference, property);
//     }

//     /// <summary>
//     /// Sets JSObject property.
//     /// </summary>
//     /// <typeparam name="T">Type of the property.</typeparam>
//     /// <param name="property">Name of the property.</param>
//     /// <param name="value">Value of the property.</param>
//     /// <param name="warn">Show warning if function is not defined.</param>
//     public void SetObjectProperty<T>(string property, T value, bool warn = true)
//     {
//         var wasm = WebAssemblyRuntime.GetInstance();

//         wasm.Invoke<T>("window._setObjectProperty", warn: warn, this.Reference, property, value!);
//     }

//     /// <summary>
//     /// Add simple event listener to object.
//     /// The listener does not use event information but it the most efficient implementation.
//     /// </summary>
//     /// <param name="eventName">Name of the event that triggers the listener.</param>
//     /// <param name="listener">Action method to be called.</param>
//     /// <param name="options">Optional parameters.</param>
//     public void AddSimpleEventListener(string eventName, Action listener, params object[] options)
//     {
//         var wasm = WebAssemblyRuntime.GetInstance();

//         if (this.dotnetRefrence == null)
//         {
//             this.dotnetRefrence = DotNetObjectReference.Create<JSObject>(this);
//         }

//         wasm.Invoke("window._addSimpleEventListener", warn: true, this.Reference, eventName, this.dotnetRefrence, nameof(JSObject.InvokeEventListenerSimple), options);

//         this.activeSimpleListeners.Add(eventName, listener);
//     }

//     /// <summary>
//     /// Add event listener to object.
//     /// This version is the slowest and should be avoided when event is going to trigger a lot.
//     /// </summary>
//     /// <param name="eventName">Name of the event that triggers the listener.</param>
//     /// <param name="listener">Action method to be called.</param>
//     /// <param name="options">Optional parameters.</param>
//     public void AddEventListener(string eventName, Action<JSObject> listener, params object[] options)
//     {
//         var wasm = WebAssemblyRuntime.GetInstance();

//         if (this.dotnetRefrence == null)
//         {
//             this.dotnetRefrence = DotNetObjectReference.Create<JSObject>(this);
//         }

//         wasm.Invoke("window._addEventListener", warn: true, this.Reference, eventName, this.dotnetRefrence, nameof(JSObject.InvokeEventListener), options);

//         this.activeListeners.Add(eventName, listener);
//     }

//     /// <summary>
//     /// Remove event listener from object.
//     /// </summary>
//     /// <param name="eventName">Name of the event that triggers the listener.</param>
//     /// <param name="options">Optional parameters.</param>
//     public void RemoveEventListener(string eventName, params object[] options)
//     {
//         var wasm = WebAssemblyRuntime.GetInstance();

//         if (this.dotnetRefrence == null)
//         {
//             this.dotnetRefrence = DotNetObjectReference.Create<JSObject>(this);
//         }

//         wasm.Invoke("window._removeEventListener", warn: true, this.Reference, eventName, options);

//         if (this.activeSimpleListeners.ContainsKey(eventName))
//         {
//             this.activeSimpleListeners.Remove(eventName);
//         }
//         else
//         {
//             this.activeListeners.Remove(eventName);
//         }
//     }

//     /// <summary>
//     /// Faster implementation of callback for event listeners, but without event information.
//     /// </summary>
//     /// <param name="eventName">Type of event.</param>
//     [JSInvokable]
//     public void InvokeEventListenerSimple(string eventName)
//     {
//         this.activeSimpleListeners[eventName].Invoke();
//     }

//     /// <summary>
//     /// Callback for event listeners.
//     /// </summary>
//     /// <param name="eventName">Type of event.</param>
//     /// <param name="el">Event element.</param>
//     [JSInvokable]
//     public void InvokeEventListener(string eventName, IJSObjectReference el)
//     {
//         this.activeListeners[eventName].Invoke(new JSObject(el));
//     }

//     /// <summary>
//     /// Dispose the object.
//     /// </summary>
//     public void Dispose()
//     {
//         this.dotnetRefrence?.Dispose();
//     }
// }

// /// <summary>
// /// Represents an array of JSObject.
// /// </summary>
// /// <typeparam name="T">Array type.</typeparam>
// public class JSObjectArray<T> : IDisposable
// {
//     private JSObject jsobject;
//     private int? length = null;

//     /// <summary>
//     /// Initializes a new instance of the <see cref="JSObjectArray{T}"/> class.
//     /// </summary>
//     /// <param name="obj">JSObject that is an array of T.</param>
//     public JSObjectArray(JSObject obj)
//     {
//         this.jsobject = obj;
//     }

//     /// <summary>
//     /// Gets the length of the Array.
//     /// </summary>
//     public int Length
//     {
//         get
//         {
//             if (!this.length.HasValue)
//             {
//                 this.length = this.jsobject.GetObjectProperty<int>("length");
//             }

//             return this.length.Value;
//         }
//     }

//     /// <summary>
//     /// Indexer.
//     /// </summary>
//     /// <param name="i">index.</param>
//     /// <returns>Array object.</returns>
//     public T? this[int i]
//     {
//         get { return this.jsobject.GetObjectProperty<T>(i.ToString()); }
//         set { this.jsobject.SetObjectProperty(i.ToString(), value); }
//     }

//     /// <summary>
//     /// Dispose object.
//     /// </summary>
//     public void Dispose()
//     {
//         this.jsobject.Dispose();
//     }
// }