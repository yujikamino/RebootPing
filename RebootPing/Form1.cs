using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FTD2XX_NET;

namespace RebootPing
{
    public partial class Form1 : Form
    {
        Myftdi ftdi = new Myftdi();

        public Form1()
        {
            InitializeComponent();
            ftdi.ftdiDeviceEnumulate(ftdiDeviceList);

        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            if (ftdi.m_ftdiDeviceCount == 0)
            {
                return;
            }

            ftdi.ftdiRelayInit(ftdiDeviceList);

            ftdi.powerOff();
            System.Threading.Thread.Sleep(1000);
            ftdi.powerOn();

            ftdi.m_ftdiDevice.Close();
        }

        private void relayOnButton_Click(object sender, EventArgs e)
        {
            if (ftdi.m_ftdiDeviceCount == 0)
            {
                return;
            }
            ftdi.ftdiRelayInit(ftdiDeviceList);
            ftdi.powerOn();

            ftdi.m_ftdiDevice.Close();

        }

        private void relayOffButton_Click(object sender, EventArgs e)
        {
            if (ftdi.m_ftdiDeviceCount == 0)
            {
                return;
            }
            ftdi.ftdiRelayInit(ftdiDeviceList);

            ftdi.powerOff();

            ftdi.m_ftdiDevice.Close();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            ftdi.logClose();
        }
    }
}
