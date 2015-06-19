using System;

namespace SIFCONT
{
	public class clsVendedor
	{
		public int Cod_Vendedor=0;
		public String Usuario_Vendedor="";
		public bool Activo=false;
		public String strMensaje="";

		public bool Ubicar(int nNum){
			bool rs =false;
			String strSql = String.Format("select Cod_Vendedor, Usuario_Vendedor, Activo FROM Vendedores where Cod_Vendedor= {0}", nNum);
			System.Data.DataSet tmpDs= new System.Data.DataSet();
			tmpDs = MainActivity.LlenarDataSet (strSql);
			if (MainActivity.TieneDatos (tmpDs)) {
				foreach (System.Data.DataTable tbl in tmpDs.Tables) {
					foreach (System.Data.DataRow row in tbl.Rows) {
						rs=true;
						Cod_Vendedor= nNum;
						Usuario_Vendedor = row[1].ToString();
						Activo=bool.Parse(row[2].ToString());
					}
				}
			}
			tmpDs.Dispose();
			return  rs;
		}
		public void Nuevo(){
			Cod_Vendedor=0;
			Usuario_Vendedor = "";
			Activo = false;
			strMensaje = "";
		}
		public clsVendedor ()
		{
			Nuevo ();
		}
	}
}

