using System.Runtime.CompilerServices;
using System.Xml;

namespace Takeep.Core
{
	public class TakeepXml
	{
		public static void Keep (Item item)
		{
			if (!CheckNulls (item))
			{
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