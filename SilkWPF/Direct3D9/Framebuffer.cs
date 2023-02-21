using Silk.NET.Direct3D9;
using SilkWPF.Common;
using System.Windows.Interop;

namespace SilkWPF.Direct3D9;

public unsafe class Framebuffer : FramebufferBase
{
    public RenderContext Context { get; }

    public override int FramebufferWidth { get; }

    public override int FramebufferHeight { get; }

    public override nint D3dSurface { get; }

    public override D3DImage D3dImage { get; }

    public Framebuffer(RenderContext context, int framebufferWidth, int framebufferHeight)
    {
        Context = context;
        FramebufferWidth = framebufferWidth;
        FramebufferHeight = framebufferHeight;

        IDirect3DSurface9* surface;
        context.Device->CreateRenderTarget((uint)FramebufferWidth, (uint)FramebufferHeight, context.Format, MultisampleType.MultisampleNone, 0, 0, &surface, null);
        context.Device->SetRenderTarget(0, surface);

        D3dSurface = (IntPtr)surface;
        D3dImage = new D3DImage();
        D3dImage.Lock();
        D3dImage.SetBackBuffer(D3DResourceType.IDirect3DSurface9, D3dSurface);
        D3dImage.Unlock();
    }

    public override void Dispose()
    {
        base.Dispose();

        GC.SuppressFinalize(this);
    }
}
