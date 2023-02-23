using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

Console.WriteLine("Enter Root CA Name");
string? certName = Console.ReadLine();
await ServerMgr.StartServerAsync(certName);

public class ServerMgr
{
	public static async Task StartServerAsync(string certFriendlyName = "")
	{
		TcpListener listener = new TcpListener(IPAddress.Any, 5544);
		listener.Start();

		while (true)
		{
			Console.WriteLine("Waiting connect");
			TcpClient client = await listener.AcceptTcpClientAsync();
			await ProcessClientAsync(client, certFriendlyName);
		}
	}

	public static async Task ProcessClientAsync(TcpClient client, string certFriendlyName = "")
	{
		try
		{
			// Create an SslStream to decrypt the network connection
			SslStream sslStream = new SslStream(client.GetStream(), false, ValidateClientCertificate);

			// Authenticate the client connection using the server certificate
			X509Store store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
			store.Open(OpenFlags.ReadOnly);
			X509Certificate2Collection certs = store.Certificates.Find(X509FindType.FindBySubjectName, certFriendlyName, true);

			X509Certificate2 certificate = certs[0];
			// Use the certificate to authenticate the client connection

			await sslStream.AuthenticateAsServerAsync(certificate, clientCertificateRequired: false, enabledSslProtocols: SslProtocols.Tls12, checkCertificateRevocation: true);
			Console.WriteLine("Connected");

			Console.ReadLine();
		}
		catch (Exception ex)
		{
			Console.WriteLine("Exception encountered while processing client: " + ex.ToString());
		}
	}

	public static bool ValidateClientCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
	{
		Console.WriteLine("Policy Errors: " + sslPolicyErrors.ToString());
		// Don't care about actually validating client cert
		return true;
	}
}