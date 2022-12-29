using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SilkWPF.Common;

public abstract class GameBase : Control
{
    protected readonly Stopwatch _stopwatch = Stopwatch.StartNew();

    protected TimeSpan _lastRenderTime = TimeSpan.FromSeconds(-1);
    protected TimeSpan _lastFrameStamp;

    public abstract event Action Ready;
    public abstract event Action<TimeSpan> Render;
    public abstract event Action<object, TimeSpan> UpdateFrame;

    public GameBase()
    {
        EventManager.RegisterClassHandler(typeof(Control), Keyboard.KeyDownEvent, new KeyEventHandler(OnKeyDown), true);
        EventManager.RegisterClassHandler(typeof(Control), Keyboard.KeyUpEvent, new KeyEventHandler(OnKeyUp), true);
    }

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
        if ((e as RenderingEventArgs)?.RenderingTime is TimeSpan currentRenderTime)
        {
            if (currentRenderTime == _lastRenderTime)
            {
                return;
            }

            InvalidateVisual();

            _lastRenderTime = currentRenderTime;
        }
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if ((e.OriginalSource as GameBase) == null)
        {
            KeyEventArgs args = new(e.KeyboardDevice, e.InputSource, e.Timestamp, e.Key)
            {
                RoutedEvent = Keyboard.KeyDownEvent
            };
            RaiseEvent(args);
        }
    }

    private void OnKeyUp(object sender, KeyEventArgs e)
    {
        if ((e.OriginalSource as GameBase) == null)
        {
            KeyEventArgs args = new(e.KeyboardDevice, e.InputSource, e.Timestamp, e.Key)
            {
                RoutedEvent = Keyboard.KeyUpEvent
            };
            RaiseEvent(args);
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
            OnDraw(drawingContext);
        }
    }

    protected abstract void OnStart();
    protected abstract void OnDraw(DrawingContext drawingContext);
    protected abstract void OnSizeChanged(SizeChangedInfo sizeInfo);
}
