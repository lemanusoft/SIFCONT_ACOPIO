using System;

namespace SIFCONT
{
	public class clsDispositivo
	{
		 public int Num=0;
		 public String Nombre="";
		 public String Marca="";
		 public String Modelo="";
		 public String Asignado_A="";
		 public String Estado="";
		 public String Observaciones="";
		 public int Cod_Vendedor=0;
		 public String strMensaje="";
		 public short Es_Sincroniza_Todos_los_Clientes=0;

		 public bool Ubicar( bool repetir){
			bool rs =false;
			try{
				String strSql = String.Format("select * FROM Disp_Móviles where upper(Nombre) = '{0}'", MainActivity.NombreDispositivo.Trim().Replace("'","''").ToUpper());
				System.Data.DataSet tmpDs= new System.Data.DataSet();
				tmpDs = MainActivity.LlenarDataSet (strSql);
				if (MainActivity.TieneDatos (tmpDs)) {
					foreach (System.Data.DataTable tbl in tmpDs.Tables) {
						foreach (System.Data.DataRow row in tbl.Rows) {
							rs=true;
							Num= int.Parse(row[0].ToString());
							Nombre = row[1].ToString();
							Marca=row[2].ToString();
							Modelo =row[3].ToString();
							Asignado_A=row[4].ToString();
							Estado=row[5].ToString();
							Observaciones=row[6].ToString();
							Cod_Vendedor=int.Parse(row[7].ToString());
							Es_Sincroniza_Todos_los_Clientes=short.Parse(row[8].ToString());
						}
					}
				}
				tmpDs.Dispose();


			} catch (Exception e){
				strMensaje = e.ToString ();
			}

			return  rs;
		}

		public  void Nuevo(){
			Num=0;
			Nombre="";
			Marca="";
			Modelo="";
			Asignado_A="";
			Estado="";
			Observaciones="";
			Cod_Vendedor=0;
			strMensaje="";
			Es_Sincroniza_Todos_los_Clientes=0;
		}
		public clsDispositivo ()
		{
		}
	}
}

