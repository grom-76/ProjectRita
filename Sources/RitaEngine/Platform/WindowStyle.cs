namespace RitaEngine.Platform;

public enum WindowStyle
{
  Fullscreen=-1,
    Popup,
    TitleBar,
    CloseButtons,
    Resizable,
    Bordered,
    Default=Bordered | Resizable | TitleBar,
}
