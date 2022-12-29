using Silk.NET.Direct3D9;
using Silk.NET.Maths;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Windows.Controls;

namespace SilkWPF.Direct3D9.Sample;

/// <summary>
/// MiniTri.xaml 的交互逻辑
/// </summary>
public unsafe partial class MiniTri : UserControl
{
    [StructLayout(LayoutKind.Sequential)]
    struct Vertex
    {
        public Vector4 Position;
        public uint Color;
    }

    private readonly Vertex[] _vertices =
    {
        new Vertex() { Color = 4294901760u, Position = new Vector4(400.0f, 100.0f, 0.5f, 1.0f) },
        new Vertex() { Color = 4278190335u, Position = new Vector4(650.0f, 500.0f, 0.5f, 1.0f) },
        new Vertex() { Color = 4278222848u, Position = new Vector4(150.0f, 500.0f, 0.5f, 1.0f) }
    };
    private readonly Vertexelement9[] _vertexelements =
    {
        new Vertexelement9(0, 0, 3, 0, 9, 0),
        new Vertexelement9(0, 16, 4, 0, 10, 0),
        new Vertexelement9(255, 0, 17, 0, 0, 0)
    };

    private IDirect3DVertexBuffer9* vertices;
    private IDirect3DVertexDeclaration9* ppDecl;

    public MiniTri()
    {
        InitializeComponent();

        Game.Ready += Game_Ready;
        Game.Render += Game_Render;
        Game.Start();
    }

    private void Game_Ready()
    {
        fixed (Vertex* ptr = &_vertices[0])
        {
            fixed (Vertexelement9* vertexElems = &_vertexelements[0])
            {
                void* ppbData;
                Game.Device->CreateVertexBuffer(3 * 20, D3D9.UsageWriteonly, 0, Pool.Default, ref vertices, null);
                vertices->Lock(0, 0, &ppbData, 0);
                System.Runtime.CompilerServices.Unsafe.CopyBlockUnaligned(ppbData, ptr, (uint)(sizeof(Vertex) * _vertices.Length));
                vertices->Unlock();

                Game.Device->CreateVertexDeclaration(vertexElems, ref ppDecl);
            }
        }
    }

    private void Game_Render(TimeSpan obj)
    {
        Game.Device->Clear(0, null, D3D9.ClearTarget, 4294937600u, 1.0f, 0);
        Game.Device->BeginScene();

        Game.Device->SetStreamSource(0, vertices, 0, 20);
        Game.Device->SetVertexDeclaration(ppDecl);
        Game.Device->DrawPrimitive(Primitivetype.Trianglelist, 0, 1);

        Game.Device->EndScene();
        Game.Device->Present((Rectangle<int>*)IntPtr.Zero, (Rectangle<int>*)IntPtr.Zero, 1, (RGNData*)IntPtr.Zero);
    }
}
