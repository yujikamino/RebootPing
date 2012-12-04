using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FTD2XX_NET;

namespace RebootPing
{
    class Myftdi
    {

        private FTDI m_ftdiDevice = new FTDI();
        private FTDI.FT_DEVICE_INFO_NODE[] m_ftdiDeviceList = null;


        public void ftdiDeviceEnumulate()
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



    }
}
