using Silk.NET.Direct3D9;
using System.Windows.Interop;

namespace SilkWPF.Direct3D9;

public unsafe class Framebuffer
{
    public RenderContext Context { get; }

    public int FramebufferWidth { get; }

    public int FramebufferHeight { get; }

    public D3DImage D3dImage { get; }

    public Framebuffer(RenderContext context, int framebufferWidth, int framebufferHeight)
    {
        Context = context;
        FramebufferWidth = framebufferWidth;
        FramebufferHeight = framebufferHeight;

        IDirect3DTexture9* texture;
        IDirect3DSurface9* surface;
        context.Device->CreateTexture((uint)FramebufferWidth, (uint)FramebufferHeight, 1, D3D9.UsageRendertarget, context.Format, Pool.Default, &texture, null);
        texture->GetSurfaceLevel(0, &surface);

        D3dImage = new D3DImage();
        D3dImage.Lock();
        D3dImage.SetBackBuffer(D3DResourceType.IDirect3DSurface9, (IntPtr)surface);
        D3dImage.Unlock();
    }
}
