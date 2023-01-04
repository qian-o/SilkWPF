﻿using System.Windows.Interop;

namespace SilkWPF.Common;

public abstract class FramebufferBase : IDisposable
{
    public abstract int FramebufferWidth { get; }

    public abstract int FramebufferHeight { get; }

    public abstract D3DImage D3dImage { get; }

    public abstract void Dispose();
}
