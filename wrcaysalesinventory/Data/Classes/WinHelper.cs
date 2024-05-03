using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.NetworkInformation;
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

        public static void CloseDialog(Button btn)
        {
            ButtonAutomationPeer btnPeer = (ButtonAutomationPeer)UIElementAutomationPeer.CreatePeerForElement(btn);
            if (btnPeer != null)
            {
                IInvokeProvider invoke = (IInvokeProvider)btnPeer.GetPattern(PatternInterface.Invoke);
                invoke?.Invoke();
            }
        }

        public static void PaginationPageCount(Pagination pagination, int total)
        {
            if(pagination != null)
            {
                if (total <= 30)
                {
                    pagination.MaxPageCount = 1;
                    return;
                }

                if (30 / total > 0)
                {
                    pagination.MaxPageCount = total / 30 + 1;
                }
                else
                {
                    pagination.MaxPageCount = total / 30;
                }
            }

        }

        public static void AuditActivity(string e, string d)
        {
            try
            {
                SqlConnection con = SqlBaseConnection.GetInstance();
                SqlCommand sqlCommand = new("INSERT INTO tlbauditlogs (actor_id, event, description) VALUES (@uid, @e, @d)", con);
                sqlCommand.Parameters.AddWithValue("@uid", GlobalData.Config.UserID);
                sqlCommand.Parameters.AddWithValue("@e",e );
                sqlCommand.Parameters.AddWithValue("@d", d);

                sqlCommand.ExecuteNonQuery();
            } catch
            {

            }
        }

        public static string CapitalizeData(string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                int len = input.Split(' ').Length;
                if (len > 1)
                {
                    string[] tokens = input.Split(' ');
                    string temp = "";
                    foreach(string token in tokens)
                    {
                        temp += token[0].ToString().ToUpper() + token.Substring(1, token.Length-1) + " ";
                    }
                    return temp.Trim(' ');
                } else
                {
                    return input[0].ToString().ToUpper() + input.Substring(1, input.Length - 1);
                }
            } else
            {
                return input;
            }
        }

    }
}
