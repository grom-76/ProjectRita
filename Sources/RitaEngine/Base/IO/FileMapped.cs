namespace RitaEngine.Base.IO;

using System.IO.MemoryMappedFiles;


/// <summary>
/// utilise memory mapped file 
/// https://docs.microsoft.com/en-us/dotnet/standard/io/memory-mapped-files
/// </summary>
[SkipLocalsInit, StructLayout(LayoutKind.Sequential,Pack =4)]
public class FileMapped : IEquatable<FileMapped> , IDisposable
{
    private readonly MemoryMappedFile memoryMappedFiles;
    private readonly MemoryMappedViewAccessor _accessor;
    private long _position;
    private string _name;
    private long _length ;
    /// <summary>
    /// nom du fichier ouvert
    /// </summary>
    public  string Name => _name ;

    /// <summary>Longueur en bytes du fichier  </summary>
    public  long Length => _length;

    /// <summary>
    /// instance avec le nom du fichier est si lecture ou ecriture
    /// </summary>
    /// <param name="filename"></param>
    /// <param name="read"></param>
    public FileMapped(string filename, bool read=true)
    {
        // if ( !System.IO.File.Exists(filename))
        //     throw new Exception("FileNot exist :" + filename);

        _length = new System.IO.FileInfo(filename).Length; 
            
        if ( !read )
        {
            memoryMappedFiles =MemoryMappedFile.CreateNew (filename, 1024 );   
            var stream = memoryMappedFiles.CreateViewStream();
            stream.ReadByte( );
        }
        else 
        {
            memoryMappedFiles = MemoryMappedFile.CreateFromFile(filename,System.IO.FileMode.Open);
        }
        _accessor = memoryMappedFiles.CreateViewAccessor();   
        _position = _accessor.PointerOffset;
        _name= filename;
        // TODO find another way ;
        
    }
    /// <summary>
    /// 
    /// </summary>
    public void Dispose()
    {
        _accessor.Dispose();
        memoryMappedFiles.Dispose();
    }  
#region READ
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public short ReadShort()
    {
        if( EOF) throw new Exception("Reach End of File "); 
        _position += 2;
        return _accessor.ReadInt16(_position-2);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bytes">tableau resultat</param>
    /// <param name="count">nombre de byte à lire a partir de la position courante</param>
    /// <returns>0 if suceess else error </returns>
    public int ReadBytes( ref byte[] bytes , int count )
    {
        var result = _accessor.ReadArray<byte>( _position , bytes, 0, count);
        return result ;
    }

    
#endregion

#region WRITE

#endregion

#region  POSITIONNEMENT
/// <summary>
/// indique true si fin de ligne
/// </summary>
    public bool EOF =>  _position >= _length;
    //SEEK avance from origine
    // ADVANCE from current position 
    // Backward  recul from current position 
    // EOF if is end of file 
    
    /// <summary>
    /// Seek , positiotne le curseur dans la fichier a la postion donnée
    /// </summary>
    /// <returns></returns>
    public void Positionnement(int position )
    {
        // _accessor.SafeMemoryMappedViewHandle
        // _accessor.
    }
#endregion

#region  OVERRIDE
    public override string ToString() => "IOFILe Mapped : " + _name;
    public override int GetHashCode() => this.ToString().GetHashCode();
    public override bool Equals(object? obj) => (obj is FileMapped test) && Name.Equals(test.Name);
    public bool Equals(FileMapped? other)=> this.Name.Equals( other!.Name);
    public static bool operator ==( FileMapped left , FileMapped right) => left.Equals(right);
    public static bool operator !=( FileMapped left , FileMapped right) => !left.Equals(right);        
#endregion

}

