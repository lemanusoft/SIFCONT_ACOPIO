using System;

namespace SIFCONT
{
	public class clsPD_X_Cliente
	{
		 public int Num=0;
		 public String Cod="";
		 public String Cod_Producto="";
		 public int Tipo_Condición=1;
		 public int Valor_Condición=0;
		 public double Descuento=0.00;
		  public String strMensaje="";

		 public double Obtener_Descuento(String sCod, String sCod_Producto, int nCantidad){
			String strSql = String.Format("select * FROM PD_x_Clientes where Cod= '{0}' and Cod_Producto = '{1}'", sCod.Replace("'","''"), sCod_Producto.Replace("'","''"));
			System.Data.DataSet tmpDs= new System.Data.DataSet();
			tmpDs = MainActivity.LlenarDataSet (strSql);
			if (MainActivity.TieneDatos (tmpDs)) {
				foreach (System.Data.DataTable tbl in tmpDs.Tables) {
					foreach (System.Data.DataRow row in tbl.Rows) {
						Num= int.Parse(row[0].ToString());
						Cod = sCod;
						Cod_Producto = sCod_Producto;
						Tipo_Condición=int.Parse(row[3].ToString());
						Valor_Condición=int.Parse(row[4].ToString());
						Descuento=0.00;
						if(Tipo_Condición==1){
							Descuento=double.Parse(row[5].ToString());
						} else {
							if(Tipo_Condición==2 && nCantidad >= Valor_Condición){
								Descuento=double.Parse(row[5].ToString());
							} else {
								if(Tipo_Condición==3 && nCantidad%Valor_Condición==0){
									Descuento=double.Parse(row[5].ToString());
								}
							}
						}
					}
				}
			}


			return  Descuento;
		}

		 public bool Ubicar(int nNum){
			bool rs =false;
			String strSql = String.Format("select * FROM PD_x_Clientes where Num= {0}", nNum);
			System.Data.DataSet tmpDs= new System.Data.DataSet();
			tmpDs = MainActivity.LlenarDataSet (strSql);
			if (MainActivity.TieneDatos (tmpDs)) {
				foreach (System.Data.DataTable tbl in tmpDs.Tables) {
					foreach (System.Data.DataRow row in tbl.Rows) {
						rs=true;
						Num= nNum;
						Cod = row[1].ToString();
						Cod_Producto = row[2].ToString();
						Tipo_Condición=int.Parse(row[3].ToString());
						Valor_Condición=int.Parse(row[4].ToString());
						Descuento=Double.Parse(row[5].ToString());
					}
				}
			}
			tmpDs.Dispose ();
			return  rs;
		}


		public  void Nuevo(){
			Num=0;
			Cod="";
			Cod_Producto="";
			Tipo_Condición=1;
			Valor_Condición=0;
			Descuento=0.00;
			strMensaje="";
		}
		public clsPD_X_Cliente ()
		{
		}
	}
}

