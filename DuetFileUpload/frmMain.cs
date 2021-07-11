using System;
using System.Text;
using System.Threading;
using System.Windows.Forms;

//Request library
using System.Net;
using System.IO;
using System.Net.Http;

//CamBam API
using CamBam;
using CamBam.UI;
using CamBam.CAM;
using CamBam.CAD;
using CamBam.Geom;
using CamBam.Library;


// Duet Http Requests
// https://github.com/Duet3D/RepRapFirmware/wiki/HTTP-requests
//
//http://alphaCNC/rr_connect?password=reprap	//NB: password=reprap is needed
//{"err":0,"sessionTimeout":8000,"boardType":"duetethernet102","apiLevel":1}
//
//http://alphaCNC/rr_status
//{"status":"O","heaters":[0.0],"active":[0.0],"standby":[0.0],"hstat":[0],"pos":[0.000,10.000,80.000],"machine":[0.000,10.000,80.000],"sfactor":100.0,"efactor":[],"babystep":0.000,"tool":0,"probe":"0","fanPercent":[0.0,0,0,0,-1,-1,-1,-1,-1,-1,-1,-1,-1],"fanRPM":[0,-1,-1],"homed":[0,0,0],"msgBox.mode":-1}
//
//http://alphaCNC/rr_config
//{"axisMins":[-102.00,-88.00,-10.00],"axisMaxes":[102.00,85.00,80.00],"accelerations":[200.00,200.00,20.00,500.00,500.00,500.00,500.00,500.00,500.00,500.00,250.00,250.00],"currents":[1200,1200,1200,0,0,0,0,0,0,0,0,0],"firmwareElectronics":"Duet Ethernet 1.02 or later","firmwareName":"RepRapFirmware for Duet 2 WiFi/Ethernet","firmwareVersion":"3.3RC3","firmwareDate":"2021-05-26", "sysdir":"0:/sys/","idleCurrentFactor":30.0,"idleTimeout":30.0,"minFeedrates":[15.00,15.00,0.20,15.00,15.00,15.00,15.00,15.00,15.00,15.00,2.00,2.00],"maxFeedrates":[40.00,40.00,4.00,100.00,100.00,100.00,100.00,100.00,100.00,100.00,20.00,20.00]}
//
//http://alphaCNC/rr_filelist?dir=gcodes
//{"dir":"gcodes","first":0,"files":[{ "type":"d","name":"Calibration","size":0,"date":"2021-05-15T11:27:06"},{ "type":"f","name":"SquareDiamondCircle 42mm-3mm sheet.nc","size":42714,"date":"2021-06-12T16:36:10"},{ "type":"f","name":"HV Connector.nc","size":6179,"date":"2021-01-29T14:47:00"},{ "type":"f","name":"SquareDiamondCircle 42mm.nc","size":31198,"date":"2021-06-12T16:16:20"},{ "type":"f","name":"gtest.g","size":4,"date":"2021-07-07T21:11:26"},{ "type":"f","name":"LS-25F-BasePlate_PB_inc_Tabs.nc","size":8927,"date":"2021-06-18T18:07:22"},{ "type":"f","name":"LS-25F-TopPlate_PB_inc_Tabs.nc","size":12078,"date":"2021-06-18T18:04:54"},{ "type":"f","name":"LS-25F-BasePlate_PB_inc_Tabs_countersinks.nc","size":15150,"date":"2021-06-20T12:08:02"}],"next":0}


namespace CamBamPlugIn
{

	public partial class frmMain : Form
	{
		
		private Uri DuetURL;
		private string GcodeFilename;

		public frmMain()
		{
			InitializeComponent();
		}

		private void frmMain_Load(object sender, EventArgs e)
		{
			try
			{
				CADFile project = CamBamUI.MainUI.CADFileTree.CADFile;
				ICADView view = CamBamUI.MainUI.ActiveView;

				//If a current project file doesn't exist or hasn't been saved then warn the user and exit.
				if (project.Filename == null)
				{
					MessageBox.Show("Please save the project first", "CamBam");
					this.Close();
				}

				//Use CamBam to generate a G-code file for the current view	and update the filename textbox							
				CAMUtils.GenerateGCodeOutput(view);
				do
				{
					Application.DoEvents();
				} while (view.IsThinking);
				GcodeFilename = FileUtils.GetFullPath(project.MachiningOptions.CADFile, project.MachiningOptions.OutFile);
				txtFilename.Text = Path.GetFileName(GcodeFilename);

				//Get the stored URI for the Duet board and update the URI textbox if valid
				bool uriValid = Uri.TryCreate(Properties.Settings.Default.DuetURL, UriKind.Absolute, out DuetURL) 
								&& DuetURL.Scheme == Uri.UriSchemeHttp;
				if (uriValid)
				{
					txtURL.Text = DuetURL.ToString();
				}
				else
				{
					txtURL.Text = "http:\\\\";
				}
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message,"Error [Form Load]",MessageBoxButtons.OK,MessageBoxIcon.Warning);
				this.Close();
			}
		}

		//Set the focus to the status text box
		private void frmMain_Shown(object sender, EventArgs e)
		{
			txtStatus.Focus();
		}

		//User event: file upload
		private void btnUploadFile_Click(object sender, EventArgs e)
		{
			try
			{
				//Get the filename to be saved on the Duet
				string DuetFilename = "gcodes/" + txtFilename.Text;

				//Get the file content
				string Gcode;
				using (StreamReader reader = new StreamReader(GcodeFilename))
				{
					Gcode = reader.ReadToEnd();
				}

				//Check the Duet URL entered by the user is valid
				bool uriValid = Uri.TryCreate(txtURL.Text, UriKind.Absolute, out DuetURL)
								&& DuetURL.Scheme == Uri.UriSchemeHttp;				
				if (uriValid)
				{
					Properties.Settings.Default.DuetURL = DuetURL.AbsoluteUri;
					Properties.Settings.Default.Save();
				}
				else
				{
					MessageBox.Show("Duet URL is not valid.", "Error [Upload]", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return;
				}

				//Try connecting to the Duet
				txtStatus.AppendText($"Connecting to {DuetURL.AbsoluteUri}...\r\n");
				HttpClient client = new HttpClient();				
				int ires = connect(client, DuetURL.AbsoluteUri);

				//If successful, try uploading the file
				if (ires == 0)
				{
					txtStatus.AppendText($"Uploading file {DuetFilename}...\t");
					if (uploadFile(client, DuetFilename, Gcode) == 0)
					{
						txtStatus.AppendText("Success!\r\n");
					}
					else
					{
						txtStatus.AppendText("Failed.\r\n");
					}

					//Disconnect
					txtStatus.AppendText($"Disconnecting...\r\n");
					disconnect(client);
				}
				else
				{
					txtStatus.AppendText("connect error!");
				}

				//Close the Client
				client.Dispose();
								
				//Wait then close the plugin
				Thread.Sleep(2000);	
				this.Close();
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message, "Error [Upload]", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				this.Close();
			}

		}


		//Connect to the Duet at the specified URL
		public int connect(HttpClient client, string URL)
		{
			try
			{
				//Initialise the Http Client
				if (client == null)
				{
					HttpClientHandler handler = new HttpClientHandler()
					{
						AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
					};
					client = new HttpClient(handler);
				}

				//Set the base address
				client.BaseAddress = new Uri(URL);

				//Try to connect
				var Result = client.GetStringAsync("rr_connect?password=reprap").Result;
				if (Result.ToString().Contains("\"err\":0"))
					return 0;
				else
					return 1;
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error [Connect]: {ex.Message}");
				return ex.HResult;
			}

		}

		//Disconnect from the Duet
		public int disconnect(HttpClient client)
		{
			//Disconnect from the Duet
			var Result = client.GetStringAsync("rr_disconnect").Result;
			if (Result.ToString().Contains("OK"))
				return 0;
			else
				return 1;

		}

		//upload a file to the Duet
		public int uploadFile(HttpClient client, string Filename, string Content)
		{
			try
			{
				var content = new StringContent(Content, Encoding.UTF8, "application/text");
				string post = $"rr_upload?Name={Filename}";
				var Result = client.PostAsync(post,content).Result;
				if (Result.ToString().Contains("OK"))
					return 0;
				else
					return 1;
				
			}
			catch (HttpRequestException e)
			{
				MessageBox.Show($"Error [POST]: \r\n{e.Message}");
				return e.HResult;
			}
		}

	}
}

