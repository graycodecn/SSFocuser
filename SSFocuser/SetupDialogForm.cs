// ASCOM Focuser driver for SSFocuser
//
// Description:	Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam 
//				nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam 
//				erat, sed diam voluptua. At vero eos et accusam et justo duo 
//				dolores et ea rebum. Stet clita kasd gubergren, no sea takimata 
//				sanctus est Lorem ipsum dolor sit amet.
//
// Implements:	ASCOM Focuser interface version: <To be completed by driver developer>
// Author:		HeShouSheng <graycode@qq.com><www.graycode.cn>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// 15-03-2020	HSS	2.0.0	Second version, created from ASCOM driver template
// --------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using ASCOM.Utilities;
using ASCOM.SSFocuser;

using System.Net;

using System.Timers;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Text.RegularExpressions;//正则表达
using SSFSCP;

namespace ASCOM.SSFocuser
{
    [ComVisible(false)]					// Form not registered for COM!
    public partial class SetupDialogForm : Form
    {
        private SSFocuserSerialCommandProtocol MySCP;
        private System.Timers.Timer t;
        private delegate void TimerFunc();
        private static byte SlewStatue;
        private static byte i;
        //private bool FirstTime = true;
        private string ErrorInfo;

        // usb消息定义
        public const int WM_DEVICE_CHANGE = 0x219;
        public const int DBT_DEVICEARRIVAL = 0x8000;
        public const int DBT_DEVICE_REMOVE_COMPLETE = 0x8004;
        public const UInt32 DBT_DEVTYP_PORT = 0x00000003;

        [StructLayout(LayoutKind.Sequential)]
        struct DEV_BROADCAST_HDR
        {
            public UInt32 dbch_size;
            public UInt32 dbch_devicetype;
            public UInt32 dbch_reserved;
        }

        [StructLayout(LayoutKind.Sequential)]
        protected struct DEV_BROADCAST_PORT_Fixed
        {
            public uint dbcp_size;
            public uint dbcp_devicetype;
            public uint dbcp_reserved;
        }

        public SetupDialogForm()
        {
            InitializeComponent();
            // Initialise current values of user settings from the ASCOM Profile

            InitUI();

            MySCP = new SSFocuserSerialCommandProtocol();

            SlewStatue = 0; i = 0;

            if (!Focuser.connectedState)//用一个定时器执行第一次串口初始化，避免界面打开缓慢
            {
                tabControl1.SelectedIndex = 0;
                UartUninitial();
                UartInitial();
                t = new System.Timers.Timer(500);//500毫秒执行一次；
                t.Elapsed += new System.Timers.ElapsedEventHandler(TimerEvent);//到达时间的时候执行事件；
                t.AutoReset = true;//设置是执行一次（false）还是一直执行(true)；
                t.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；
            }
        }
        /// <summary>
        /// 检测USB串口的拔插
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_DEVICE_CHANGE) // 捕获USB设备的拔出消息WM_DEVICECHANGE
            {
                switch (m.WParam.ToInt32())
                {
                    case DBT_DEVICE_REMOVE_COMPLETE: // USB拔出 
                        DEV_BROADCAST_HDR dbhd = (DEV_BROADCAST_HDR)Marshal.PtrToStructure(m.LParam, typeof(DEV_BROADCAST_HDR));
                        if (dbhd.dbch_devicetype == DBT_DEVTYP_PORT)
                        {
                            string portName = Marshal.PtrToStringUni((IntPtr)(m.LParam.ToInt32() + Marshal.SizeOf(typeof(DEV_BROADCAST_PORT_Fixed))));
                            //MessageBox.Show(portName+" 已拔出");
                            Display();
                        }

                        break;
                    case DBT_DEVICEARRIVAL: // USB插入获取对应串口名称
                        DEV_BROADCAST_HDR dbhdr = (DEV_BROADCAST_HDR)Marshal.PtrToStructure(m.LParam, typeof(DEV_BROADCAST_HDR));
                        if (dbhdr.dbch_devicetype == DBT_DEVTYP_PORT)
                        {
                            string portName = Marshal.PtrToStringUni((IntPtr)(m.LParam.ToInt32() + Marshal.SizeOf(typeof(DEV_BROADCAST_PORT_Fixed))));
                            //MessageBox.Show(portName + " 已插入");
                            Display();
                        }
                        break;
                }
            }
            base.WndProc(ref m);
        }
        
        public void TimerEvent(object source, System.Timers.ElapsedEventArgs e)
        {
            if (MySCP.Connected)
            {
                if (SlewStatue == 1)
                {
                    MySCP.SlewIn();
                }
                else if (SlewStatue == 2)
                {
                    MySCP.SlewOut();
                }
                MySCP.GetPosition();
                //if (MyFocuser.IsMoving)
                MySCP.GetMovState();
                if (i >= 2)
                {
                    //MySCP.GetTemperature();
                    MySCP.GetVelocity();
                    i = 0;
                }
                else
                    i++;
                TimerFunc func = new TimerFunc(UpdateStatus);
                this.Invoke(func, new Object[] { });
            }
        }
        public void UpdateStatus()
        {
            if (MySCP.IsMoving)
            {
                buttonDefineZero.Enabled = false;
                //buttonDefineMax.Enabled = false;
            }
            else
            {
                buttonDefineZero.Enabled = true;
                //buttonDefineMax.Enabled = true;
            }
            textBoxPosition.Text = MySCP.Position.ToString();
            textBoxVelocity.Text = MySCP.Velocity.ToString();
            //currentPosition.Text = " Temperature: " + Focuser.m_CurrentTemperature.ToString() + " Speed: " + Focuser.m_MotorSpeed.ToString();
        }
        private void cmdOK_Click(object sender, EventArgs e) // OK button event handler
        {
            // Place any validation constraint checks here
            // Update the state variables with results from the dialogue
            //Focuser.comPort = (string)comboBoxComPort.SelectedItem;
            Focuser.tl.Enabled = chkTrace.Checked;
            if (Focuser.connectedState)//非连接状态才操作
                return;
            try
            {
                UartUninitial();

                Focuser.comPort = (string)comboBoxcomPort.SelectedItem;
                if ((textBoxMaxStep.Text!="")&&(Convert.ToInt32(textBoxMaxStep.Text)>0))
                {
                    Focuser.m_MaxStep = Convert.ToInt32(textBoxMaxStep.Text);
                    Focuser.m_MaxIncrement = Focuser.m_MaxStep;
                }
                if ((textBoxStepSize.Text != "") && (Convert.ToSingle(textBoxStepSize.Text) > 0))
                    Focuser.m_StepSize = Convert.ToSingle(textBoxStepSize.Text);
                /*
                if ((textBoxStepPerDeg.Text != "") && (Convert.ToSingle(textBoxStepPerDeg.Text) > 0))
                    Focuser.m_TempCompRatio = Convert.ToSingle(textBoxStepPerDeg.Text);
                if ((textBoxThreshold.Text != "") && (Convert.ToSingle(textBoxThreshold.Text) > 0))
                    Focuser.m_TempCompDegs = Convert.ToSingle(textBoxThreshold.Text);
               */
                Focuser.PC2Focuser = checkBoxPC2Focuser.Checked;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void cmdCancel_Click(object sender, EventArgs e) // Cancel button event handler
        {
            if (!Focuser.connectedState)//非连接状态才操作，乙方连接时查看设置串口导致串口冲突
                UartUninitial();
            Close();
        }

        private void SetupDialogForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!Focuser.connectedState)
                UartUninitial();
        }

        private void BrowseToAscom(object sender, EventArgs e) // Click on ASCOM logo event handler
        {
            try
            {
                System.Diagnostics.Process.Start("http://ascom-standards.org/");
            }
            catch (System.ComponentModel.Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                    MessageBox.Show(noBrowser.Message);
            }
            catch (System.Exception other)
            {
                MessageBox.Show(other.Message);
            }
        }

        private void InitUI()
        {
            chkTrace.Checked = Focuser.tl.Enabled;
            // set the list of com ports to those that are currently available
            /*
            comboBoxComPort.Items.Clear();
            comboBoxComPort.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());      // use System.IO because it's static
            // select the current port if possible
            if (comboBoxComPort.Items.Contains(Focuser.comPort))
            {
                comboBoxComPort.SelectedItem = Focuser.comPort;
            }
            */
            Display();
        }

        private void linkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if ((MySCP.Connected) || (Focuser.connectedState))
                System.Diagnostics.Process.Start("http://" + MySCP.Website);
            else
            { 
                OpenJoinUsForm();
            }
        }
        private void linkLabelJoinUs_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenJoinUsForm();
        }
        private void OpenJoinUsForm()
        {
            Form OpenedForm = Application.OpenForms["JoinUsForm"];  //查找是否打开过about窗体  
            if ((OpenedForm == null) || (OpenedForm.IsDisposed)) //如果没有打开过
            {
                JoinUsForm MyJoinUsForm = new JoinUsForm();
                MyJoinUsForm.Show();   //打开子窗体出来
            }
            else
            {
                OpenedForm.Activate(); //如果已经打开过就让其获得焦点  
                OpenedForm.WindowState = FormWindowState.Normal;//使Form恢复正常窗体大小
            }
        }
        private void Display()
        {
            comboBoxcomPort.Items.Clear();
            comboBoxcomPort.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());      // use System.IO because it's static
            if (comboBoxcomPort.Items.Count > 0)
            {
                if (comboBoxcomPort.Items.Contains(Focuser.comPort))
                {
                    comboBoxcomPort.SelectedItem = Focuser.comPort;
                }
                else
                    comboBoxcomPort.SelectedIndex = 0;
            }
            if (Focuser.connectedState)//已连接状态查看Setup
            {
                labelDevice.ForeColor = System.Drawing.Color.Black;
                labelDevice.Text = "Device: " + MySCP.Device;
                labelVendor.Text = "Company: " + MySCP.Vendor;
                linkLabel.Text = MySCP.Website;
                labelEmail.Text = "Email: " + MySCP.Email;
                labelTel.Text = "Tel: " + MySCP.Tel;
                comboBoxcomPort.Enabled = false;
                textBoxMaxStep.Enabled = false;
                textBoxStepSize.Enabled = false;
                comboBoxVelocity.Enabled = false;
                buttonSlewIn.Enabled = false;
                buttonSlewOut.Enabled = false;
                buttonDefineZero.Enabled = false;
                //textBoxStepPerDeg.Enabled = false;
                //textBoxThreshold.Enabled = false;
                checkBoxPC2Focuser.Enabled = false;
                chkTrace.Enabled = false;
            }
            textBoxMaxStep.Text = Focuser.m_MaxStep.ToString();
            textBoxStepSize.Text = Focuser.m_StepSize.ToString("F2");

            //textBoxStepPerDeg.Text = (Focuser.m_TempCompRatio >= 0 ? "+" : "-") + Focuser.m_TempCompRatio.ToString("F2");
            //textBoxThreshold.Text = Focuser.m_TempCompDegs.ToString();

            checkBoxPC2Focuser.Checked = Focuser.PC2Focuser;  
        }

        private void comboBoxcomPort_SelectedValueChanged(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
            UartUninitial();
            UartInitial();
        }

        public void UartInitial()
        {
            try
            {
                MySCP.ComPort = (string)comboBoxcomPort.SelectedItem;
                MySCP.UartInitial();
                if (MySCP.Connected)
                {
                    //查询版本信息
                    if (MySCP.CheckDevice())
                    {
                        labelDevice.ForeColor = System.Drawing.Color.Black;
                        labelDevice.Text = "Device: " + MySCP.Device;
                        labelVendor.Text = "Company: " + MySCP.Vendor;
                        linkLabel.Text = MySCP.Website;
                        labelEmail.Text = "Email: " + MySCP.Email;
                        labelTel.Text = "Tel: " + MySCP.Tel;
                        comboBoxVelocity.Text = MySCP.Velocity.ToString();
                    }
                    else
                    {
                        labelDevice.ForeColor = System.Drawing.Color.Red;
                        labelDevice.Text = "No device detected,Check connection and refresh!";
                        labelVendor.Text = "Developer: Graycode Team";
                        linkLabel.Text = "Graycode Team";
                        labelEmail.Text = "Email: graycode(at)qq.com";
                        labelTel.Text = "WeChat: graycode";

                        UartUninitial();
                    }
                }
                else//这里不用UartUninitial();初始化未成功会自动关闭
                {
                    labelDevice.ForeColor = System.Drawing.Color.Red;
                    labelDevice.Text = "No device detected,Check connection and refresh!";
                    labelVendor.Text = "Developer: Graycode Team";
                    linkLabel.Text = "Graycode Team";
                    labelEmail.Text = "Email: graycode(at)qq.com";
                    labelTel.Text = "WeChat: graycode";
                }
            }
            //catch (IOException ex)
            catch (Exception ex)
            {
                ErrorInfo = ex.ToString();
                labelDevice.ForeColor = System.Drawing.Color.Red;
                labelDevice.Text = "No device detected,Check connection and refresh!";
                labelVendor.Text = "Developer: Graycode Team";
                linkLabel.Text = "Graycode Team";
                labelEmail.Text = "Email: graycode(at)qq.com";
                labelTel.Text = "WeChat: graycode";

                UartUninitial();
                
            }
        }

        public void UartUninitial()
        {
            if(MySCP!=null)
                MySCP.UartUninitial();
        }

        private void textBoxMaxStep_KeyPress(object sender, KeyPressEventArgs e)
        {
            Focuser.OnlyEnterPlusInt(sender, e);
        }

        private void textBoxStepSize_KeyPress(object sender, KeyPressEventArgs e)
        {
            Focuser.OnlyEnterPlusNumber(sender, e);
        }

        private void buttonSlewIn_MouseDown(object sender, MouseEventArgs e)
        {
            if (MySCP.Connected)
                SlewStatue = 1;
        }

        private void buttonSlewIn_MouseUp(object sender, MouseEventArgs e)
        {
            SlewStatue = 0;
            if (MySCP.Connected)
                MySCP.Stop();
        }

        private void buttonSlewOut_MouseDown(object sender, MouseEventArgs e)
        {
            if (MySCP.Connected)
                SlewStatue = 2;
        }

        private void buttonSlewOut_MouseUp(object sender, MouseEventArgs e)
        {
            SlewStatue = 0;
            if (MySCP.Connected)
                MySCP.Stop();
        }

        private void buttonDefineZero_Click(object sender, EventArgs e)
        {
            if (MySCP.Connected)
                MySCP.DefinePosition(0);
        }

        private void comboBoxVelocity_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(MySCP.Connected)
                MySCP.SetVelocity(Convert.ToByte(comboBoxVelocity.SelectedItem));
        }

        private void checkBoxPC2Focuser_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}