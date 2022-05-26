using System.Xml;
using System.Xml.Serialization;

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

			string env = Environment.CurrentDirectory;

			if (!Directory.Exists (env + "/keepsheets"))
			{
				Directory.CreateDirectory (env + "/keepsheets");
			}

			string directory = env + "/keepsheets";

			#endregion

			#region Check File

			string xmlPath = directory + "/default.xml";

			var stramDefaultXml = new StreamWriter (xmlPath);

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

			//xmlDefault.DocumentElement.AppendChild (parentItem);
			xmlDefault.AppendChild (parentItem);

			xmlDefault.Save (stramDefaultXml);

			stramDefaultXml.Close ();
		}
	}
}