﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

		List<string> listOfNewFiles = new List<string>();
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
			listOfNewFiles.Clear();
			if (FindText.Text == "" || ReplaceWith.Text == "")
			{
				alertMessage.Text = "Input fields cannot be empty";
				alertMessage.Foreground = Brushes.Red;
				alertMessage.Visibility = Visibility.Visible;
				return;
			}

			switch (Task.SelectedIndex)
			{
				case 0:
					GenerateP11DFiles();
					GenerateEDIFiles();
					break;
				case 1:
					GenerateP11DFiles();
					break;
				case 2:
					GenerateEDIFiles();
					break;
				
				case 3:
					ContructFiles(true);
					break;
				case 4:
					ExecuteSQL();
					break;
			}
			WritFilesToGit(listOfNewFiles);
		}

		private void ExecuteSQL()
		{
			var files=Path.GetFileName(@"C:\ihcm.git\ComplianceChanges_2019");
			string sqlConnectionString = "Data Source=local;Initial Catalog=Doves;Integrated Security=True";

			FileInfo file = new FileInfo(@"C:\ihcm.git\ComplianceChanges_2019");

			string script = file.OpenText().ReadToEnd();

			SqlConnection conn = new SqlConnection(sqlConnectionString);

			//Server server = new Server(new ServerConnection(conn));

			//server.ConnectionContext.ExecuteNonQuery(script);

		}

		private void GenerateEDIFiles()
		{
			try
			{
				List<string> paths = new List<string>();
				List<string> Input_files = new List<string>();


				//getFindAndReplaceText(out string  EPS_FindYear, out string EPS_ReplaceYear);

				var FindYear = FindText.Text.Substring(2, 2);
				var ReplaceYear = ReplaceWith.Text.Substring(2, 2);

				//EPS
				Input_files.Add(@"C:\" + CodePath.Text + @"\IPRLUK\Source\dotNet\Adp.IpUk.EDI\Adp.IpUk.EDI.Interchange\Lines\LineEPS" + (Convert.ToInt16(FindYear) + 2).ToString() + ".cs");
				Input_files.Add(@"C:\" + CodePath.Text + @"\IPRLUK\Source\dotNet\Adp.IpUk.EDI\Adp.IpUk.EDI.Interchange\Messages\MessageEPS" + (Convert.ToInt16(FindYear) + 2).ToString() + ".cs");

				Input_files.Add(@"C:\" + CodePath.Text + @"\IPRLUK\Source\dotNet\Adp.IpUk.EDI\Adp.IpUk.EDI.Interchange\Lines\LineFPS" + (Convert.ToInt16(FindYear) + 2).ToString() + ".cs");
				//Input_files.Add(@"C:\" + CodePath.Text + @"\IPRLUK\Source\dotNet\Adp.IpUk.EDI\Adp.IpUk.EDI.Interchange\Messages\MessageFPS" + (Convert.ToInt16(ReplaceYear) + 1).ToString() + ".cs");
				//Comment- Manual creation
				//Input_files.Add(@"C:\" + CodePath.Text + @"\IPRLUK\Source\dotNet\Adp.IpUk.EDI\Adp.IpUk.EDI.Interchange\Messages\MessageFPS" + (Convert.ToInt16(ReplaceYear) + 1).ToString() + ".cs");

				//Input_files.Add(@"C:\" + CodePath.Text + @"\IPRLUK\Source\dotNet\Adp.IpUk.EDI\Adp.IpUk.EDI.Interchange\Lines\LineEYU" + FindYear + ".cs");
				//Input_files.Add(@"C:\" + CodePath.Text + @"\IPRLUK\Source\dotNet\Adp.IpUk.EDI\Adp.IpUk.EDI.Interchange\Messages\MessageEYU" + FindYear + ".cs");

				InputFiles(Input_files);
				ContructFiles(false);
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




				List<string> test = new List<string>();



				//Comment- For this file different logic has to be implemented as the content in the file has to be replaced with a check for version.
				//test.Add(Directory.GetFiles(@"C:\ihcm.git\IPRLUK\Source\Database\eForms\Upgrade", $@"*Insert_eForm_WS2b_TaxYear{FindText.Text}.sql")[0]);
				//Comment- For this file different logic has to be implemented as the content in the file has to be replaced with a check for version.
				//test.Add(Directory.GetFiles(@"C:\ihcm.git\IPRLUK\Source\Database\eForms\Upgrade", $@"*_Insert__IPSYS_EFORM_Forms_P11D_TaxYear{FindText.Text}.sql")[0]);

				//Files to be created manually
				//test.Add(Directory.GetFiles(@"C:\ihcm.git\IPRLUK\Source\Database\eForms\Upgrade", $@"*Insert_eForm_WS2b_TaxYear{FindText.Text}.sql")[0]);
				//test.Add(Directory.GetFiles(@"C:\ihcm.git\IPRLUK\Source\Database\eForms\Upgrade", $@"*_Insert__IPSYS_EFORM_Forms_P11D_TaxYear{FindText.Text}.sql")[0]);


				//test[0] = test[0].Replace(test[0].Substring(test[0].LastIndexOf('\\') + 1, (test[0].IndexOf('_') - 1 - test[0].LastIndexOf('\\'))), DateTime.Now.ToString("yyyyMMdd"));

				foreach (var path in paths)
				{
					Input_files.AddRange(Directory.GetFiles(path).ToList());
				}

				Input_files.Add(@"C:\" + CodePath.Text + @"\IPRLUK\IPUKMeta\Package\PreDeploy\anytime\30 Payroll (UK)\040 Views\Client\040 P11D\vwP11D" + FindText.Text + "CARSANDFUELRESULT.pre.sql");
				Input_files.Add(@"C:\" + CodePath.Text + @"\IPRLUK\IPUKMeta\Package\PreDeploy\anytime\30 Payroll (UK)\040 Views\Client\040 P11D\vwP11D" + FindText.Text + "EMPLOYEEDETAILS.pre.sql");
				Input_files.Add(@"C:\" + CodePath.Text + @"\IPRLUK\IPUKMeta\Package\PreDeploy\anytime\30 Payroll (UK)\040 Views\Client\040 P11D\vwP11D" + FindText.Text + "RESULTS.pre.sql");

				//comment- update unique identifier value after creatig new file
				Input_files.Add(Directory.GetFiles(@"C:\ihcm.git\IPRLUK\IPUKMeta\Package\PostDeploy\onetime\30 Payroll (UK)\DBO", $@"*InsertDataInToP11DTradeOrgEntTypeTable{FindText.Text}.sql")[0]);
				Input_files.Add(Directory.GetFiles(@"C:\ihcm.git\IPRLUK\IPUKMeta\Package\PostDeploy\onetime\30 Payroll (UK)\DBO", $@"*Insert_rows_p11dexpensesmadetype_and_p11dothersubtype_{FindText.Text}.sql")[0]);
				Input_files.Add(Directory.GetFiles(@"C:\ihcm.git\IPRLUK\Source\Database\Static\Upgrade", $@"*2018*DBTask_P11DBenefitControlInsertion.sql")[0]);


				//Input_files.Add(Directory.GetFiles(@"C:\ihcm.git\IPRLUK\IPUKMeta\Package\PostDeploy\onetime\30 Payroll (UK)\DBO", $@"*Insert_rows_p11dexpensesmadetype_and_p11dothersubtype_{FindText.Text}.sql")[0]);


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
				ContructFiles(true);
			}
			catch (Exception ex)
			{
				alertMessage.Text = ex.ToString();
				alertMessage.Foreground = Brushes.Red;
				alertMessage.Visibility = Visibility.Visible;
			}
		}
		private void ContructFiles(bool skipDirectoryCreation)
		{
			try
			{
				var filesCopyFolder = $@"C:\{CodePath.Text}\ComplianceChanges_{ReplaceWith.Text}\";
				if (!Directory.Exists(filesCopyFolder))
				{
					Directory.CreateDirectory(filesCopyFolder);
				}

				foreach (var file in files)
				{
					// Processing
					var file_extension = Path.GetExtension(file); // ex: .cls
					string file_path;
					string new_file = null;
					string copy_file = null;
					string text_to_replace = FindText.Text;
					string replacing_text = ReplaceWith.Text;
					string file_name=null;

					if (skipDirectoryCreation)
					{
						file_path = Path.GetDirectoryName(file).Replace(text_to_replace, ""); // ex: C:\TextReplacer\TextReplacer
																							  //file_name = Path.GetFileNameWithoutExtension(file).Replace(text_to_replace, replacing_text); // ex: ediLineEPS19
																							  //file_extension = Path.GetExtension(file);

						if ((file.ToString().Contains("InsertDataInToP11DTradeOrgEntTypeTable") || file.ToString().Contains("p11dexpensesmadetype")))
						{
							file_name = file.Replace(file.Substring(file.LastIndexOf('\\') + 1, (file.IndexOf('_') - file.LastIndexOf('\\')) - 1), DateTime.Now.ToString("yyyyMMdd"));
							file_name = Path.GetFileNameWithoutExtension(file_name).Replace(text_to_replace, replacing_text);
							file_path = Path.GetDirectoryName(file);
							new_file = Path.Combine(file_path, $@"{file_name}{file_extension}");
						}
						else if ((file.ToString().Contains("P11DBenefitControlInsertion")))
						{
							//manual: replace text manually in the file
							//manual: check leap years related files and replace files
							file_name = file.Replace(file.Substring(file.LastIndexOf('\\') + 5, (file.IndexOf('_', file.IndexOf('_') + 1) - file.LastIndexOf('\\')) - 5), DateTime.Now.ToString("yyyyMMdd"));
							file_name = Path.GetFileNameWithoutExtension(file_name).Replace(text_to_replace, replacing_text);
							file_path = Path.GetDirectoryName(file);
							new_file = Path.Combine(file_path, $@"{file_name}{file_extension}");

						}
						else
						{
							if (!Directory.Exists(file_path + replacing_text))
							{
								Directory.CreateDirectory(file_path + replacing_text);
							}
							file_name = Path.GetFileNameWithoutExtension(file).Replace(text_to_replace, replacing_text); // ex: ediLineEPS19
							new_file = Path.Combine(file_path + replacing_text, $@"{file_name}{file_extension}");
						}
					}

					else if (!skipDirectoryCreation)
					{

						if (file.ToString().Contains("FPS") || file.ToString().Contains("EPS"))

						{
							text_to_replace = (Convert.ToInt16(text_to_replace.Substring(2, 2)) + 2).ToString();
							replacing_text = (Convert.ToInt16(replacing_text.Substring(2, 2)) + 2).ToString();

						}
						//else if (file.ToString().Contains("EYU"))

						//{
						//	text_to_replace = text_to_replace.Substring(2, 2);
						//	replacing_text = replacing_text.Substring(2, 2);

						//}

						file_path = Path.GetDirectoryName(file); // ex: C:\TextReplacer\TextReplacer

						//file_extension = Path.GetExtension(file);

						file_name = Path.GetFileNameWithoutExtension(file).Replace(text_to_replace, replacing_text); // ex: ediLineEPS19


						new_file = Path.Combine(file_path, $@"{file_name}{file_extension}");

					}
					copy_file = Path.Combine(filesCopyFolder, $@"{ file_name}{ file_extension}");
					listOfNewFiles.Add(new_file);
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
					using (var streamWriter1 = new StreamWriter(new FileStream(copy_file, FileMode.Create, FileAccess.ReadWrite), file_encoding))
					using (var streamWriter = new StreamWriter(new FileStream(new_file, FileMode.Create, FileAccess.ReadWrite), file_encoding))
					{
						streamWriter.Write(file_content.ToString());
						streamWriter1.Write(file_content.ToString());
					}
					//Path.Combine(filesCopyFolder, new_file);
				}
				//WritFilesToGit(listOfNewFiles);
			}
			catch (Exception ex)
			{
				alertMessage.Text = ex.ToString();
				alertMessage.Foreground = Brushes.Red;
				alertMessage.Visibility = Visibility.Visible;
			}
		}

		private void WritFilesToGit(List<string> listOfNewFiles)
		{
			var appended_files = listOfNewFiles.Select(f => $"\"{f}\"")
										.Aggregate("add ", (acc, f) => $"{acc} {f}");

			string path = $@"C:\{ CodePath.Text}\ComplianceChanges_{ ReplaceWith.Text}\FilesCreatedPaths.txt";

			TextWriter tw = new StreamWriter(path);
			foreach (var f in listOfNewFiles)
				tw.WriteLine(f);
			tw.Close();

			//Console.WriteLine(appended_files);
			using (Process process = new Process())
			{
				var startInfo = new ProcessStartInfo
				{
					WorkingDirectory = $@"C:\{CodePath.Text}",
					WindowStyle = ProcessWindowStyle.Hidden,
					FileName = "\"C:\\Program Files\\Git\\bin\\git.exe\"",
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

				alertMessage.Foreground = Brushes.Violet;
				alertMessage.Text = "Files created, check " + path + " for list of files created";
				alertMessage.Visibility = Visibility.Visible;
			}
		}

		private void Task_Selected(object sender, RoutedEventArgs e)
		{
			int index=Task.SelectedIndex;

			//ComboBoxItem cb = sender as ComboBoxItem;

			if (index == 0 || index == 1 || index == 2)
			{
				PathForCode.Visibility = Visibility.Visible;
				DropLocation.Visibility = Visibility.Hidden;
			}
			else if (index == 4)
			{
				PathForCode.Visibility = Visibility.Hidden;
				DropLocation.Visibility = Visibility.Visible;
			}
			else
			{
				PathForCode.Visibility = Visibility.Hidden;
				DropLocation.Visibility = Visibility.Hidden;
			}

		}
	}
}
