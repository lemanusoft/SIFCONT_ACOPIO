using System;
using Android.Text.Format;
namespace SIFCONT
{
	public class clsVentas_D
	{
		  public int NoDoc=0;
		  public DateTime FechaDoc= new DateTime();
		  public String Comentario= "";
		  public String IdBodega = "";
		  public String Usuario = "";
		  public String Cod_Cliente="";
		  public bool Exonerado=false;
		  public String Autorización_DGI="";
		  public bool Crédito=false;
		  public short Cod_Lista_Precios=0;
		  public int Cod_Vendedor=0;
		  public int Plazo=0;
		  public String Dirección="";
		  public String Identificación="";
		  public  Double Total=0.00;
		  public String strErrorMsg="";


		public bool Ubicar(int nNoDoc){
			bool rs =false;
			String strSql = String.Format("select * from Ventas_D where NoDoc= {0}", nNoDoc);
			System.Data.DataSet tmpDs= new System.Data.DataSet();
			tmpDs = MainActivity.LlenarDataSet (strSql);
			if (MainActivity.TieneDatos (tmpDs)) {
				foreach (System.Data.DataTable tbl in tmpDs.Tables) {
					foreach (System.Data.DataRow row in tbl.Rows) {
						rs=true;
						NoDoc=nNoDoc;
						FechaDoc = DateTime.Parse(row[1].ToString());
						Comentario= row[2].ToString();
						IdBodega = row[3].ToString();
						Usuario = row[4].ToString();
						Cod_Cliente=row[5].ToString();
						Exonerado=bool.Parse(row[6].ToString());
						Autorización_DGI=row[7].ToString();
						Crédito=bool.Parse(row[8].ToString());
						Cod_Lista_Precios=short.Parse(row[9].ToString());
						Cod_Vendedor=int.Parse(row[10].ToString());
						Plazo=int.Parse(row[11].ToString());
						Dirección=row[12].ToString();
						Identificación=row[13].ToString();
						strErrorMsg="";
						Total=CalcularTotal();
					}
				}
			}
			tmpDs.Dispose ();
			return  rs;
		}

		public bool AgregarProducto( int nCantidad, bool EsExonerado, double nDescuentoPorciento, double nDescuentoTotal, int nBonificado, clsProductos objProducto, clsPrecios objPrecio) {
			bool rs = false;
			int nItem=0;
			int nItems=0;
			String strSql ="";
			if(MainActivity.objUsuario.Cod_Vendedor > 0 && MainActivity.objUsuario.Usuario.Trim().Length>0)
			{
				String strSqlVerificar= String.Format("SELECT COUNT(*) AS ITEMS FROM VENTAS_DD WHERE NODOC = {0}", NoDoc);
				nItems=MainActivity.EjecutarInt(strSqlVerificar);
				if(nItems >= 15){
					strErrorMsg="Ha sobrepasado el máximo de 15 productos";
				}
				else {
					strSqlVerificar = String.Format("SELECT item FROM VENTAS_DD WHERE NODOC = {0} and Cod_Producto='{1}'", NoDoc, objProducto.Cod_Producto.Replace("'","''"));
					nItems=MainActivity.EjecutarInt(strSqlVerificar);
					if(nItem > 0){
						strSql  = String.Format("update VENTAS_DD set  Cantidad={0}, Precio={1}, Exonerado={2}, Descuento_Porciento={3}, Descuento_Total={4}, Bonificiado={5}, Retener_IVA={6}, LP={7} where Item={8}",
							nCantidad, objPrecio.Precio, EsExonerado==true?1:0, nDescuentoPorciento, nDescuentoTotal, nBonificado, objProducto.Retiene_IVA==true?1:0, objPrecio.Cod_Lista_Precios, nItem);
					}
					else {
						strSql ="INSERT INTO VENTAS_DD(NoDoc, Cod_Producto, Cantidad, Precio, Exonerado, Descuento_Porciento, Descuento_Total, Bonificiado, Retener_IVA, LP)";
						strSql  += String.Format(" VALUES({0}, '{1}', {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9})", NoDoc, objProducto.Cod_Producto.Replace("'","''"), nCantidad, objPrecio.Precio, EsExonerado==true?1:0, nDescuentoPorciento, nDescuentoTotal, nBonificado, objProducto.Retiene_IVA==true?1:0, objPrecio.Cod_Lista_Precios);

					}
					try{
						rs=MainActivity.Ejecutar(strSql);
						Total=CalcularTotal();
						if(rs==true) {strErrorMsg="Producto agregado al pedido";} else {strErrorMsg="";};
					} catch (Exception ex) {
						strErrorMsg=ex.ToString();
					}
				}
			} else
			{
				strErrorMsg="No fue posible determinar Código de Vendedor o Usuario";
			}


			return  rs;
		}

		 public  bool Actualizar(String sCodCliente, short nExonerado, short nCrédito, short nCodListaPrecios, int nCodVendedor, int nPlazo, String sDirección, String sLatitud, String sLongitud){
			try {
				bool rs=false;
				String strSql =String.Format("UPDATE VENTAS_D SET FechaDoc=date('Now'), Usuario= '{0}', Cod_Cliente='{1}', Exonerado={2}, Crédito={3}, Cod_Lista_Precios={4}, Cod_Vendedor={5}, Plazo={6}, Latitud='{7}', Longitud='{8}' where nodoc={9}",  MainActivity.objUsuario.Usuario.Replace("'","''"), sCodCliente.Replace("'","''"),nExonerado, nCrédito, nCodListaPrecios, nCodVendedor, nPlazo, sLatitud, sLongitud, NoDoc);
				rs=MainActivity.Ejecutar(strSql);
				return rs;
			} catch (Exception ex) {
				strErrorMsg = ex.Message;
				return false;
			}
		}

		public  bool Anular() {
			bool rs = false;
			String strSql =String.Format("UPDATE VENTAS_D SET FechaDoc=date('Now'), Usuario= '{0}', Cod_Cliente='', Exonerado=0, Crédito=0, Cod_Lista_Precios=1, Cod_Vendedor={1}, Plazo=0, Dirección='' where nodoc={2}",  MainActivity.objUsuario.Usuario.Replace("'","''"), MainActivity.objUsuario.Cod_Vendedor,this.NoDoc);
			MainActivity.Ejecutar(strSql);
			strSql=String.Format("DELETE FROM VENTAS_DD WHERE NODOC = {0}", NoDoc);
			rs = MainActivity.Ejecutar(strSql);
			this.Total=0.00;
			return rs;
		}

		public  bool Aplicar() {
			bool rs = false;
			String strSql =String.Format("UPDATE VENTAS_D SET Aplicado=1, FechaDoc=date('Now') where nodoc={0}",  NoDoc);
			rs = MainActivity.Ejecutar(strSql);
			return rs;
		}

		public  bool Quitar(String sCodProducto) {
			bool rs = false;
			String strSql =String.Format("DELETE FROM VENTAS_DD where nodoc={0} and Cod_Producto = '{1}'",  NoDoc, sCodProducto.Replace("'","''"));
			rs = MainActivity.Ejecutar(strSql);
			Total=CalcularTotal();
			return rs;
		}

		public  bool Nuevo(bool Es_Rellamado){
			bool rs = false;
			DateTime tmr = new DateTime (); 
			tmr = DateTime.Now;

			String strSql = String.Format ("select * from Ventas_D where Aplicado=0");
			System.Data.DataSet tmpDs= new System.Data.DataSet();
			tmpDs = MainActivity.LlenarDataSet (strSql);
			if (MainActivity.TieneDatos (tmpDs)) {
				foreach (System.Data.DataTable tbl in tmpDs.Tables) {
					foreach (System.Data.DataRow row in tbl.Rows) {
						rs=true;
						NoDoc=int.Parse(row[0].ToString());
						FechaDoc = DateTime.Parse(row[1].ToString());
						Comentario= row[2].ToString();
						IdBodega = row[3].ToString();
						Usuario = row[4].ToString();
						Cod_Cliente=row[5].ToString();
						Exonerado=bool.Parse(row[6].ToString());
						Autorización_DGI=row[7].ToString();
						Crédito=bool.Parse(row[8].ToString());
						Cod_Lista_Precios=short.Parse(row[9].ToString());
						Cod_Vendedor=int.Parse(row[10].ToString());
						Plazo=int.Parse(row[11].ToString());
						Dirección=row[12].ToString();
						Identificación=row[13].ToString();
						strErrorMsg="";
						Total=CalcularTotal();
					}
				}
			}
			else
			{
				strSql="insert into Ventas_D(FechaDoc, Comentario, IdBodega, Usuario, Cod_Cliente, Exonerado, Autorización_DGI, Crédito, Cod_Lista_Precios, Cod_Vendedor, Plazo, Dirección, Identificacion, Aplicado, Sincronizado) Values(date('now'), '', '', 'Usuario', '', 0, '', 0, 0, 0, 0, 'Dirección', 'Identificacion', 0, 0);";
				MainActivity.Ejecutar(strSql);
				strErrorMsg="";
				if(!Es_Rellamado){
					Nuevo(true);
				}
			}
			tmpDs.Dispose ();
			return  rs;
		}

		public Double CalcularTotal(){
			Double rs=0.00;
			String strSql="SELECT SUM(DD.CANTIDAD*DD.PRECIO*RETENER_IVA) AS [GRAVADO] ";
			strSql+=", SUM(DD.CANTIDAD*DD.PRECIO*ABS(RETENER_IVA-1)) AS [ EXENTO]";
			strSql+=", SUM(DD.DESCUENTO_TOTAL) AS DESCUENTO";
			strSql+=", SUM((DD.CANTIDAD*DD.PRECIO - DD.DESCUENTO_TOTAL)*RETENER_IVA*0.15) AS [TOTAL IVA]";
			strSql+=" FROM VENTAS_DD DD ";
			strSql+=String.Format(" WHERE DD.NODOC = {0};", NoDoc);
			System.Data.DataSet tmpDs= new System.Data.DataSet();
			tmpDs = MainActivity.LlenarDataSet (strSql);
			if (MainActivity.TieneDatos (tmpDs)) {
				foreach (System.Data.DataTable tbl in tmpDs.Tables) {
					foreach (System.Data.DataRow row in tbl.Rows) {
						rs += double.Parse (row [0].ToString ());
						rs += double.Parse (row [1].ToString ());
						rs -= double.Parse (row [2].ToString ());
						rs += double.Parse(row[3].ToString());
					}
				}
			}
				
			return rs;
		}

		public int Total_Pedidos_a_Sincronizar(){
			int Rs =0;
			String strSql = "select sum(1) Pedidos FROM Ventas_D where Aplicado= 1 and Sincronizado= 0;";
			Rs = MainActivity.EjecutarInt(strSql);
			return Rs;
		}

		public void Nuevo(){
			NoDoc=0;
			FechaDoc= DateTime.Now;
			Comentario= "";
			IdBodega = "";
			Usuario = "";
			Cod_Cliente="";
			Exonerado=false;
			Autorización_DGI="";
			Crédito=false;
			Cod_Lista_Precios=0;
			Cod_Vendedor=MainActivity.objUsuario.Cod_Vendedor;
			Plazo=0;
			Dirección="";
			Identificación="";
			Total=0.00;
			strErrorMsg="";	
		}

		public clsVentas_D ()
		{
			
		}
	}
}

