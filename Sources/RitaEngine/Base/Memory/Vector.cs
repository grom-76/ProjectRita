namespace RitaEngine.Base.Containers;



/// <summary>
/// Replace System.Generic.Collection.List
/// Much faster, but same method
/// </summary>
/// <typeparam name="T"></typeparam>
[StructLayout(LayoutKind.Sequential, Pack = 4 )]
public struct Vector<T>  : IDisposable , IEquatable< Vector<T> >
{
    private T[] array ;
    private int index ;
    private int capacity ;
    // private string Type;

    /// <summary>
    /// Return number of item in List
    /// </summary>
    public int Count => index;

    /// <summary>
    /// Indique le nombre maxi d'item a la creation
    /// </summary>
    /// <param name="c"></param>
    public Vector( int c=4) => (index,capacity,array) = (0,c, System.GC.AllocateArray<T>(c,false));

    /// <summary>
    /// creer une list de 4 elements vide
    /// </summary>
    public Vector() => (index,capacity,array) = (0,4, System.GC.AllocateArray<T>(4,false));

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    // [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Add( [NotNull] T value) 
    {
        if (index >= array.Length)
            Expand();

        array[index++] = value;
    }

    public void Push(T value)=> Add(value);

    /// <summary>
    /// clear value but capicity is alway keep
    /// </summary>
    public void Clear()
    {
        Array.Clear( array,0 , capacity);
        index = 0;
    }

    // public T Get(int index)
    //     => array[index];

    // public void Set(int index, T value)
    //     => array[index] = value;

    private void Expand()
    {
        var newCapacity = array.Length * 2;

        T[] newArray = new T[newCapacity];
        Array.Copy(array, newArray, array.Length);
        // Unsafe.CopyBlock(ref)
        array = newArray;

        capacity = newCapacity;
    }

    /// <summary>
    /// Write like array  myarray[index]
    /// </summary>
    /// <value></value>
    public T this[int index]
    {
        get  =>  array[ index ];
        set  => array[ index ] = value;
    }

    // Obsolete do not use this 
    // public ref T Current
    // {
    //     get
    //     {
    //         if (array is null || index < 0 || index > capacity)
    //         {
    //             throw new InvalidOperationException();
    //         }
    //         return ref array[Offset+index];
    //     }
    // }

    /// <summary>
    /// swap between two value in array
    /// </summary>
    /// <param name="indexA"></param>
    /// <param name="indexB"></param>
    public void Swap( int indexA, int indexB)
    {
        T temp        = array[indexA];
        array[indexA] = array[indexB];
        array[indexB] = temp;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Array.Clear( array,0, capacity);
        array = null!;
        capacity =0;
        index =0;
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// fill all the array with one value 
    /// </summary>
    /// <param name="value"></param>
    public void Fill( T value)  => Array.Fill(array, value);

    /// <summary>
    /// remove an item at index and refill array 
    /// </summary>
    /// <param name="index"></param>
    public void RemoveAt(int index)
    {
        this.index--;
        if ( index < this.index)
        {
            Array.Copy(array,index+1,array,index, this.index - index);
        }
        array[this.index] = default!;// vide 
    }

    /// <summary>
    /// find index of item
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public int IndexOf(T item)
        => Array.IndexOf(array, item, 0, Count);

    
    /// <summary>
    /// romve an item if exist and use removeat function
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool Remove(T item)
    {
        var index = IndexOf(item);
        if (index >= 0)
        {
            RemoveAt(index);
            return true;
        }
        return false;
    }

    public bool Pop(T item)=> Remove(item);//compatibilité with c name

    /// <summary> Find an item with predicat like x => x== value </summary>
    /// <param name="match"> like  x => x.value.Equal(done)</param>
    /// <returns>an item or deault</returns>
    public T Find(Predicate<T> match)
    {
        for (var i = 0; i < Count; i++)
        {
            if (match(array[i]))
            {
                return array[i];
            }
        }
        return default!;
    }

    /// <summary>
    /// find all items coresponding at predicat (ex: x => x > value )
    /// </summary>
    /// <param name="match"></param>
    /// <returns></returns>
    public System.Collections.IEnumerable FindMany(Predicate<T> match)
    {
        for( var i =0; i < Count;i++)
        {
            if (match(array[i]))
            {
                yield return array[i];
            }
        }
    }

    /// <summary>
    /// return all values sous forme de tableau 
    /// </summary>
    /// <returns></returns>
    public  T[] ToArray() => array;

    /// <summary>
    /// aarrange all item by value plus petit au plus grand 
    /// </summary>
    public void Sort()   => Array.Sort(array);

    public override string ToString()   =>  "List  count :" + Count.ToString() +" items";
    public override int GetHashCode()   =>HashCode.Combine ( capacity , Count );
    public override bool Equals(object? obj) => obj is Vector<T> data && this.Equals(data) ;
    public bool Equals(Vector<T> other)=>    capacity == other.capacity && Count == other.Count  ;
    public static bool operator ==(Vector<T> left, Vector<T> right) => left.Equals(right);
    public static bool operator !=(Vector<T> left, Vector<T>  right) => !left.Equals(right) ;

    // public List<T>  GetWithOffset(int offset)
    // {
    //     Offset += offset;
    //     return this;
    // }

// public static List<T> operator + (List<T> p, int offset)
    //     => 	p.GetWithOffset(offset);

    // public static List<T> operator ++ (List<T>  p)
    //     => p.GetWithOffset(1);

    // public bool MoveNext() => ++index < capacity;
    
    // public void Reset() => index = -1;
// OBSOLETE NE PAS UTILISER ( FOREACh TRES LENT)
//         /// <summary>
//         /// Need For Foreach implementation
//         /// this is very slow use : for( int i=0; i< refList.Count; i++)
//         /// </summary>
//         /// <returns></returns>
//         // public RefEnumerator GetEnumerator() => new RefEnumerator(array, capacity);
//         // //IEnumerator IEnumerable.GetEnumerator() => throw new NotSupportedException();

//         // public struct RefEnumerator
//         // {
//         //     private T[] array;

//         //     private int index;
//         //     private int capacity;

//         //     public RefEnumerator(T[] target, int capacity)
//         //     {
//         //         array = target;
//         //         index = -1;
//         //         this.capacity = capacity;
//         //     }

//         //     public ref T Current
//         //     {
//         //         get
//         //         {
//         //             if (array is null || index < 0 || index > capacity)
//         //             {
//         //                 throw new InvalidOperationException();
//         //             }
//         //             return ref array[index];
//         //         }
//         //     }

//         //     public void Dispose(){}

//         //     public bool MoveNext() => ++index < capacity;

//         //     public void Reset() => index = -1;
//         // }

}


/// <summary>
    /// Used for transfering data to the CPP libraries https://github.com/tortuga-foundation/tortuga/blob/master/Tortuga.Core/Utils/NativeList.cs
    /// </summary>
//     public unsafe class VectorNativeList<T> : IDisposable where T : struct
//     {
//         private byte* _dataPtr;
//         private uint _elementCapacity;
//         private uint _count;

//         public const int DefaultCapacity = 4;
//         private const float GrowthFactor = 2f;
//         private static readonly uint s_elementByteSize = InitializeTypeSize();

//         public VectorNativeList() : this(DefaultCapacity) { }
//         public VectorNativeList(uint capacity)
//         {
//             Allocate(capacity);
//         }

//         public VectorNativeList(uint capacity, uint count)
//         {
//             Allocate(capacity);
//             _count = count;
//         }

//         public VectorNativeList(VectorNativeList<T> existingList)
//         {
//             Allocate(existingList._elementCapacity);
//             Unsafe.CopyBlock(_dataPtr, existingList._dataPtr, existingList._count * s_elementByteSize);
//         }

//         public IntPtr Data
//         {
//             get
//             {
//                 ThrowIfDisposed();
//                 return new IntPtr(_dataPtr);
//             }
//         }

//         public uint Count
//         {
//             get
//             {
//                 ThrowIfDisposed();
//                 return _count;
//             }
//             set
//             {
//                 ThrowIfDisposed();
//                 if (value > _elementCapacity)
//                 {
//                     uint newLements = value - Count;
//                     CoreResize(value);
//                     Unsafe.InitBlock(_dataPtr + _count * s_elementByteSize, 0, newLements * s_elementByteSize);
//                 }

//                 _count = value;
//             }
//         }

//         public ref T this[uint index]
//         {
//             get
//             {
//                 ThrowIfDisposed();

//                 if (index >= _count)
//                 {
//                     throw new ArgumentOutOfRangeException(nameof(index));
//                 }

//                 return ref Unsafe.AsRef<T>(_dataPtr + index * s_elementByteSize);
//             }
//         }

//         public ref T this[int index]
//         {
//             get
//             {
//                 ThrowIfDisposed();

//                 if (index < 0 || index >= _count)
//                 {
//                     throw new ArgumentOutOfRangeException(nameof(index));
//                 }

//                 return ref Unsafe.AsRef<T>(_dataPtr + index * s_elementByteSize);
//             }
//         }

//         // public ReadOnlyNativeListView<T> GetReadOnlyView()
//         // {
//         //     ThrowIfDisposed();
//         //     return new ReadOnlyNativeListView<T>(this, 0, _count);
//         // }

//         // public ReadOnlyNativeListView<T> GetReadOnlyView(uint start, uint count)
//         // {
//         //     ThrowIfDisposed();

//         //     if (start + count > _count)
//         //     {
//         //         throw new ArgumentOutOfRangeException();
//         //     }

//         //     return new ReadOnlyNativeListView<T>(this, start, count);
//         // }

//         // public View<ViewType> GetView<ViewType>() where ViewType : struct
//         // {
//         //     ThrowIfDisposed();
//         //     return new View<ViewType>(this);
//         // }

//         public bool IsDisposed => _dataPtr == null;

//         public void Add(ref T item)
//         {
//             ThrowIfDisposed();
//             if (_count == _elementCapacity)
//             {
//                 CoreResize((uint)(_elementCapacity * GrowthFactor));
//             }

//             Unsafe.Copy(_dataPtr + _count * s_elementByteSize, ref item);
//             _count += 1;
//         }

//         public void Add(T item)
//         {
//             ThrowIfDisposed();
//             if (_count == _elementCapacity)
//             {
//                 CoreResize((uint)(_elementCapacity * GrowthFactor));
//             }

//             Unsafe.Write(_dataPtr + _count * s_elementByteSize, item);
//             _count += 1;
//         }

//         public void Add(void* data, uint numElements)
//         {
//             ThrowIfDisposed();
//             uint needed = _count + numElements;
//             if (numElements > _elementCapacity)
//             {
//                 CoreResize((uint)(needed * GrowthFactor));
//             }

//             Unsafe.CopyBlock(_dataPtr + _count * s_elementByteSize, data, numElements * s_elementByteSize);
//             _count += numElements;
//         }

//         public bool Remove(ref T item)
//         {
//             ThrowIfDisposed();
//             bool result = IndexOf(ref item, out uint index);
//             if (result)
//             {
//                 CoreRemoveAt(index);
//             }

//             return result;
//         }

//         public bool Remove(T item) => Remove(ref item);

//         public void RemoveAt(uint index)
//         {
//             ThrowIfDisposed();

//             if (index >= _count)
//             {
//                 throw new ArgumentOutOfRangeException(nameof(index));
//             }

//             CoreRemoveAt(index);
//         }

//         public void Clear()
//         {
//             ThrowIfDisposed();
//             _count = 0;
//         }

//         public bool IndexOf(ref T item, out uint index)
//         {
//             ThrowIfDisposed();
//             byte* itemPtr = (byte*)Unsafe.AsPointer(ref item);
//             for (index = 0; index < _count; index++)
//             {
//                 byte* ptr = _dataPtr + index * s_elementByteSize;
//                 if (Equals(ptr, itemPtr, s_elementByteSize))
//                 {
//                     return true;
//                 }
//             }

//             return false;
//         }

//         public bool IndexOf(T item, out uint index)
//         {
//             ThrowIfDisposed();
//             byte* itemPtr = (byte*)Unsafe.AsPointer(ref item);
//             for (index = 0; index < _count; index++)
//             {
//                 byte* ptr = _dataPtr + index * s_elementByteSize;
//                 if (Equals(ptr, itemPtr, s_elementByteSize))
//                 {
//                     return true;
//                 }
//             }

//             return false;
//         }

//         public IntPtr GetAddress(uint index)
//         {
//             ThrowIfDisposed();

//             if (index >= _count)
//             {
//                 throw new ArgumentOutOfRangeException(nameof(index));
//             }

//             return new IntPtr(_dataPtr + (index * s_elementByteSize));
//         }

//         public void Resize(uint elementCount)
//         {
//             ThrowIfDisposed();
//             CoreResize(elementCount);
//             if (_elementCapacity < _count)
//             {
//                 _count = _elementCapacity;
//             }
//         }

//         private static uint InitializeTypeSize()
//         {

//             return (uint)Unsafe.SizeOf<T>();
//         }

//         private void CoreResize(uint elementCount)
//         {
//             _dataPtr = (byte*)Marshal.ReAllocHGlobal(new IntPtr(_dataPtr), (IntPtr)(elementCount * s_elementByteSize));
//             _elementCapacity = elementCount;
//         }

//         private void Allocate(uint elementCount)
//         {
//             _dataPtr = (byte*)Marshal.AllocHGlobal((int)(elementCount * s_elementByteSize));
//             _elementCapacity = elementCount;
//         }

//         private bool Equals(byte* ptr, byte* itemPtr, uint s_elementByteSize)
//         {
//             for (int i = 0; i < s_elementByteSize; i++)
//             {
//                 if (ptr[i] != itemPtr[i])
//                 {
//                     return false;
//                 }
//             }

//             return true;
//         }

//         [MethodImpl(MethodImplOptions.AggressiveInlining)]
//         private void CoreRemoveAt(uint index)
//         {
//             Unsafe.CopyBlock(_dataPtr + index * s_elementByteSize, _dataPtr + (_count - 1) * s_elementByteSize, s_elementByteSize);
//             _count -= 1;
//         }

//         [MethodImpl(MethodImplOptions.AggressiveInlining)]
//         private void ThrowIfDisposed()
//         {

//             if (_dataPtr == null)
//             {
//                 throw new ObjectDisposedException(nameof(Data));
//             }

//         }

//         public void Dispose()
//         {
//             ThrowIfDisposed();
//             Marshal.FreeHGlobal(new IntPtr(_dataPtr));
//             _dataPtr = null;
//         }



//         // public Enumerator GetEnumerator()
//         // {
//         //     ThrowIfDisposed();
//         //     return new Enumerator(_dataPtr, _count);
//         // }

//         // IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

//         // IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

//         // public struct Enumerator : IEnumerator<T>
//         // {
//         //     private byte* _basePtr;
//         //     private uint _count;
//         //     private uint _currentIndex;
//         //     private T _current;

//         //     public Enumerator(byte* basePtr, uint count)
//         //     {
//         //         _basePtr = basePtr;
//         //         _count = count;
//         //         _currentIndex = 0;
//         //         _current = default(T);
//         //     }

//         //     public T Current => _current;
//         //     object IEnumerator.Current => Current;

//         //     public bool MoveNext()
//         //     {
//         //         if (_currentIndex != _count)
//         //         {
//         //             _current = Unsafe.Read<T>(_basePtr + _currentIndex * s_elementByteSize);
//         //             _currentIndex += 1;
//         //             return true;
//         //         }

//         //         return false;
//         //     }

//         //     public void Reset()
//         //     {
//         //         _current = default(T);
//         //         _currentIndex = 0;
//         //     }

//         //     public void Dispose() { }
//         // }

//         // public struct View<ViewType> : IEnumerable<ViewType> where ViewType : struct
//         {
//             private static readonly uint s_elementByteSize = (uint)Unsafe.SizeOf<ViewType>();
//             private readonly NativeList<T> _parent;

//             public View(NativeList<T> parent)
//             {
//                 _parent = parent;
//             }

//             public uint Count => (_parent.Count * NativeList<T>.s_elementByteSize) / s_elementByteSize;

//             public ViewType this[uint index]
//             {
//                 get
//                 {
// #if VALIDATE
//                     if (index >= Count)
//                     {
//                         throw new ArgumentOutOfRangeException(nameof(index));
//                     }
// #else
//                     Debug.Assert(index < Count);
// #endif
//                     return Unsafe.Read<ViewType>(_parent._dataPtr + index * s_elementByteSize);
//                 }
//             }

//             public Enumerator GetEnumerator() => new Enumerator(this);

//             IEnumerator<ViewType> IEnumerable<ViewType>.GetEnumerator() => GetEnumerator();

//             IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

//             public struct Enumerator : IEnumerator<ViewType>
//             {
//                 private View<ViewType> _view;
//                 private uint _currentIndex;
//                 private ViewType _current;

//                 public Enumerator(View<ViewType> view)
//                 {
//                     _view = view;
//                     _currentIndex = 0;
//                     _current = default(ViewType);
//                 }

//                 public ViewType Current => _view[_currentIndex];
//                 object IEnumerator.Current => Current;

//                 public bool MoveNext()
//                 {
//                     if (_currentIndex != _view.Count)
//                     {
//                         _current = _view[_currentIndex];
//                         _currentIndex += 1;
//                         return true;
//                     }

//                     return false;
//                 }

//                 public void Reset()
//                 {
//                     _currentIndex = 0;
//                     _current = default(ViewType);
//                 }

//                 public void Dispose() { }
//             }
//         }
    
//     }

    /// <summary>
    /// Used for transfering data to the CPP libraries
    /// </summary>
//     public struct ReadOnlyNativeListView<T> : IEnumerable<T> where T : struct
//     {
//         private readonly NativeList<T> _list;
//         private readonly uint _start;
//         public readonly uint Count;

//         internal ReadOnlyNativeListView(NativeList<T> list, uint start, uint count)
//         {
//             _list = list;
//             _start = start;
//             Count = count;
//         }

//         public T this[uint index]
//         {
//             get
//             {
// #if VALIDATE
//                 if (index >= Count)
//                 {
//                     throw new ArgumentOutOfRangeException(nameof(index));
//                 }
// #else
//                 Debug.Assert(index < Count);
// #endif
//                 return _list[index + _start];
//             }
//         }

//         public T this[int index]
//         {
//             get
//             {
// #if VALIDATE
//                 if (index < 0 || index >= Count)
//                 {
//                     throw new ArgumentOutOfRangeException(nameof(index));
//                 }
// #else
//                 Debug.Assert(index >= 0 && index < Count);
// #endif
//                 return _list[(uint)index + _start];
//             }
//         }

//         public Enumerator GetEnumerator() => new Enumerator(this);

//         IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

//         IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

//         public struct Enumerator : IEnumerator<T>
//         {
//             private ReadOnlyNativeListView<T> _view;
//             private uint _currentIndex;
//             private T _current;

//             public Enumerator(ReadOnlyNativeListView<T> view)
//             {
//                 _view = view;
//                 _currentIndex = view._start;
//                 _current = default(T);
//             }

//             public T Current => _view[_currentIndex];
//             object IEnumerator.Current => Current;

//             public bool MoveNext()
//             {
//                 if (_currentIndex != _view._start + _view.Count)
//                 {
//                     _current = _view[_currentIndex];
//                     _currentIndex += 1;
//                     return true;
//                 }

//                 return false;
//             }

//             public void Reset()
//             {
//                 _currentIndex = _view._start;
//                 _current = default(T);
//             }

//             public void Dispose() { }
//         }
//     }


