namespace RitaEngine.Platform.API.DirectX.DSound;

using DWORD = System.UInt32; // A 32-bit unsigned integer. The range is 0 through 4294967295 decimal.
using LPCWSTR = System.Char;//An LPCWSTR is a 32-bit pointer to a constant string of 16-bit Unicode characters, which MAY be null-terminated.
using SHORT = System.Int16;//A 16-bit integer. The range is -32768 through 32767 decimal.
using System;
using System.Security;
using BOOL = System.Int32;
using LONG = System.Int32;
using WORD  = System.UInt16;
using BYTE = System.Byte;
using HRESULT = System.UInt32;

#region Direct Sound 
public unsafe static class Dsound
{
    public static readonly GUID CLSID_DirectSound8 = new(0x3901CC3F,0x84B5, 0x4FA4,0xBA,0x35,0xAA,0x81,0x72,0xB8, 0xA0,0x9B  );
    public static readonly GUID  CLSID_DirectSound= new( 0x47d4d946, 0x62e8, 0x11cf, 0x93, 0xbc, 0x44, 0x45, 0x53, 0x54, 0x0, 0x0);
    public static readonly GUID  IID_IDirectSound = new( 0x279AFA83, 0x4981, 0x11CE, 0xA5, 0x21, 0x00, 0x20, 0xAF, 0x0B, 0xE5, 0x60);
    public static readonly GUID CLSID_AllObject = new(0xaa114de5,0xc262, 0x4169,0xa1,0xc8,0x23,0xd6,0x98,0xcc, 0x73,0xb5  );
    public static readonly GUID CLSID_Null = new(0x00000000, 0x0000, 0x0000, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00);
    private static unsafe delegate* unmanaged<GUID*,void**, void* , DSERR> PFN_DirectSoundCreate8 = null; 
    public static DSERR DirectSoundCreate8(  GUID* lpcGUID,void** lplpDSC, void* pUnkOuter ) => PFN_DirectSoundCreate8(lpcGUID, lplpDSC,  pUnkOuter);

    public static void Load_Dsound(PFN_GetSymbolPointer load,IntPtr module)
    {
        PFN_DirectSoundCreate8 = (delegate* unmanaged<GUID*,void**, void* , DSERR>)load(module, nameof(DirectSoundCreate8 ));
    }

    public static void Unload_Dsound()
    {
        PFN_DirectSoundCreate8 = null;
    }
    // public int (CALLBACK *LPDSENUMCALLBACKW)(LPGUID, LPCWSTR, LPCWSTR, LPVOID);
// HRESULT DirectSoundCaptureCreate8(         LPCGUID lpcGUID,LPDIRECTSOUNDCAPTURE8 * lplpDSC, LPUNKNOWN pUnkOuter)
// HRESULT DirectSoundCaptureEnumerateW( LPDSENUMCALLBACK lpDSEnumCallback, LPVOID lpContext) // for unicode ezlsa A

// HRESULT DirectSoundEnumerateW(LPDSENUMCALLBACK lpDSEnumCallback,LPVOID lpContext)
// HRESULT DirectSoundFullDuplexCreate8( LPCGUID pcGuidCaptureDevice, LPCGUID pcGuidRenderDevice,    LPCDSCBUFFERDESC pcDSCBufferDesc,     LPCDSBUFFERDESC pcDSBufferDesc,
//          HWND hWnd, DWORD dwLevel,  LPDIRECTSOUNDFULLDUPLEX * ppDSFD, LPDIRECTSOUNDCAPTUREBUFFER8 * ppDSCBuffer8, LPDIRECTSOUNDBUFFER8 * ppDSBuffer8, LPUNKNOWN pUnkOuter)

[StructLayout(LayoutKind.Sequential)]
public unsafe struct DSCBUFFERDESC 
{
    public DWORD dwSize;
    public DWORD dwFlags;
    public DWORD dwBufferBytes;
    public DWORD dwReserved;
    public WAVEFORMATEX* lpwfxFormat;
    public DWORD dwFXCount;
    public DSCEFFECTDESC* lpDSCFXDesc;
} ;

[StructLayout(LayoutKind.Sequential)]
public  struct DSCEFFECTDESC 
{
    public DWORD dwSize;
    public DWORD dwFlags;
    public GUID guidDSCFXClass;
    public GUID guidDSCFXInstance;
    public DWORD dwReserved1;
    public DWORD dwReserved2;
} ;

[StructLayout(LayoutKind.Sequential)]
public struct DSCAPS
{
    public DWORD           dwSize;
    public DWORD           dwFlags;
    public DWORD           dwMinSecondarySampleRate;
    public DWORD           dwMaxSecondarySampleRate;
    public DWORD           dwPrimaryBuffers;
    public DWORD           dwMaxHwMixingAllBuffers;
    public DWORD           dwMaxHwMixingStaticBuffers;
    public DWORD           dwMaxHwMixingStreamingBuffers;
    public DWORD           dwFreeHwMixingAllBuffers;
    public DWORD           dwFreeHwMixingStaticBuffers;
    public DWORD           dwFreeHwMixingStreamingBuffers;
    public DWORD           dwMaxHw3DAllBuffers;
    public DWORD           dwMaxHw3DStaticBuffers;
    public DWORD           dwMaxHw3DStreamingBuffers;
    public DWORD           dwFreeHw3DAllBuffers;
    public DWORD           dwFreeHw3DStaticBuffers;
    public DWORD           dwFreeHw3DStreamingBuffers;
    public DWORD           dwTotalHwMemBytes;
    public DWORD           dwFreeHwMemBytes;
    public DWORD           dwMaxContigFreeHwMemBytes;
    public DWORD           dwUnlockTransferRateHwBuffers;
    public DWORD           dwPlayCpuOverheadSwBuffers;
    public DWORD           dwReserved1;
    public DWORD           dwReserved2;
} 

[StructLayout(LayoutKind.Sequential)]
public  unsafe struct DSBUFFERDESC
{
    public DWORD           dwSize;
    public DWORD           dwFlags;
    public DWORD           dwBufferBytes;
    public DWORD           dwReserved;
    public WAVEFORMATEX*  lpwfxFormat;
    public Guid           guid3DAlgorithm;
} ;

public unsafe struct DirectSound8
{
    private void* lpVtbl = null;
    // private delegate* unmanaged<void* ,DWORD,DSERR> PFN_SetCooperativeLevel = null;

    public DirectSound8(void* LPDIRECTSOUND8)
    {
        lpVtbl = LPDIRECTSOUND8 ;
        // ds8 =  LPDIRECTSOUND8 ;
        // PFN_SetCooperativeLevel =(delegate* unmanaged<void* ,DWORD,DSERR> )   ((void**)(*(void**)LPDIRECTSOUND8))[6] ;
    }

    public void Release()
    {
        // PFN_SetCooperativeLevel = null;
    }

    public DSERR SetCooperativeLevel( void* hwnd, CooperativeLevel level)
    {
        // int ret = (delegate* unmanaged<void*, void* ,CooperativeLevel,DSERR> )  ((void**)(*(void**)lpVtbl))[6]   (lpVtbl, &hwnd, level);
        var ret = ((delegate* unmanaged<void*, void*, CooperativeLevel,DSERR>)  ((void**)(*(void**)lpVtbl))[6] ) (lpVtbl, hwnd,level);
        return ret;

    }
    //    => PFN_SetCooperativeLevel( hwnd, (uint)level);

    //     public void* QueryInterface       () ;
    // public ulong AddRef        () ;
    // public ulong Release       ();

    // // IDirectSound methods
    // public void CreateSoundBuffer    ( DSBUFFERDESC* pcDSBufferDesc, IDirectSoundBuffer8 ppDSBuffer, void* pUnkOuter) {}
    // // public abstract void GetCaps              ( DSCAPS pDSCaps);
    // // public abstract void DuplicateSoundBuffer ( IDirectSoundBuffer8 pDSBufferOriginal, IDirectSoundBuffer8 ppDSBufferDuplicate) ;
    // public DSERR SetCooperativeLevel  ( void* hwnd, DWORD dwLevel);
    // public void Compact              () ;
    // public void GetSpeakerConfig     ( DWORD* pdwSpeakerConfig) ;
    // public void SetSpeakerConfig     ( DWORD dwSpeakerConfig);
    // public void Initialize           ( Guid* pcGuidDevice) ;

    // // // IDirectSound8 methods
    // public  void VerifyCertification  (DWORD* pdwCertified);
}

public enum CooperativeLevel : uint 
{	
    Normal = unchecked((int)1),			
    Priority = unchecked((int)2),			
    Exclusive = unchecked((int)3),			
    WritePrimary = unchecked((int)4),			
}

public interface IDirectSoundBuffer8
{

}

public enum DSERR : uint
{
    DS_OK                                 = 0,
    DS_NO_VIRTUALIZATION                   = (0x0878000A),
    DSERR_ALLOCATED                        = (0x8878000A),
    DSERR_CONTROLUNAVAIL                   = (0x8878001E),
    DSERR_INVALIDPARAM                     = (0x80070057),
    DSERR_INVALIDCALL                      = (0x88780032),
    DSERR_GENERIC                          = (0x80004005),
    DSERR_PRIOLEVELNEEDED                  = (0x88780046),
    DSERR_OUTOFMEMORY                      = (0x8007000E),
    DSERR_BADFORMAT                        = (0x88780064),
    DSERR_UNSUPPORTED                      = (0x80004001),
    DSERR_NODRIVER                         = (0x88780078),
    DSERR_ALREADYINITIALIZED               = (0x88780082),
    DSERR_NOAGGREGATION                    = (0x80040110),
    DSERR_BUFFERLOST                       = (0x88780096),
    DSERR_OTHERAPPHASPRIO                  = (0x887800A0),
    DSERR_UNINITIALIZED                    = (0x887800AA),
    DSERR_NOINTERFACE                      = (0x80004002),
    DSERR_ACCESSDENIED                     = (0x80070005),
    DSERR_BUFFERTOOSMALL                   = (0x887800B4),
    DSERR_DS8_REQUIRED                     = (0x887800BE),
    DSERR_SENDLOOP                         = (0x887800C8),
    DSERR_BADSENDBUFFERGUID                = (0x887800D2),
    DSERR_OBJECTNOTFOUND                   = (0x88781161),
    DSERR_FXUNAVAILABLE                    = (0x887800DC),
}

}

// #define WIN32_LEAN_AND_MEAN
// #define _WIN32_WINNT 0x0501
// #define DIRECTINPUT_VERSION 0x0800

// #include <windows.h>
// #include <dinput.h>
// #include <dinputd.h>
// #include <stdio.h>
// #include <stdint.h>
// #include <locale.h>
// #include <memory>

// #pragma comment(lib, "dinput8.lib")
// #pragma comment(lib, "dxguid.lib")


// struct DinputMouse {
//     DinputMouse(LPDIRECTINPUT8 pDi, HWND hWnd) {
//         init(pDi, hWnd);
//     }

//     ~DinputMouse() {
//         cleanup();
//     }

//     HRESULT init(LPDIRECTINPUT8 pDi, HWND hWnd) {
//         HRESULT hr = S_OK;
//         if(! pMouse && pDi) {
//             const auto coop = DISCL_NONEXCLUSIVE | DISCL_BACKGROUND;
//             hr = FAILED(hr) ? hr : pDi->CreateDevice(GUID_SysMouse, &pMouse, 0);
//             hr = FAILED(hr) ? hr : pMouse->SetDataFormat(&c_dfDIMouse2);
//             hr = FAILED(hr) ? hr : pMouse->SetCooperativeLevel(hWnd, coop);
//             hr = FAILED(hr) ? hr : pMouse->Acquire();
//         }
//         reset();
//         return hr;
//     }

//     void cleanup() {
//         if(pMouse) {
//             pMouse->Unacquire();
//             pMouse->Release();
//             pMouse = nullptr;
//         }
//     }

//     void reset() {
//         buttons = 0;
//         trigger = 0;
//         release = 0;
//         dx      = 0;
//         dy      = 0;
//         dz      = 0;
//     }

//     void update() {
//         if(! pMouse) {
//             reset();
//         } else {
//             DIMOUSESTATE2 dims2 {};
//             const auto hr = pMouse->GetDeviceState(sizeof(dims2), &dims2);
//             if(FAILED(hr)) {
//                 HRESULT hra;
//                 do {
//                     hra = pMouse->Acquire();
//                 } while(hra == DIERR_INPUTLOST);
//                 reset();
//             } else {
//                 dx = static_cast<decltype(dx)>(dims2.lX);
//                 dy = static_cast<decltype(dy)>(dims2.lY);
//                 dz = static_cast<decltype(dz)>(dims2.lZ);

//                 uint32_t b = 0;
//                 for(size_t i = 0; i < _countof(dims2.rgbButtons); ++i) {
//                     if(dims2.rgbButtons[i] & 0x80) {
//                         b |= 1 << i;
//                     }
//                 }

//                 const auto p = buttons;
//                 buttons = b;
//                 trigger =   b  & (b ^ p);
//                 release = (~b) & (b ^ p);
//             }
//         }
//     }

//     LPDIRECTINPUTDEVICE8    pMouse { nullptr };
//     uint32_t    buttons {};
//     uint32_t    trigger {};
//     uint32_t    release {};
//     int32_t     dx {};      // X-axis
//     int32_t     dy {};      // Y-axis
//     int32_t     dz {};      // Wheel
// };


// struct Dinput {
//     Dinput(HWND hWnd) {
//         init(hWnd);
//     }

//     ~Dinput() {
//         cleanup();
//     }

//     HRESULT init(HWND hWnd) {
//         HRESULT hr = DirectInput8Create(
//             GetModuleHandle(0), DIRECTINPUT_VERSION, IID_IDirectInput8
//             , reinterpret_cast<void**>(&pDi), 0);
//         pDiMouse = std::unique_ptr<DinputMouse>(
//             new DinputMouse { pDi, hWnd }
//         );
//         reset();
//         return hr;
//     }

//     void            cleanup()   { pDiMouse->cleanup(); }
//     void            reset()     { pDiMouse->reset(); }
//     void            update()    { pDiMouse->update(); }
//     DinputMouse*    getMouse()  { return pDiMouse.get(); }

//     LPDIRECTINPUT8                  pDi { nullptr };
//     std::unique_ptr<DinputMouse>    pDiMouse;
// };


// void mainLoop(HWND hWnd, HANDLE hStdin) {
//     Dinput dinput { hWnd };

//     printf("Press ESCAPE to exit\n");
//     for(;;) {
//         INPUT_RECORD ir {};
//         DWORD nEvent = 0;
//         const auto r = PeekConsoleInputW(hStdin, &ir, 1, &nEvent);
//         if(r && nEvent) {
//             FlushConsoleInputBuffer(hStdin);
//             if(   KEY_EVENT == ir.EventType
//                && VK_ESCAPE == ir.Event.KeyEvent.wVirtualKeyCode
//             ) {
//                 break;
//             }
//         }

//         dinput.update();
//         const auto* mouse = dinput.getMouse();
//         printf("BTN:%08x, dx=%+4d, dy=%+4d, dz=%+4d\r",
//                mouse->buttons, mouse->dx, mouse->dy, mouse->dz);
//         Sleep(10);
//     }
//     printf("\n");
// }


// int main() {
//     const auto hWnd = GetConsoleWindow();
//     const auto hStdin = GetStdHandle(STD_INPUT_HANDLE);
//     const auto hStdout = GetStdHandle(STD_OUTPUT_HANDLE);
//     SetConsoleCtrlHandler([](DWORD dwCtrlType) { return TRUE; }, TRUE);

//     DWORD oldMode = 0;
//     GetConsoleMode(hStdin, &oldMode);

//     DWORD newMode = oldMode | ENABLE_MOUSE_INPUT;
//     newMode &= ~(ENABLE_PROCESSED_INPUT | ENABLE_ECHO_INPUT | ENABLE_INSERT_MODE
//                | ENABLE_QUICK_EDIT_MODE | ENABLE_LINE_INPUT);
//     SetConsoleMode(hStdin, newMode);

//     mainLoop(hWnd, hStdin);

//     SetConsoleMode(hStdin, oldMode);
// }

#endregion
