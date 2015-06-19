using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace SIFCONT
{
	public class clsUti
	{
		public int MsgBox(Context Cntx, String sMsg, String sTitle){
			AlertDialog.Builder dlgAlert  = new AlertDialog.Builder(Cntx);
			dlgAlert.SetMessage(sMsg);
			dlgAlert.SetTitle(sTitle);
			dlgAlert.SetPositiveButton("OK", (senderAlert, args) => {});
			dlgAlert.SetCancelable(true);
			dlgAlert.Create().Show();
			return 0;
		}
		public clsUti ()
		{
		}
	}
}

