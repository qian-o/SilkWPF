using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics.Wgl;
using SilkWPF.Common;
using System.Windows;
using System.Windows.Media;

namespace SilkWPF.OpenGL;

public class GameControl : GameBase
{
    private readonly Settings _settings;

    private RenderContext _context;
    private Framebuffer _framebuffer;

    public override event Action Ready;
    public override event Action<TimeSpan> Render;
    public override event Action<object, TimeSpan> UpdateFrame;

    public GameControl() : this(new Settings())
    {

    }

    public GameControl(Settings settings)
    {
        _settings = settings;
    }

    protected override void OnStart()
    {
        if (_context == null)
        {
            _context = new RenderContext(_settings);

            Ready?.Invoke();
        }
    }

    protected override void OnSizeChanged(SizeChangedInfo sizeInfo)
    {
        if (_context != null && sizeInfo.NewSize.Width > 0 && sizeInfo.NewSize.Height > 0)
        {
            _framebuffer?.Dispose();
            _framebuffer = new Framebuffer(_context, (int)sizeInfo.NewSize.Width, (int)sizeInfo.NewSize.Height);
        }
    }

    protected override void OnDraw(DrawingContext drawingContext)
    {
        if (_framebuffer != null)
        {
            TimeSpan curFrameStamp = _stopwatch.Elapsed;

            _framebuffer.D3dImage.Lock();
            Wgl.DXLockObjectsNV(_context.GlDeviceHandle, 1, new[] { _framebuffer.DxInteropRegisteredHandle });
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, _framebuffer.GLFramebufferHandle);
            GL.Viewport(0, 0, _framebuffer.FramebufferWidth, _framebuffer.FramebufferHeight);

            Render?.Invoke(curFrameStamp - _lastFrameStamp);

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            Wgl.DXUnlockObjectsNV(_context.GlDeviceHandle, 1, new[] { _framebuffer.DxInteropRegisteredHandle });
            _framebuffer.D3dImage.AddDirtyRect(new Int32Rect(0, 0, _framebuffer.FramebufferWidth, _framebuffer.FramebufferHeight));
            _framebuffer.D3dImage.Unlock();

            drawingContext.PushTransform(_framebuffer.TranslateTransform);
            drawingContext.PushTransform(_framebuffer.FlipYTransform);

            Rect rect = new(0, 0, _framebuffer.D3dImage.Width, _framebuffer.D3dImage.Height);
            drawingContext.DrawImage(_framebuffer.D3dImage, rect);

            drawingContext.Pop();
            drawingContext.Pop();

            UpdateFrame?.Invoke(this, curFrameStamp - _lastFrameStamp);

            _lastFrameStamp = curFrameStamp;
        }
    }
}
