using System;
using Java.Sql;

namespace SIFCONT
{
	public class clsBDRemotaMSSQL
	{
		public Java.Sql.IConnection conSQL;
		public String strError ="";
		String sLogin="SA";
		String sPass="ABC*123";
		String sServer="192.168.0.25";
		String sBD="SIVC4";

		public bool ConnectToDatabase(){
			try {
				Java.Lang.Class.ForName ("net.sourceforge.jtds.jdbc.Driver").NewInstance();	
				String url = String.Format("jdbc:jtds:sqlserver://{0}:1433/{1}", sServer,sBD);
				//conSQL= DriverManager.GetConnection(url, sLogin, sPass);
				return true;
			} catch (NullReferenceException ex) {
				strError = ex.Message;
				return false;
			}
		}

		public clsBDRemotaMSSQL ()
		{
			

		}
	}
}

