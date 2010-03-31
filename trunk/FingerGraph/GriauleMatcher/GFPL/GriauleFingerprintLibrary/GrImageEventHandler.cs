namespace GriauleFingerprintLibrary
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    internal delegate void GrImageEventHandler(string source, int width, int height, [In] IntPtr rawImage, int res);
}

