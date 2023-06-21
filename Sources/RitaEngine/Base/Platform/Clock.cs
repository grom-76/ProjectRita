
namespace RitaEngine.Base.Platform;

using RitaEngine.Base.Platform.Config;
using RitaEngine.Base.Platform.Structures;

[ StructLayout(LayoutKind.Sequential, Pack = BaseHelper.FORCE_ALIGNEMENT),SkipLocalsInit]
public struct Clock: IEquatable<Clock>
{
    private ClockData _data;
    private ClockFunctions _funcs;


    public Clock() { }
  
    // <summary> Universal Time,  Tick & Frequence </summary>
    // https://github.com/Syncaidius/MoltenEngine/blob/master/Molten.Utility/Timing.cs

    public void Init( PlatformConfig config)
    {
        InitClock(ref _data , ref _funcs , in config);
    }

    public void Pause() => Pause(ref _data, ref _funcs);
    public void Release()
    {
        // nothing here ...
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Update()=>_data.LoopMethod(ref _data ,ref _funcs);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ulong GetCurrentTick()=> GetTick( ref _funcs);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ulong GetFrequency() => GetFrequency(ref _funcs);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public double GetElapsed_ms() => GetElapsed_ms(ref _data ,ref _funcs);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int GetApproximativFPS() => GetApproximativFPS(ref _data ,ref _funcs);


    private unsafe static void InitClock(ref ClockData data , ref ClockFunctions func , in PlatformConfig config)
    {
        #if WIN64
        func = new( config.LibraryName_Clock);
        data.FixedTimeStep  = config.Clock_FixedTimeStep;
        data.OneOverFrequency  = 1.0 / GetFrequency(ref func) ;
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

    private unsafe static ulong GetTick(ref ClockFunctions func )
    {
        #if WIN64
        _ = func.QueryPerformanceCounter( out UInt64 tick);
        return tick;
        #else
        return 0L;
        #endif
        
    }

    private unsafe static ulong GetFrequency(ref ClockFunctions func ) 
    {
        #if WIN64
        _= func.QueryPerformanceFrequency( out UInt64 freq);
        return freq;
        #else
        return 0L;
        #endif
       
    }

    /// <summary>
    /// //See microsoft . https://docs.microsoft.com/en-us/windows/win32/sysinfo/acquiring-high-resolution-time-stamps
    /// </summary>
    // [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    
    private  static double GetElapsed_ms(ref ClockData data ,ref ClockFunctions func ) 
    => (GetTick(ref func) -  data.PreviousTick) * data.OneOverFrequency; 

    private static void Pause(ref ClockData data  ,ref ClockFunctions func)
    {
        data.IsPaused = true;
        data.PreviousTick = GetTick(ref func);
        data.Elapsed_ms = 0.0;
        //time stop
        //Save accumulate time  ( temps depuis le debut de la pause)
    }

//source : https://github.com/discosultan/VulkanCore/blob/master/Samples/MiniFramework/Timer.cs

    // private ulong _baseTime;
    // private ulong _pausedTime;
    // private ulong _stopTime;
    // private ulong _prevTime;
    // private ulong _currTime;
    // private bool _stopped;
    // public void Reset()
    // {
    //     ulong curTime = GetCurrentTick();
    //     _baseTime = curTime;
    //     _prevTime = curTime;
    //     _stopTime = 0;
    //     _stopped = false;
    // }

    // public void Start()
    // {
    //     ulong startTime = GetCurrentTick();
    //     if (_stopped)
    //     {
    //         _pausedTime += (startTime - _stopTime);
    //         _prevTime = startTime;
    //         _stopTime = 0;
    //         _stopped = false;
    //     }
    // }

    // public void Stop()
    // {
    //     if (!_stopped)
    //     {
    //         ulong curTime = GetCurrentTick();
    //         _stopTime = curTime;
    //         _stopped = true;
    //     }
    // }

    // public void Tick()
    // {
    //     if (_stopped)
    //     {
    //         _deltaTime = 0.0;
    //         return;
    //     }

    //     ulong curTime =GetCurrentTick();
    //     _currTime = curTime;
    //     _deltaTime = (_currTime - _prevTime) * _secondsPerCount;

    //     _prevTime = _currTime;
    //     if (_deltaTime < 0.0)
    //         _deltaTime = 0.0;
    // }
    // public float TotalTime
    // {
    //     get
    //     {
    //         if (_stopped)
    //             return (float)((_stopTime - _pausedTime - _baseTime) * _secondsPerCount);

    //         return (float)((_currTime - _pausedTime - _baseTime) * _secondsPerCount);
    //     }
    // }


    private static int GetApproximativFPS(ref ClockData data ,ref ClockFunctions func )
    => (int)  RitaEngine.Base.Math.Helper.Round(   ( 1.0/  GetElapsed_ms( ref  data ,ref  func) ));
    

    // [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe static void UpdateClock( ref ClockData data ,ref ClockFunctions func  )=> data.LoopMethod(ref data ,ref func);

    public unsafe static void Update_Default( ref ClockData data ,ref ClockFunctions func  )
    {
        #if WIN64
        data.Elapsed_ms = GetElapsed_ms(ref  data ,ref  func );
        data.PreviousTick = GetTick(ref func );
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

    #region OVERRIDE    
    public override string ToString() => string.Format($"Data Window " );
    public override int GetHashCode() => HashCode.Combine( _data.GetHashCode(), _funcs.GetHashCode());
    public override bool Equals(object? obj) => obj is Clock  window && this.Equals(window) ;
    public bool Equals(Clock other)=>  _data.Equals(other._data) ;
    public static bool operator ==(Clock  left, Clock right) => left.Equals(right);
    public static bool operator !=(Clock  left, Clock right) => !left.Equals(right);
    #endregion
} 
    

