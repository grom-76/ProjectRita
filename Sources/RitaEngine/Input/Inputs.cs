namespace RitaEngine.Input;

using RitaEngine.API.Window;
using RitaEngine.Platform;
using RitaEngine.Base;
using RitaEngine.API;

[StructLayout(LayoutKind.Sequential, Pack = BaseHelper.FORCE_ALIGNEMENT),SkipLocalsInit]
public struct Inputs
{
    private InputData _data ;
    private InputFunctions _funcs ;
    public Inputs()  {  }
  
    public unsafe void Init(PlatformConfig config, in Window win)
    {
        #if WIN64
        _data = new();
        _data.User32 = Libraries.Load( config.LibraryName_User32);
        _data.WindowHandle = win.GetWindowHandle();
        _funcs = new( Libraries.GetUnsafeSymbol , _data.User32);

        InputImplement.KeyMapping(config.Input_MappingKeysToAzerty,  config.Input_Langue == "FRA" ?0:1);

        #else

        #endif

    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Update()=> InputImplement.Update(ref _funcs ,ref _data);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Release()=> InputImplement.Release(ref _data);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsKeyDown( int key)=> InputImplement.IsKeyDown(ref _data, key);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsKeyUp( int key)=> InputImplement.IsKeyUp(ref _data, key);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsKeyPressed( int key)=> InputImplement.IsKeyPressed(ref _data, key);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsKeyReleased( int key)=> InputImplement.IsKeyReleased(ref _data, key);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int[]  GetKeysPressed()=> InputImplement.GetKeysPressed(ref _data);



    //TODO convert int[ ] to list of name key 
    
    /// Vérifie si la touche passé en paramètre est appuyé
    /// </summary>
    /// <param name="key">code de la touche a tester voir enum Keys de 0 à 255</param>
    /// <returns>true si la touche est enfoncé sinon false</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool IsKeyState(int key) =>  InputImplement.IsKeyState(ref _funcs, key);
    
    /// <summary>
    /// Vérifie si la touche passé en paramètre est appuyé
    /// Attention fonctionne que si une Fenetre est activé
    /// </summary>
    /// <param name="key">code de la touche a tester voir enum Keys de 0 à 255</param>
    /// <returns>true si la touche est enfoncé sinon false</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsAsyncKeyState( int key)=> InputImplement.IsAsyncKeyState(ref _funcs, key);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public  bool IsMouseButtonDown(int  button ) => InputImplement.IsMouseButtonDown(ref _data, button);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public  bool IsMouseButtonUp(int  button ) => InputImplement.IsMouseButtonUp(ref _data, button);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public  bool IsMouseButtonReleased(int  button ) => InputImplement.IsMouseButtonReleased(ref _data, button);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public  bool IsMouseButtonPressed(int  button ) => InputImplement.IsMouseButtonPressed(ref _data, button);

    /// <summary>
    /// Obtiens la position de la souris sur l'écran (Monitor)
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe void GetCurrentMousePositionOnScreen( ref int x , ref int y ) => InputImplement.GetMousePos(ref _funcs , _data.WindowHandle,false, ref x , ref y);
     /// <summary>
    /// Obtiens la position de la souris sur la Fenetre RitaEngine (Window)
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe void GetCurrentMousePositionOnWindow( ref int x , ref int y ) => InputImplement.GetMousePos(ref _funcs , _data.WindowHandle,true, ref x , ref y);
    

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe void SetMousePositionOnScreen( int x, int y )=> InputImplement.SetMousePos(ref _funcs, _data.WindowHandle , false, x,  y );

     [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe void SetMousePositionOnWindow( int x, int y )=> InputImplement.SetMousePos(ref _funcs, _data.WindowHandle , true, x,  y );

   

}


[SuppressUnmanagedCodeSecurity, StructLayout(LayoutKind.Sequential, Pack = BaseHelper.FORCE_ALIGNEMENT),SkipLocalsInit]
public static class InputImplement
{
 //TODO SETDELTA in update  is mouse leave ....

    #region IMPLEMENTATION
    public unsafe static int Update(ref InputFunctions funcs, ref InputData data)
    {
        #if WIN64

        fixed( byte* keys = data.CurrentKeys , prev = data.PreviousKeys )
        {
            Unsafe.CopyBlock(prev, keys, 256);
            _ = funcs.GetKeyboardState( keys);
        }
        data.Mouse_PreviousFrame_Position_X = data.Mouse_CurrentFrame_Position_X;
        data.Mouse_PreviousFrame_Position_Y = data.Mouse_CurrentFrame_Position_Y;
        
        GetMousePos(ref funcs , data.WindowHandle, true, ref data.Mouse_CurrentFrame_Position_X , ref data.Mouse_CurrentFrame_Position_Y);
        
       data.Mouse_CurrentFrame_Delta_X = ( data.Mouse_CurrentFrame_Position_X -  data.Mouse_PreviousFrame_Position_X );
       data.Mouse_CurrentFrame_Delta_Y = ( data.Mouse_CurrentFrame_Position_Y -  data.Mouse_PreviousFrame_Position_Y );
       
       #elif LINUX

       #endif
       return 0;
    }

    public static int Release(ref InputData data)
    {
        #if WIN64
       
        #else

        #endif
        return 0;
    }

    public  static bool IsKeyDown( ref InputData input , int key)
    #if WIN64
        => (input.CurrentKeys [key & 0xff] & 0x80) != 0 && (input.PreviousKeys[key & 0xff] & 0x80) != 0;
    #else
        => false;
    #endif
       
    public  static bool  IsKeyUp(ref InputData input , int key )
    #if WIN64
        =>     (input.CurrentKeys [key & 0xff] & 0x80) == 0 && (input.PreviousKeys[key & 0xff] & 0x80) == 0;
    #else
        => false;
    #endif
    
    public  static bool  IsKeyPressed(ref InputData input ,int key )
    #if WIN64
        =>     (input.CurrentKeys [key & 0xff] & 0x80 ) != 0 && (input.PreviousKeys[key & 0xff] & 0x80 ) == 0;
      #else
        => false;
    #endif

    public  static bool  IsKeyReleased( ref InputData input ,int key )
    #if WIN64
        =>     (input.CurrentKeys [key & 0xff] & 0x80) == 0 && (input.PreviousKeys[key & 0xff] & 0x80) != 0;
      #else
        => false;
    #endif

    public static int[]  GetKeysPressed(ref InputData input )
    {
        #if WIN64
        int count =0;
        for(int i = 0 ; i < 256; i++)
        {
            if ( input.CurrentKeys[i] == 1)count++;
        }
        if (  count ==0) return null!;

        int[] list = new int[count];

        int index =0 ;
        for(int i = 0 ; i < 256; i++)
        {
            if ( input.CurrentKeys[i] == 1)list[index++] = i ;
        }

        return list;
        #else
        return null!;
        #endif
    }

    public  unsafe static bool IsKeyState(ref InputFunctions input ,int key) 
    #if WIN64
    => (input.GetKeyState( key& 0xFF) & 0x80 ) != 0;
    #else
        => false;
    #endif

    public unsafe static bool IsAsyncKeyState(ref InputFunctions input , int key)
    #if WIN64
        => (input.GetAsyncKeyState(key& 0xFF)  & 0x80 ) != 0;
    #else
        => false;
    #endif

   
    public  static  bool IsMouseButtonDown(ref InputData input ,int  button )
    #if WIN64
        => (input.CurrentKeys [button & 0xFF] & 0x80) != 0 && (input.PreviousKeys[button & 0xFF] & 0x80) != 0;
    #else
        => false;
    #endif
    
   
    public static bool IsMouseButtonUp(ref InputData input , int  button )
    #if WIN64
        => (input.CurrentKeys [button & 0xFF] & 0x80) == 0 && (input.PreviousKeys[button & 0xFF] & 0x80) == 0;
          #else
        => false;
    #endif
    
   
    public static bool IsMouseButtonPressed(ref InputData input , int  button )
    #if WIN64
        => (input.CurrentKeys [button & 0xFF] & 0x80) != 0 && (input.PreviousKeys[button & 0xFF] & 0x80) == 0;
          #else
        => false;
    #endif
    
    public static  bool IsMouseButtonReleased( ref InputData input ,int  button )
    #if WIN64
        => (input.CurrentKeys [button & 0xFF] & 0x80) == 0 && (input.PreviousKeys[button & 0xFF] & 0x80) != 0; 
    #else
        => false;
    #endif

   
    public static unsafe void GetMousePos(ref InputFunctions input ,void* hwnd, bool onWindow,ref int x, ref int y)
    {
        #if WIN64
        POINT p ;
        input.GetCursorPos( &p);
        if( hwnd != null && onWindow)
            input.ScreenToClient(hwnd ,&p );
        x = p.X;
        y = p.Y;
        #else
        x = 0; y = 0;
        #endif
    }

  
    public unsafe static void SetMousePos(ref InputFunctions input  ,void* hwnd, bool onWindow,int x, int y ) 
    {
         #if WIN64
        POINT p  = new POINT(x, y);

        if (hwnd != null && onWindow )
            input.ClientToScreen(hwnd,&p);

        input.SetCursorPos(p.X, p.Y);
        #else

        #endif
    }

    public static void KeyMapping(bool azerty = true, int lang =0 /*0 = FRA , 1 = ENG*/)
    {
        #if WIN64
        //TODO Lang = FR   azerty , qwerty other , ..
        InputKeys.Escape =(byte)( lang == 0 ? (azerty ? 0x1B : 0x1B) : (azerty ? 0x1B : 0x1B) ) ;
        InputKeys.Space =(byte)( lang == 0 ? (azerty ?  0x20 :  0x20) : (azerty ? 0x20 :  0x20) ) ;
        InputKeys.Left = 0x25;
        InputKeys.Up = 0x26;
        InputKeys.Right = 0x27;
        InputKeys.Down = 0x28;
       #endif

    }
    #endregion
    #region Raylib Bindinggs
      /// <summary>
    /// /// INput manager see https://github.com/vchelaru/FlatRedBall/blob/NetStandard/Engines/FlatRedBallXNA/FlatRedBall/Input/Windows/WindowsInputEventManager.cs
    /// </summary>
    // public struct Inputs{
    //     //GETKEYBOARD
    //     //GETMOUSE
    //     //GETJOYSTICK
    //     //GETHAPTIC
    //     //GET
    // } // Keyboard Mouse WindowEvents?
    //------------------------------------------------------------------------------------
    // Input Handling Functions (Module: core)
    //------------------------------------------------------------------------------------

    // Input-related functions: keyboard
    // bool IsKeyPressed(int key);                             // Check if a key has been pressed once
    // bool IsKeyDown(int key);                                // Check if a key is being pressed
    // bool IsKeyReleased(int key);                            // Check if a key has been released once
    // bool IsKeyUp(int key);                                  // Check if a key is NOT being pressed
    // void SetExitKey(int key);                               // Set a custom key to exit program (default is ESC)
    // int GetKeyPressed(void);                                // Get key pressed (keycode), call it multiple times for keys queued, returns 0 when the queue is empty
    // int GetCharPressed(void);                               // Get char pressed (unicode), call it multiple times for chars queued, returns 0 when the queue is empty

    // // Input-related functions: gamepads
    // bool IsGamepadAvailable(int gamepad);                   // Check if a gamepad is available
    // const char *GetGamepadName(int gamepad);                // Get gamepad internal name id
    // bool IsGamepadButtonPressed(int gamepad, int button);   // Check if a gamepad button has been pressed once
    // bool IsGamepadButtonDown(int gamepad, int button);      // Check if a gamepad button is being pressed
    // bool IsGamepadButtonReleased(int gamepad, int button);  // Check if a gamepad button has been released once
    // bool IsGamepadButtonUp(int gamepad, int button);        // Check if a gamepad button is NOT being pressed
    // int GetGamepadButtonPressed(void);                      // Get the last gamepad button pressed
    // int GetGamepadAxisCount(int gamepad);                   // Get gamepad axis count for a gamepad
    // float GetGamepadAxisMovement(int gamepad, int axis);    // Get axis movement value for a gamepad axis
    // int SetGamepadMappings(const char *mappings);           // Set internal gamepad mappings (SDL_GameControllerDB)

    // // Input-related functions: mouse
    // bool IsMouseButtonPressed(int button);                  // Check if a mouse button has been pressed once
    // bool IsMouseButtonDown(int button);                     // Check if a mouse button is being pressed
    // bool IsMouseButtonReleased(int button);                 // Check if a mouse button has been released once
    // bool IsMouseButtonUp(int button);                       // Check if a mouse button is NOT being pressed
    // int GetMouseX(void);                                    // Get mouse position X
    // int GetMouseY(void);                                    // Get mouse position Y
    // Vector2 GetMousePosition(void);                         // Get mouse position XY
    // Vector2 GetMouseDelta(void);                            // Get mouse delta between frames
    // void SetMousePosition(int x, int y);                    // Set mouse position XY
    // void SetMouseOffset(int offsetX, int offsetY);          // Set mouse offset
    // void SetMouseScale(float scaleX, float scaleY);         // Set mouse scaling
    // float GetMouseWheelMove(void);                          // Get mouse wheel movement for X or Y, whichever is larger
    // Vector2 GetMouseWheelMoveV(void);                       // Get mouse wheel movement for both X and Y
    // void SetMouseCursor(int cursor);                        // Set mouse cursor

    // // Input-related functions: touch
    // int GetTouchX(void);                                    // Get touch position X for touch point 0 (relative to screen size)
    // int GetTouchY(void);                                    // Get touch position Y for touch point 0 (relative to screen size)
    // Vector2 GetTouchPosition(int index);                    // Get touch position XY for a touch point index (relative to screen size)
    // int GetTouchPointId(int index);                         // Get touch point identifier for given index
    // int GetTouchPointCount(void);                           // Get number of touch points
    #endregion
  
}
