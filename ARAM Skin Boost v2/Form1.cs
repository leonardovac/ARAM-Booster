using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Management;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace ARAM_Skin_Boost_v2
{
    public partial class Form1 : Form
	{
		//Start of form
		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			
		}

		// Bordeless movable form
		public const int WM_NCLBUTTONDOWN = 0xA1;
		public const int HT_CAPTION = 0x2;
		[DllImportAttribute("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
		[DllImportAttribute("user32.dll")]
		public static extern bool ReleaseCapture();
		private void Form1_MouseDown(object sender, MouseEventArgs e)
		{
			ReleaseCapture();
			SendMessage(this.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
		}

		//GitHubs's button hotlink
		private void pictureBox2_MouseMove(object sender, EventArgs e)
		{
			pictureBox2.Image = Properties.Resources.GitHub_Logo2;
		}

		private void pictureBox2_MouseLeave(object sender, EventArgs e)
		{
			pictureBox2.Image = Properties.Resources.GitHub_Logo;
		}

		private void pictureBox2_Click(object sender, EventArgs e)
		{
           System.Diagnostics.Process.Start("https://github.com/leonardovac/ARAM-Skin-Booster");
        }

		// Close, help and minimize buttons
		private void label1_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void label1_MouseMove(object sender, MouseEventArgs e)
		{
			label1.ForeColor = System.Drawing.ColorTranslator.FromHtml("#b73434");
		}

		private void label1_MouseLeave(object sender, EventArgs e)
		{
            label1.ForeColor = System.Drawing.ColorTranslator.FromHtml("#4a5c67");
		}
		private void label8_Click(object sender, EventArgs e)
		{
			string message = "- Original code by matix2 (UC);\n\nCheck his GitHub.";
			string caption = "Kudos to matix2";
			MessageBoxButtons buttons = MessageBoxButtons.OK;
			DialogResult result;

			result = MessageBox.Show(message, caption, buttons);
			if (result == System.Windows.Forms.DialogResult.OK)
			{
				System.Diagnostics.Process.Start("https://github.com/matix2/aram-skin-booster");
			}
		}

		private void label8_MouseMove(object sender, MouseEventArgs e)
		{
			label8.ForeColor = System.Drawing.ColorTranslator.FromHtml("#009486");
		}

		private void label8_MouseLeave(object sender, EventArgs e)
		{
			label8.ForeColor = System.Drawing.ColorTranslator.FromHtml("#4a5c67");
		}

		private void label9_Click(object sender, EventArgs e)
		{
			WindowState = FormWindowState.Minimized;
		}

		private void label9_MouseMove(object sender, MouseEventArgs e)
		{
			label9.ForeColor = System.Drawing.ColorTranslator.FromHtml("#009486");
		}

		private void label9_MouseLeave(object sender, EventArgs e)
		{
			label9.ForeColor = System.Drawing.ColorTranslator.FromHtml("#4a5c67");
		}

		// Button color change
		private void pictureBox1_MouseLeave(object sender, EventArgs e)
		{
			pictureBox1.Image = Properties.Resources.btn1;
		}

		private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
		{
			pictureBox1.Image = Properties.Resources.btn2;
		}

		private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
		{
			pictureBox1.Image = Properties.Resources.btn3;
		}

		//End of form

		private void pictureBox1_Click(object sender, EventArgs e)
		{
			try
			{
				ValueTuple<string, string> valueTuple = GetInfo();
				PostRequest(valueTuple.Item1, valueTuple.Item2);
				Thread.Sleep(250);
			}
			catch (Exception)
			{
				string message = "Something went wrong...";
				string caption = "Error";
				MessageBoxButtons buttons = MessageBoxButtons.OK;
				DialogResult result;

				result = MessageBox.Show(message, caption, buttons);
				if (result == System.Windows.Forms.DialogResult.OK)
				{
					Application.Exit();
				}
			}
		}

		static ValueTuple<string, string> GetInfo()
		{
			string text = "";
			string text2 = "";
			ManagementClass managementClass = new ManagementClass("Win32_Process");
			foreach (ManagementBaseObject managementBaseObject in managementClass.GetInstances())
			{
				ManagementObject managementObject = (ManagementObject)managementBaseObject;
				if (managementObject["Name"].Equals("LeagueClientUx.exe"))
				{
					foreach (object obj in Regex.Matches(managementObject["CommandLine"].ToString(), string_0, regexOptions_0))
					{
						Match match = (Match)obj;
						if (!string.IsNullOrEmpty(match.Groups["port"].ToString()))
						{
							text2 = match.Groups["port"].ToString();
						}
						else if (!string.IsNullOrEmpty(match.Groups["token"].ToString()))
						{
							text = match.Groups["token"].ToString();
						}
					}
				}
			}
			if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(text2))
			{
				string message = "Is your client running?";
				string caption = "LeagueClientUx.exe not found!";
				MessageBoxButtons buttons = MessageBoxButtons.YesNo;
				DialogResult result;

				result = MessageBox.Show(message, caption, buttons);
				if (result == System.Windows.Forms.DialogResult.Yes)
				{
                    Application.Exit();
				}
			}
			return new ValueTuple<string, string>(text, text2);
		}
		
		static void PostRequest(string string_1, string string_2)
		{
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
			ServicePointManager.ServerCertificateValidationCallback = (RemoteCertificateValidationCallback)Delegate.Combine(ServicePointManager.ServerCertificateValidationCallback, new RemoteCertificateValidationCallback(Class1.NewClass.Main));
			RestClient restClient = new RestClient("https://127.0.0.1:" + string_2)
			{
				Authenticator = new HttpBasicAuthenticator("riot", string_1)
			};
			RestRequest request = new RestRequest("/lol-champ-select/v1/team-boost/purchase", Method.POST); // This is everything you need to know, these 200 lines of code could be reduced to 20.
			restClient.Execute(request);
		}

		static readonly string string_0 = "\"--remoting-auth-token=(?'token'.*?)\" | \"--app-port=(?'port'|.*?)\"";
        static readonly RegexOptions regexOptions_0 = RegexOptions.Multiline;
    }
	public class Class1
	{
		internal bool Main(object object_0, X509Certificate x509Certificate_0, X509Chain x509Chain_0, SslPolicyErrors sslPolicyErrors_0)
		{
			return true;
		}

        public static RemoteCertificateValidationCallback Callback { get; }
        public static Class1 NewClass { get; } = new Class1();
    }
}
