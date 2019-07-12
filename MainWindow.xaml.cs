using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TextReplacer
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		List<string> files = new List<string>();
		RadioButton rb;
		private void FilesDrop(object sender, DragEventArgs e)
		{
			var dropped = ((string[])e.Data.GetData(DataFormats.FileDrop));

			var dropped_files = dropped.ToList();

			FileAttributes attr = File.GetAttributes(dropped_files[0]);
			attr.HasFlag(FileAttributes.Directory);
			if (attr.HasFlag(FileAttributes.Directory))
			{
				//FileDump.Text += dropped_files[0];
				dropped_files = Directory.GetFiles(dropped_files[0]).ToList();
			}
			InputFiles(dropped_files);
		}
		private void InputFiles(List<string> input_files)
		{
			files.Clear();
			input_files.ForEach(f =>
			{
				files.Add(f);
				if (FileDump.Text != "")
				{
					FileDump.Text += "\n";
				}
				FileDump.Text += f;
			});

		}
		private void ClearFiles_Click(object sender, RoutedEventArgs e)
		{
			files = new List<string>();
			FileDump.Text = "";
			alertMessage.Text = "";
			FindText.Text = "";
			ReplaceWith.Text = "";
			alertMessage.Visibility = Visibility.Hidden;
		}
		public static Encoding GetFileEncoding(string srcFile)
		{
			// *** Use Default of Encoding.Default (Ansi CodePage)
			Encoding enc = Encoding.Default;
			// *** Detect byte order mark if any - otherwise assume default
			byte[] buffer = new byte[5];
			FileStream file = new FileStream(srcFile, FileMode.Open);
			file.Read(buffer, 0, 5);
			file.Close();
			if (buffer[0] == 0xef && buffer[1] == 0xbb && buffer[2] == 0xbf)
				enc = Encoding.UTF8;
			else if (buffer[0] == 0xfe && buffer[1] == 0xff)
				enc = Encoding.Unicode;
			else if (buffer[0] == 0 && buffer[1] == 0 && buffer[2] == 0xfe && buffer[3] == 0xff)
				enc = Encoding.UTF32;
			else if (buffer[0] == 0x2b && buffer[1] == 0x2f && buffer[2] == 0x76)
				enc = Encoding.UTF7;
			return enc;
		}
		private void Submit_Click(object sender, RoutedEventArgs e)
		{

			if (FindText.Text == "" || ReplaceWith.Text == "")
			{
				alertMessage.Text = "Input fields cannot be empty";
				alertMessage.Foreground = Brushes.Red;
				alertMessage.Visibility = Visibility.Visible;
				return;
			}

			switch (rb.Name)
			{
				case "P11D":
					GenerateP11DFiles();
					break;
				case "EDI":
					GenerateEDIFiles();
					break;
				case "ALL":
					GenerateP11DFiles();
					GenerateEDIFiles();
					break;
				case "OTHER":
					ContructFiles(true, FindText.Text, ReplaceWith.Text);
					break;
			}

		}
		private void GenerateEDIFiles()
		{
			try
			{
				List<string> paths = new List<string>();
				List<string> Input_files = new List<string>();

				//paths.Add(@"C:\" + CodePath.Text + @"\IPRLUK\IPUKMeta\Package\PostDeploy\anytime\30 Payroll (UK)\040 Views\Client\040 P11D\" + FindText.Text);
				//paths.Add(@"C:\" + CodePath.Text + @"\IPRLUK\IPUKMeta\Package\PostDeploy\anytime\30 Payroll (UK)\050 SP\AdpUKPayrollWebApi\P11D\" + FindText.Text);
				//paths.Add(@"C:\" + CodePath.Text + @"\IPRLUK\IPUKMeta\Package\PostDeploy\anytime\30 Payroll (UK)\050 SP\AdpIpUkP11D\" + FindText.Text);
				////paths.Add(@"C:\" + CodePath.Text + @"\IPRLUK\IPUKMeta\Package\PostDeploy\anytime\30 Payroll (UK)\040 Views\Client\040 P11D\" + FindText.Text);
				////050 SP\AdpUKPayrollWebApi\P11D

				////paths.ForEach(path =>
				////{
				////	Input_files = Directory.GetFiles(path).ToList();
				////});

				//foreach (var path in paths)
				//{
				//	Input_files.AddRange(Directory.GetFiles(path).ToList());
				//}

				var EPS_FindYear = FindText.Text.Substring(0,2);
				var EPS_ReplaceYear = ReplaceWith.Text.Substring(0, 2);

				//EPS
				Input_files.Add(@"C:\" + CodePath.Text + @"\IPRLUK\Source\dotNet\Adp.IpUk.EDI\Adp.IpUk.EDI.Interchange\Lines\LineEPS" + EPS_FindYear + ".cs");
				Input_files.Add(@"C:\" + CodePath.Text + @"\IPRLUK\Source\dotNet\Adp.IpUk.EDI\Adp.IpUk.EDI.Interchange\Messages\MessageEPS" + EPS_FindYear + ".cs");
				//Input_files.Add(@"C:\" + CodePath.Text + @"\IPRLUK\IPUKMeta\Package\PreDeploy\anytime\30 Payroll (UK)\040 Views\Client\040 P11D\vwP11D" + FindText.Text + "RESULTS.pre.sql");

				//Create folders for maintanace work

				//dropped_files.Add(@"C:\" + CodePath.Text + @"\IPRLUK\IPUKMeta\Package\PostDeploy\onetime\30 Payroll (UK)\DBO\_InsertDataInToP11DTradeOrgEntTypeTable" + FindText.Text + "CARSANDFUELRESULT.pre.sql");
				//dropped_files.Add(@"C:\" + CodePath.Text + @"\IPRLUK\IPUKMeta\Package\PostDeploy\anytime\30 Payroll (UK)\040 Views\Client\040 P11D\vwP11D" + FindText.Text + "EMPLOYEEDETAILS.pre.sql");
				//dropped_files.Add(@"C:\" + CodePath.Text + @"\IPRLUK\IPUKMeta\Package\PreDeploy\anytime\30 Payroll (UK)\040 Views\Client\040 P11D\vwP11D" + FindText.Text + "RESULTS.pre.sql");
				//dropped_files.Add(@"C:\" + CodePath.Text + @"\IPRLUK\IPUKMeta\Package\PreDeploy\anytime\30 Payroll (UK)\040 Views\Client\040 P11D\vwP11D" + FindText.Text + "RESULTS.pre.sql");
				//C:\ihcm.git\IPRLUK\IPUKMeta\Package\PostDeploy\onetime\30 Payroll(UK)\DBO\20181122.2_InsertDataInToP11DTradeOrgEntTypeTable2018.sql
				//C:\ihcm.git\IPRLUK\IPUKMeta\Package\PostDeploy\onetime\30 Payroll(UK)\DBO\20181122.1_Insert_rows_p11dexpensesmadetype_and_p11dothersubtype_2018.sql
				//C:\ihcm.git\IPRLUK\Source\Database\eForms\Upgrade\20180220_Insert_eForm_WS2b_TaxYear2017.sql
				//C:\ihcm.git\IPRLUK\Source\Database\eForms\Upgrade\20171221_Insert__IPSYS_EFORM_Forms_P11D_TaxYear2017.sql

				InputFiles(Input_files);
				ContructFiles(false, FindText.Text.Substring(2, 2), ReplaceWith.Text.Substring(2, 2));
			}
			catch (Exception ex)
			{
				alertMessage.Text = ex.ToString();
				alertMessage.Foreground = Brushes.Red;
				alertMessage.Visibility = Visibility.Visible;
			}
		}
		private void GenerateP11DFiles()
		{
			try
			{
				List<string> paths = new List<string>();
				List<string> Input_files = new List<string>();


				paths.Add(@"C:\" + CodePath.Text + @"\IPRLUK\IPUKMeta\Package\PostDeploy\anytime\30 Payroll (UK)\040 Views\Client\040 P11D\" + FindText.Text);
				paths.Add(@"C:\" + CodePath.Text + @"\IPRLUK\IPUKMeta\Package\PostDeploy\anytime\30 Payroll (UK)\050 SP\AdpUKPayrollWebApi\P11D\" + FindText.Text);
				paths.Add(@"C:\" + CodePath.Text + @"\IPRLUK\IPUKMeta\Package\PostDeploy\anytime\30 Payroll (UK)\050 SP\AdpIpUkP11D\" + FindText.Text);
				//paths.Add(@"C:\" + CodePath.Text + @"\IPRLUK\IPUKMeta\Package\PostDeploy\anytime\30 Payroll (UK)\040 Views\Client\040 P11D\" + FindText.Text);
				//050 SP\AdpUKPayrollWebApi\P11D

				//paths.ForEach(path =>
				//{
				//	Input_files = Directory.GetFiles(path).ToList();
				//});

				foreach (var path in paths)
				{
					Input_files.AddRange(Directory.GetFiles(path).ToList());
				}

				Input_files.Add(@"C:\" + CodePath.Text + @"\IPRLUK\IPUKMeta\Package\PreDeploy\anytime\30 Payroll (UK)\040 Views\Client\040 P11D\vwP11D" + FindText.Text + "CARSANDFUELRESULT.pre.sql");
				Input_files.Add(@"C:\" + CodePath.Text + @"\IPRLUK\IPUKMeta\Package\PreDeploy\anytime\30 Payroll (UK)\040 Views\Client\040 P11D\vwP11D" + FindText.Text + "EMPLOYEEDETAILS.pre.sql");
				Input_files.Add(@"C:\" + CodePath.Text + @"\IPRLUK\IPUKMeta\Package\PreDeploy\anytime\30 Payroll (UK)\040 Views\Client\040 P11D\vwP11D" + FindText.Text + "RESULTS.pre.sql");

				//Create folders for maintanace work

				//dropped_files.Add(@"C:\" + CodePath.Text + @"\IPRLUK\IPUKMeta\Package\PostDeploy\onetime\30 Payroll (UK)\DBO\_InsertDataInToP11DTradeOrgEntTypeTable" + FindText.Text + "CARSANDFUELRESULT.pre.sql");
				//dropped_files.Add(@"C:\" + CodePath.Text + @"\IPRLUK\IPUKMeta\Package\PostDeploy\anytime\30 Payroll (UK)\040 Views\Client\040 P11D\vwP11D" + FindText.Text + "EMPLOYEEDETAILS.pre.sql");
				//dropped_files.Add(@"C:\" + CodePath.Text + @"\IPRLUK\IPUKMeta\Package\PreDeploy\anytime\30 Payroll (UK)\040 Views\Client\040 P11D\vwP11D" + FindText.Text + "RESULTS.pre.sql");
				//dropped_files.Add(@"C:\" + CodePath.Text + @"\IPRLUK\IPUKMeta\Package\PreDeploy\anytime\30 Payroll (UK)\040 Views\Client\040 P11D\vwP11D" + FindText.Text + "RESULTS.pre.sql");
				//C:\ihcm.git\IPRLUK\IPUKMeta\Package\PostDeploy\onetime\30 Payroll(UK)\DBO\20181122.2_InsertDataInToP11DTradeOrgEntTypeTable2018.sql
				//C:\ihcm.git\IPRLUK\IPUKMeta\Package\PostDeploy\onetime\30 Payroll(UK)\DBO\20181122.1_Insert_rows_p11dexpensesmadetype_and_p11dothersubtype_2018.sql
				//C:\ihcm.git\IPRLUK\Source\Database\eForms\Upgrade\20180220_Insert_eForm_WS2b_TaxYear2017.sql
				//C:\ihcm.git\IPRLUK\Source\Database\eForms\Upgrade\20171221_Insert__IPSYS_EFORM_Forms_P11D_TaxYear2017.sql

				InputFiles(Input_files);
				ContructFiles(true, FindText.Text, ReplaceWith.Text);
			}
			catch (Exception ex)
			{
				alertMessage.Text = ex.ToString();
				alertMessage.Foreground = Brushes.Red;
				alertMessage.Visibility = Visibility.Visible;
			}
		}
		private void ContructFiles(bool skipDirectoryCreation,string text_to_replace,string replacing_text)
		{
			try
			{

				string path = @"C:\"+ CodePath.Text+@"\FilesCreatedPaths.txt";
				//if (!File.Exists(path))
				//{
				//	File.Create(path);
				//}

				TextWriter tw = new StreamWriter(path);

				files.ForEach(file =>
				{
					// Processing
					var file_extension = Path.GetExtension(file); // ex: .cls
					string file_path;
					string new_file=null;

					var file_name = Path.GetFileNameWithoutExtension(file).Replace(text_to_replace, replacing_text); // ex: ediLineEPS19
					if (skipDirectoryCreation)
					{
						 file_path = Path.GetDirectoryName(file).Replace(text_to_replace, ""); // ex: C:\TextReplacer\TextReplacer
						 
						//file_extension = Path.GetExtension(file);

						if (!Directory.Exists(file_path + replacing_text))
						{
							Directory.CreateDirectory(file_path + replacing_text);
						}

						new_file = Path.Combine(file_path + replacing_text, $@"{file_name}{file_extension}");
					}

					else if (!skipDirectoryCreation)
					{
						file_path = Path.GetDirectoryName(file); // ex: C:\TextReplacer\TextReplacer
						
						//file_extension = Path.GetExtension(file);

						new_file = Path.Combine(file_path, $@"{file_name}{file_extension}");
					}
					
					var file_encoding = GetFileEncoding(file);
					var file_content = new StringBuilder();
					const int BufferSize = 128;
					using (var fileStream = File.OpenRead(file))
					using (var streamReader = new StreamReader(fileStream, file_encoding, true, BufferSize))
					{
						string line;
						while ((line = streamReader.ReadLine()) != null)
						{
							// Process Line
							//file_content.AppendLine(Regex.Replace(line, $@"\{text_to_replace},\b", replacing_text, RegexOptions.IgnoreCase));
							file_content.AppendLine(line.Replace(text_to_replace, replacing_text));
						}
					}

					using (var streamWriter = new StreamWriter(new FileStream(new_file, FileMode.Create, FileAccess.ReadWrite), file_encoding))
					{
						streamWriter.Write(file_content.ToString());

						//string gitCommand = "git";
						//string gitAddArgument = @"add " + "\"" + new_file + "\"";
						////string gitCommitArgument = @"commit ""explanations_of_changes"" "
						////string gitPushArgument = @"push our_remote"

						//var startInfo = new ProcessStartInfo();
						//startInfo.WorkingDirectory = $"C:\\{CodePath.Text}";
						//startInfo.FileName = @"C:\Program Files\Git\git-bash.exe";
						//startInfo.Arguments = @"add " + "\"" + new_file + "\"";
						//startInfo.UseShellExecute = false;
						//startInfo.CreateNoWindow = true;
						//using (Process process = Process.Start(startInfo.FileName, startInfo.Arguments))
						//{
						//	process.WaitForExit();
						//}

						//Process.Start(gitCommand, gitAddArgument);
						//Process.Start(gitCommand, gitCommitArgument);
						//Process.Start(gitCommand, gitPushArgument);
						//AppDomain.CurrentDomain.BaseDirectory

						var files = new string[] { @"C:\Program Files\IIS\Microsoft Web Deploy\Microsoft.Web.Deployment.dll", @"C:\Program Files\IIS\Microsoft Web Deploy\Microsoft.Web.Deployment.dll" };
						var appended_files = files.Select(file => $"\"{file}\"")
							.Aggregate("", (acc, file) => $"{acc} {file}");
						Console.WriteLine(appended_files);
						using (Process process = new Process())
						{
							var startInfo = new ProcessStartInfo
							{
								WorkingDirectory = @"C:\Users\gubbalpa\Documents\ihcm.pf.webv2",
								WindowStyle = ProcessWindowStyle.Hidden,
								FileName = "git",
								//RedirectStandardInput = true,
								RedirectStandardOutput = true,
								RedirectStandardError = true,
								UseShellExecute = false,
								Arguments = appended_files,
							};

							process.StartInfo = startInfo;
							process.Start();
							var output = process.StandardOutput.ReadToEnd();
							var error = process.StandardError.ReadToEnd();
							process.WaitForExit();
							//Console.WriteLine(output);
							//Console.WriteLine(error);
						}

					}

					//string gitCommand = "git";

					//new_file.Split();
					//string gitAddArgument = @"add ""IPRLUK/IPUKMeta/Package/PostDeploy/anytime/30 Payroll (UK)/040 Views/Client/040 P11D/2019/""";//@"add " + "\"" + new_file.Replace(@"C:\ihcm.git\", "");
					////string gitCommitArgument = @"commit ""explanations_of_changes"" "
					////string gitPushArgument = @"push our_remote"

					//Process.Start(

					//Process.Start(gitCommand, gitAddArgument);
					//Process.Start(gitCommand, gitCommitArgument);
					//Process.Start(gitCommand, gitPushArgument);
					tw.WriteLine(new_file.ToString());

					alertMessage.Foreground = Brushes.Green;
					alertMessage.Text = "Files created, check " + path+ " for list of files created";
					alertMessage.Visibility = Visibility.Visible;
				});
				tw.Close();
			}
			catch (Exception ex)
			{
				alertMessage.Text = ex.ToString();
				alertMessage.Foreground = Brushes.Red;
				alertMessage.Visibility = Visibility.Visible;
			}
		}

		private void P11D_Checked(object sender, RoutedEventArgs e)
		{
			rb = sender as RadioButton;
			//choiceTextBlock.Text = "You chose: " + rb.GroupName + ": " + rb.Name;
			DropLocation.Visibility = Visibility.Hidden;
			PathForCode.Visibility = Visibility.Visible;
		}

		private void EDI_Checked(object sender, RoutedEventArgs e)
		{
			rb = sender as RadioButton;
			DropLocation.Visibility = Visibility.Hidden;
			PathForCode.Visibility = Visibility.Visible;
			//choiceTextBlock.Text = "You chose: " + rb.GroupName + ": " + rb.Name;
		}

		private void Other_Checked(object sender, RoutedEventArgs e)
		{
			rb = sender as RadioButton;
			DropLocation.Visibility = Visibility.Visible;
			PathForCode.Visibility = Visibility.Hidden;

		}

		private void ALL_Checked(object sender, RoutedEventArgs e)
		{
			rb = sender as RadioButton;
			//choiceTextBlock.Text = "You chose: " + rb.GroupName + ": " + rb.Name;
			DropLocation.Visibility = Visibility.Hidden;
			PathForCode.Visibility = Visibility.Visible;
		}
	}
}
