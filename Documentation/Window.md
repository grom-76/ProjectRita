# WINDOW

## Installation Window

### PREREQUIS

    Window 8 10 11
    Driver avec vulkan installé
    [ SDK .Net7.0 ](https://dotnet.microsoft.com/en-us/download)
    Visual studio code
    git
    VS code extension DOC : omnisharp / C#Extension

### Utilisation

    Copie le repertoire Build\Window\*.* dans votre répertoire créer pour votre jeu
    dans le csproj ajouter la ligne suivante : 

### Get started  like monogame

    program.cs
        using game = g new();
        g.Run();

    game.cs
    
    public class Game : MCJEngine.GameBase
    {
        public Init()
        {

        }

        public Load()
        {

        }

        public Loop()
        {

        }

        public Shutdown()
        {

        }
    }

### get started like Viking:  c++ as sdl / raylib / glfw / sfml /

```c#
// program.cs
    using var game = new MCJEngine.GameContext( );
    game.Surface.Settigns.Title ="Mon Jeu";
    game.Create();

    while( game.HasQit() )
    {
        game.Update();

        if(game.HasRenderReady())
        {

            game.DisplayOn();
        }
    }

    game.Dispose(); // not necessary if use keyword using 
```

### Compilation / Execution

    dotnet build et dotnet run  : thats all
