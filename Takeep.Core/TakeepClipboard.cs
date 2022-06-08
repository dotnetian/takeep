using System.Runtime.InteropServices;

namespace Takeep.Core
{
	public class TakeepClipboard
	{
		[DllImport ("user32.dll")]
		internal static extern bool OpenClipboard (IntPtr hWndNewOwner);

		[DllImport ("user32.dll")]
		internal static extern bool CloseClipboard ();

		[DllImport ("user32.dll")]
		internal static extern bool SetClipboardData (uint uFormat, IntPtr data);

		public static void Copy (string yourString)
		{
			OpenClipboard (IntPtr.Zero);
			var ptr = Marshal.StringToHGlobalUni (yourString);
			SetClipboardData (13, ptr);
			CloseClipboard ();
			Marshal.FreeHGlobal (ptr);
		}
	}
}
