/*
*  This file is part of cBorderless
*  cBorderless forces applications into a full screen windowed borderless mode
*  (C) Charles Machalow (csm10495) - MIT License
*/

using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace cBorderless
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region Win32 Definitions
        /// <summary>
        /// Win32 defined constants
        /// </summary>
        private const int SW_HIDE = 0;
        private const int SW_SHOW = 1;
        private const int GWL_STYLE = -16;
        private const int GWL_EXSTYLE = -20;
        private const long WS_SYSMENU = 0x00080000L;
        private const long WS_POPUP = 0x80000000L;
        private const long WS_CLIPCHILDREN = 0x02000000L;
        private const long WS_CLIPSIBLINGS = 0x08000000L;
        private const long WS_VISIBLE = 0x10000000L;
        private const long WS_CAPTION = 0x00040000L;
        private const long WS_THICKFRAME = 0x00C00000L;
        private const long WS_MINIMIZE = 0x20000000L;
        private const long WS_MAXIMIZE = 0x01000000L;
        private const long WS_EX_DLGMODALFRAME = 0x00000001L;
        private const long WS_EX_CLIENTEDGE = 0x00000200L;
        private const long WS_EX_STATICEDGE = 0x00020000L;
        private const uint SWP_FRAMECHANGED = 0x0020;
        private const uint SWP_NOZORDER = 0x0004;
        private const uint SWP_NOREPOSITION = 0x0200;

        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true, EntryPoint = "SetWindowPos")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int width, int height, uint uFlags);

        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true, EntryPoint = "SetWindowLong")]
        private static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true, EntryPoint = "GetWindowLong")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        #endregion

        /// <summary>
        /// makes the process selected in the combobox fullscreen windowed borderless
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_make_borederless_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process[] SelectedProcess = System.Diagnostics.Process.GetProcessesByName(combobox_processes.SelectedItem.ToString());

            if (SelectedProcess.Length > 0)
            {
                IntPtr hwnd = SelectedProcess[0].MainWindowHandle;
                long style = GetWindowLong(hwnd, GWL_STYLE);
                style &= ~(WS_CAPTION | WS_THICKFRAME | WS_MINIMIZE | WS_MAXIMIZE | WS_SYSMENU);
                SetWindowLong(hwnd, GWL_STYLE, (int)style);

                long lExStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
                lExStyle &= ~(WS_EX_DLGMODALFRAME | WS_EX_CLIENTEDGE | WS_EX_STATICEDGE);
                SetWindowLong(hwnd, GWL_EXSTYLE, (int)lExStyle);
                SetWindowPos(SelectedProcess[0].MainWindowHandle, IntPtr.Zero, -0, -0, Screen.GetBounds(this).Width, Screen.GetBounds(this).Height, (SWP_FRAMECHANGED | SWP_NOZORDER | SWP_NOREPOSITION));
            }
        }

        /// <summary>
        /// called when the refresh button is clicked
        /// calls the refreshProcesses method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_refresh_Click(object sender, EventArgs e)
        {
            refreshProcesses();
        }

        /// <summary>
        /// called on form loading
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            refreshProcesses();
        }

        /// <summary>
        /// Goes through all running processes and add them by name to the combobox
        /// </summary>
        private void refreshProcesses()
        {
            combobox_processes.Items.Clear();
            System.Diagnostics.Process[] processes = System.Diagnostics.Process.GetProcesses();
            foreach (System.Diagnostics.Process i in processes)
            {
                //don't add duplicates
                if (!combobox_processes.Items.Contains(i.ProcessName))
                {
                    combobox_processes.Items.Add(i.ProcessName);
                }
            }

            combobox_processes.Items.Remove("");

            combobox_processes.SelectedIndex = 0;
        }
    }
}
