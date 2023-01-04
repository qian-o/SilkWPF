using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SilkWPF.Common;

public abstract class GameBase<TFrame> : Control where TFrame : FramebufferBase
{
    protected readonly Stopwatch _stopwatch = Stopwatch.StartNew();

    protected TimeSpan _lastRenderTime = TimeSpan.FromSeconds(-1);
    protected TimeSpan _lastFrameStamp;

    protected TFrame Framebuffer { get; set; }

    public abstract event Action Ready;
    public abstract event Action<TimeSpan> Render;
    public abstract event Action<object, TimeSpan> UpdateFrame;

    public void Start()
    {
        if (!DesignerProperties.GetIsInDesignMode(this))
        {
            IsVisibleChanged += (_, e) =>
            {
                if ((bool)e.NewValue)
                {
                    CompositionTarget.Rendering += CompositionTarget_Rendering;
                }
                else
                {
                    CompositionTarget.Rendering -= CompositionTarget_Rendering;
                }
            };

            Loaded += (_, _) => InvalidateVisual();

            OnStart();
        }
    }

    private void CompositionTarget_Rendering(object sender, EventArgs e)
    {
        RenderingEventArgs args = (RenderingEventArgs)e;

        if (_lastRenderTime != args.RenderingTime)
        {
            InvalidateVisual();

            _lastRenderTime = args.RenderingTime;
        }
    }

    protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
    {
        if (!DesignerProperties.GetIsInDesignMode(this))
        {
            OnSizeChanged(sizeInfo);
        }
    }

    protected override void OnRender(DrawingContext drawingContext)
    {
        if (DesignerProperties.GetIsInDesignMode(this))
        {
            DesignTimeHelper.DrawDesign(this, drawingContext);
        }
        else
        {
            if (Framebuffer != null && Framebuffer.D3dImage.IsFrontBufferAvailable)
            {
                OnDraw(drawingContext);

                _stopwatch.Restart();
            }
        }
    }

    protected abstract void OnStart();
    protected abstract void OnDraw(DrawingContext drawingContext);
    protected abstract void OnSizeChanged(SizeChangedInfo sizeInfo);
}
