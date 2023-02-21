using Silk.NET.Direct3D9;
using System.Windows.Interop;

namespace SilkWPF.Common;

public abstract unsafe class FramebufferBase : IDisposable
{
    public abstract int FramebufferWidth { get; }

    public abstract int FramebufferHeight { get; }

    public abstract nint D3dSurface { get; }

    public abstract D3DImage D3dImage { get; }

    public virtual void Dispose() => ((IDirect3DSurface9*)D3dSurface)->Release();
}
