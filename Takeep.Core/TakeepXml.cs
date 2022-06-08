using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Xml;

namespace Takeep.Core
{
	public class TakeepXml
	{
		public static void Keep (Item item)
		{
			if (!CheckNulls (item.Name))
			{
				return;
			}

			if (item.Content == null)
			{
				EditNotepad (item.Name);
				return;
			}

			string directory = CheckDirectory ();

			#region Check File

			string xmlPath = directory + "/default.xml";

			if (!File.Exists (xmlPath))
			{
				File.WriteAllText (xmlPath, "<?xml version=\"1.0\" encoding=\"utf-8\"?><Items></Items>");
			}

			#endregion

			XmlDocument xmlDefault = new ();

			xmlDefault.Load (xmlPath);

			XmlElement parentItem = xmlDefault.CreateElement ("Item");

			XmlElement name = xmlDefault.CreateElement ("Name");
			name.InnerText = item.Name;

			XmlElement content = xmlDefault.CreateElement ("Content");
			content.InnerText = item.Content;

			parentItem.AppendChild (name);
			parentItem.AppendChild (content);

			xmlDefault.DocumentElement.AppendChild (parentItem);

			xmlDefault.Save (xmlPath);

			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine ("✓ Successfully added the item!");
			Console.ForegroundColor = ConsoleColor.White;
		}

		public static void Take (string name)
		{
			Item? item = GetItem (name);

			if (!CheckNulls (name))
			{
				return;
			}

			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine ($"■ This is the content of {item.Name}:");
			Console.ForegroundColor = ConsoleColor.White;

			Console.WriteLine (item.Content);
		}

		public static void Remove (string name)
		{
			if (!CheckNulls (name))
			{
				return;
			}

			string defaultXml = CheckDirectory () + "/default.xml";

			XmlDocument document = new ();
			document.Load (defaultXml);

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

			Console.ForegroundColor = ConsoleColor.Red;
			Console.Write ("No items found with name ");
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.Write (name);
			Console.ForegroundColor = ConsoleColor.White;
		}

		public static void List ()
		{
			string defaultXml = CheckDirectory () + "/default.xml";

			XmlDocument document = new ();
			document.Load (defaultXml);

			XmlNodeList nodes = document.DocumentElement.SelectNodes ("Item");

			foreach (XmlNode node in nodes)
			{
				Console.WriteLine ("	" + node["Name"].InnerText);
			}
		}

		public static void Edit (Item item)
		{
			if (!CheckNulls (item))
			{
				return;
			}

			string defaultXml = CheckDirectory () + "/default.xml";

			XmlDocument document = new ();
			document.Load (defaultXml);

			XmlNodeList nodes = document.DocumentElement.SelectNodes ("Item");

			foreach (XmlNode node in nodes)
			{
				if (node["Name"].InnerText == item.Name)
				{
					node["Content"].InnerText = item.Content;

					document.Save (defaultXml);

					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine ("✓ Successfully edited the item!");
					Console.ForegroundColor = ConsoleColor.White;
				}
			}
		}

		private static string CheckDirectory ()
		{
			string env = $"C:/Users/{Environment.UserName}/Documents";

			if (!Directory.Exists (env + "/keepsheets"))
			{
				Directory.CreateDirectory (env + "/keepsheets");
			}

			return env + "/keepsheets";
		}

		private static Item GetItem (string name)
		{
			string directory = CheckDirectory ();

			string file = directory + "/default.xml";

			XmlDocument document = new ();
			document.Load (file);

			XmlNodeList nodes = document.DocumentElement.SelectNodes ("Item");

			foreach (XmlNode node in nodes)
			{
				if (node["Name"].InnerText == name)
				{
					return new Item { Name = node["Name"].InnerText, Content = node["Content"].InnerText };
				}
			}

			return null;
		}

		private static void EditNotepad (string name)
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
				
				return;
			}

			Keep (new Item { Name = name, Content = finalContent });

			File.Delete (filePath);
			#endregion
		}

		private static bool CheckNulls (Item item, [CallerMemberName] string callerName = "")
		{
			if (item == null || item.Name == null || item.Content == null || string.IsNullOrWhiteSpace (item.Name) || string.IsNullOrWhiteSpace (item.Content))
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