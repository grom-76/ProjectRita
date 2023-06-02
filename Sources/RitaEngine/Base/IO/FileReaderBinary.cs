

namespace RitaEngine.Base.IO;

          [SkipLocalsInit, StructLayout(LayoutKind.Sequential )]
    public sealed class Reader : IDisposable, IEquatable<Reader>
    {
     private readonly System.IO.FileStream? fs;

    /// <summary>
    /// Constructeur de base passer le nom du fichier à lire 
    /// </summary>
    /// <param name="filename"></param>
    public Reader(string filename)
    {
        fs = new System.IO.FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read);
        Log.Info($"Reader Open: {Name}");
        // Atomics.MemoryStats.Allocated();
    }
        
    private Reader(){} //interdit le constructeur vide

    /// <summary>
    /// move from current postion
    /// use negative position
    /// </summary>
    /// <param name="offset">position</param>
    public void Rewind(in int offset) => fs!.Seek(offset, System.IO.SeekOrigin.Current);

    /// <summary>
    /// move from current position, use positive offset
    /// </summary>
    /// <param name="offset">position</param>
    /// <returns></returns>
    public long Forward(in int offset) => fs!.Seek(offset, System.IO.SeekOrigin.Current);

    /// <summary>
    /// move from origin (0) of file
    /// </summary>
    /// <param name="position"></param>
    public void Seek(in int position) => fs!.Seek(position, System.IO.SeekOrigin.Begin);

    /// <summary>
    /// Retourne le nom du ficheir ouvert sinon null
    /// </summary>
    public string Name => fs!.Name;
    /// <summary>
    /// Retourne la position en bytes du pointeur 
    /// </summary>
    public long Position => fs!.Position;
    /// <summary>
    /// Indique si le pointeur est à la fin du fichier
    /// </summary>
    public bool EOF => fs!.Position == fs.Length;
    /// <summary>
    /// Lecture d'un byte non signé  ( 8 bit, byte , 1 octet ) -255 à + 255
    /// </summary>
    /// <returns></returns>
    public byte Byte() => System.Convert.ToByte(fs!.ReadByte());
    /// <summary>
    /// Lecture d'un byte signé  ( 8 bit, byte , 1 octet ) 0 à 512
    /// </summary>
    /// <returns></returns>
    public sbyte SByte() => System.Convert.ToSByte(fs!.ReadByte());
    /// <summary>
    /// lecture d'un entier court non signé ( short , 2byte , 2octet , 16 bit ) -32767 à 32767
    /// </summary>
    /// <returns></returns>
    public ushort UShort() => (ushort)(Byte() + (Byte() << 8));
    /// <summary>
    /// lecture d'un entier court signé ( ushort , 2byte  , 16 bit ) 0 à 65535
    /// </summary>
    /// <returns></returns>
    public short Short() => (short)(SByte() + (SByte() << 8));
    /// <summary>
    /// lecture d'un entier 32 bit int non signé  ( int , 4bytes, 32 bits) 
    /// </summary>
    /// <returns></returns>
    public uint UInt() => (uint)( (Byte() + (Byte() << 8) ) + ( (Byte() + (Byte() << 8)) << 16));
    
    /// <summary>
    /// lecture d'un entier 32 bit int signé  ( int , 4bytes, 32 bits) 0 à 326545
    /// </summary>
    /// <returns></returns>
    public int Int() => (int)( (Byte() + (Byte() << 8))  + ( (Byte() + (Byte() << 8)) << 16));
    
    /// <summary>
    /// lecture d'un entier 64bit long non signé ( long , 8bytes , 64 bits) 
    /// </summary>
    /// <returns></returns>
    public ulong ULong() => (ulong)((UInt()) + (UInt() << 32));
    /// <summary>
    /// lecture d'un entier 64bit long signé ( long , 8bytes , 64 bits) 0 à 16m
    /// </summary>
    /// <returns></returns>
    public long Long() => (long)((this.Int()) + (Int() << 32));

    /// <summary>
    /// Lecture d'un tableau de unsigned bytes
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="size"></param>
    public void Bytes(ref byte[] buffer, in int size)
    {
        for (int i = 0; i < size; i++)
            buffer[i] = Byte();
    }

    /// <summary>
    /// Lecture d'un tableau de bytes signé
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="size"></param>
    public void Buffer_SByte(ref sbyte[] buffer, in int size)
    {
        for (int i = 0; i < size; i++)
            buffer[i] = SByte();
    }

    /// <summary>
    /// Lecture d'un buffer de bytes
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="start"></param>
    /// <param name="end"></param>
    public void Buffer(ref byte[] buffer, int start, int end)
        => fs!.Read(buffer, start, end);

    /// <summary>
    /// lecture d'un tableau de short entier court non signé ( sons )
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="size"></param>
    public void Buffer_U16(ushort[] buffer, int size)
    {
        for (int i = 0; i < size; i++)
            buffer[i] = UShort();
    }

    /// <summary>
    /// lecture d'un tableau de short entier court non signé ( sons )
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="size"></param>
    public void Buffer_16(short[] buffer, int size)
    {
        for (int i = 0; i < size; i++)
            buffer[i] = Short();
    }

    public string String( int count )
    {
        var bytes = new byte[count];
        fs!.Read(bytes ,0,count);
        return System.Text.Encoding.UTF8.GetString(bytes);  
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Release();
        GC.SuppressFinalize(this);
    }

    public void Release()
    {
        if ( _disposed)return ;
        Log.Info($"Reader Close : {Name}");
        fs?.Close();
        _disposed = true ;
        // Atomics.MemoryStats.Freed();
    }
    private bool _disposed = false;

    public override string ToString() => "Reader: " + Name;
    public override int GetHashCode() => this.ToString().GetHashCode();
    public override bool Equals(object? obj) => (obj is Reader test) && Name.Equals(test.Name);
    public bool Equals(Reader? other)=> this.Name.Equals( other!.Name);
    public static bool operator ==(Reader left , Reader right )      => left.Equals(right);
    public static bool operator !=(Reader left , Reader right )      => !left.Equals(right);
    }

