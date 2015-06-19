using System;

namespace SIFCONT
{
	public class clsZona
	{
		public int Cod_Zona=0;
		public String Zona="";
		public String strMensaje="";

		public bool Ubicar(int nNum){
			bool rs =false;
			String strSql = String.Format("select Cod_Zona, Zona FROM Zonas where Cod_Zona= {0}", nNum);
			System.Data.DataSet tmpDs= new System.Data.DataSet();
			tmpDs = MainActivity.LlenarDataSet (strSql);
			if (MainActivity.TieneDatos (tmpDs)) {
				foreach (System.Data.DataTable tbl in tmpDs.Tables) {
					foreach (System.Data.DataRow row in tbl.Rows) {
						rs=true;
						Cod_Zona= nNum;
						Zona = row[1].ToString();
					}
				}
			}
			tmpDs.Dispose ();
			return  rs;
		}

		public void Nuevo(){
			Cod_Zona=0;
			Zona="";
			strMensaje="";
		}
		public clsZona ()
		{
			Nuevo ();
		}
	}
}

