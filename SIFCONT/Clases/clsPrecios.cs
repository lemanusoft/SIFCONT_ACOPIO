using System;

namespace SIFCONT
{
	public class clsPrecios
	{
		 public int Cod_Lista_Precios=0;
		 public String Cod_Producto="";
		 public Double Precio=0.00;
		 public  Double Precio_con_IVA=0.00;
		 public String strMensaje="";

		 public bool Ubicar(int nCod_Lista_Precios, String sCod_Producto){
			bool rs =false;
			String strSql = String.Format("select Precio FROM Precios where Cod_Lista_Precios= {0} and Cod_Producto = '{1}'", nCod_Lista_Precios, sCod_Producto.Replace("'","''"));
			System.Data.DataSet tmpDs= new System.Data.DataSet();
			tmpDs = MainActivity.LlenarDataSet (strSql);
			if (MainActivity.TieneDatos (tmpDs)) {
				foreach (System.Data.DataTable tbl in tmpDs.Tables) {
					foreach (System.Data.DataRow row in tbl.Rows) {
						rs=true;
						Precio= double.Parse(row[0].ToString());
						Cod_Lista_Precios=nCod_Lista_Precios;
						Cod_Producto=sCod_Producto;

						clsProductos objProds=new clsProductos();
						if(objProds.Ubicar(Cod_Producto)==true){
							if(objProds.Retiene_IVA==true){
								Precio_con_IVA=double.Parse(row[0].ToString())*1.15;
							}
							else
							{
								Precio_con_IVA=double.Parse(row[0].ToString());
							}
						}

					}
				}
			}
			tmpDs.Dispose ();
			return  rs;
		}

		public  void Nuevo(){
			Cod_Lista_Precios=0;
			Cod_Producto="";
			Precio=0.00;
			Precio_con_IVA=0.00;
			strMensaje="";
		}

		public clsPrecios ()
		{
			Nuevo ();
		}
	}
}

