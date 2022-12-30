using Silk.NET.Direct3D9;
using SilkWPF.Common;
using System.Windows;
using System.Windows.Media;
using Rect = System.Windows.Rect;

namespace SilkWPF.Direct3D9;

public unsafe class GameControl : GameBase
{
    private RenderContext _context;
    private Framebuffer _framebuffer;

    public IDirect3DDevice9Ex* Device { get; private set; }
    public Format Format { get; private set; }

    public override event Action Ready;
    public override event Action<TimeSpan> Render;
    public override event Action<object, TimeSpan> UpdateFrame;

    protected override void OnStart()
    {
        if (_context == null)
        {
            _context = new RenderContext();
            Device = _context.Device;
            Format = _context.Format;

            Ready?.Invoke();
        }
    }

    protected override void OnSizeChanged(SizeChangedInfo sizeInfo)
    {
        if (_context != null && sizeInfo.NewSize.Width > 0 && sizeInfo.NewSize.Height > 0)
        {
            _framebuffer = new Framebuffer(_context, (int)sizeInfo.NewSize.Width, (int)sizeInfo.NewSize.Height);
        }
    }

    protected override void OnDraw(DrawingContext drawingContext)
    {
        if (_framebuffer != null)
        {
            TimeSpan curFrameStamp = _stopwatch.Elapsed;

            _framebuffer.D3dImage.Lock();

            Render?.Invoke(curFrameStamp - _lastFrameStamp);

            _framebuffer.D3dImage.AddDirtyRect(new Int32Rect(0, 0, _framebuffer.FramebufferWidth, _framebuffer.FramebufferHeight));
            _framebuffer.D3dImage.Unlock();

            Rect rect = new(0, 0, _framebuffer.D3dImage.Width, _framebuffer.D3dImage.Height);
            drawingContext.DrawImage(_framebuffer.D3dImage, rect);

            UpdateFrame?.Invoke(this, curFrameStamp - _lastFrameStamp);

            _lastFrameStamp = curFrameStamp;
        }
    }
}
