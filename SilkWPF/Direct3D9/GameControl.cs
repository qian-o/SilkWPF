using Silk.NET.Direct3D9;
using SilkWPF.Common;
using System.Numerics;
using System.Windows;
using System.Windows.Media;
using Rect = System.Windows.Rect;

namespace SilkWPF.Direct3D9;

public unsafe class GameControl : GameBase
{
    private RenderContext _context;
    private Framebuffer _framebuffer;

    public IDirect3DDevice9Ex* Device { get; private set; }

    public override event Action Ready;
    public override event Action<TimeSpan> Render;
    public override event Action<object, TimeSpan> UpdateFrame;

    protected override void OnStart()
    {
        if (_context == null)
        {
            _context = new RenderContext();
            Device = _context.Device;

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

    public static Format MakeFourCC(byte c1, byte c2, byte c3, byte c4)
    {
        return (Format)((((((c4 << 8) | c3) << 8) | c2) << 8) | c1);
    }

    public static int ColorToBgra(Color color)
    {
        return color.B | (color.G << 8) | (color.R << 16) | (color.A << 24);
    }

    public static int ColorToRgba(Color color)
    {
        return color.R | (color.G << 8) | (color.B << 16) | (color.A << 24);
    }

    public static int ColorToAbgr(Color color)
    {
        return color.A | (color.B << 8) | (color.G << 16) | (color.R << 24);
    }

    public static Color HsvToColor(Vector4 hsv)
    {
        float num = hsv.X * 360f;
        float y = hsv.Y;
        float z = hsv.Z;
        float num2 = z * y;
        float num3 = num / 60f;
        float num4 = num2 * (1f - Math.Abs(num3 % 2f - 1f));
        float num5;
        float num6;
        float num7;
        if (num3 >= 0f && num3 < 1f)
        {
            num5 = num2;
            num6 = num4;
            num7 = 0f;
        }
        else if (num3 >= 1f && num3 < 2f)
        {
            num5 = num4;
            num6 = num2;
            num7 = 0f;
        }
        else if (num3 >= 2f && num3 < 3f)
        {
            num5 = 0f;
            num6 = num2;
            num7 = num4;
        }
        else if (num3 >= 3f && num3 < 4f)
        {
            num5 = 0f;
            num6 = num4;
            num7 = num2;
        }
        else if (num3 >= 4f && num3 < 5f)
        {
            num5 = num4;
            num6 = 0f;
            num7 = num2;
        }
        else if (num3 >= 5f && num3 < 6f)
        {
            num5 = num2;
            num6 = 0f;
            num7 = num4;
        }
        else
        {
            num5 = 0f;
            num6 = 0f;
            num7 = 0f;
        }

        float num8 = z - num2;
        return Color.FromScRgb(hsv.W, num5 + num8, num6 + num8, num7 + num8);
    }
}
