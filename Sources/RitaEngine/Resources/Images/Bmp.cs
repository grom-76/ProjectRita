namespace  RitaEngine.Resources.Images;

using System;
using RitaEngine.Base.IO;


/// <summary>
/// Stand alon rewrite stb bmp 
/// </summary>
public class BmpReader : IDisposable
{
    internal byte[] data= null!;
    internal int Width ;
    internal int height;
    internal int color;

    // Private bmp info
    private uint offset =0 ;
    private uint hsz =0;
    private uint imgX=0;
    private uint imgY=0;
    private uint bpp=0;

    private uint compress=0;

    private  uint r=0;
    private uint g=0;
    private uint b=0;
    private uint a = 0;
    private uint all=0;

    private readonly Reader io;
    internal BmpReader(string filename,int req_comp=0)
    {
        if ( string.IsNullOrEmpty(filename))
            throw new Exception("File not to be empty");

        _ =req_comp;
        io = new Reader(filename);
    }

    /// <summary>
    ///  Lecture de l'entete du fichier
    /// </summary>
    internal void  BMPHeader()
    {
        _ = io.Byte();
        _ = io.Byte();
        //if ( b != ='B' || m != 'M')return "error not Bmp";

        io.Forward(8);
        offset = io.UInt();//54
        hsz = io.UInt();//40
        //if ( hsz != 12 && hsz != 40 && hsz != 56 && hsz != 108 && hsz != 124) return "Unlnown bmp";

        imgX = hsz == 12 ? io.UShort() : io.UInt() ;
        imgY = hsz == 12 ? io.UShort() : io.UInt() ;

        if (  io.UShort() != 1 ) return ; //if ( u4 != 1) return " Bad bmp" ; 

        bpp = io.UShort();

        if ( hsz == 12) return ;

        compress =  io.UInt() ;
        //if ( compress ==1 || compress == 2 ) return "BMP RLE" 
        io.Forward(20);

        if ( hsz == 40 || hsz == 56 )
        {
            if ( hsz == 56 )
                io.Forward(16);

            if ( bpp == 16 || bpp==32 )
            {
                if ( compress == 0 )
                {
                    r = ( bpp == 32 ) ? 0xffu << 16 : 31u << 10;
                    g = ( bpp == 32 ) ? 0xffu << 8 : 31u << 5;
                    b = ( bpp == 32 ) ? 0xffu << 0 : 31u << 0;
                    a = ( bpp == 32 ) ? 0xffu << 24 : 0;
                }
                else if ( compress == 3 )
                {
                    r = (uint)io.UInt();
                    g = (uint)io.UInt();
                    b = (uint)io.UInt();
                    //if (info->mr == info->mg && info->mg == info->mb)return "bad BMP";   
                }
                else
                {
                    //return BAD BMP
                }
            }
        }
        else // 108 124 ........
        {
            r = io.UInt();
            g = io.UInt();
            b = io.UInt();
            a = io.UInt();

            io.Forward( hsz == 124 ? 52 + 16 :  52 );
        }
    }

    internal void LoadBMP(int req_comp=0)
    {
        all = 255;
        BMPHeader();

        byte[] pal = new byte[ 256 * 4 ];
        var psize = (hsz == 12) ?
                        bpp < 24 ? (int)((offset - 14 - 24) / 3) : 0
                    : bpp < 16 ? (int)((offset - 14 - hsz) >> 2) : 0;

        var context_img_n = a != 0 ? 4 : 3 ;

        // for alpha 
        int target = req_comp !=0 && req_comp >=3  ? req_comp: context_img_n;

        if ( target ==0 || imgX <= 0 || imgY <=0 )
            throw new Exception("target imx ou imy");

        data = new byte[ target * imgX * imgY];

        if ( data == null)
            throw new Exception("out of memory ") ;// 	return ((byte*)((ulong)((stbi__err("outofmem")) != 0 ? ((byte*)null) : (null))));

        int width;
        int pad;
        if (bpp < 16)
        {
            int z = 0;

            if ((psize == 0) || (psize > 256))
                throw new Exception("invalid");

            for (int i = 0; i < psize; ++i)
            {
                pal[(i * 4) + 2] = (byte)io.Byte();
                pal[(i * 4) + 1] = (byte)io.Byte();
                pal[(i * 4) + 0] = (byte)io.Byte();

                if (hsz != 12)
                    io.SByte();

                pal[(i * 4) + 3] = (byte)(255);
            }

            //stbi__skip(s, (int)(offset - 14 - hsz - psize * ((hsz) == (12) ? 3 : 4)));
            io.Rewind((int)(offset - 14 - hsz - (psize * (hsz == 12 ? 3 : 4))));

            if (bpp == 1)
            {
                width = (int)((imgX + 7) >> 3);
            }
            else if (bpp == 4)
            {
                width = (int)((imgX + 1) >> 1);
            }
            else if (bpp == 8)
            {
                width = (int)(imgX);
            }
            else
            {
                Array.Clear(data, 0, data.Length); //CRuntime.free(data);
                throw new Exception("bad bpp");//return ((byte*)((ulong)((stbi__err("bad bpp")) != 0 ? ((byte*)null) : (null))));
            }

            pad = (-width & 3);

            //monochrome
            if (bpp == 1)
            {
                for (int j = 0; j < ((int)(imgY)); ++j)
                {
                    int bit_offset = 7;
                    int v = (int)io.Byte();

                    for (int i = 0; i < ((int)imgX); ++i)
                    {
                        int color = (int)((v >> bit_offset) & 0x1);
                        data[z++] = (byte)pal[(color * 4) + 0];
                        data[z++] = (byte)pal[(color * 4) + 1];
                        data[z++] = (byte)pal[(color * 4) + 2];
                        if (target == 4)
                        {
                            data[z++] = (byte)(255);
                        }

                        if ((i + 1) == ((int)(imgX)))
                            break;
                        if ((--bit_offset) < 0)
                        {
                            bit_offset = (int)(7);
                            v = (int)io.Byte();
                        }
                    }
                    //stbi__skip(s, (int)(pad));
                    io.Rewind(pad);
                }
            }
            else
            {
                for (int j = 0; j < (int)imgY ; ++j)
                {
                    for (int i = 0; i < (int)imgX ; i += 2)
                    {
                        int v = (int)io.Byte();
                        int v2 = 0;
                        if (bpp == 4)
                        {
                            v2 = (int)(v & 15);
                            v >>= 4;
                        }
                        data[z++] = (byte)(pal[(v * 4) + 0]);
                        data[z++] = (byte)(pal[(v * 4) + 1]);
                        data[z++] = (byte)(pal[(v * 4) + 2]);

                        if (target == 4)
                            data[z++] = (byte)(255);

                        if ((i + 1) == ((int)(imgX)))
                            break;

                        v = (int)((bpp == 8) ? io.Byte() : v2);

                        data[z++] = (byte)(pal[(v * 4) + 0]);
                        data[z++] = (byte)(pal[(v * 4) + 1]);
                        data[z++] = (byte)(pal[(v * 4) + 2]);

                        if (target == 4)
                            data[z++] = (byte)(255);
                    }

                    io.Rewind(pad);//stbi__skip(s, (int)(pad));
                }
            }
        }
        else// case of Bpp > 16
        {
            int z = 0;
            io.Rewind((int)(offset - 14 - hsz));//stbi__skip(s, (int)(offset - 14 - hsz));

            width = bpp == 24 ? (int)(3 * imgX) : bpp == 16 ? (int)(2 * imgX) : 0;

            pad = (int)((-width) & 3);

            int easy;
            if (bpp == 24)
            {
                easy = 1;
            }
            else if (bpp == 32 &&  (b == 0xff) && (g == 0xff00) && (r == 0x00ff0000) && (a == 0xff000000) )
            {
                easy = 2;
            }
            else
            {
                easy = 0;
            }

            if (easy == 0 && ((r == 0) || (g == 0) || (b == 0)))
                throw new Exception("BAD mask"); // return ((byte*)((ulong)((stbi__err("bad masks")) != 0 ? ((byte*)null) : (null))));

            int rshift = (int)(High_bit((uint)(r)) - 7);
            int rcount = (int)(BitCount((uint)(r)));
            int gshift = (int)(High_bit((uint)(g)) - 7);
            int gcount = (int)(BitCount((uint)(g)));
            int bshift = (int)(High_bit((uint)(b)) - 7);
            int bcount = (int)(BitCount((uint)(b)));
            int ashift = (int)(High_bit((uint)(a)) - 7);
            int acount = (int)(BitCount((uint)(a)));

            var a0 = easy != 0 ? 2 : 0;
            var a1 = easy != 0 ? 1 : 1;
            var a2 = easy != 0 ? 0 : 2;

            for (int j = 0; j < ((int)(imgY)); ++j)
            {
                for (int i = 0; i < ((int)(imgX)); ++i)
                {
                    uint v = easy != 0 ? 0 : (uint)(bpp == 16 ? (uint)io.UShort() : io.UInt());

                    data[z + a0] = easy != 0 ?
                                (byte)io.Byte()
                            : ((byte)((ShiftSigned((uint)(v & r), (int)(rshift), (int)(rcount))) & 255));
                    data[z + a1] = easy != 0 ?
                                (byte)io.Byte()
                            : ((byte)((ShiftSigned((uint)(v & g), (int)(gshift), (int)(gcount))) & 255));
                    data[z + a2] = easy != 0 ?
                                (byte)io.Byte()
                            : ((byte)((ShiftSigned((uint)(v & b), (int)(bshift), (int)(bcount))) & 255));
                    z += 3;

                    var a = easy != 0 ?
                                (byte)(easy == 2 ? io.Byte() : 255)
                            : (uint)( this.a != 0 ? ShiftSigned((uint)(v & this.a), (int)ashift, (int)acount) : 255);

                    all |= (uint)(a);

                    if (target == 4)//Composante alpha
                        data[z++] = easy != 0 ? (byte)a : (byte)( a & 255 );
                }
                io.Rewind(pad);//stbi__skip(s, (int)(pad));	
            }
        }

        // si on a recomande les rgba ( req_comp) mais que les info sont rgb on rempli alors avec 255
        if ( target == 4 && all  == 0)
        {
            for( int i = (int)( (4 *imgX*imgY) - 1 ); i >=0; i -= 4  )
                data[i] = 255;
        }
        // FlipVertically(ref data, (int)imgX,(int)imgY, target);

        if ((req_comp != 0) && (req_comp != target))
        {
            // data = stbi__convert_format(data, (int)(target), (int)(req_comp), (uint)(s.img_x), (uint)(s.img_y));
            // if ((data) == (null))
            // 	return data;
        }

        this.Width = (int)imgX;
        this.height = (int)imgY;
        this.color = context_img_n;

        // return data;
    }

    internal static void FlipVertically(ref byte[] data, int width, int height, int target  )
    {
        var widthTarget = width * target;
        for (int j = 0 ; j < ( height >> 1); ++j)
        {
            long p1 = j * widthTarget;
            long p2 = (height - 1 - j) * widthTarget;

            for (int i = 0; i < widthTarget; ++i)
            {
                // swapping 
                data[ p1 + i ] = (byte)(data[p1+ i] ^ data[p2+i]);
                data[ p2 + i ] = (byte)(data[p1+ i] ^ data[p2+i]);
                data[ p1 + i ] = (byte)(data[p1+ i] ^ data[p2+i]);
            }
        }
    }

    internal static int ShiftSigned(uint v, int shift, int bits)
    {
        uint[] mul_table = new uint[9];
        mul_table[0] = (uint)(0);
        mul_table[1] = (uint)(0xff);
        mul_table[2] = (uint)(0x55);
        mul_table[3] = (uint)(0x49);
        mul_table[4] = (uint)(0x11);
        mul_table[5] = (uint)(0x21);
        mul_table[6] = (uint)(0x41);
        mul_table[7] = (uint)(0x81);
        mul_table[8] = (uint)(0x01);

        uint[] shift_table = new uint[9];
        shift_table[0] = (uint)(0);
        shift_table[1] = (uint)(0);
        shift_table[2] = (uint)(0);
        shift_table[3] = (uint)(1);
        shift_table[4] = (uint)(0);
        shift_table[5] = (uint)(2);
        shift_table[6] = (uint)(4);
        shift_table[7] = (uint)(6);
        shift_table[8] = (uint)(0);

        if (shift < 0)
            v <<= -shift;
        else
            v >>= shift;
        v >>= (8 - bits);
        return (int)((int)(v * (int)mul_table[bits]) >> (int)shift_table[bits]);
    }

    internal static int High_bit(uint z)
    {
        int n = (int)(0);
        if (z == 0)
            return (int)(-1);

        if (z >= 0x10000)
        {
            n += (int)(16);
            z >>= 16;
        }

        if (z >= 0x00100)
        {
            n += (int)(8);
            z >>= 8;
        }

        if (z >= 0x00010)
        {
            n += (int)(4);
            z >>= 4;
        }

        if (z >= 0x00004)
        {
            n += (int)(2);
            z >>= 2;
        }

        if (z >= 0x00002)
        {
            n += (int)(1);
            //  TODO uncomment this line if not work z >>= 1;
        }

        return (int)(n);
    }

    internal static int BitCount(uint a)
    {
        a = (uint)((a & 0x55555555) + ((a >> 1) & 0x55555555));
        a = (uint)((a & 0x33333333) + ((a >> 2) & 0x33333333));
        a = (uint)((a + (a >> 4)) & 0x0f0f0f0f);
        a = (uint)(a + (a >> 8));
        a = (uint)(a + (a >> 16));
        return (int)(a & 0xff);
    }

    internal void Dispose()
    {
        Array.Clear(data,0, data.Length);
        data = null!;
        GC.SuppressFinalize(this);
    }

    void IDisposable.Dispose()
    {
        throw new NotImplementedException();
    }
}
