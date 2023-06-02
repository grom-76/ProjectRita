

namespace RitaEngine.Base.Debug
{
    public static class Performance
{
    /// <summary>
    /// action methode a tester  
    /// </summary>
    /// <param name="action"></param>
    /// <param name="loop"></param>
    /// <param name="iteration"></param>
    /// <param name="diagnostic"></param>
    public static void Preformance( System.Action action,  int loop=5,int iteration=10_000, bool diagnostic=false)
    {
        // var before = System.Diagnostics.Process.GetCurrentProcess().VirtualMemorySize64;
        // Profilers.Use.Trace(ProfilersPriority.TITLE ,0,title,"","",0 );
        // Profilers.Use.Trace(ProfilersPriority.BENCHMARK ,0,$"Test with {loop} loops and {iteration} iterations","","",0 );

        #region Warmup
        System.GC.Collect();//clean Garbage            
        System.GC.WaitForPendingFinalizers();//wait for the finalizer queue to empty
        System.GC.Collect();//clean Garbage

        //var totalmemoryin = System.GC.GetTotalMemory(true);
        var timings = new double[loop];
        System.Diagnostics.Stopwatch stopwatch = new();
        double avg = 0.0, max = 0, min = 0;
        //prevent the JIT Compiler from optimizing Fkt calls away
        //long seed = System.Environment.TickCount;

        System.Diagnostics.Process process = System.Diagnostics.Process.GetCurrentProcess() ;
        // var saveprocessaffinity = process.ProcessorAffinity;

        //use the second Core/Processor for the test
        // process.ProcessorAffinity = new System.IntPtr(2);
        //prevent "Normal" Processes from interrupting Threads
        process.PriorityClass = System.Diagnostics.ProcessPriorityClass.High;
        //prevent "Normal" Threads from interrupting this thread
        // System.Threading.Thread.CurrentThread.Priority = System.Threading.ThreadProfilersPriority.Highest;

        #endregion

        #region  for CPU

        // var startTime = System.Diagnostics.Process.GetCurrentProcess().TotalProcessorTime;

        for (int j = 0; j < iteration; j++)        action();
        // var stopTime = System.Diagnostics.Process.GetCurrentProcess().TotalProcessorTime;
        //var elapsed_cpu = (stopTime - startTime).TotalMilliseconds  ;
        // Profilers.Use.Trace(ProfilersPriority.BENCHMARK ,0,"CPU TIMER elapsed  : "+ elapsed_cpu.ToString() ,"","",0 );
        #endregion 

        #region Thread Measure

        // int CurrentThread = System.Threading.Thread.CurrentThread.ManagedThreadId;
        // donne le nombre de thrad actif 
        // commence a zero ? 
        // CurrentThread--;
        // see  : https://stackoverflow.com/questions/16376191/measuring-code-execution-time
        // System.TimeSpan thread_startTime= process.Threads[CurrentThread].UserProcessorTime; // i being your thread number, make it 0 for main

        for (int j = 0; j < iteration; j++)    action();

        // System.TimeSpan duration = process.Threads[CurrentThread].UserProcessorTime.Subtract(thread_startTime);
        //Profilers.Use.Trace(ProfilersPriority.BENCHMARK ,0,"Current procces  total : " +duration.TotalMilliseconds.ToString() ,"","",0 );
        #endregion
        #region Time

        for (int i = 0; i < timings.Length; i++)
        {
            stopwatch.Restart();

            for (int j = 0; j < iteration; j++)
                action();

            stopwatch.Stop();
            timings[i] = stopwatch.Elapsed.TotalMilliseconds;

            // Profilers.Use.Trace(ProfilersPriority.BENCHMARK ,0,$"loop n° {i}  time : {timings[i]} ms" ,"","",0 );
        }
        #endregion

        #region Average
        foreach( var value in timings )
        {
            max = value> max ? value : max ;
            min = value< min ? value : min ;
            avg += value;
        }

        // var average = loop> 2? ( avg - min - max) / (timings.Length -2 ) : avg / (timings.Length  );
        // var onetime = average/ iteration; // execution de la fonction une fois 
        // Profilers.Use.Trace(ProfilersPriority.PROFILER ,0,$"\t Average ; {average} ms  for {timings.Length}  Min : {min} Max : {max}  One call : {onetime}" ,"","",0 );
        #endregion
        //TODO median and memory consomed
        // var totalbytes = System.GC.GetAllocatedBytesForCurrentThread();
        // var after = System.Diagnostics.Process.GetCurrentProcess().VirtualMemorySize64;
        // var totalmemoryout = System.GC.GetTotalMemory(true);
        // var totalmemoryconsumed = after - before ;

        //Profilers.Use.Trace(ProfilersPriority.BENCHMARK ,0,"Memory",$"Memory Start : {totalmemoryin}\nTotal memory :{totalmemoryout}\n diffrence : {totalmemoryout - totalmemoryin} \nTotal Bytes : {totalbytes} \nConsumed : {totalmemoryconsumed} \n  " ,"",0 );
        //restore process affinity 
        // process.ProcessorAffinity = saveprocessaffinity;

        if(diagnostic)
            ReportDiagnostic();
    }

    /// <summary>
    /// affichage du resultat en mode console 
    /// </summary>
    /// <returns></returns>
    public  static string  ReportDiagnostic( )
    {
        //TODO: machine name
        var myProcess = System.Diagnostics.Process.GetCurrentProcess();
        myProcess.Refresh();
        //var   cpuCounter = new System.C PerformanceCounter("Processor", "% Processor Time", "_Total", machineName);      
        // instance.Pid =System.Diagnostics.Process.GetCurrentProcess().Id ;
        const int mo = 1024*1024 ;
        string msg =$"  Physical memory usage     : {myProcess.WorkingSet64/mo} Mo\n";
        msg+=$"  Base priority             : {myProcess.BasePriority}\n";
        msg+=$"  Priority class            : {myProcess.PriorityClass}\n";
        msg+=$"  User processor time       : {myProcess.UserProcessorTime.TotalMilliseconds} ms\n";
        msg+=$"  Privileged processor time : {myProcess.PrivilegedProcessorTime.TotalMilliseconds} ms\n";
        msg+=$"  Total processor time      : {myProcess.TotalProcessorTime.TotalMilliseconds} ms\n";
        msg+=$"  Paged system memory size  : {myProcess.PagedSystemMemorySize64/1024} Ko\n";
        msg+=$"  Paged memory size         : {myProcess.PagedMemorySize64/mo} Mo\n";

        var peakPagedMem   = myProcess.PeakPagedMemorySize64;
        var peakVirtualMem = myProcess.PeakVirtualMemorySize64;
        var peakWorkingSet = myProcess.PeakWorkingSet64;

        msg+=($"\n  Peak physical memory usage : {peakWorkingSet/mo} Mo \n");
        msg+=($"  Peak paged memory usage    : {peakPagedMem/mo} Mo\n");
        msg+=($"  Peak virtual memory usage  : {peakVirtualMem/(mo*1024)} Go\n");

        return msg;
        //HERE Profilers.Use.Notice
        //Profilers.Use.Trace(ProfilersPriority.BENCHMARK ,0,"Diagnostic","\n"+msg ,"",0 );
    }
}


}
