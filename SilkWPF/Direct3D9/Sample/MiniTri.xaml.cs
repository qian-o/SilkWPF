using System.Windows.Controls;

namespace SilkWPF.Direct3D9.Sample;

/// <summary>
/// MiniTri.xaml 的交互逻辑
/// </summary>
public unsafe partial class MiniTri : UserControl
{
    public MiniTri()
    {
        InitializeComponent();

        Game.Ready += Game_Ready;
        Game.Render += Game_Render;
        Game.UpdateFrame += Game_UpdateFrame;
        Game.Start();
    }

    private void Game_Ready()
    {

    }

    private void Game_Render(TimeSpan obj)
    {

    }

    private void Game_UpdateFrame(object arg1, TimeSpan arg2)
    {

    }
}
