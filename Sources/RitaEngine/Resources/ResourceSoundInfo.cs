namespace RitaEngine.Resources;

public struct ResourceSoundInfo
{
    public int AudioFormat    ;// (2 octets) : Format du stockage dans le fichier (1: PCM, 2: ADPCM  , ...)
    public int NbrCanaux      ;// (2 octets) : Nombre de canaux (de 1 � 6, cf. ci-dessous)
    public int Frequence      ;// (4 octets) : Fr�quence d'�chantillonnage (en hertz) [Valeurs standardis�es : 11025, 22050, 44100 et �ventuellement 48000 et 96000]
    public int BytePerSec     ;// (4 octets) : Nombre d'octets � lire par seconde (i.e., Frequence * BytePerBloc).
    public int BytePerBloc    ;// (2 octets) : Nombre d'octets par bloc d'�chantillonnage (i.e., tous canaux confondus : NbrCanaux * BitsPerSample/8).
    public int BitsPerSample  ;// (2 octets) : Nombre de bits utilis�s pour le codage de chaque �chantillon (8, 16, 24)
}


