

namespace RitaEngine.Base
{



    namespace IO
    {
        public static class Folders{
              public static string Combine(params string[] folders)
    {
        string result = string.Empty;
        #if WIN 
        foreach( string folder in folders)
        {
            result += folder +'\\';
        }
        #else

        #endif
        return result;
    }

    public static void Create(string folder)
    {
        #if WIN 
        System.IO.Directory.CreateDirectory(folder);
        #else

        #endif
    }

    public static bool IsExist(string folder)
    {
        #if WIN 
        return System.IO.Directory.Exists(folder);
        #else
        return System.IO.Directory.Exists(folder);
        #endif
    }

    public static bool Delete(string folder)
    {
        #if WIN 
        System.IO.Directory.Delete(folder);
        return true;
        #else
        return true;
        #endif
    }
            //MOunt partition ???? path 
   public static string CombinePAth(params string[] folders)
    {
        string result = string.Empty;
        #if WIN 
        foreach( string folder in folders)
        {
            result += folder +'\\';
        }
        #else

        #endif
        return result;
    }

    public static void CreateFolder(string folder)
    {
        #if WIN 
        System.IO.Directory.CreateDirectory(folder);
        #else

        #endif
    }

    public static bool IsFolderExist(string folder)
    {
        #if WIN 
        return System.IO.Directory.Exists(folder);
        #else
        return System.IO.Directory.Exists(folder);
        #endif
    }

    public static bool DeleteFolder(string folder)
    {
        #if WIN 
        System.IO.Directory.Delete(folder);
        return true;
        #else
        return true;
        #endif
    }



        }//Directory // PATH
    }
    
}
