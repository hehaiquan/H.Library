using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class QRCode : Form
    {
        public QRCode()
        {
            InitializeComponent();


            var ms = new MemoryStream();
            GetQRCode("http://news.qq.com/a/20160725/041905.htm", ms, 4);
            System.Drawing.Image bottomImage = System.Drawing.Image.FromStream(ms);
            pictureBox1.Image = bottomImage;
        }


        /// <summary>
        /// 获取二维码
        /// </summary>
        /// <param name="strContent">待编码的字符</param>
        /// <param name="ms">输出流</param>
        /// <param name="ModuleSize">字体大小</param>
        ///<returns>True if the encoding succeeded, false if the content is empty or too large to fit in a QR code</returns>
        public static bool GetQRCode(string strContent, MemoryStream ms, int ModuleSize = 12)
        {

            ErrorCorrectionLevel Ecl = ErrorCorrectionLevel.M; //误差校正水平,越高,二维码的有效像素点就越多
            QuietZoneModules QuietZones = QuietZoneModules.Four;  //空白区域 
            var encoder = new QrEncoder(Ecl);

            QrCode qr;
            if (encoder.TryEncode(strContent, out qr))//对内容进行编码，并保存生成的矩阵
            {
                var render = new GraphicsRenderer(new FixedModuleSize(ModuleSize, QuietZones));
                render.WriteToStream(qr.Matrix, ImageFormat.Png, ms);
            }
            else
            {
                return false;
            }
            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            var ms = new MemoryStream();
            GetQRCode(txtContent.Text, ms,int.Parse(txtSize.Text));
            System.Drawing.Image bottomImage = System.Drawing.Image.FromStream(ms);
            pictureBox1.Image = bottomImage;
        }
    }
}
