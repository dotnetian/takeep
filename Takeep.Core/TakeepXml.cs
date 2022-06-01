using System.Xml;

namespace Takeep.Core
{
	public class TakeepXml
	{
		public static void Keep (Item item)
		{
			#region Check Nulls

			if (item == null)
			{
				throw new ArgumentNullException (nameof (item));
			}

			#endregion

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
		}

		public static Item Take (string name)
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
					return new Item
					{
						Name = name,
						Content = node["Content"].InnerText
					};
				}
			}

			return null;
		}

		public static void Remove (string name)
		{
			string defaultXml = CheckDirectory () + "/default.xml";

			XmlDocument document = new ();
			document.Load (defaultXml);

			XmlNodeList nodes = document.DocumentElement.SelectNodes ("Item");

			foreach (XmlNode node in nodes)
			{
				if (node["Name"].InnerText == name)
				{
					node.ParentNode.RemoveChild(node);
					document.Save (defaultXml);
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
	}
}