namespace RitaEngine.Base.Platform.Structures;

[StructLayout(LayoutKind.Sequential, Pack = BaseHelper.FORCE_ALIGNEMENT),SkipLocalsInit]
public  struct WindowEvent : IEquatable<WindowEvent>
{
    public delegate void EventDelegate(  nuint wParam,  nint lParam);

    /// <summary> Perte du focus fenetre n'est plus active </summary>
    public EventDelegate OnKillFocus = EmptyEvent;
    /// <summary> . </summary>
    public EventDelegate OnSetFocus =EmptyEvent;
    /// <summary> . </summary>
    public EventDelegate OnQuit =EmptyEvent;
    /// <summary> . </summary>
    public EventDelegate OnDestroy=EmptyEvent;
    /// <summary> . </summary>
    public EventDelegate OnClose=EmptyEvent;
    /// <summary>. </summary>
    public EventDelegate OnKeyUp=EmptyEvent;
    /// <summary> . </summary>
    public EventDelegate OnShowWindow =EmptyEvent;
    /// <summary> . </summary>
    public EventDelegate OnKeyPress=EmptyEvent;
    /// <summary> . </summary>
    public EventDelegate OnInput= EmptyEvent;
    /// <summary> . </summary>
    public EventDelegate OnMouseButtonClic= EmptyEvent;
    /// <summary>  .</summary>
    public EventDelegate OnSize = EmptyEvent;

    private static void EmptyEvent(nuint wParam, nint lParam)
    {
       _=wParam; _=lParam;
    }

    public WindowEvent()
    {

    }

    public void Release()
    {
        OnKeyPress = null!;
		OnKillFocus = null!;
		OnSetFocus = null!;
		OnQuit = null!;
		OnDestroy= null!;
		OnClose= null!;
		OnKeyUp= null!;
		OnShowWindow = null!;
		OnInput = null!;
		OnMouseButtonClic = null!;
		OnSize = null!;
    }

    #region OVERRIDE    
    public override string ToString() => string.Format($"Window Event " );
    public unsafe override int GetHashCode() => HashCode.Combine( OnKillFocus, OnSetFocus);
    public override bool Equals(object? obj) => obj is WindowEvent  window && this.Equals(window) ;
    public unsafe bool Equals(WindowEvent other)=>  OnKillFocus == other.OnKillFocus && OnSetFocus == other.OnSetFocus ;
    public static bool operator ==(WindowEvent  left,WindowEvent right) => left.Equals(right);
    public static bool operator !=(WindowEvent  left,WindowEvent right) => !left.Equals(right);
    #endregion
}
