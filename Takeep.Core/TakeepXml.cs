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

			StreamReader file = new (xmlPath);

			if (File.Exists (xmlPath))
			{
				// TODO: Continue
			}

			#endregion

			var serializer = new XmlSerializer (typeof (Item));



			/* #region Write
			var b = new Book
			{
				Title = "Matin's book"
			};

			var writer = new XmlSerializer (typeof (Book));

			var wfile = new StreamWriter ("C:/users/matin/desktop/XMLTest.xml");

			writer.Serialize (wfile, b);

			wfile.Close ();
			#endregion
			#region Read
			XmlSerializer reader = new (typeof (Book));

			StreamReader file = new ("C:/users/matin/desktop/XMLTest.xml");

			Book overview = (Book)reader.Deserialize (file);

			file.Close ();

			Console.WriteLine (overview.Title);
			#endregion */
		}
	}
}