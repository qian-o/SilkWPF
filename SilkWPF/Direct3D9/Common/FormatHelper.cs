using Silk.NET.Direct3D9;

namespace SilkWPF.Direct3D9.Common;

public static class FormatHelper
{
    public static Format MakeFourCC(byte c1, byte c2, byte c3, byte c4)
    {
        return (Format)((((((c4 << 8) | c3) << 8) | c2) << 8) | c1);
    }
}
