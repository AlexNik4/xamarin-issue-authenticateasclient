using System;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;

namespace SharedLib
{
	public class ConnectionMgr
	{
		private readonly string Hostname = "192.168.0.5";
		private const int Port = 5544;

		public ConnectionMgr(string ip = null)
		{
			if (!String.IsNullOrEmpty(ip))
			{
				Hostname = ip;
			}
		}

		public void TestConnect()
		{
			var tcpClient = new TcpClient();
			IAsyncResult result = tcpClient.BeginConnect(Hostname, Port, null, null);
			var connected = result.AsyncWaitHandle.WaitOne(2000, true);
			if (connected)
			{
				tcpClient.EndConnect(result);
				var stream = new SslStream(tcpClient.GetStream(), false, ValidateServerCertificate, null);
				try
				{
					stream.AuthenticateAsClient(Hostname);

					Console.WriteLine("Xamarin.Android Correctly Executes Here");
				}
				catch (Exception ex)
				{
					Console.WriteLine(".NET6.0-Android Throws Exception Here");
					Console.WriteLine(ex.ToString());
				}
			}
		}

		private bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
			Console.WriteLine("Policy Errors: " + sslPolicyErrors.ToString());
			// Don't care about actually validating server cert
			return true;
		}
	}
}
