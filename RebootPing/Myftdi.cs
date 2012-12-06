using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FTD2XX_NET;


namespace RebootPing
{
    class Myftdi
    {
        public UInt32 m_ftdiDeviceCount;
        public FTDI m_ftdiDevice = new FTDI();
        public FTDI.FT_DEVICE_INFO_NODE[] m_ftdiDeviceList = null;
        public int[] m_ftdiDeviceMap = null;

        private System.IO.Stream logStream = null;
        private System.IO.StreamWriter logWriter = null;


        //ftdiデバイスを読み込んで、コンボボックスに挿入
        //初期読み込みに利用
        internal void ftdiDeviceEnumulate(System.Windows.Forms.ComboBox ftdiDeviceList)
        {
            ftdiDeviceList.Items.Clear();

            FTDI.FT_STATUS ftStatus = m_ftdiDevice.GetNumberOfDevices(ref m_ftdiDeviceCount);
            if (ftStatus != FTDI.FT_STATUS.FT_OK)
            {
                return;
            }
            if (m_ftdiDeviceCount == 0)
            {
                return;
            }

            m_ftdiDeviceMap = new int[m_ftdiDeviceCount];
            for (int i = 0; i < m_ftdiDeviceCount; i++)
            {
                m_ftdiDeviceMap[i] = -1;
            }
            m_ftdiDeviceList = new FTDI.FT_DEVICE_INFO_NODE[m_ftdiDeviceCount];
            ftStatus = m_ftdiDevice.GetDeviceList(m_ftdiDeviceList);
            if (ftStatus == FTDI.FT_STATUS.FT_OK)
            {
                for (int i = 0; i < m_ftdiDeviceCount; i++)
                {
                    if (m_ftdiDeviceList[i].Type == FTDI.FT_DEVICE.FT_DEVICE_2232)
                    {
                        m_ftdiDeviceMap[ftdiDeviceList.Items.Count] = i;
                        string devName = string.Format("{0}: {1} ({2})", i, m_ftdiDeviceList[i].Description.ToString(), m_ftdiDeviceList[i].SerialNumber.ToString());
                        ftdiDeviceList.Items.Add(devName);
                    }
                }
                ftdiDeviceList.SelectedIndex = 0;
            }
        }

        //デバイスの初期化 リレーをOFFにします。
        public bool powerOff()
        {
            if (m_ftdiDevice.IsOpen)
            {
                byte[] bits = { 0 };
                uint len = 0;
                m_ftdiDevice.Write(bits, 1, ref len);
                log("Power OFF");
                return true;
            }
            else
            {
                return false;
            }
        }

        //リレーをONにします。
        public bool powerOn()
        {
            if (m_ftdiDevice.IsOpen)
            {
                byte[] bits = { 0 };
                bits[0] |= 0x04;
                bits[0] |= 0x10;
                uint len = 0;
                m_ftdiDevice.Write(bits, 1, ref len);
                log("Power ON");
                return true;
            }
            else
            {
                return false;
            }
        }
        //string型の前に、現在の時間を表示させます
        public void log(string msg)
        {
            if (logWriter != null)
            {
                try
                {
                    logWriter.Write(DateTime.Now.ToString() + " : " + msg + "\r\n");
                    logWriter.Flush();
                }
                catch
                {
                }
            }
        }
        //logを閉じます
        public void logClose()
        {
            if (logWriter != null)
            {
                logWriter.Close();
            }
            if (logStream != null)
            {
                logStream.Close();
            }
        }
    }
}
