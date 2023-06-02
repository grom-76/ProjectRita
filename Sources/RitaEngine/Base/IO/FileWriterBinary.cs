

namespace RitaEngine.Base.IO;


[SkipLocalsInit, StructLayout(LayoutKind.Sequential )]
public sealed class Writer : IDisposable, IEquatable<Writer>
{

    private readonly System.IO.FileStream? fs ;

    /// <summary>
    /// Constructeur obligatoire avec nom du fichier
    /// </summary>
    /// <param name="filename"></param>
    public Writer(string filename)
    {
        // Guard.ThrowIf(  !System.IO.File.Exists(filename) );
        Log.Info($"Writer Open : {filename}");
        fs =new System.IO.FileStream(filename ,System.IO.FileMode.Append, System.IO.FileAccess.Write);
        // Atomics.MemoryStats.Allocated();
    }
        
    private Writer() {} // NOt autorised

    /// <summary>
    /// Retourne le nom du fichier ouvret courant
    /// </summary>
    public string Name => fs!.Name ;

    /// <inheritdoc />
    public void Dispose()
    {
        Release();
        GC.SuppressFinalize(this);
    }

    public void Release()
    {
        if ( _disposed)return ;
        Log.Info($"Writer Close : {Name}");
        fs?.Close();
        _disposed = true ;
        // Atomics.MemoryStats.Freed();
    }

    private bool _disposed = false;

    /// <summary>
    /// Fonction de base ecriture 
    /// </summary>
    /// <param name="content">Tableau de byte</param>
    /// <param name="offset">depart dans la chaine byte</param>
    /// <param name="size">longueur a écrire</param>
    public void Buffer(in byte[] content , int offset = 0 , int size=-1 )
        => fs!.Write( IsReverse(content), offset ,size == -1 ?  content.Length: size );

    private static byte[] IsReverse( ReadOnlySpan<byte> bytes)
    {
        if ( !BitConverter.IsLittleEndian)
            Array.Reverse(bytes.ToArray());
        return bytes.ToArray();
    }

    /// <summary>
    /// ecriture d'un boolean ( 1 byte ,  8bits)
    /// </summary>
    /// <param name="value"></param>
    public void Bool(bool value) => Buffer(   BitConverter.GetBytes(value) );

    /// <summary>
    /// ecriture d'un flotant (float, 4 bytes , 32 bits)
    /// </summary>
    /// <param name="value"></param>
    public void Float(float value) => Buffer(   BitConverter.GetBytes(value) );

    /// <summary>
    /// ecriture d'un flotant precision double (double, 8 bytes , 64 bits)
    /// </summary>
    /// <param name="value"></param>
    public void Double(double value) => Buffer(   BitConverter.GetBytes(value) );
    /// <summary>
    /// ecriture d'un caractere UTF16 (char, 2 bytes , 16 bits)
    /// </summary>
    /// <param name="value"></param>
    public void Char(char value) => Buffer(   BitConverter.GetBytes(value) );

    /// <summary>
    /// ecriture d'un entier court  (short , 2 bytes , 16 bits)
    /// </summary>
    /// <param name="value"></param>
    public void Short( short value) => Buffer(   BitConverter.GetBytes(value) );
    
    /// <summary>
    /// ecriture d'un entier court non signé (ushort , 2 bytes , 16 bits)
    /// </summary>
    /// <param name="value"></param>
    public void UShort( ushort value) => Buffer(   BitConverter.GetBytes(value) );

    /// <summary>
    /// ecriture d'un entier  (int , 4 bytes , 32 bits)
    /// </summary>
    /// <param name="value"></param>
    public void Int( int value) => Buffer(   BitConverter.GetBytes(value) );
    /// <summary>
    /// ecriture d'un entier non signé (uint , 4 bytes , 32 bits)
    /// </summary>
    /// <param name="value"></param>
    public void UInt( uint value) => Buffer(   BitConverter.GetBytes(value) );
    /// <summary>
    /// ecriture d'un entier long (long , 8 bytes , 64 bits)
    /// </summary>
    /// <param name="value"></param>
    public void Long( long value) => Buffer(   BitConverter.GetBytes(value) );
    /// <summary>
    /// ecriture d'un entier long non signé (ulong , 8 bytes , 64 bits)
    /// </summary>
    /// <param name="value"></param>
    public void ULong( ulong value) => Buffer(   BitConverter.GetBytes(value) );
    /// <summary>
    /// ecriture d'une valeur en fonction de son type T
    /// </summary>
    /// <param name="value"></param>
    /// <typeparam name="T"></typeparam>
    public void BufferValue<T>(T value) where T: notnull
    {
        if ( value is bool mbool) Bool(mbool); 
        if ( value is char mchar) Char(mchar); 
        if ( value is short mshort) Short(mshort); 
        if ( value is ushort mushort) UShort(mushort); 
        if ( value is int mint) Int(mint);
        if ( value is uint muint) UInt(muint); 
        if ( value is long mlong) Long(mlong); 
        if ( value is ulong mulong) ULong( mulong); 
        if ( value is float mfloat) Float(mfloat); 
        if ( value is double mdouble) Double(mdouble); 
    }        
    /// <summary>
    /// ecriture d'une chaine de caractère string (convertion en bytes)
    /// </summary>
    /// <param name="chaine"></param>
    /// <param name="sizelimit"></param>
    public void String( string chaine, int sizelimit=-1)
    {
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(chaine);
        if ( sizelimit == -1) sizelimit = bytes.Length;

        var result = new byte[sizelimit];
        Array.Clear( result,0 , sizelimit);
        Array.Copy(bytes,result,bytes.Length );
        Buffer( result,0,sizelimit);
    }

    /// <inheritdoc />
    public override string ToString() => "Writer: " + Name;
    /// <inheritdoc />
    public override int GetHashCode() => this.ToString().GetHashCode();
    /// <inheritdoc />
    public override bool Equals(object? obj) => (obj is Writer test) && Name.Equals(test.Name);
    /// <inheritdoc />
    public bool Equals(Writer? other)=> this.Name.Equals( other!.Name);
    /// <inheritdoc />
    public static bool operator ==( Writer left , Writer right) => left.Equals(right);
    /// <inheritdoc />
    public static bool operator !=( Writer left , Writer right) => !left.Equals(right);        
    }
    