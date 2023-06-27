
namespace RitaEngine.Base.Platform;

using RitaEngine.Base.Platform.Config;
using RitaEngine.Base.Platform.Structures;

[ StructLayout(LayoutKind.Sequential, Pack = BaseHelper.FORCE_ALIGNEMENT),SkipLocalsInit]
public struct Clock: IEquatable<Clock>
{
    private ClockData _data = new();
    private ClockFunctions _funcs;

    public Clock() { }

    public void Init( PlatformConfig config) => ClockImplement.InitClock(ref _data , ref _funcs , in config);

    public void Release() {   /* nothing here to keep same architecture ...*/  }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Update()=>_data.LoopMethod(ref _data ,ref _funcs);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ulong GetCurrentTick()=> ClockImplement.GetTick( ref _funcs);

    /// <summary>
    /// Frequency is often represented as the number of oscillations or cycles that occur within a certain period of time. In physics, frequency is typically measured in hertz (Hz), which is the number of oscillations or cycles per second
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ulong GetFrequency() => ClockImplement.GetFrequency(ref _funcs);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public double GetElapsedInMiliSec() => ClockImplement.GetElapsedInMiliSec(ref _data ,ref _funcs);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int GetApproximativFPS() => ClockImplement.GetApproximativFPS(ref _data ,ref _funcs);

    /// <summary>
    /// Do when got focus or end move/resize window
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Start() => ClockImplement.Start( ref _data ,ref _funcs);
    /// <summary>
    /// Do when lost focus or in resize / move mode 
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Pause() => ClockImplement.Stop(ref _data, ref _funcs);

    /// <summary>
    /// Do before loop
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Reset() => ClockImplement.Reset(ref _data, ref _funcs);
    public float TotalTime
        => _data.IsInPause 
            ? (float)((_data.StopTime - _data.PausedTime - _data.BaseTime) * _data.SecondPerCycle) 
            : (float)((ClockImplement.GetTick( ref _funcs) - _data.PausedTime - _data.BaseTime) * _data.SecondPerCycle);

    public float DeltaTime => (float) _data.ElapsedInMiliSec;

    #region OVERRIDE    
    public override string ToString() => string.Format($"Clock" );
    public override int GetHashCode() => HashCode.Combine( _data.GetHashCode(), _funcs.GetHashCode());
    public override bool Equals(object? obj) => obj is Clock  clock && this.Equals(clock) ;
    public bool Equals(Clock other)=>  _data.Equals(other._data) ;
    public static bool operator ==(Clock  left, Clock right) => left.Equals(right);
    public static bool operator !=(Clock  left, Clock right) => !left.Equals(right);
    #endregion
} 
    

public static partial class ClockImplement
{
    public unsafe static void InitClock(ref ClockData data , ref ClockFunctions func , in PlatformConfig config)
    {
        #if WIN64
        func = new( config.LibraryName_Kernel);
        data.FixedTimeStep  = config.Clock_FixedTimeStep;
        data.SecondPerCycle  = 1.0 / GetFrequency(ref func) ;
        #endif
        switch( config.Clock_LoopMode)
        {
            case ClockLoopMode.Default :
                data.LoopMethod  = Update_Default;
                break;
            case ClockLoopMode.Interpolation :
                data.LoopMethod = Update_Interpolation;
                break;
            case ClockLoopMode.Accurate :
                data.LoopMethod = Update_Accurate;
                break;                       
            case ClockLoopMode.FixedTimeStep:
                data.LoopMethod = Update_FixedTimeStep;
                break;     
            case ClockLoopMode.SequencingPattern:
                data.LoopMethod = Update_SequencingPattern;
                break;
            default :
                break;
        } 
    }

    public unsafe static ulong GetTick(ref ClockFunctions func )
    {
        #if WIN64
        _ = func.QueryPerformanceCounter( out UInt64 tick);
        return tick;
        #else
        return 0L;
        #endif
    }

    public unsafe static ulong GetFrequency(ref ClockFunctions func ) 
    {
        #if WIN64
        _= func.QueryPerformanceFrequency( out UInt64 freq);
        return freq;
        #else
        return 0L;
        #endif
    }

    public  static double GetElapsedInMiliSec(ref ClockData data ,ref ClockFunctions func ) 
        => (GetTick(ref func) -  data.PreviousTick) * data.SecondPerCycle; 

// https://github.com/Syncaidius/MoltenEngine/blob/master/Molten.Utility/Timing.cs
//source : https://github.com/discosultan/VulkanCore/blob/master/Samples/MiniFramework/Timer.cs
//See microsoft . https://docs.microsoft.com/en-us/windows/win32/sysinfo/acquiring-high-resolution-time-stamps    
   
    public static void Reset(ref ClockData data  ,ref ClockFunctions func)
    {
        ulong curTime = GetTick(ref func );
        data.BaseTime = curTime;
        data.PreviousTick = curTime;
        data.StopTime = 0;
        data.IsInPause = false;
    }

    public  static void Start(ref ClockData data  ,ref ClockFunctions func)
    {
        ulong startTime = GetTick(ref func );
        if (data.IsInPause)
        {
            data.PausedTime += (startTime - data.StopTime);
            data.PreviousTick = startTime;
            data.StopTime = 0;
           data.IsInPause = false;
        }
    }

    public  static void Stop(ref ClockData data  ,ref ClockFunctions func)
    {
        if (!data.IsInPause)
        {
            ulong curTime =  GetTick(ref func );
            data.StopTime = curTime;
           
        }
        data.IsInPause = true;
    }

    public  static void Tick(ref ClockData data  ,ref ClockFunctions func)
    {
        if (data.IsInPause)
        {
            data.ElapsedInMiliSec = 0.0;
            return;
        }

        ulong curTime =GetTick(ref func );

        data.ElapsedInMiliSec = ( curTime  - data.PreviousTick) * data.SecondPerCycle ;

        data.PreviousTick = curTime;
        if ( data.ElapsedInMiliSec < 0.0)
            data.ElapsedInMiliSec = 0.0;
    }


    public static int GetApproximativFPS(ref ClockData data ,ref ClockFunctions func )
        => (int)  RitaEngine.Math.Helper.Round(   ( 1.0/  GetElapsedInMiliSec( ref  data ,ref  func) ));

    public unsafe static void Update_Default( ref ClockData data ,ref ClockFunctions func  )
    {
        #if WIN64
        Tick(ref data ,ref func);
        #endif
    }

    /// <summary>
    /// Need for physics loop
    /// Use interpolation between frame see : https://dewitters.com/dewitters-gameloop/
    /// </summary>
    public unsafe static void Update_Interpolation( ref ClockData data ,ref ClockFunctions func )
    {
        //  const int TICKS_PER_SECOND = 25; /// 25fois par seconde 
        //     const int SKIP_TICKS = 1000 / TICKS_PER_SECOND; 
        //     const int MAX_FRAMESKIP = 5;

        //     DWORD next_game_tick = GetTickCount();
        //     int loops;
        //     float interpolation;

        //     bool game_is_running = true;
        //     while( game_is_running ) {

        //         loops = 0;
        //         while( GetTickCount() > next_game_tick && loops < MAX_FRAMESKIP) {
        //             update_game();

        //             next_game_tick += SKIP_TICKS;
        //             loops++;
        //         }

        //         interpolation = float( GetTickCount() + SKIP_TICKS - next_game_tick )
        //                         / float( SKIP_TICKS );
        //         display_game( interpolation );
    }
    /// <summary>
    /// Need for physics loop
    /// Use Accurate between frame see :  https://gamedev.stackexchange.com/questions/52841/the-most-efficient-and-accurate-game-loop
    /// </summary>
    public unsafe static void Update_Accurate(ref ClockData data ,ref ClockFunctions func  )
    {
        //  this.requestFocus();
        //       long lastTime = System.nanoTime();
        //       double amountOfTicks = 60.0;
        //       double ns = 1000000000 / amountOfTicks;
        //       double delta = 0;
        //       long Clock = System.currentTimeMillis();
        //       int frames = 0;
        //       while(isRunning) {
        //        long now = System.nanoTime();
        //        delta += (now - lastTime) / ns;
        //        lastTime = now;
        //        while(delta >= 1) {
        //         update();
        //         //updates++;
        //         delta--;
        //        }
        //        render();
        //        frames++;

        //        if(System.currentTimeMillis() - Clock > 1000) {
        //         Clock += 1000;
        //         frames = 0;
        //         //updates = 0;
        //        }
        //       }
        //       stop();
    }
    /// <summary>
    /// https://gafferongames.com/post/fix_your_timestep/
    /// </summary>
    public unsafe static void Update_FixedTimeStep(ref ClockData data ,ref ClockFunctions func  )
    {
        //     double t = 0.0;
        //     double dt = 0.01;                                  itme.t Dt float

        //     double currentTime = hires_time_in_seconds();      PreviousTime  in second
        //     double accumulator = 0.0;                           Accumulator = 0.0f

        //     State previous;
        //     State current;                                      FIXED_LOOP = 0.25 sec

        //     while ( !quit )
        //     {
        //         double newTime = time();                    CurrentTime = getTime
        //         double frameTime = newTime - currentTime;   time FRameTime = current - previous
        //         if ( frameTime > 0.25 )                     if FrameTim > FIXED LOOP

        //             frameTime = 0.25;                           frameTime = Fixed_loop
        //         currentTime = newTime;

        //         accumulator += frameTime;                   Accumulator += framtime in sec

        //         while ( accumulator >= dt )             while( accu >= dt )

        //         {
        //             previousState = currentState;           
        //             integrate( currentState, t, dt );       UpdatePhysics( t, dt )
        //             t += dt;                                t += dt ;
        //             accumulator -= dt;                      accumlator -= dt ;
        //         }

        //         const double alpha = accumulator / dt;      alpha(interpolation)
        //                                                             = accu/dt + (.0-alpha)

        //         State state = currentState * alpha + 
        //             previousState * ( 1.0 - alpha );

        //         render( state );
        //     }

    }
    /// <summary>
    /// // https://gameprogrammingpatterns.com/game-loop.html
    /// </summary>
    public unsafe static void Update_SequencingPattern( ref ClockData data ,ref ClockFunctions func  )
    {
        
        // double previous = getCurrentTime();
        // double lag = 0.0;
        // MS_PER_UPDATE = 1/ 60 ? ( fixe time step)
        // while (true)
        // {
        //   double current = getCurrentTime();
        //   double elapsed = current - previous;
        //   previous = current;
        //   lag += elapsed;

        //   processInput();

        //   while (lag >= MS_PER_UPDATE)
        //   {
        //     update();
        //     lag -= MS_PER_UPDATE;
        //   }

        //   render();

    }
  
}
