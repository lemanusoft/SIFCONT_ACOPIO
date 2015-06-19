using System;

namespace SIFCONT
{
	public class clsListaPrecios
	{
		 public int Cod_Lista_Precios=0;
		 public String Lista_Precios="";
		 public bool Activo=false;
		 public String strMensaje="";

		 public bool Ubicar(int nNum){
			bool rs =false;
			String strSql = String.Format("select Cod_Lista_Precios, Lista_Precios, Activo FROM Lista_Precios where Cod_Lista_Precios= {0}", nNum);
			System.Data.DataSet tmpDs= new System.Data.DataSet();
			tmpDs = MainActivity.LlenarDataSet (strSql);
			if (MainActivity.TieneDatos (tmpDs)) {
				foreach (System.Data.DataTable tbl in tmpDs.Tables) {
					foreach (System.Data.DataRow row in tbl.Rows) {
						rs=true;
						Cod_Lista_Precios= nNum;
						Lista_Precios = row[1].ToString();
						Activo= bool.Parse(row[2].ToString());
					}
				}
			}
			tmpDs.Dispose ();
			return  rs;
		}

		public void Nuevo(){
			Cod_Lista_Precios=0;
			Lista_Precios="";
			Activo=false;
			strMensaje="";
		}
		public clsListaPrecios ()
		{
		}
	}
}

