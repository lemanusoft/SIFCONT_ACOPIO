using System;

namespace SIFCONT
{
	public class clsUsuarios
	{
		public  String Usuario="";
		public    String Contraseña="";
		public  String Nombres="";
		public  String Apellidos="";
		public  int Num =0;
		public    int Cod_Vendedor=0;
		public    String strMensaje="";

		void  Nuevo(){
			Num= 0;
			Usuario ="";
			Nombres = "";
			Apellidos = "";
			Contraseña = "";
			Cod_Vendedor=0;
		}

		public bool  VerificarUsuario(String sUsuario, String sContraseña){
			try {
				bool rs = false;
				String strSql = String.Format("select Num, Usuario, Nombres, Apellidos, Contraseña, Cod_Vendedor from Usuario where UPPER(Usuario)= UPPER('{0}') and Contraseña = '{1}'", sUsuario.Replace("'","''"), sContraseña.Replace("'","''"));
				System.Data.DataSet tmpDs= new System.Data.DataSet();
					tmpDs = MainActivity.LlenarDataSet (strSql);
					if (MainActivity.TieneDatos(tmpDs)) {
						foreach (System.Data.DataTable tbl in tmpDs.Tables) {
							foreach (System.Data.DataRow row in tbl.Rows) {
								rs=true;
							Num= int.Parse(row[0].ToString());
								Usuario = sUsuario;
							Nombres = row[2].ToString();
							Apellidos = row[3].ToString();
							Contraseña = row[4].ToString();
							Cod_Vendedor=int.Parse(row[5].ToString());
							}
						}
					}
				tmpDs.Dispose();
				return  rs;
			} catch (Exception ex) {
				strMensaje = ex.Message;
				return false;
			}
		}

		public clsUsuarios ()
		{
			Nuevo ();
		}
	}
}

