using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormDemo
{
    public partial class PingForm : Form
    {
        public PingForm()
        {
            InitializeComponent();
        }

        private void btnPing_Click(object sender, EventArgs e)
        {
            listBoxResult.Items.Clear();

            string ip = txtIP.Text.ToString().Trim();

            Ping ping = new Ping();

            PingOptions options = new PingOptions();
            options.DontFragment = true;

            string data = "send data";
            byte[] buffer = Encoding.ASCII.GetBytes(data);

            int timeout = 120;

            PingReply reply = ping.Send(ip, timeout, buffer, options);
            if (reply.Status == IPStatus.Success)
            {
                listBoxResult.Items.Add("响应的主机地址：" + reply.Address.ToString());
                listBoxResult.Items.Add("响应时间：" + reply.RoundtripTime);
                listBoxResult.Items.Add("生命周期(TTL)：" + reply.Options.Ttl);
                listBoxResult.Items.Add("是否控制数据包的分段：" + reply.Options.DontFragment);
                listBoxResult.Items.Add("缓冲区大小：" + reply.Buffer.Length);
            }
            else
            {
                listBoxResult.Items.Add(reply.Status.ToString());
            }

        }
    }
}
