using System;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;

namespace AutoLogIn_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!CheckForInternetConnection() && CheckForServerConnection())
            {
                string str = LoginAndDownloadData("put_the_user_id", "put_the_user_pass");
                System.Console.WriteLine(str);
                System.Console.ReadLine();
            }

        }

        private static string LoginAndDownloadData(string auth_user, string auth_pass)
        {
            string zone = "esties", redirurl = "";
            string pBody = $"auth_user={auth_user}&auth_pass={auth_pass}&zone={zone}&redirurl={redirurl}";
            var loginAddress = "https://195.251.164.129";//Student residence of Samos Aegen University ip server for login to get internet access.
            var cookies = new CookieContainer();
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(loginAddress);
            req.CookieContainer = cookies;
            req.AllowAutoRedirect = true;
            req.ContentLength = pBody.Length;
            req.ContentType = "application/x-www-form-urlencoded";
            req.Method = "POST";
            System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };
            var stream = req.GetRequestStream();
            stream.Write(Encoding.UTF8.GetBytes(pBody), 0,
                pBody.Length);

            var resp = (HttpWebResponse)req.GetResponse();

            using (StreamReader sr = new StreamReader(resp.GetResponseStream(), Encoding.Default))
            {
                return sr.ReadToEnd();
            }

        }

        public static bool CheckForInternetConnection()
        {

            try
            {
                Ping myPing = new Ping();
                String host = "www.google.com";
                byte[] buffer = new byte[32];
                int timeout = 1000;
                PingOptions pingOptions = new PingOptions();
                PingReply reply = myPing.Send(host, timeout, buffer, pingOptions);
                return (reply.Status == IPStatus.Success);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool CheckForServerConnection()
        {

            try
            {
                Ping myPing = new Ping();
                String host = "10.10.11.254";//Student residence of Samos Aegen University Local Server
                byte[] buffer = new byte[32];
                int timeout = 1000;
                PingOptions pingOptions = new PingOptions();
                PingReply reply = myPing.Send(host, timeout, buffer, pingOptions);
                return (reply.Status == IPStatus.Success);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
