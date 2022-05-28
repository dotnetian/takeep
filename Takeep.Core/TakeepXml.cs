using System.Text.Unicode;
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

			#region Check Directory

			//string env = Environment.CurrentDirectory;
			string appCall = Environment.ProcessPath;
			int pathIndex = appCall.LastIndexOf ('\\');
			string env = appCall.Substring (0, pathIndex);

			if (!Directory.Exists (env + "/keepsheets"))
			{
				Directory.CreateDirectory (env + "/keepsheets");
			}

			string directory = env + "/keepsheets";

			#endregion

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
	}
}