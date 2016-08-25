using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace LRXTOOL
{
     

    public class HTTPSend
    {
       
        /// <summary>
        /// 模拟HTTP发送数据（有参数）
        /// </summary>
        /// <param name="URL">传输地址</param>
        /// <param name="postData">传输的参数</param>
        /// <param name="Method">传输方式（GET/POST）</param>
        /// <returns></returns>
        public static string HttpYesParam(string URL, string postData, string Method)
        {
            string content = string.Empty;
            try
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(URL));
                webReq.Method = Method;
                webReq.ContentType = "application/x-www-form-urlencoded";
                webReq.ContentLength = byteArray.Length;

                Stream newStream = webReq.GetRequestStream();
                newStream.Write(byteArray, 0, byteArray.Length);
                newStream.Close();

                HttpWebResponse reponse = (HttpWebResponse)webReq.GetResponse();
                StreamReader sr = new StreamReader(reponse.GetResponseStream(), Encoding.Default);
                content = sr.ReadToEnd();

                sr.Close();
                reponse.Close();
                newStream.Close();
            }
            catch (Exception ex)
            {
                content = ex.ToString();
            }
            return content;
        }
        /// <summary>
        /// 模拟HTTP发送数据（无参数）
        /// </summary>
        /// <param name="URL">传输地址</param>
        /// <param name="Method">传输方式（GET/POST）</param>
        /// <returns></returns>
        public static string HttpNoParam(string URL, string Method)
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(RemoteCertificateValidationCallback);

            string content = "";
            try
            {
                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(URL);
                req.Method = Method;
                using (WebResponse wr = req.GetResponse())
                {
                    HttpWebResponse myResponse = (HttpWebResponse)req.GetResponse();
                    StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
                    content = reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {

            }
            return content;
        }

        public static bool RemoteCertificateValidationCallback(Object sender,X509Certificate certificate,X509Chain chain,System.Net.Security.SslPolicyErrors sslPolicyErrors
)
        {
            //Return True to force the certificate to be accepted.
            return true;
        }
    }
}
