using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace WPF_BingeBox.Classes
{
    class MonitorHelper
    {
        public static NativeMethods.RECT GetMonitorDetails(IntPtr hwnd)
        {
            IntPtr monitorHandle = NativeMethods.MonitorFromWindow(hwnd, NativeMethods.MONITOR_DEFAULTTONEAREST);

            if(monitorHandle != IntPtr.Zero)
            {
                var monitorInfo = new NativeMethods.MONITORINFO();
                monitorInfo.cbSize = Marshal.SizeOf(typeof(NativeMethods.MONITORINFO));

                if (NativeMethods.GetMonitorInfo(monitorHandle, ref monitorInfo))
                {
                    var bounds = monitorInfo.rcMonitor;
                    return bounds;
                }
                else
                {
                    return new NativeMethods.RECT();
                }
            }
            else
            {
                return new NativeMethods.RECT();
            }
        }
    }
}
