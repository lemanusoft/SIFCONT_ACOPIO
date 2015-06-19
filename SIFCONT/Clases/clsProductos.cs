using System;

namespace SIFCONT
{
	public class clsProductos
	{
		public int IdProducto=0;
		public String Cod_Producto="";
		public String Descripción="";
		public bool Retiene_IVA=false;
		public bool Es_Servicio=false;
		public bool Activo=false;
		public bool Aplica_Descuento=false;
		public double Descuento_Máximo=0.00;
		public String Marca="";
		public String Cod_Presentación="";
		public String strMensaje="";

		public bool Ubicar(String sCod_Producto){
			bool rs =false;
			try {
				String strSql = String.Format("select IdProducto, Cod_Producto, Descripción, Retiene_IVA, Es_Servicio, Activo, Aplica_Descuento, Descuento_Maximo, Marca, Cod_Presentación FROM Productos where Cod_Producto= '{0}'", sCod_Producto.Replace("'","''"));
				System.Data.DataSet tmpDs= new System.Data.DataSet();
				tmpDs = MainActivity.LlenarDataSet (strSql);
				if (MainActivity.TieneDatos (tmpDs)) {
					foreach (System.Data.DataTable tbl in tmpDs.Tables) {
						foreach (System.Data.DataRow row in tbl.Rows) {
							rs=true;
							IdProducto= int.Parse(row[0].ToString());
							Cod_Producto = sCod_Producto;
							Descripción=row[2].ToString();
							Retiene_IVA=bool.Parse(row[3].ToString());
							Es_Servicio=bool.Parse(row[4].ToString());
							Activo=bool.Parse(row[5].ToString());
							Aplica_Descuento=bool.Parse(row[6].ToString());
							Descuento_Máximo=int.Parse(row[7].ToString());
							Marca=row[8].ToString();
							Cod_Presentación=row[9].ToString();
						}
					}
				}
				tmpDs.Dispose ();
			} catch (Exception e){
				strMensaje=e.ToString();
			}
			return  rs;
		}

		public void Nuevo(){
			IdProducto=0;
			Cod_Producto="";
			Descripción="";
			Retiene_IVA=false;
			Es_Servicio=false;
			Activo =false;
			Aplica_Descuento=false;
			Descuento_Máximo=0.00;
			Marca="";
			Cod_Presentación="";
			strMensaje="";
		}
		public clsProductos ()
		{
			Nuevo ();
		}

	}
}

