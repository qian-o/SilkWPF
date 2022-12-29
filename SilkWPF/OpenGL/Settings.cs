using OpenTK.Windowing.Common;

namespace SilkWPF.OpenGL;

public class Settings
{
    public int MajorVersion { get; set; } = 3;

    public int MinorVersion { get; set; } = 3;

    public ContextFlags GraphicsContextFlags { get; set; } = ContextFlags.Default;

    public ContextProfile GraphicsProfile { get; set; } = ContextProfile.Core;
}
