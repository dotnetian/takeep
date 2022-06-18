using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Xml;

namespace Takeep.Core
{
	public class TakeepXml
	{
		public static void Keep (Item item)
		{
			#region Check if name is null

			if (!CheckNulls (item.Name))
			{
				return;
			}

			#endregion

			#region Check if item exists

			if (GetItem (item.Name) != null)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine ("The process was aborted: An item with this name currenty exists");
				Console.ForegroundColor = ConsoleColor.White;

				return;
			}

			#endregion

			#region Check if should be opened in notepad

			if (item.Text == null)
			{
				AddNotepad (item.Name);
				return;
			}

			#endregion

			#region Keep item

			string xmlPath = CheckDirectory () + CheckFile ();

			XmlDocument xmlDefault = new ();

			xmlDefault.Load (xmlPath);

			XmlElement parentItem = xmlDefault.CreateElement ("Item");

			XmlElement name = xmlDefault.CreateElement ("Name");
			name.InnerText = item.Name;

			XmlElement content = xmlDefault.CreateElement ("Text");
			content.InnerText = item.Text;

			parentItem.AppendChild (name);
			parentItem.AppendChild (content);

			xmlDefault.DocumentElement.AppendChild (parentItem);

			xmlDefault.Save (xmlPath);

			#endregion

			#region Write success

			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine ("✓ Successfully added the item!");
			Console.ForegroundColor = ConsoleColor.White;

			#endregion
		}

		public static bool Take (string name, bool copy = false, bool notepad = false)
		{
			#region Check if name is null

			Item? item = GetItem (name);

			if (!CheckNulls (name))
			{
				return false;
			}

			#endregion

			#region Check if item is valid

			if (item == null)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.Write ("No items found with name ");
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.Write (name);
				Console.ForegroundColor = ConsoleColor.White;

				return false;
			}

			#endregion

			#region Check if should be opened in notepad

			if (notepad)
			{
				ViewNotepad (name);
			}
			#endregion

			#region Check if should be printed on screen

			else if (!copy)
			{
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine ($"■ This is the content of {item.Name}:");
				Console.ForegroundColor = ConsoleColor.White;

				Console.WriteLine (item.Text);
			}
			#endregion

			#region Check if should be copied to clipboard
			else
			{
				TakeepClipboard.Copy (item.Text);

				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine ($"■ {item.Name} successfully copied to your clipboard");
				Console.ForegroundColor = ConsoleColor.White;
			}
			#endregion

			return true;
		}

		public static void Remove (string name)
		{
			#region Check if name is null

			if (!CheckNulls (name))
			{
				return;
			}

			#endregion

			#region Load Document

			string defaultXml = CheckDirectory () + CheckFile ();

			XmlDocument document = new ();
			document.Load (defaultXml);

			#endregion

			#region Find Item

			XmlNodeList nodes = document.DocumentElement.SelectNodes ("Item");

			foreach (XmlNode node in nodes)
			{
				if (node["Name"].InnerText == name)
				{
					node.ParentNode.RemoveChild (node);
					document.Save (defaultXml);

					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine ("✓ Successfully removed the item!");
					Console.ForegroundColor = ConsoleColor.White;

					return;
				}
			}

			#endregion

			#region Write Error

			Console.ForegroundColor = ConsoleColor.Red;
			Console.Write ("No items found with name ");
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.Write (name);
			Console.ForegroundColor = ConsoleColor.White;

			#endregion
		}

		public static void List (string name = "")
		{
			#region Load Document

			string defaultXml = CheckDirectory () + CheckFile ();

			XmlDocument document = new ();
			document.Load (defaultXml);

			#endregion

			#region List Items By Name

			XmlNodeList nodes = document.DocumentElement.SelectNodes ("Item");

			int returns = 0;

			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine ("■ The list of items of default keepsheet:");
			Console.ForegroundColor = ConsoleColor.White;

			if (!string.IsNullOrEmpty (name))
			{
				foreach (XmlNode node in nodes)
				{
					if (node["Name"].InnerText.Contains (name))
					{
						Console.WriteLine ("	" + node["Name"].InnerText);
						returns++;
					}
				}
			}

			#endregion

			#region List All Items

			else
			{
				foreach (XmlNode node in nodes)
				{
					Console.WriteLine ("	" + node["Name"].InnerText);
					returns++;
				}
			}

			#endregion

			#region Write Error

			if (returns == 0)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine ($"The process war aborted: No item's name found containing \"{name}\"");
				Console.ForegroundColor = ConsoleColor.White;
			}

			#endregion
		}

		public static void Edit (Item item, bool notepad = false)
		{
			#region Check if item is null

			if (!CheckNulls (item))
			{
				return;
			}

			#endregion

			#region Check if should be opened in notepad

			else if (notepad)
			{
				item.Text = EditNotepad (item.Name);

				return;
			}

			#endregion

			#region Load Document

			string defaultXml = CheckDirectory () + CheckFile ();

			XmlDocument document = new ();
			document.Load (defaultXml);

			#endregion

			#region Edit Item

			XmlNodeList nodes = document.DocumentElement.SelectNodes ("Item");

			foreach (XmlNode node in nodes)
			{
				if (node["Name"].InnerText == item.Name)
				{
					node["Text"].InnerText = item.Text;

					document.Save (defaultXml);

					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine ("✓ Successfully edited the item!");
					Console.ForegroundColor = ConsoleColor.White;

					return;
				}
			}

			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine ($"The procces was aborted: No items found with name {item.Name}");
			Console.ForegroundColor = ConsoleColor.White;

			#endregion
		}

		private static string CheckDirectory ()
		{
			string env = $"C:/Users/{Environment.UserName}/Documents";

			if (!Directory.Exists (env + "/keepsheets"))
			{
				Directory.CreateDirectory (env + "/keepsheets");
			}

			if (File.Exists (env + "/keepsheets/default.tkp"))
			{
				CheckFile ();
			}

			return env + "/keepsheets";
		}

		private static string CheckFile ()
		{
			string xmlPath = $"C:/Users/{Environment.UserName}/Documents/keepsheets/default.tkp";

			if (!File.Exists (xmlPath))
			{
				File.WriteAllText (xmlPath, "<?xml version=\"1.0\" encoding=\"utf-8\"?><Items></Items>");
			}

			return "/default.tkp";
		}

		private static Item GetItem (string name)
		{
			string directory = CheckDirectory ();

			string file = CheckFile ();

			XmlDocument document = new ();
			document.Load (file);

			XmlNodeList nodes = document.DocumentElement.SelectNodes ("Item");

			foreach (XmlNode node in nodes)
			{
				if (node["Name"].InnerText == name)
				{
					return new Item { Name = node["Name"].InnerText, Text = node["Text"].InnerText };
				}
			}

			return null;
		}

		private static void AddNotepad (string name)
		{
			string directory = CheckDirectory ();

			string filePath = directory + "/ITEM_CONTENT";
			File.Delete (filePath);

			#region Get Text
			File.WriteAllText (filePath, $"/# Enter the content of item \"{name}\".");

			FileInfo fileInfo = new (filePath);
			File.SetAttributes (filePath, FileAttributes.Hidden);


			DateTime initialWriteTime = fileInfo.LastWriteTime;
			DateTime lastWriteTime = initialWriteTime;

			var processInfo = new ProcessStartInfo ("notepad.exe", filePath)
			{
				CreateNoWindow = false,
				UseShellExecute = false,
				RedirectStandardError = true,
				RedirectStandardOutput = true,
				WorkingDirectory = @"C:\Windows\System32\"
			};

			Process p = Process.Start (processInfo);

			while (lastWriteTime == initialWriteTime)
			{
				int pid = p.Id;

				Process checkProcess = Process.GetProcessById (pid);

				if (checkProcess.HasExited)
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine ("The process was aborted: Notepad was closed");
					Console.ForegroundColor = ConsoleColor.White;

					File.Delete (filePath);

					return;
				}

				fileInfo = new (filePath);
				lastWriteTime = fileInfo.LastWriteTime;
			}

			p.Kill ();
			#endregion

			#region Process text
			string[] writtenFileContent = File.ReadAllLines (filePath);

			for (int i = 0; i < writtenFileContent.Length; i++)
			{
				if (writtenFileContent[i].StartsWith ("/# "))
				{
					writtenFileContent = writtenFileContent.Where ((source, index) => index != i).ToArray ();
				}
			}

			string finalContent = string.Empty;

			foreach (var line in writtenFileContent)
			{
				if (string.IsNullOrWhiteSpace (finalContent))
				{
					finalContent += line;
				}
				else
				{
					finalContent += Environment.NewLine + line;
				}
			}

			if (string.IsNullOrWhiteSpace (finalContent))
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine ("The process was aborted: Text was empty");
				Console.ForegroundColor = ConsoleColor.White;

				File.Delete (filePath);

				return;
			}

			Keep (new Item { Name = name, Text = finalContent });

			File.Delete (filePath);
			#endregion
		}

		private static void ViewNotepad (string name)
		{
			string directory = CheckDirectory ();

			string filePath = directory + "/ITEM_CONTENT";
			File.Delete (filePath);

			File.WriteAllText (filePath, $"/# This is the content of item \"{name}\":{Environment.NewLine}");
			File.AppendAllText (filePath, GetItem (name).Text);

			FileInfo fileInfo = new (filePath);
			File.SetAttributes (filePath, FileAttributes.Hidden);

			DateTime initialWriteTime = fileInfo.LastWriteTime;
			DateTime lastWriteTime = initialWriteTime;

			Console.WriteLine ("Opening Notepad...");

			var processInfo = new ProcessStartInfo ("notepad.exe", filePath)
			{
				CreateNoWindow = false,
				UseShellExecute = false,
				RedirectStandardError = true,
				RedirectStandardOutput = true,
				WorkingDirectory = @"C:\Windows\System32\"
			};

			Process p = Process.Start (processInfo);

			while (lastWriteTime == initialWriteTime)
			{
				int pid = p.Id;

				Process checkProcess = Process.GetProcessById (pid);

				if (checkProcess.HasExited)
				{
					File.Delete (filePath);

					return;
				}

				fileInfo = new (filePath);
				lastWriteTime = fileInfo.LastWriteTime;
			}

			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine ("Editing the text will not affect the item. If you want to edit the item, please use the \"edit\" command.");
			Console.ForegroundColor = ConsoleColor.White;

			p.Kill ();
		}

		private static string EditNotepad (string name)
		{
			string directory = CheckDirectory ();

			string filePath = directory + "/ITEM_CONTENT";
			File.Delete (filePath);

			#region Get Text
			File.WriteAllText (filePath, $"/# This is the content of item \"{name}\":{Environment.NewLine}");
			File.AppendAllText (filePath, GetItem (name).Text);

			FileInfo fileInfo = new (filePath);
			File.SetAttributes (filePath, FileAttributes.Hidden);


			DateTime initialWriteTime = fileInfo.LastWriteTime;
			DateTime lastWriteTime = initialWriteTime;

			var processInfo = new ProcessStartInfo ("notepad.exe", filePath)
			{
				CreateNoWindow = false,
				UseShellExecute = false,
				RedirectStandardError = true,
				RedirectStandardOutput = true,
				WorkingDirectory = @"C:\Windows\System32\"
			};

			Process p = Process.Start (processInfo);

			while (lastWriteTime == initialWriteTime)
			{
				int pid = p.Id;

				Process checkProcess = Process.GetProcessById (pid);

				if (checkProcess.HasExited)
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine ("The process was aborted: Notepad was closed");
					Console.ForegroundColor = ConsoleColor.White;

					File.Delete (filePath);

					return null;
				}

				fileInfo = new (filePath);
				lastWriteTime = fileInfo.LastWriteTime;
			}

			p.Kill ();
			#endregion

			#region Process text
			string[] writtenFileContent = File.ReadAllLines (filePath);

			for (int i = 0; i < writtenFileContent.Length; i++)
			{
				if (writtenFileContent[i].StartsWith ("/# "))
				{
					writtenFileContent = writtenFileContent.Where ((source, index) => index != i).ToArray ();
				}
			}

			string finalContent = string.Empty;

			foreach (var line in writtenFileContent)
			{
				if (string.IsNullOrWhiteSpace (finalContent))
				{
					finalContent += line;
				}
				else
				{
					finalContent += Environment.NewLine + line;
				}
			}

			if (string.IsNullOrWhiteSpace (finalContent))
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine ("The process was aborted: Text was empty");
				Console.ForegroundColor = ConsoleColor.White;

				File.Delete (filePath);

				return null;
			}


			File.Delete (filePath);

			return finalContent;
			#endregion
		}

		private static bool CheckNulls (Item item, [CallerMemberName] string callerName = "")
		{
			if (item == null || item.Name == null || item.Text == null || string.IsNullOrWhiteSpace (item.Name) || string.IsNullOrWhiteSpace (item.Text))
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.Write ("You must enter both name & content correctly. For more info, enter: ");
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.Write ("tkp ");
				Console.ForegroundColor = ConsoleColor.White;
				Console.Write ($"{callerName.ToLower ()} ");
				Console.ForegroundColor = ConsoleColor.DarkGray;
				Console.Write ("--help");
				Console.ForegroundColor = ConsoleColor.White;

				return false;
			}

			return true;
		}

		private static bool CheckNulls (string name, [CallerMemberName] string callerName = "")
		{
			if (string.IsNullOrWhiteSpace (name))
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.Write ("You must enter name correctly. For more info, enter: ");
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.Write ("tkp ");
				Console.ForegroundColor = ConsoleColor.White;
				Console.Write ($"{callerName.ToLower ()} ");
				Console.ForegroundColor = ConsoleColor.DarkGray;
				Console.Write ("--help");
				Console.ForegroundColor = ConsoleColor.White;

				return false;
			}

			return true;
		}
	}
}