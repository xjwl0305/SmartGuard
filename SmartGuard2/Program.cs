using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmartGuard2
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpWindowText, int nMaxCount);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetClassName(IntPtr hwnd, StringBuilder lpClassName, int nMaxCount);
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindow(IntPtr hWnd, int uCmd);
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
        public static void KillErrorProcess(String wndTitle, int ValidprocessID)
        {
            uint processID = 0;
            StringBuilder Title = new StringBuilder(256);
            StringBuilder lpClassName = new StringBuilder(256);
            IntPtr tempHwnd = FindWindow(null, null); // 최상위 윈도우 핸들 찾기               

            while (tempHwnd.ToInt32() != 0) //에러 Dialog를 직접 찾을 방법이 없으로로 윈도우를 모두 뒤져야 한다
            {
                tempHwnd = GetWindow(tempHwnd, 2); // 다음 윈도우 핸들 찾기   
                GetWindowText(tempHwnd, Title, Title.Capacity + 1); //윈도우 Title 찾기

                if (Title.ToString().IndexOf(wndTitle) >= 0)
                {
                    GetWindowThreadProcessId(tempHwnd, out processID); //윈도우의 프로세스 ID 찾기                 
                    if (processID != ValidprocessID) //에러 메세지 박스, 다이알로그 등 윈도우 타이틀로 찾아서 Launch시킨것과 같은지 확인
                    {
                        GetClassName(tempHwnd, lpClassName, lpClassName.Capacity + 1);
                        if (lpClassName.ToString().CompareTo("#32770") ==
                            0) //Dialog,MessageBox인 것은 닫고 다른 응용프로그램이면 닫지 않는다
                        {
                            SendMessage(tempHwnd, 0x0010, -1, -1);
                        }
                    }
                }
            }
        }
    }
}