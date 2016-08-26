using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormDemo
{
    public partial class EncoderDecoder : Form
    {
        public EncoderDecoder()
        {
            InitializeComponent();

            txtContent.Text = "新苗向日葵饼干";
        }

        private void btnEncDec_Click(object sender, EventArgs e)
        {
            //编码  
            string codeType = cbEncodeType.SelectedItem.ToString();

            //获得编码类型 默认选择（gb2312）  
            codeType = codeType.Substring(0, codeType.IndexOf('['));

            //获得一个 gb2312 编码类型的编码器  
            Encoder encoder = Encoding.GetEncoding(codeType).GetEncoder();

            //将字符串转换为一组char数组  
            char[] chars = txtContent.Text.ToCharArray();

            //声明一个长度为‘编码为byte后产生的字节数’  
            byte[] bytes = new byte[encoder.GetByteCount(chars, 0, chars.Length, true)];

            //进行编码，将chars数组中的字符编码到byte数组中  
            encoder.GetBytes(chars, 0, chars.Length, bytes, 0, true);

            //将 8 位无符号整数数组的值转换为其用 Base64 数字编码的等效字符串 显示到控件中。  
            txtEncoder.Text = Convert.ToBase64String(bytes);

            //解码  
            //获得编码类型为 gb2312 的解码器  
            Decoder decoder = Encoding.GetEncoding(codeType).GetDecoder();

            //进行解码，将byte数组中的8位无符号整数转换为 char字符  
            int charLen = decoder.GetChars(bytes, 0, bytes.Length, chars, 0);   
            String strResult = "";
            foreach (char c in chars)
            {
                strResult += c.ToString();
            }
            txtDecoder.Text = strResult;
        }

        private void EncoderDecoder_Load(object sender, EventArgs e)
        {
            foreach (EncodingInfo item in Encoding.GetEncodings())
            {
                var ec = item.GetEncoding();
                cbEncodeType.Items.Add(string.Format("{0}[{1}]", ec.HeaderName, ec.EncodingName));
            }
            cbEncodeType.SelectedIndex = cbEncodeType.FindString("gb2312");
        }

    }
}
