using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace AprilTag.Interop {

public sealed class Family : SafeHandleZeroOrMinusOneIsInvalid
{
    #region SafeHandle implementation

    Family() : base(true) {}

    protected override bool ReleaseHandle()
    {
        _DestroyTag36h11(handle);
        return true;
    }

    #endregion

    #region Public methods

    public static Family CreateTag36h11()
      => _CreateTag36h11();

    #endregion

    #region Unmanaged interface

    // [DllImport(Config.DllName, EntryPoint = "tagStandard41h12_create")]
    // private static extern Family _CreateTagStandard41h12();

    // [DllImport(Config.DllName, EntryPoint = "tagStandard41h12_destroy")]
    // private static extern void _DestroyTagStandard41h12(IntPtr ptr);

    [DllImport(Config.DllName, EntryPoint = "tag36h11_create")]
    private static extern Family _CreateTag36h11();

    [DllImport(Config.DllName, EntryPoint = "tag36h11_destroy")]
    private static extern void _DestroyTag36h11(IntPtr ptr);

    #endregion
}

} // namespace AprilTag.Interop
