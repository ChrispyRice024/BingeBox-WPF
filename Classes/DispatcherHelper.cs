using Microsoft.UI.Dispatching;
using System.Runtime.InteropServices;
using WinSys = Windows.System;
using System.Security.RightsManagement;

namespace WPF_BingeBox.Classes
{
    class DispatcherHelper
    {
        private static DispatcherQueueController _dispatcherQueueController;

        [StructLayout(LayoutKind.Sequential)]
        private struct DispatcherQueueOptions
        {
            public uint dwSize;
            public int threadType;
            public int apartmentType;
        }

        [DllImport("coremessaging.dll")]
        private static extern int CreateDispatcherQueueController(
            DispatcherQueueOptions options,
            out DispatcherQueueController _dispatcherQueueController);

        public static void EnsureIsPatcherQueue()
        {
            if(DispatcherQueue.GetForCurrentThread() == null)
            {
                DispatcherQueueOptions options = new DispatcherQueueOptions();
                options.dwSize = (uint)Marshal.SizeOf(typeof(DispatcherQueueOptions));
                options.threadType = 2;
                options.apartmentType = 2;

                CreateDispatcherQueueController(options, out _dispatcherQueueController);
            }
        }
    }
}
