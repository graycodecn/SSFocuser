using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.IO.Ports;
using System.Timers;
using System.Threading;

namespace SSFSCP
{
    public class SSFocuserSerialCommandProtocol
    {
        private SerialPort MySerialPort;

        private string sComPort;
        private int iBaudRate;
        private string RecvBuf;
        private int BChar = -1;
        private int EChar = -1;
        private char SendInitials, RecvInitials;

        private bool bConnected;
        //Device info
        private bool bHasDevice;
        private string sSN;
        private string sDevice, sVendor, sWebsite, sEmail, sTel;
        //Common
        private bool bIsMoving;
        private int iPosition;
        private double fTemperature;
        //Motor
        private byte iVelocity;
        private bool bReversed;
        private byte iMicroStep;
        private bool bLowPower;
        //Initial
        private bool bPC2Focuser;
        private bool bInitialized;
        private System.Timers.Timer TimerInitialize;

        private String ErrorInfo;

        //构造函数
        public SSFocuserSerialCommandProtocol()
        {
            sComPort = "COM1";
            iBaudRate = 9600;
            bHasDevice = false;
            sSN = "0000000000000000";
            sDevice = "SSFocuser";
            sVendor = "Graycode Team";
            sWebsite = "www.fhxy.com";
            sEmail = "graycode(at)qq.com";
            sTel = "+8608885441222";
            bIsMoving = false;
            iPosition = 0;
            fTemperature = -273.15;
            iVelocity = 8;
            iMicroStep = 8;
            bLowPower = true;
            bPC2Focuser = false;
            bInitialized = false;
        }
        //设备是否连接
        public bool Connected
        {
            get
            {
                return bConnected;
            }
        }
        //串口号属性
        public string ComPort
        {
            get
            {
                return sComPort;
            }
            set
            {
                sComPort = value;
            }
        }
        //波特率属性
        public int BaudRate
        {
            get
            {
                return iBaudRate;
            }
            set
            {
                iBaudRate = value;
            }
        }
        //Serial number
        public string SN
        {
            get
            {
                return sSN;
            }
        }
        //Focuser name
        public string Device
        {
            get
            {
                return sDevice;
            }
        }
        //Vendor
        public string Vendor
        {
            get
            {
                return sVendor;
            }
        }
        //Website
        public string Website
        {
            get
            {
                return sWebsite;
            }
        }
        //Email
        public string Email
        {
            get
            {
                return sEmail;
            }
        }
        //Tel
        public string Tel
        {
            get
            {
                return sTel;
            }
        }
        //Position
        public int Position
        {
            get
            {
                return iPosition;
            }
            set
            {
                iPosition = value;
            }
        }
        //IsMoving
        public bool IsMoving
        {
            get
            {
                return bIsMoving;
            }
            set
            {
                bIsMoving = value;
            }
        }
        //Temperature
        public double Temperature
        {
            get
            {
                return fTemperature;
            }
        }
        //Velocity
        public byte Velocity
        {
            get
            {
                return iVelocity;
            }
        }
        //Reversed
        public bool Reversed
        {
            get
            {
                return bReversed;
            }
        }
        //MicroStep
        public byte MicroStep
        {
            get
            {
                return iMicroStep;
            }
        }
        //LowPower mode
        public bool LowPower
        {
            get
            {
                return bLowPower;
            }
        }
        //初始化模式属性
        public bool PC2Focuser
        {
            get
            {
                return bPC2Focuser;
            }
            set
            {
                bPC2Focuser = value;
            }
        }
        //Initialized
        public bool Initialized
        {
            get
            {
                return bInitialized;
            }
        }
        //Uart Initial
        public void UartInitial()
        {
            try
            {
                MySerialPort = new SerialPort();
                MySerialPort.PortName = sComPort;
                MySerialPort.DataBits = 8;
                MySerialPort.Parity = Parity.None;
                MySerialPort.StopBits = StopBits.One;
                MySerialPort.BaudRate = iBaudRate;
                //MySerialPort.NewLine = "\r\n";
                MySerialPort.DataReceived += new SerialDataReceivedEventHandler(this.UartRecvMsg);
                if (!MySerialPort.IsOpen)
                    MySerialPort.Open();
                bConnected = true;
            }
            //catch (IOException ex)
            catch (Exception ex)
            {
                ErrorInfo = ex.ToString();
                if (MySerialPort.IsOpen)
                {
                    MySerialPort.Close();
                }
                MySerialPort.Dispose();
                MySerialPort = null;
                bConnected = false;
            }
        }
        //Uart Uninitial
        public void UartUninitial()
        {
            try
            {
                if (MySerialPort.IsOpen)
                {
                    MySerialPort.Close();
                }
                MySerialPort.Dispose();
                MySerialPort = null;
                bConnected = false;
            }
            catch (Exception ex)
            {
                ErrorInfo = ex.ToString();
                bConnected = false;
            }
        }
        //Send command (wait for return value, timeout=900mS)
        public bool SendMsg(string command)
        {
            bool ret = false;
            byte i = 0, j = 0;
            SendInitials = Convert.ToChar(command.Substring(2, 1));
            RecvInitials = 'U';
            for (i = 0; i < 3; i++)
            {
                UartSendMsg(command);
                for (j = 0; j < 60; j++)//每个5mS查一次，最长可执行60次（等待300mS）
                {
                    if (RecvInitials == SendInitials)//接收命令正确则退出本循环
                    {
                        ret = true;
                        break;
                    }
                    Thread.Sleep(5);
                }
                if (RecvInitials == SendInitials)//接收命令正确则退出不在发送命令
                {
                    break;
                }
            }
            return ret;
        }
        //Send command
        public bool UartSendMsg(string Msg)
        {
            try
            {
                if (bConnected)
                {
                    MySerialPort.Write(Msg);
                }
                return true;
            }
            catch (IOException ex)//不能弹出窗口，否则串口线拔出后一直提示无法关闭控制软件
            {
                ErrorInfo = ex.ToString();
                return false;
            }
        }
        //Receive Func
        public void UartRecvMsg(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            try
            {
                RecvBuf = RecvBuf + MySerialPort.ReadExisting();
                for (int i = 0; i < RecvBuf.Length; i++)//可能有多条命令
                {
                    if (RecvBuf[i] == ':')//找到协议头
                        BChar = i;
                    if (RecvBuf[i] == '#')//找到协议尾
                        EChar = i;
                    if ((BChar >= 0) && (EChar > BChar))//找到一条完整命令
                    {
                        //处理数据
                        ProcessData(RecvBuf.Substring(BChar, EChar - BChar + 1));
                        BChar = -1;
                        EChar = -1;
                        //TimeCount = 0;//超时计数复位
                    }
                }
                if (BChar < 0)//找不到协议头
                    RecvBuf = "";
                else
                {
                    RecvBuf = RecvBuf.Substring(BChar, RecvBuf.Length - BChar);
                }
                if (RecvBuf.Length >= 256)//避免有一个冒号的非命令字节堵塞
                    RecvBuf = "";
                BChar = -1;
                EChar = -1;
            }
            catch (Exception ex)
            {
                ErrorInfo = ex.ToString();
            }
        }
        //Process receive data
        private void ProcessData(string data)
        {
            RecvInitials = data[1];//当前返回命令标志位
            data = data.Substring(2, data.Length - 3);
            switch (RecvInitials)
            {
                case '?'://ACK return 
                    {
                        string[] TempArray = data.Split('~');
                        if ((TempArray.Length >= 5) && ((TempArray[0] == "SS") || (TempArray[0] == "SSF")))
                        {
                            bHasDevice = true;
                            sDevice = TempArray[1];
                            sVendor = TempArray[2];
                            sWebsite = TempArray[3];
                            sEmail = TempArray[4];
                            sTel = TempArray[5];
                        }
                        break;
                    }
                case 's'://Serial number
                    {
                        sSN = data;
                        break;
                    }
                case 'B'://
                    {
                        bIsMoving = (Convert.ToByte(data) == 0) ? false : true;
                        break;
                    }
                case 'i':
                    {
                        bInitialized = (Convert.ToByte(data) == 0) ? false : true;
                        if (bPC2Focuser)//初始化以电脑存储位置为准，适用于调焦器不带掉电存储功能的情况
                        {
                            if (!bInitialized)//未初始化就进行初始化
                            {
                                //用一个定时器执行串口初始化，避免界面打开缓慢
                                TimerInitialize = new System.Timers.Timer();
                                TimerInitialize.Elapsed += new ElapsedEventHandler(InitializeFocuser);
                                TimerInitialize.Interval = 10;
                                TimerInitialize.AutoReset = false;//执行一次 false，一直执行true  
                                //是否执行System.Timers.Timer.Elapsed事件  
                                TimerInitialize.Enabled = true;
                            }
                        }
                        else//初始化以调焦器存储位置为准
                        {
                            if (bInitialized)//已初始化就设去初始化，否则调焦器掉电不存储位置
                                SetInitState(false);
                        }
                        break;
                    }
                case 'l':
                    {
                        bLowPower = (Convert.ToByte(data) == 0) ? false : true;
                        break;
                    }
                case 'r':
                    {
                        bReversed = (Convert.ToByte(data) == 0) ? false : true;
                        break;
                    }
                case 'm':
                    {
                        iMicroStep = Convert.ToByte(data);
                        break;
                    }
                case 'v':
                    {
                        iVelocity = Convert.ToByte(data);
                        break;
                    }
                case 'p':
                    {
                        iPosition = Convert.ToInt32(data);
                        break;
                    }
                case 't':
                    {
                        fTemperature = Convert.ToDouble(data);
                        break;
                    }
            }
        }
        //查找设备
        public bool CheckDevice()
        {
            if ((SendMsg(":F?#")) && (bHasDevice))
                return true;
            else
                return false;
        }
        //往外移动
        public void SlewOut()
        {
            //SendMsg(":F+#");
            UartSendMsg(":F+#");//只发送一次，不接收返回，否则效率低
        }
        //往里移动
        public void SlewIn()
        {
            //SendMsg(":F-#");
            UartSendMsg(":F-#");
        }
        //移动到指定位置
        public bool MoveTo(int Destnation)
        {
            bIsMoving = true;//先设置运动状态
            return SendMsg(":FP" + Destnation.ToString() + "#");
        }
        //Stop
        public bool Stop()
        {
            return SendMsg(":FQ#");
        }
        //Get Serial Number
        public bool GetSN()
        {
            return SendMsg(":Fs#");
        }
        //Define Position
        public bool DefinePosition(int Position)
        {
            return SendMsg(":FD" + Position.ToString() + "#");
        }
        //Set bluetooth name
        public void SetBluetooth(string Name)
        {
            //SendMsg(":Fb" + Name.Trim() + "#");//发三次单片机会死机
            UartSendMsg(":Fb" + Name.Trim() + "#");
        }
        //Get device and vendor info
        public bool GetVersion()
        {
            return SendMsg(":F?#");
        }
        //Get moving state
        public bool GetMovState()
        {
            return SendMsg(":FB#");
        }
        //Get position
        public bool GetPosition()
        {
            return SendMsg(":Fp#");
        }
        //Get tepperature
        public bool GetTemperature()
        {
            return SendMsg(":Ft#");
        }
        //Set initial state, Position data will store in the focuser memory if LocalStore=false
        public bool SetInitState(bool LocalStore)
        {
            if (LocalStore)
                return SendMsg(":FI1#");
            else
                return SendMsg(":FI0#");
        }
        //Get initial state
        public bool GetInitState()
        {
            return SendMsg(":Fi#");
        }
        //设置低功耗模式
        public bool SetLowPower(bool LowPower)
        {
            if (LowPower)
                return SendMsg(":FL1#");
            else
                return SendMsg(":FL0#");
        }
        //获取功耗模式
        public bool GetPowerWaste()
        {
            return SendMsg(":Fl#");
        }
        //Set Motor Dir Reversed
        public bool SetReverse(bool reversed)
        {
            if (reversed)
                return SendMsg(":FR1#");
            else
                return SendMsg(":FR0#");
        }
        //获取反向状态
        public bool GetReverse()
        {
            return SendMsg(":Fr#");
        }
        //设置细分
        public bool SetMicrostep(byte Mode)
        {
            return SendMsg(":FM" + Mode.ToString("D3") + "#");
        }
        //获取细分
        public bool GetMicrostep()
        {
            return SendMsg(":Fm#");
        }
        //设置速度
        public bool SetVelocity(byte Speed)
        {
            return SendMsg(":FV" + Speed.ToString("D1") + "#");
        }
        //获取速度
        public bool GetVelocity()
        {
            return SendMsg(":Fv#");
        }
        //初始化调焦器
        private void InitializeFocuser(object source, System.Timers.ElapsedEventArgs e)
        {
            DefinePosition(iPosition);//校准调焦器位置
            SetInitState(true);//设置已初始化，这样调焦器掉电不存储位置
            GetInitState();//再次获取初始化状态进行确认，如不成功继续初始化
        }
    }
}
