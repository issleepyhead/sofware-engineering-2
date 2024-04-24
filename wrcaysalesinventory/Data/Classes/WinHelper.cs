using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;

namespace wrcaysalesinventory.Data.Classes
{
    internal class WinHelper
    {
        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("winmm.dll", EntryPoint = "mciSendString", CharSet = CharSet.Auto)]
        public static extern int MciSendString(string lpstrCommand, string lpstrReturnString, int uReturnLength, int hwndCallback);

        public static void CloseDialog(ref Button btn)
        {
            ButtonAutomationPeer btnPeer = (ButtonAutomationPeer)UIElementAutomationPeer.CreatePeerForElement(btn);
            if (btnPeer != null)
            {
                IInvokeProvider invoke = (IInvokeProvider)btnPeer.GetPattern(PatternInterface.Invoke);
                invoke?.Invoke();
            }
        }
    }
}
