

namespace RitaEngine.Base.Debug
{
    /// <summary>
    /// profiler for loop 
    /// </summary>
    public static class Profiler
{
//     Stopwatch timer = new Stopwatch();
// timer.Start();
// // insert some code to execute here
// timer.Stop();
// Console.WriteLine("Time elapsed: {0:hh\\:mm\\:ss}", stopwatch.Elapsed);
   
    /// <summary>
    /// wrapper for time used in class ( for stand alone class)
    /// </summary>
    /// <returns></returns>
    private static ulong StartChrono() => 0L;
    /// <summary>
    /// wrapper for time used in class ( for stand alone class)
    /// </summary>
    /// <returns></returns>
    private static ulong StopChrono() => 0L;
    private static System.Collections.Generic.List<uint> loop_Frames = new (16);
    private static System.Collections.Generic.List<ulong> loop_StartTimes = new (16);
    private static System.Collections.Generic.List<ulong> loop_Total = new (16); 
    private static System.Collections.Generic.List<int> loop_Level = new (16);
    private static System.Collections.Generic.List<float> loop_Average = new (16);
    private static System.Collections.Generic.List<float> loop_Min = new (16);
    private static System.Collections.Generic.List<float> loop_Max = new (16);
    private static System.Collections.Generic.List<string> loop_Names = new (16);
    private static System.Collections.Generic.List<bool> loop_IsProcessing = new (16);

    /// <summary>
    /// Start measure 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    public static void Begin(int id , string name )
    {
        var current = StartChrono() ;  
        if ( id != loop_Names.Count )
        {
            loop_StartTimes[id] = current;  
            loop_Frames[id]++; 
        }
        else
        {
            loop_Names.Add(name);
            loop_StartTimes.Add(current  );
            // loop_Ids.Add(id);
            loop_Average.Add(0.0f);
            loop_Frames.Add(1);
            loop_Total.Add(0);
            
            loop_Min.Add(0.0f);
            loop_Max.Add(0.0f);
            loop_IsProcessing.Add(true);
            loop_Level.Add( ( id != 0  && loop_IsProcessing[id-1]==true ) ?  loop_Level[id-1]+1 : 0 );    
        }
        // if ids exist => change starttime and quit 
    }

    /// <summary>
    /// End timer   ready for next frame
    /// </summary>
    /// <param name="id"></param>
    public static void End(int id)
    {
        var current = StopChrono();  
        loop_Total[id] +=  ( current - loop_StartTimes[id]) ;
        loop_Average[id]  = (float)(loop_Total[id] / loop_Frames[id]) ; 

        loop_Min[id] = loop_Min[id] ==0.0f ?   loop_Average[id] :loop_Min[id] >loop_Average[id] ? loop_Average[id] : loop_Min[id];
        loop_Max[id] = loop_Max[id] ==0.0f ?   loop_Average[id] : loop_Max[id] <loop_Average[id] ? loop_Average[id] : loop_Max[id];
        loop_IsProcessing[id] = false;
    }

    /// <summary>
    /// Return string for display report
    /// </summary>
    /// <returns></returns>
    public static string Report()
    {
        string report = string.Empty;

        report +="LVL  | NAME                         | FRAMES   | TOTAL SEC  | AVERAGE    | MIN        | MAX        | FPS         |  % USED \n";
        for( int i =0 ; i< loop_IsProcessing.Count ; i++)
        {
            report += string.Format("{0,-5}" ,loop_Level[i]);
            report += "|" + string.Format("{0,-30}" ,loop_Names[i]);
            report += "|" + string.Format("{0,10}" ,loop_Frames[i] );
            report += "|" + string.Format("{0,12}" ,loop_Total[i] );
            report += "|" + string.Format("{0,12}" ,loop_Average[i] );
            report += "|" + string.Format("{0,12}" ,loop_Min[i] ); 
            report += "|" + string.Format("{0,12}" ,loop_Max[i] );
            report += "|" + string.Format("{0,12}" , (1/loop_Average[i]) );
            if( i> 0)
                report += "|" + string.Format("{0,12}", ((loop_Total[i] * 100)  / (float)loop_Total[0])   );
            report += "\n";
        }
        return report ;
    }
}


}
