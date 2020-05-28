//tabs=4
// --------------------------------------------------------------------------------
// TODO fill in this information for your driver, then remove this line!
//
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
//


// This is used to define code in the template that is specific to one class implementation
// unused code canbe deleted and this definition removed.
#define Focuser

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Runtime.InteropServices;

using ASCOM;
using ASCOM.Astrometry;
using ASCOM.Astrometry.AstroUtils;
using ASCOM.Utilities;
using ASCOM.DeviceInterface;
using System.Globalization;
using System.Collections;

using System.IO;
using System.IO.Ports;
//using System.Net;
//using System.Net.Sockets;
using System.Timers;
using System.Threading;
using System.Windows.Forms;
using System.Text.RegularExpressions;//正则表达
using System.Data;
using SSFSCP;

namespace ASCOM.SSFocuser
{
    //
    // Your driver's DeviceID is ASCOM.SSFocuser.Focuser
    //
    // The Guid attribute sets the CLSID for ASCOM.SSFocuser.Focuser
    // The ClassInterface/None addribute prevents an empty interface called
    // _SSFocuser from being created and used as the [default] interface
    //
    // TODO Replace the not implemented exceptions with code to implement the function or
    // throw the appropriate ASCOM exception.
    //

    /// <summary>
    /// ASCOM Focuser Driver for SSFocuser.
    /// </summary>
    [Guid("da3205ae-dd6e-4188-9fea-7bed542cba3d")]
    [ClassInterface(ClassInterfaceType.None)]
    public class Focuser : IFocuserV2
    {
        /// <summary>
        /// ASCOM DeviceID (COM ProgID) for this driver.
        /// The DeviceID is used by ASCOM applications to load the driver at runtime.
        /// </summary>
        internal static string driverID = "ASCOM.SSFocuser.Focuser";
        // TODO Change the descriptive string for your driver then remove this line
        /// <summary>
        /// Driver description that displays in the ASCOM Chooser.
        /// </summary>
        //private static string driverDescription = "ASCOM Focuser Driver for SSFocuser.";

        //private static string driverDescription = "SS Focuser";
        private static string driverDescription = "SS Focuser";

        internal static string comPortProfileName = "COM Port"; // Constants used for Profile persistence
        internal static string comPortDefault = "COM1";
        internal static string traceStateProfileName = "Trace Level";
        internal static string traceStateDefault = "false";

        internal static string comPort; // Variables to hold the currrent device configuration
        internal static int baudRate; // graycode
        

        /// <summary>
        /// Private variable to hold the connected state
        /// </summary>
        //private bool connectedState;
        public static bool connectedState;//供设置对话框访问graycode

        /// <summary>
        /// Private variable to hold an ASCOM Utilities object
        /// </summary>
        private Util utilities;

        /// <summary>
        /// Private variable to hold an ASCOM AstroUtilities object to provide the Range method
        /// </summary>
        private AstroUtils astroUtilities;

        /// <summary>
        /// Variable to hold the trace logger object (creates a diagnostic log file with information that you specify)
        /// </summary>
        internal static TraceLogger tl;


        //////////////graycode///////////
        private SSFocuserSerialCommandProtocol MySCP;
        
        //Low power mode
        public static bool m_LowPower;
        //Focuser
        public static int m_MaxStep, m_MaxIncrement;
        public static float m_StepSize;
        //Temperature
        //public static float m_TempCompRatio;
        //public static float m_TempCompDegs;
        //Motor
        //public static bool m_MotorReverse;
        //public static byte m_MotorMicrostep;
        //public static byte m_MotorSpeed;
        //Other
        //public static bool m_IsMoving, m_TempCompAvailable, m_TempComp;
        public static bool m_TempComp;
        public static int m_Position;
        //public static double m_StartTemperature, m_CurrentTemperature;

        public static bool PC2Focuser;
        
        //internal static byte TimeCount = 0;
        //internal static byte TimeOut = 60;//60秒超时
        //private String ErrorInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="SSFocuser"/> class.
        /// Must be public for COM registration.
        /// </summary>
        public Focuser()
        {
            tl = new TraceLogger("", "SSFocuser");

            ReadProfile(); // Read device configuration from the ASCOM Profile store

            tl.LogMessage("Focuser", "Starting initialisation");

            connectedState = false; // Initialise connected to false
            utilities = new Util(); //Initialise util object
            astroUtilities = new AstroUtils(); // Initialise astro utilities object
            //TODO: Implement your additional construction here

            tl.LogMessage("Focuser", "Completed initialisation");
            //这里是setupdialog后才初始化

            MySCP = new SSFocuserSerialCommandProtocol();

            m_Position = ReadPosition();
            //m_IsMoving = false;
            m_TempComp = false;
            //m_TempCompAvailable = true;       
        }
        //
        // PUBLIC COM INTERFACE IFocuserV2 IMPLEMENTATION
        //

        #region Common properties and methods.

        /// <summary>
        /// Displays the Setup Dialog form.
        /// If the user clicks the OK button to dismiss the form, then
        /// the new settings are saved, otherwise the old values are reloaded.
        /// THIS IS THE ONLY PLACE WHERE SHOWING USER INTERFACE IS ALLOWED!
        /// </summary>
        public void SetupDialog()
        {
            // consider only showing the setup dialog if not connected
            // or call a different dialog if connected
            if (IsConnected)
                System.Windows.Forms.MessageBox.Show("Already connected, just press OK");

            using (SetupDialogForm F = new SetupDialogForm())
            {
                var result = F.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    WriteProfile(); // Persist device configuration values to the ASCOM Profile store
                }
            }
        }

        public ArrayList SupportedActions
        {
            get
            {
                tl.LogMessage("SupportedActions Get", "Returning empty arraylist");
                return new ArrayList();
            }
        }

        public string Action(string actionName, string actionParameters)
        {
            LogMessage("", "Action {0}, parameters {1} not implemented", actionName, actionParameters);
            throw new ASCOM.ActionNotImplementedException("Action " + actionName + " is not implemented by this driver");
        }

        public void CommandBlind(string command, bool raw)
        {
            CheckConnected("CommandBlind");
            // Call CommandString and return as soon as it finishes
            this.CommandString(command, raw);
            // or
            throw new ASCOM.MethodNotImplementedException("CommandBlind");
            // DO NOT have both these sections!  One or the other
        }

        public bool CommandBool(string command, bool raw)
        {
            CheckConnected("CommandBool");
            string ret = CommandString(command, raw);
            // TODO decode the return string and return true or false
            // or
            throw new ASCOM.MethodNotImplementedException("CommandBool");
            // DO NOT have both these sections!  One or the other
        }

        public string CommandString(string command, bool raw)
        {
            CheckConnected("CommandString");
            // it's a good idea to put all the low level communication with the device here,
            // then all communication calls this function
            // you need something to ensure that only one command is in progress at a time

            throw new ASCOM.MethodNotImplementedException("CommandString");
        }

        public void Dispose()
        {
            // Clean up the tracelogger and util objects
            tl.Enabled = false;
            tl.Dispose();
            tl = null;
            utilities.Dispose();
            utilities = null;
            astroUtilities.Dispose();
            astroUtilities = null;
        }

        public bool Connected
        {
            get
            {
                LogMessage("Connected", "Get {0}", IsConnected);
                return IsConnected;
            }
            set
            {
                tl.LogMessage("Connected", "Set {0}", value);
                if (value == IsConnected)
                    return;

                if (value)
                {
                    //
                    //LogMessage("Connected Set", "Connecting to port {0}", comPort);
                    // TODO connect to the device
                    UartInitial();
                }
                else
                {
                    //LogMessage("Connected Set", "Disconnecting from port {0}", comPort);
                    // TODO disconnect from the device
                    UartUninitial();
                }
            }
        }

        public string Description
        {
            // TODO customise this device description
            get
            {
                tl.LogMessage("Description Get", driverDescription);
                return driverDescription;
            }
        }

        public string DriverInfo
        {
            get
            {
                Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                // TODO customise this driver description
                string driverInfo = "Information about the driver itself. Version: " + String.Format(CultureInfo.InvariantCulture, "{0}.{1}", version.Major, version.Minor);
                tl.LogMessage("DriverInfo Get", driverInfo);
                return driverInfo;
            }
        }

        public string DriverVersion
        {
            get
            {
                Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                string driverVersion = String.Format(CultureInfo.InvariantCulture, "{0}.{1}", version.Major, version.Minor);
                tl.LogMessage("DriverVersion Get", driverVersion);
                return driverVersion;
            }
        }

        public short InterfaceVersion
        {
            // set by the driver wizard
            get
            {
                LogMessage("InterfaceVersion Get", "2");
                return Convert.ToInt16("2");
            }
        }

        public string Name
        {
            get
            {
                string name = "Short driver name - please customise";
                tl.LogMessage("Name Get", name);
                return name;
            }
        }

        #endregion

        #region IFocuser Implementation

        //private int focuserPosition = 0; // Class level variable to hold the current focuser position
        //private const int focuserSteps = 10000;

        public bool Absolute
        {
            get
            {
                tl.LogMessage("Absolute Get", true.ToString());
                return true; // This is an absolute focuser
            }
        }

        public void Halt()
        {
            //tl.LogMessage("Halt", "Not implemented");
            //throw new ASCOM.MethodNotImplementedException("Halt");
            tl.LogMessage("Halt", "Halt");
            MySCP.Stop();
        }

        public bool IsMoving
        {
            get
            {
                //tl.LogMessage("IsMoving Get", false.ToString());
                //return false; // This focuser always moves instantaneously so no need for IsMoving ever to be True
                tl.LogMessage("IsMoving Get", MySCP.IsMoving.ToString());
                MySCP.GetMovState();
                return MySCP.IsMoving;
            }
        }

        public bool Link
        {
            get
            {
                tl.LogMessage("Link Get", this.Connected.ToString());
                return this.Connected; // Direct function to the connected method, the Link method is just here for backwards compatibility
            }
            set
            {
                tl.LogMessage("Link Set", value.ToString());
                this.Connected = value; // Direct function to the connected method, the Link method is just here for backwards compatibility
            }
        }

        public int MaxIncrement
        {
            get
            {
                //tl.LogMessage("MaxIncrement Get", focuserSteps.ToString());
                //return focuserSteps; // Maximum change in one move
                tl.LogMessage("MaxIncrement Get", m_MaxIncrement.ToString());
                return m_MaxIncrement;
            }
        }

        public int MaxStep
        {
            get
            {
                //tl.LogMessage("MaxStep Get", focuserSteps.ToString());
                //return focuserSteps; // Maximum extent of the focuser, so position range is 0 to 10,000
                tl.LogMessage("MaxStep Get", m_MaxStep.ToString());
                return m_MaxStep;
            }
        }

        public void Move(int Position)//
        {
            tl.LogMessage("Move", Position.ToString());
            //focuserPosition = Position; // Set the focuser position
            MySCP.MoveTo(Position);
        }

        public int Position//Moving期间不发Position，只查询IsMoving
        {
            get
            {
                //return focuserPosition; // Return the focuser position
                MySCP.GetPosition();
                if (m_Position != MySCP.Position)//存储位置到本地计算机PC
                {
                    m_Position = MySCP.Position;
                    WritePosition(m_Position);
                }
                return MySCP.Position; // Return the focuser position
            }
        }

        public double StepSize
        {
            get
            {
                //tl.LogMessage("StepSize Get", "Not implemented");
                //throw new ASCOM.PropertyNotImplementedException("StepSize", false);
                tl.LogMessage("StepSize Get", m_StepSize.ToString());
                return m_StepSize;
            }
        }

        public bool TempComp
        {
            get
            {
                //tl.LogMessage("TempComp Get", false.ToString());
                //return false;
                tl.LogMessage("TempComp Get", m_TempComp.ToString());
                return m_TempComp;
            }
            set
            {
                //tl.LogMessage("TempComp Set", "Not implemented");
                //throw new ASCOM.PropertyNotImplementedException("TempComp", false);
                m_TempComp = value;
            }
        }

        public bool TempCompAvailable
        {
            get
            {
                //tl.LogMessage("TempCompAvailable Get", false.ToString());
                //return false; // Temperature compensation is not available in this driver
                tl.LogMessage("TempCompAvailable Get", true.ToString());
                return true;
            }
        }

        public double Temperature
        {
            get
            {
                //tl.LogMessage("Temperature Get", "Not implemented");
                //throw new ASCOM.PropertyNotImplementedException("Temperature", false);
                tl.LogMessage("Temperature Get", MySCP.Temperature.ToString());
                MySCP.GetTemperature();
                return MySCP.Temperature;
            }
        }

        #endregion

        #region Private properties and methods
        // here are some useful properties and methods that can be used as required
        // to help with driver development

        #region ASCOM Registration

        // Register or unregister driver for ASCOM. This is harmless if already
        // registered or unregistered. 
        //
        /// <summary>
        /// Register or unregister the driver with the ASCOM Platform.
        /// This is harmless if the driver is already registered/unregistered.
        /// </summary>
        /// <param name="bRegister">If <c>true</c>, registers the driver, otherwise unregisters it.</param>
        private static void RegUnregASCOM(bool bRegister)
        {
            using (var P = new ASCOM.Utilities.Profile())
            {
                P.DeviceType = "Focuser";
                if (bRegister)
                {
                    P.Register(driverID, driverDescription);
                }
                else
                {
                    P.Unregister(driverID);
                }
            }
        }

        /// <summary>
        /// This function registers the driver with the ASCOM Chooser and
        /// is called automatically whenever this class is registered for COM Interop.
        /// </summary>
        /// <param name="t">Type of the class being registered, not used.</param>
        /// <remarks>
        /// This method typically runs in two distinct situations:
        /// <list type="numbered">
        /// <item>
        /// In Visual Studio, when the project is successfully built.
        /// For this to work correctly, the option <c>Register for COM Interop</c>
        /// must be enabled in the project settings.
        /// </item>
        /// <item>During setup, when the installer registers the assembly for COM Interop.</item>
        /// </list>
        /// This technique should mean that it is never necessary to manually register a driver with ASCOM.
        /// </remarks>
        [ComRegisterFunction]
        public static void RegisterASCOM(Type t)
        {
            RegUnregASCOM(true);
        }

        /// <summary>
        /// This function unregisters the driver from the ASCOM Chooser and
        /// is called automatically whenever this class is unregistered from COM Interop.
        /// </summary>
        /// <param name="t">Type of the class being registered, not used.</param>
        /// <remarks>
        /// This method typically runs in two distinct situations:
        /// <list type="numbered">
        /// <item>
        /// In Visual Studio, when the project is cleaned or prior to rebuilding.
        /// For this to work correctly, the option <c>Register for COM Interop</c>
        /// must be enabled in the project settings.
        /// </item>
        /// <item>During uninstall, when the installer unregisters the assembly from COM Interop.</item>
        /// </list>
        /// This technique should mean that it is never necessary to manually unregister a driver from ASCOM.
        /// </remarks>
        [ComUnregisterFunction]
        public static void UnregisterASCOM(Type t)
        {
            RegUnregASCOM(false);
        }

        #endregion

        /// <summary>
        /// Returns true if there is a valid connection to the driver hardware
        /// </summary>
        private bool IsConnected
        {
            get
            {
                // TODO check that the driver hardware connection exists and is connected to the hardware
                return connectedState;
            }
        }

        /// <summary>
        /// Use this function to throw an exception if we aren't connected to the hardware
        /// </summary>
        /// <param name="message"></param>
        private void CheckConnected(string message)
        {
            if (!IsConnected)
            {
                throw new ASCOM.NotConnectedException(message);
            }
        }

        /// <summary>
        /// Read the device configuration from the ASCOM Profile store
        /// </summary>
        internal void ReadProfile()
        {
            using (Profile driverProfile = new Profile())
            {
                driverProfile.DeviceType = "Focuser";
                tl.Enabled = Convert.ToBoolean(driverProfile.GetValue(driverID, traceStateProfileName, string.Empty, traceStateDefault));

                comPort = driverProfile.GetValue(driverID, comPortProfileName, string.Empty, comPortDefault);
                baudRate = Convert.ToInt32(driverProfile.GetValue(driverID, "BaudRate", string.Empty, "9600"));

                m_LowPower = Convert.ToBoolean(driverProfile.GetValue(driverID, "LowPower", string.Empty, true.ToString()));

                //m_MotorReverse = Convert.ToBoolean(driverProfile.GetValue(driverID, "MotorReverse", string.Empty, false.ToString()));
                //m_MotorMicrostep = Convert.ToByte(driverProfile.GetValue(driverID, "MotorMicrostep", string.Empty, "8"));
                //m_MotorSpeed = Convert.ToByte(driverProfile.GetValue(driverID, "MotorSpeed", string.Empty, "4"));

                m_MaxStep = Convert.ToInt32(driverProfile.GetValue(driverID, "MaxStep", string.Empty, "50000"));
                m_StepSize = Convert.ToSingle(driverProfile.GetValue(driverID, "StepSize", string.Empty, "10.0"));
                m_MaxIncrement = Convert.ToInt32(driverProfile.GetValue(driverID, "MaxIncrement", string.Empty, "10000"));

                PC2Focuser = Convert.ToInt32(driverProfile.GetValue(driverID, "PC2Focuser", string.Empty, "0")) == 1 ? true : false;               
            }
        }

        /// <summary>
        /// Write the device configuration to the  ASCOM  Profile store
        /// </summary>
        internal void WriteProfile()
        {
            using (Profile driverProfile = new Profile())
            {
                driverProfile.DeviceType = "Focuser";
                driverProfile.WriteValue(driverID, traceStateProfileName, tl.Enabled.ToString());

                driverProfile.WriteValue(driverID, comPortProfileName, comPort.ToString());
                driverProfile.WriteValue(driverID, "BaudRate", baudRate.ToString());

                driverProfile.WriteValue(driverID, "LowPower", m_LowPower.ToString());

                //driverProfile.WriteValue(driverID, "MotorReverse", m_MotorReverse.ToString());
                //driverProfile.WriteValue(driverID, "MotorMicrostep", m_MotorMicrostep.ToString());
                //driverProfile.WriteValue(driverID, "MotorSpeed", m_MotorSpeed.ToString());

                driverProfile.WriteValue(driverID, "MaxStep", m_MaxStep.ToString());
                driverProfile.WriteValue(driverID, "StepSize", m_StepSize.ToString());
                driverProfile.WriteValue(driverID, "MaxIncrement", m_MaxIncrement.ToString());

                driverProfile.WriteValue(driverID, "PC2Focuser", PC2Focuser==true?"1":"0");                
            }
        }
        int ReadPosition()
        {
            using (Profile driverProfile = new Profile())
            {
                driverProfile.DeviceType = "Focuser";
                return Convert.ToInt32(driverProfile.GetValue(driverID, "StorePosition", string.Empty, "0"));
            }
        }
        void WritePosition(int mpos)
        {
            using (Profile driverProfile = new Profile())
            {
                driverProfile.DeviceType = "Focuser";
                driverProfile.WriteValue(driverID, "StorePosition", mpos.ToString());
            }
        }
        /// <summary>
        /// Log helper function that takes formatted strings and arguments
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        internal static void LogMessage(string identifier, string message, params object[] args)
        {
            var msg = string.Format(message, args);
            tl.LogMessage(identifier, msg);
        }
        #endregion

        #region User Properties and methods

        //////////////graycode//////////////

        public void UartInitial()
        {  
            tl.LogMessage("Connected Set", "Connecting to port " + comPort);
            MySCP.ComPort=comPort;
            MySCP.UartInitial();
            if (MySCP.Connected)
            {
                if (MySCP.CheckDevice())
                {
                    connectedState = true;
                    MySCP.PC2Focuser = PC2Focuser;//将存储模式参数传给SCP
                    if (PC2Focuser)
                    {
                        MySCP.Position = ReadPosition();//将PC存储的位置赋值给SCP
                        m_Position = MySCP.Position;//将PC存储的位置作为当前位置
                        MySCP.GetInitState();//查询初始化状态,MySCP里面会自动初始化
                    } 
                }
                else
                {
                    UartUninitial();
                    MessageBox.Show("No device detected, please check connection!");
                }
            }
            else//这里不用UartUninitial();初始化未成功会自动关闭
            {
                MessageBox.Show("No device detected, please check connection!");
            }
        }
        public void UartUninitial()
        {
            connectedState = false;
            try
            {
                if (MySCP != null)
                    MySCP.UartUninitial();
            }
            catch 
            {
                ;
            }
        }
        // 只能输入数字（含负号小数点）
        public static void OnlyEnterNumber(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8 && e.KeyChar != 13 && e.KeyChar != 45 && e.KeyChar != 46)
            {
                e.Handled = true;
            }

            //输入为负号时，只能输入一次且只能输入一次
            if (e.KeyChar == 45 && (((TextBox)sender).SelectionStart != 0 || ((TextBox)sender).Text.IndexOf("-") >= 0)) e.Handled = true;
            if (e.KeyChar == 46 && ((TextBox)sender).Text.IndexOf(".") >= 0) e.Handled = true;
        }
        // 只能输入正实数
        public static void OnlyEnterPlusNumber(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8 && e.KeyChar != 13 && e.KeyChar != 46)
            {
                e.Handled = true;
            }
            if (e.KeyChar == 46 && ((TextBox)sender).Text.IndexOf(".") >= 0) e.Handled = true;
        }
        // 只能输入整数
        public static void OnlyEnterInt(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8 && e.KeyChar != 13 && e.KeyChar != 45)
            {
                e.Handled = true;
            }

            //输入为负号时，只能输入一次且只能输入一次
            if (e.KeyChar == 45 && (((TextBox)sender).SelectionStart != 0 || ((TextBox)sender).Text.IndexOf("-") >= 0)) e.Handled = true;
        }
        // 只能输入正整数
        public static void OnlyEnterPlusInt(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8 && e.KeyChar != 13)
            {
                e.Handled = true;
            }
        }
        //存储配置数据，4种重载
        public void WriteValue(string key, string value)
        {
            using (Profile driverProfile = new Profile())
            {
                driverProfile.DeviceType = "Focuser";
                driverProfile.WriteValue(driverID, key, value);
            }
        }

        public void WriteValue(string key, int value)
        {
            WriteValue(key, value.ToString());
        }

        public void WriteValue(string key, double value)
        {
            WriteValue(key, value.ToString());
        }

        public void WriteValue(string key, bool value)
        {
            //this.WriteValue(key, ((value == true) ? 1 : 0).ToString());
            WriteValue(key, ((value == true) ? 1 : 0).ToString());
        }
        //读取配置数据，4种重载，无数据返回默认值
        public string ReadValue(string key, string defaultvalue)
        {
            using (Profile driverProfile = new Profile())
            {
                driverProfile.DeviceType = "Focuser";
                return driverProfile.GetValue(driverID, key, string.Empty, defaultvalue);
            }
        }

        public int ReadValue(string key, int defaultvalue)
        {
            return Int32.Parse(ReadValue(key, defaultvalue.ToString()));
        }

        public double ReadValue(string key, double defaultvalue)
        {
            return Double.Parse(ReadValue(key, defaultvalue.ToString()));
        }

        public bool ReadValue(string key, bool defaultvalue)
        {
            return ReadValue(key, Convert.ToInt32(defaultvalue)) == 0 ? false : true;
        }
        
        #endregion
    }
}
