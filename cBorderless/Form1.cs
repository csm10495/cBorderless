﻿/*
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
        /// <summary>
        /// Constructor
        /// If given no args, runs via gui
        /// If there is 1 or more args, attempts to find a process with each arg string and make it windowed borderless
        /// If there is more than 1 arg, closes
        /// If an arg follows the form MAX_WAIT=<seconds> then have the process be continually modified for that given time frame. Will also wait that long for the process.
        /// -v will make it send a notify icon approx every second during scanning
        /// </summary>
        /// <param name="args">Appliaction arguments</param>
        public Form1(string[] args)
        {
            NotifyIcon notifyIcon = new NotifyIcon();
            const string timedWaitKeyword = "MAX_WAIT";
            if (args.Length == 0)
            {
                InitializeComponent();
            }
            else if (args.Length >= 1)
            {
                bool foundProcess = false;
                bool verbose = false; // -v flag... will notify every second during scan
                int endTime = -1;
                int waitTime = 0;
                int startTime = getTimeInSeconds();
                int lastSecondDisplayed = -1;
                do
                {
                    int time = getTimeInSeconds();

                    foreach (string processString in args)
                    {
                        // Look for MAX_WAIT to know how long to try this loop for. Only do this once (unless MAX_WAIT is -1).
                        if (processString.StartsWith(timedWaitKeyword) && processString.Contains("=") && endTime == -1)
                        {
                            // MAX_WAIT=10 
                            // Makes us take at most 10 seconds of waiting.
                            waitTime = Convert.ToInt16(processString.Split('=')[1]);
                            endTime = startTime + waitTime;
                        }

                        // Look for verbose flag
                        if (!verbose && processString == "-v")
                        {
                            verbose = true;
                        }

                        System.Diagnostics.Process[] SelectedProcess = System.Diagnostics.Process.GetProcessesByName(processString);

                        if (SelectedProcess.Length >= 1)
                        {
                            this.makeProcessBorderless(SelectedProcess[0]);
                            foundProcess = true;
                        }
                    }

                    // Update every second
                    if (verbose && time != lastSecondDisplayed)
                    {
                        displayBubble(notifyIcon, String.Format("Time left for cBorderless operation: {0} seconds.", (endTime - time)), 500);
                        lastSecondDisplayed = time;
                    }
                } while (getTimeInSeconds() < endTime);

                notifyIcon.Visible = false;

                if (!foundProcess)
                {
                    MessageBox.Show(String.Format("No processes were found with the given args. {0} was set to {1}. Exiting.", timedWaitKeyword, waitTime), "cBorderless");
                }
                Environment.Exit(0);
            }
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
        /// Gets the epoch time in seconds
        /// </summary>
        /// <returns></returns>
        private int getTimeInSeconds()
        {
            return (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
        }

        /// <summary>
        /// Displays a bubble notification
        /// </summary>
        /// <param name="notifyIcon">The NotifyIcon to use</param>
        /// <param name="text">Text to display</param>
        /// <param name="milliseconds">Number of ms to display for</param>
        private void displayBubble(NotifyIcon notifyIcon, string text, int milliseconds)
        {
            notifyIcon.Visible = false;
            notifyIcon.Icon = System.Drawing.SystemIcons.Exclamation;
            notifyIcon.Visible = true;
            notifyIcon.ShowBalloonTip(milliseconds, "cBorderless", text, ToolTipIcon.Info);
        }

        /// <summary>
        /// makes the process selected in the combobox (or really whatever is the current text in the combobox) fullscreen windowed borderless
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_make_borderless_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process[] SelectedProcess = System.Diagnostics.Process.GetProcessesByName(combobox_processes.Text);
                makeProcessBorderless(SelectedProcess[0]);
            }
            catch(Exception)
            {
                MessageBox.Show("Unable to find process with given name: \"" + combobox_processes.Text + "\". Refreshing processes list.", "cBorderless: Exception Caught!");
                refreshProcesses();
            }

        }

        /// <summary>
        /// Calls needed Windows APIs to make the given process windowed borderless
        /// </summary>
        /// <param name="SelectedProcess">A given System.Diagnostics.Process</param>
        private void makeProcessBorderless(System.Diagnostics.Process SelectedProcess)
        {
            IntPtr hwnd = SelectedProcess.MainWindowHandle;
            long style = GetWindowLong(hwnd, GWL_STYLE);
            style &= ~(WS_CAPTION | WS_THICKFRAME | WS_MINIMIZE | WS_MAXIMIZE | WS_SYSMENU);
            SetWindowLong(hwnd, GWL_STYLE, (int)style);
            long lExStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
            lExStyle &= ~(WS_EX_DLGMODALFRAME | WS_EX_CLIENTEDGE | WS_EX_STATICEDGE);
            SetWindowLong(hwnd, GWL_EXSTYLE, (int)lExStyle);
            SetWindowPos(SelectedProcess.MainWindowHandle, IntPtr.Zero, -0, -0, Screen.GetBounds(this).Width, Screen.GetBounds(this).Height, (SWP_FRAMECHANGED | SWP_NOZORDER | SWP_NOREPOSITION));
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

        private void revision_info_button_Click(object sender, EventArgs e)
        {
            MessageBox.Show(RevisionInfo.RevisionString, "cBorderless Info");
        }
    }
}
