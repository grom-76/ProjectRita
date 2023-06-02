

namespace RitaEngine.Base.Debug
{

        using System.Diagnostics;
    using System.Threading.Tasks;

    [SuppressUnmanagedCodeSecurity,SkipLocalsInit, StructLayout(LayoutKind.Sequential )]
public static class Report
{ 
    /// <summary>
    /// Attention boxing, Report format CSV chaque champs et séparer par un saut de ligne et chaque valeur par une virgule 
    /// </summary>
    /// <param name="atype"></param>
    /// <returns></returns>
    public unsafe static string CSVMemoryReport( object atype, nint addressBase =0,char separator=',', char line='\n')
    {
        if (atype == null) return string.Empty;//return new Dictionary<string, object>();
        
        if( addressBase == 0)
            addressBase = MemoryHelper.AddressOf( atype);
        
        return ResultReport( atype , addressBase, separator,  line );
    }

    private static string ResultReport(in object atype ,nint addressBase,char separator, char line )
    {
        Type t = atype.GetType();
        var fields = t.GetFields();
        StringBuilder sb = new();
        
        sb.AppendFormat("0x{0:X8}",addressBase ).Append(separator)
        .Append(t.FullName).Append(separator)
        .Append(Marshal.SizeOf(atype) ).Append(separator)
        .Append(t.Name).Append(separator)
        .Append("_").Append(line);
        
        nint address =   (addressBase );
        
        for (int i=0 ; i < fields.Length; i++)
        {
            FieldInfo fi = fields[i];
            object value = fi.GetValue(atype)! ;
            nint taille =  Marshal.SizeOf(value);
            address -= i==0 ? 0:  (nint)taille;
            sb.AppendFormat("0x{0:X8}",address ).Append(separator)
            .Append(fi.FieldType.Name).Append(separator)
            .Append(taille ).Append(separator)
            .Append(fi.Name).Append(separator)
            .Append(value?.ToString()!).Append(line);
        }
        return sb.ToString();//result;

    }

    /// <summary>
    /// Attention boxing, Report format CSV chaque champs et séparer par un saut de ligne et chaque valeur par une virgule 
    /// </summary>
    /// <param name="atype"></param>
    /// <returns></returns>
    public unsafe static string CSVMemoryReport<T>(ref T atype , nint addressBase =0,char separator=',', char line='\n') 
    {
        if (atype == null) return string.Empty;//return new Dictionary<string, object>();
        
        if( addressBase == 0)
            addressBase = MemoryHelper.AddressOf( atype);
        
        return ResultReport( atype , addressBase, separator,  line );
    }

  
   // https://learn.microsoft.com/fr-fr/archive/blogs/calvin_hsia/create-your-own-crash-dumps
    // !load \\ddelementary\autowatson\vsdbg\vsdbg.dll
          private static bool Is32BitProcess(this Process proc)
        {
            bool fIs32bit = false;
            // if we're runing on 32bit, default to true
            if (IntPtr.Size == 4)
            {
                fIs32bit = true;
            }
            bool fIsRunningUnderWow64 = false;
            // if machine is 32 bit then all procs are 32 bit
            if (NativeMethods.IsWow64Process(NativeMethods.GetCurrentProcess(), out fIsRunningUnderWow64)
                && fIsRunningUnderWow64)
            {
                // current OS is 64 bit
                if (NativeMethods.IsWow64Process(proc.Handle, out fIsRunningUnderWow64)
                      && fIsRunningUnderWow64)
                {
                    fIs32bit = true;
                }
                else
                {
                    fIs32bit = false;
                }
            }
            return fIs32bit;
        }
        

        public static async Task CreateDumpFileAsync()
        {
            //need using System.Threading.Tasks;

            //Using Run 
            await Task.Run(() =>CreateDumpFile());
            // Classic Method
            // Task.Factory.StartNew(() => { Console.WriteLine(“Hello Task :D”); });

            // // Using Delegate
            // Task task = new Task(delegate { CreateDumpFile(); });
            // task.Start();

            // // Using Action
            //  task = new Task(new Action(CreateDumpFile));
            // task.Start();

            // // Using Lambda with no method
            // task = new Task( () => { CreateDumpFile(); } );
            // task.Start();
            
            // // Using Lambda with a method
            //  task = new Task( () => CreateDumpFile() );
            // task.Start(); 
            

            // // Using FromResult (.NET 4.5)
            // int result = await Task.FromResult<int>(CreateDumpFile);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Statusof file created</returns>
        public static string CreateDumpFile()
        {
            string Title= string.Empty;
            string Status = string.Empty;
            nint hFile = nint.Zero;

            if (IntPtr.Size == 4)
            {
                Title = "CreateMiniDump Running as 32 bit, creating 32 bit dumps";
            }
            else
            {
                Title = "CreateMiniDump Running as 64 bit, creating 64 bit dumps";
            }

            string dumpFileName = System.IO.Path.Combine( System.IO.Path.GetTempPath(), "RitaEngineDump.dmp");

            var enumsItems = Enum.GetValues(typeof(NativeMethods._MINIDUMP_TYPE));

            try
            {
                var sw = new Stopwatch();
                sw.Start();
                // dumpFileName = txtDumpFile.Text.Trim();
                if (System.IO.File.Exists(dumpFileName))
                {
                    System.IO.File.Delete(dumpFileName);
                }
                hFile = NativeMethods.CreateFile(
                    dumpFileName,
                    NativeMethods.EFileAccess.GenericWrite,
                    NativeMethods.EFileShare.None,
                    lpSecurityAttributes: IntPtr.Zero,
                    dwCreationDisposition: NativeMethods.ECreationDisposition.CreateAlways,
                    dwFlagsAndAttributes: NativeMethods.EFileAttributes.Normal,
                    hTemplateFile: IntPtr.Zero);

                if (hFile == NativeMethods.INVALID_HANDLE_VALUE)
                {
                    var hr = Marshal.GetHRForLastWin32Error();
                    var ex = Marshal.GetExceptionForHR(hr);
                    throw ex?? new Exception("");
                }
                NativeMethods._MINIDUMP_TYPE dumpType = NativeMethods._MINIDUMP_TYPE.MiniDumpNormal; // 0
                foreach (var item in enumsItems)
                {
                    var dt = (NativeMethods._MINIDUMP_TYPE)item;
                    dumpType |= dt;
                }

                var exceptInfo = new NativeMethods.MINIDUMP_EXCEPTION_INFORMATION();

                // var proc = Process.GetProcessesByName( ProcName.Trim() ).FirstOrDefault();
                 
                var proc =   Process.GetProcessById( (int) NativeMethods.GetCurrentProcessId() );

                if (!proc!.Is32BitProcess() && IntPtr.Size == 4)
                {
                    throw new InvalidOperationException(
                    "Can't create 32 bit dump of 64 bit process"
                    );
                }

                var result = NativeMethods.MiniDumpWriteDump(
                        proc!.Handle,
                        proc.Id,
                        hFile,
                        dumpType,
                        ref exceptInfo,
                        UserStreamParam: IntPtr.Zero,
                        CallbackParam: IntPtr.Zero
                        );
                if (result == false)
                {
                    var hr = Marshal.GetHRForLastWin32Error();
                    var ex = Marshal.GetExceptionForHR(hr);
                    throw ex?? new Exception("");
                }

                Status = string.Format(
                        "Dump Created. Pid= {0} {1}\r File size = {2:n0}\rElapsed = {3:n3}\r",
                        proc.Id,
                        dumpType,
                        (new System.IO.FileInfo(dumpFileName)).Length,
                        sw.Elapsed.TotalSeconds
                        );
                
            }
            catch (Exception ex)
            {
               Status = ex.ToString();

            }
            finally
            {
                NativeMethods.CloseHandle(hFile);
            }
            return Status;
        }

        public static partial class NativeMethods
        {
            [DllImport("Dbghelp.dll")]
            public static extern bool MiniDumpWriteDump(
                IntPtr hProcess,
                int ProcessId,
                IntPtr hFile,
                _MINIDUMP_TYPE DumpType,
                ref MINIDUMP_EXCEPTION_INFORMATION ExceptionParam,
                IntPtr UserStreamParam,
                IntPtr CallbackParam
                );
            //https://msdn.microsoft.com/en-us/library/windows/desktop/ms680519%28v=vs.85%29.aspx?f=255&MSPPError=-2147217396
            [Flags]
            public enum _MINIDUMP_TYPE
            {
                MiniDumpNormal = 0x00000000,
                MiniDumpWithDataSegs = 0x00000001,
                MiniDumpWithFullMemory = 0x00000002,
                MiniDumpWithHandleData = 0x00000004,
                MiniDumpFilterMemory = 0x00000008,
                MiniDumpScanMemory = 0x00000010,
                MiniDumpWithUnloadedModules = 0x00000020,
                MiniDumpWithIndirectlyReferencedMemory = 0x00000040,
                MiniDumpFilterModulePaths = 0x00000080,
                MiniDumpWithProcessThreadData = 0x00000100,
                MiniDumpWithPrivateReadWriteMemory = 0x00000200,
                MiniDumpWithoutOptionalData = 0x00000400,
                MiniDumpWithFullMemoryInfo = 0x00000800,
                MiniDumpWithThreadInfo = 0x00001000,
                MiniDumpWithCodeSegs = 0x00002000,
                MiniDumpWithoutAuxiliaryState = 0x00004000,
                MiniDumpWithFullAuxiliaryState = 0x00008000,
                MiniDumpWithPrivateWriteCopyMemory = 0x00010000,
                MiniDumpIgnoreInaccessibleMemory = 0x00020000,
                MiniDumpWithTokenInformation = 0x00040000,
                MiniDumpWithModuleHeaders = 0x00080000,
                MiniDumpFilterTriage = 0x00100000,
                MiniDumpValidTypeFlags = 0x001fffff,
            };
            /*
            * 
        https://msdn.microsoft.com/en-us/library/windows/desktop/bb513622(v=vs.85).aspx
        WER_DUMP_TYPE:
        WerDumpTypeHeapDump
            MiniDumpWithDataSegs
            MiniDumpWithProcessThreadData
            MiniDumpWithHandleData
            MiniDumpWithPrivateReadWriteMemory
            MiniDumpWithUnloadedModules
            MiniDumpWithFullMemoryInfo
            MiniDumpWithThreadInfo (Windows 7 and later)
            MiniDumpWithTokenInformation (Windows 7 and later)
            MiniDumpWithPrivateWriteCopyMemory 
        WerDumpTypeMicroDump
            MiniDumpWithDataSegs
            MiniDumpWithUnloadedModules
            MiniDumpWithProcessThreadData
            MiniDumpWithoutOptionalData

        WerDumpTypeMiniDump
            MiniDumpWithDataSegs
            MiniDumpWithUnloadedModules
            MiniDumpWithProcessThreadData
            MiniDumpWithTokenInformation 
            */

            [StructLayout(LayoutKind.Sequential, Pack = 4)]
            public struct MINIDUMP_EXCEPTION_INFORMATION
            {
                public uint ThreadId;
                public IntPtr ExceptionPointers;
                public int ClientPointers;
            }

            [DllImport("kernel32.dll",
            SetLastError = true,
            CharSet = CharSet.Auto)]
            public static extern IntPtr CreateFile(
                    string lpFileName,
                    EFileAccess dwDesiredAccess,
                    EFileShare dwShareMode,
                    IntPtr lpSecurityAttributes,
                    ECreationDisposition dwCreationDisposition,
                    EFileAttributes dwFlagsAndAttributes,
                    IntPtr hTemplateFile
                );

            [DllImport("kernel32.dll",
            SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool CloseHandle(IntPtr hObject);

            public static IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);
            [Flags]
            public enum EFileAccess : uint
            {
                //
                // Standart Section
                //

                AccessSystemSecurity = 0x1000000,   // AccessSystemAcl access type
                MaximumAllowed = 0x2000000,     // MaximumAllowed access type

                Delete = 0x10000,
                ReadControl = 0x20000,
                WriteDAC = 0x40000,
                WriteOwner = 0x80000,
                Synchronize = 0x100000,

                StandardRightsRequired = 0xF0000,
                StandardRightsRead = ReadControl,
                StandardRightsWrite = ReadControl,
                StandardRightsExecute = ReadControl,
                StandardRightsAll = 0x1F0000,
                SpecificRightsAll = 0xFFFF,

                FILE_READ_DATA = 0x0001,        // file & pipe
                FILE_LIST_DIRECTORY = 0x0001,       // directory
                FILE_WRITE_DATA = 0x0002,       // file & pipe
                FILE_ADD_FILE = 0x0002,         // directory
                FILE_APPEND_DATA = 0x0004,      // file
                FILE_ADD_SUBDIRECTORY = 0x0004,     // directory
                FILE_CREATE_PIPE_INSTANCE = 0x0004, // named pipe
                FILE_READ_EA = 0x0008,          // file & directory
                FILE_WRITE_EA = 0x0010,         // file & directory
                FILE_EXECUTE = 0x0020,          // file
                FILE_TRAVERSE = 0x0020,         // directory
                FILE_DELETE_CHILD = 0x0040,     // directory
                FILE_READ_ATTRIBUTES = 0x0080,      // all
                FILE_WRITE_ATTRIBUTES = 0x0100,     // all

                //
                // Generic Section
                //

                GenericRead = 0x80000000,
                GenericWrite = 0x40000000,
                GenericExecute = 0x20000000,
                GenericAll = 0x10000000,

                SPECIFIC_RIGHTS_ALL = 0x00FFFF,
                FILE_ALL_ACCESS =
                StandardRightsRequired |
                Synchronize |
                0x1FF,

                FILE_GENERIC_READ =
                StandardRightsRead |
                FILE_READ_DATA |
                FILE_READ_ATTRIBUTES |
                FILE_READ_EA |
                Synchronize,

                FILE_GENERIC_WRITE =
                StandardRightsWrite |
                FILE_WRITE_DATA |
                FILE_WRITE_ATTRIBUTES |
                FILE_WRITE_EA |
                FILE_APPEND_DATA |
                Synchronize,

                FILE_GENERIC_EXECUTE =
                StandardRightsExecute |
                FILE_READ_ATTRIBUTES |
                FILE_EXECUTE |
                Synchronize
            }

            [Flags]
            public enum EFileShare : uint
            {
                /// <summary>
                /// 
                /// </summary>
                None = 0x00000000,
                /// <summary>
                /// Enables subsequent open operations on an object to request read access. 
                /// Otherwise, other processes cannot open the object if they request read access. 
                /// If this flag is not specified, but the object has been opened for read access, the function fails.
                /// </summary>
                Read = 0x00000001,
                /// <summary>
                /// Enables subsequent open operations on an object to request write access. 
                /// Otherwise, other processes cannot open the object if they request write access. 
                /// If this flag is not specified, but the object has been opened for write access, the function fails.
                /// </summary>
                Write = 0x00000002,
                /// <summary>
                /// Enables subsequent open operations on an object to request delete access. 
                /// Otherwise, other processes cannot open the object if they request delete access.
                /// If this flag is not specified, but the object has been opened for delete access, the function fails.
                /// </summary>
                Delete = 0x00000004
            }

            public enum ECreationDisposition : uint
            {
                /// <summary>
                /// Creates a new file. The function fails if a specified file exists.
                /// </summary>
                New = 1,
                /// <summary>
                /// Creates a new file, always. 
                /// If a file exists, the function overwrites the file, clears the existing attributes, combines the specified file attributes, 
                /// and flags with FILE_ATTRIBUTE_ARCHIVE, but does not set the security descriptor that the SECURITY_ATTRIBUTES structure specifies.
                /// </summary>
                CreateAlways = 2,
                /// <summary>
                /// Opens a file. The function fails if the file does not exist. 
                /// </summary>
                OpenExisting = 3,
                /// <summary>
                /// Opens a file, always. 
                /// If a file does not exist, the function creates a file as if dwCreationDisposition is CREATE_NEW.
                /// </summary>
                OpenAlways = 4,
                /// <summary>
                /// Opens a file and truncates it so that its size is 0 (zero) bytes. The function fails if the file does not exist.
                /// The calling process must open the file with the GENERIC_WRITE access right. 
                /// </summary>
                TruncateExisting = 5
            }

            [Flags]
            public enum EFileAttributes : uint
            {
                Readonly = 0x00000001,
                Hidden = 0x00000002,
                System = 0x00000004,
                Directory = 0x00000010,
                Archive = 0x00000020,
                Device = 0x00000040,
                Normal = 0x00000080,
                Temporary = 0x00000100,
                SparseFile = 0x00000200,
                ReparsePoint = 0x00000400,
                Compressed = 0x00000800,
                Offline = 0x00001000,
                NotContentIndexed = 0x00002000,
                Encrypted = 0x00004000,
                Write_Through = 0x80000000,
                Overlapped = 0x40000000,
                NoBuffering = 0x20000000,
                RandomAccess = 0x10000000,
                SequentialScan = 0x08000000,
                DeleteOnClose = 0x04000000,
                BackupSemantics = 0x02000000,
                PosixSemantics = 0x01000000,
                OpenReparsePoint = 0x00200000,
                OpenNoRecall = 0x00100000,
                FirstPipeInstance = 0x00080000
            }


            [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool IsWow64Process(
                [In] IntPtr hProcess,
                [Out, MarshalAs(UnmanagedType.Bool)] out bool wow64Process
                );
            [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern IntPtr GetCurrentProcess();

            [DllImport("kernel32.dll")]
            public static extern uint GetCurrentProcessId();
        }

   
}//createdumpFile // REPORT //Profiler // Performance //Diagnostic


}
