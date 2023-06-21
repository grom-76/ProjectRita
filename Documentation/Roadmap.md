# ROADMAP

    [] Faire un Editeur avec blueprint
    [] Clean Code , avec tests et documentation
    [x] Touts la configuration du system dansune même structure
    [] Faire un schema, architecture du moteur pour vulkan

ajouter une interface pour Steam
Inégration de JNI ( wasm ) pour wEB
commencer le portage c# de miniaudio et meshoptimizer
Créer un dossier IMAGES
    mettre class Pixel ( px to mm )
    Rewrite STB , void ImageSharp e SixLabor
Refaire CONFIG_RE_SYSTEM  all in one
    puis amélioré Platform ( context ) big endian, architecture, Os, GarbageCollector options, add Enum graphic ( right end , origine )

Vulkan => RIGHT HAND -X AND ZERO TO ONE
see : [https://matthewwellings.com/blog/the-new-vulkan-coordinate-system/]

    ```c#
//Suppress this code everywhere

    public unsafe nint AddressOfPtrThis( )
    { 
        #pragma warning disable CS8500
        fixed (void* pointer = &this )  { return((nint) pointer ) ; }  
        #pragma warning restore
    }
    ```

Geometric prmitive create cube with widh height (discosultan
)

