using System;

namespace SIFCONT
{
	public class clsCarteraClientes
	{
		 public String Cod="";
		 public String Nombre="";
		 public double Límite=0.00;
		 public double Saldo=0.00;
		 public short Plazo= 0;
		 public bool Activo = false;
		 public String Dirección="";
		 public String Teléfonos="";
		 public String RUC="";
		 public int Cod_Vendedor=0;
		 public double Descuento= 0.00;
		 public short Cod_Lista_Precios=0;
		 public String Comentario="";
		 public int Cod_Zona=0;
		 public String strMensaje="";

		 public bool Ubicar(String sCod){
			bool rs =false;
			String strSql = "select * FROM Clientes_Crédito where Cod LIKE '%";
			strSql += String.Format("{0}' ORDER BY COD", sCod.Replace("'", "''"));
			if(sCod.Length>0) {
				System.Data.DataSet tmpDs= new System.Data.DataSet();
				tmpDs = MainActivity.LlenarDataSet (strSql);
				if (MainActivity.TieneDatos (tmpDs)) {
					foreach (System.Data.DataTable tbl in tmpDs.Tables) {
						foreach (System.Data.DataRow row in tbl.Rows) {
							rs = true;
							Cod = sCod;
							Nombre = row[1].ToString();
							Límite =double.Parse(row[2].ToString());
							Plazo =short.Parse(row[3].ToString());
							Saldo =double.Parse( row[4].ToString());
							Activo =bool.Parse( row[5].ToString());
							Dirección = row[6].ToString();
							Teléfonos = row[7].ToString();
							RUC = row[8].ToString();
							Cod_Vendedor =int.Parse( row[9].ToString());
							Descuento =double.Parse( row[10].ToString());
							Cod_Lista_Precios =short.Parse( row[11].ToString());
							Comentario = row[12].ToString();
							Cod_Zona =int.Parse( row[13].ToString());
						}
					}
				}
				tmpDs.Dispose ();
			}
			return  rs;
		}

		 public void Nuevo(){
			Cod="";
			Nombre="";
			Límite=0.00;
			Saldo=0.00;
			Plazo= 0;
			Activo = false;
			Dirección="";
			Teléfonos="";
			RUC="";
			Cod_Vendedor=0;
			Descuento= 0.00;
			Cod_Lista_Precios=0;
			Comentario="";
			Cod_Zona=0;
		}
		public clsCarteraClientes ()
		{
			Nuevo ();
		}
	}
}

