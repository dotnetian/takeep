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

			if (!File.Exists (xmlPath))
			{
				var stramDefaultXmlInit = new StreamWriter (xmlPath);
			}

			var stramDefaultXml = new StreamWriter (xmlPath);

			#endregion

			var serializer = new XmlSerializer (typeof (Item));

			serializer.Serialize (stramDefaultXml, item);
		}
	}
}