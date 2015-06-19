using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Mono.Data.Sqlite;
using System.IO;
using Android.Content.PM;


namespace SIFCONT
{
	[Activity (Label = "SIFCONT", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		public static String NombreDispositivo="";
		public const String NombreBaseDatosSQLite = "SIFCONT.db3";
		public static String MensajeError="";

		public static String documents = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
		public static String RutaBaseDatosSQLite = Path.Combine(documents, "SIFCONT.db3");
		public static String CadenaConexion =String.Format("Data Source={0};Version=3;", RutaBaseDatosSQLite);

		public static clsVendedor objVendedor = new clsVendedor();

		/*
Objetos de LeMaNú Software. para consumo de datos
		*/
		public static clsUsuarios objUsuario = new clsUsuarios();

		public clsBDRemotaMSSQL objSQL;

		public static bool TieneDatos(System.Data.DataSet tmpDs) {
			try {
				bool rs=false;
				if(tmpDs != null){
					if(tmpDs.Tables.Count>0){
						foreach (System.Data.DataTable tbl in tmpDs.Tables) {
							if(tbl.Rows.Count>0){
								rs=true;
							}
						}
					}
				}
				return rs;
			} catch (Exception ex) {
				MensajeError = ex.Message;
				return false;
			}
		}

		public static System.Data.DataSet LlenarDataSet(String strSql){
			try {
				System.Data.DataSet tmpDs = new System.Data.DataSet();
				using( Mono.Data.Sqlite.SqliteConnection tmpCn = new Mono.Data.Sqlite.SqliteConnection(CadenaConexion))
				{
					tmpCn.Open();
					if(tmpCn != null)
					{
						using(Mono.Data.Sqlite.SqliteCommand tmpCm = new Mono.Data.Sqlite.SqliteCommand(strSql,tmpCn ))
						{
							using(Mono.Data.Sqlite.SqliteDataAdapter tmpDa = new Mono.Data.Sqlite.SqliteDataAdapter(tmpCm)){
								tmpDa.Fill(tmpDs);
							}
						}
					}
				}
				return tmpDs;
			} catch (Exception ex) {
				MensajeError = ex.Message;
				return null;
			}
		}
		public static int EjecutarInt(String strSql){
			try {
				int rs=0;
				using( Mono.Data.Sqlite.SqliteConnection tmpCn = new Mono.Data.Sqlite.SqliteConnection(CadenaConexion))
				{
					tmpCn.Open();
					if(tmpCn != null)
					{
						using(Mono.Data.Sqlite.SqliteCommand tmpCm = new Mono.Data.Sqlite.SqliteCommand(tmpCn))
						{
							tmpCm.CommandText=(strSql);
							rs=int.Parse(tmpCm.ExecuteScalar().ToString());
						}
					}
				}
				return rs;
			} catch (Exception ex) {
				MensajeError = ex.Message;
				return 0;
			}
		}

		static public bool Ejecutar(String strSql){
			try {
				bool rs=false;
				using( Mono.Data.Sqlite.SqliteConnection tmpCn = new Mono.Data.Sqlite.SqliteConnection(CadenaConexion))
				{
					tmpCn.Open();
					if(tmpCn != null)
					{
						using(Mono.Data.Sqlite.SqliteCommand tmpCm = new Mono.Data.Sqlite.SqliteCommand(tmpCn))
						{
							tmpCm.CommandText=(strSql);
							tmpCm.ExecuteNonQuery();
							rs=true;
						}
					}
				}
				return rs;
			} catch (Exception ex) {
				MensajeError = ex.Message;
				return false;
			}
		}

		public static bool AbrirBaseDatos(){
			bool rs = false;
			try {
				if(VerificarBaseDatos())
				{
					rs=true;
					MensajeError+=" Conectado a base de datos local";
				}
				else{
					rs=InicializarBD();
				}
				return rs;
			} catch (Exception ex) {
				MensajeError += ex.Message;
				return false;		
			}

		}

		public static bool VerificarBaseDatos(){
			bool rs = false;
			try {
				using( Mono.Data.Sqlite.SqliteConnection tmpCn = new Mono.Data.Sqlite.SqliteConnection(CadenaConexion))
				{
					if (!System.IO.File.Exists(RutaBaseDatosSQLite)) {
						rs=InicializarBD();
					}
					else {
						tmpCn.Open();
						if (tmpCn != null) {
							tmpCn.Close();
							rs=true;
						}
//						else 
//						{
//							rs=InicializarBD();
//						}
					}

				}			
				return rs;
			} catch (Exception ex) {
				MensajeError += ex.Message;
				return false;
			}
		}


		public static  bool InicializarBD() {
			bool rs = false;
			try {
				using( Mono.Data.Sqlite.SqliteConnection tmpCn = new Mono.Data.Sqlite.SqliteConnection(CadenaConexion))
				{
					Mono.Data.Sqlite.SqliteConnection.CreateFile(RutaBaseDatosSQLite);
					tmpCn.Open();
					if(tmpCn != null)
					{
						using(Mono.Data.Sqlite.SqliteCommand tmpCm = new Mono.Data.Sqlite.SqliteCommand(tmpCn))
						{
							//tmpCm.CommandText=("INSERT OR REPLACE INTO 'android_metadata' VALUES('es_US');"); tmpCm.ExecuteNonQuery();
							tmpCm.CommandText=("CREATE TABLE 'Zonas' ('Cod_Zona' INTEGER,'Zona' TEXT,PRIMARY KEY(Cod_Zona));"); tmpCm.ExecuteNonQuery();
							tmpCm.CommandText=("CREATE TABLE 'Ventas_DD' ('Item' INTEGER PRIMARY KEY AUTOINCREMENT,'NoDoc' INTEGER,'Cod_Producto' TEXT,'Cantidad' INTEGER,'Precio' REAL,'Exonerado' INTEGER,'Descuento_Porciento' REAL,'Descuento_Total' REAL,'Bonificiado' INTEGER,'Retener_IVA' INTEGER,'LP' INTEGER);"); tmpCm.ExecuteNonQuery();
							tmpCm.CommandText=("CREATE TABLE 'Ventas_D' ('NoDoc'	INTEGER PRIMARY KEY AUTOINCREMENT,'FechaDoc'	TEXT,'Comentario'	TEXT,'IdBodega'	TEXT,'Usuario'	TEXT,'Cod_Cliente'	TEXT,'Exonerado'	INTEGER,'Autorización_DGI'	TEXT,'Crédito'	INTEGER,'Cod_Lista_Precios'	INTEGER,'Cod_Vendedor'	INTEGER,'Plazo'	INTEGER,'Dirección'	TEXT,'IDENTIFICACION'	TEXT,'Aplicado'	INTEGER,'Sincronizado'	INTEGER,'Longitud'	TEXT,'Latitud'	TEXT);");tmpCm.ExecuteNonQuery();
							tmpCm.CommandText=("CREATE TABLE 'Vendedores' ('Cod_Vendedor'	INTEGER,'Usuario_Vendedor'	TEXT,'Activo'	INTEGER,PRIMARY KEY(Cod_Vendedor));");tmpCm.ExecuteNonQuery();
							tmpCm.CommandText=("CREATE TABLE 'Usuario' ('Num'	INTEGER,'Usuario'	TEXT UNIQUE,'Nombres'	TEXT,'Apellidos'	TEXT,'Contraseña'	TEXT,'Cod_Vendedor'	INTEGER,PRIMARY KEY(Usuario));");tmpCm.ExecuteNonQuery();
							tmpCm.CommandText=("CREATE TABLE 'ServidorSQL' ('Num'	INTEGER,'ServerName'	TEXT,'IP_Server'	TEXT,'LoginSQL'	TEXT,'BaseDatos'	TEXT,'Contraseña'	TEXT,PRIMARY KEY(Num));");tmpCm.ExecuteNonQuery();
							tmpCm.CommandText=("CREATE TABLE 'Rutas' ('Cod_Zona'	INTEGER,'Cod_Vendedor'	INTEGER,'NumDía'	INTEGER,PRIMARY KEY(Cod_Zona));");tmpCm.ExecuteNonQuery();
							tmpCm.CommandText=("CREATE TABLE 'Productos' ('IdProducto'	INTEGER,'Cod_Producto'	TEXT,'Descripción'	TEXT,'Retiene_IVA'	INTEGER,'Es_Servicio'	INTEGER,'Activo'	INTEGER,'Aplica_Descuento'	INTEGER,'Descuento_Maximo'	REAL,'Marca'	TEXT,'Cod_Presentación'	TEXT,PRIMARY KEY(IdProducto));");tmpCm.ExecuteNonQuery();
							tmpCm.CommandText=("CREATE TABLE 'Precios' ('Cod_Lista_Precios'	INTEGER,'Cod_Producto'	TEXT,'Precio'	REAL,PRIMARY KEY(Cod_Lista_Precios,Cod_Producto));");tmpCm.ExecuteNonQuery();
							tmpCm.CommandText=("CREATE TABLE 'PD_x_Clientes' ('Num'	INTEGER,'Cod'	TEXT,'Cod_Producto'	TEXT,'Tipo_Condición'	INTEGER,'Valor_Condición'	INTEGER,'Descuento'	NUMERIC,PRIMARY KEY(Num,Cod));");tmpCm.ExecuteNonQuery();
							tmpCm.CommandText=("CREATE TABLE 'Módulos' ('Cod_Módulo'	INTEGER,'Módulo'	TEXT UNIQUE,PRIMARY KEY(Cod_Módulo));");tmpCm.ExecuteNonQuery();
							tmpCm.CommandText=("CREATE TABLE 'Lista_Precios' ('Cod_Lista_Precios'	INTEGER,'Lista_Precios'	TEXT,'Activo'	INTEGER,PRIMARY KEY(Cod_Lista_Precios));");tmpCm.ExecuteNonQuery();
							tmpCm.CommandText=("CREATE TABLE 'Días_Semana' ('Num'	INTEGER,'Día'	TEXT,'D'	TEXT UNIQUE,'Es_Inicio'	INTEGER,PRIMARY KEY(Num));");tmpCm.ExecuteNonQuery();
							tmpCm.CommandText=("CREATE TABLE 'Disp_Móviles' ('Num'	INTEGER,'Nombre'	TEXT,'Marca'	TEXT,'Modelo'	TEXT,'Asignado_A'	TEXT,'Estado'	TEXT,'Observaciones'	TEXT,'Cod_Vendedor'	INTEGER,'Es_Sincroniza_Todos_los_Clientes'	INTEGER,PRIMARY KEY(Num));");tmpCm.ExecuteNonQuery();
							tmpCm.CommandText=("CREATE TABLE 'Clientes_Crédito' ('Cod'	TEXT,'Nombre'	TEXT,'Límite'	REAL,'Plazo'	INTEGER,'Saldo'	REAL,'Activo'	INTEGER,'Dirección'	TEXT,'Teléfonos'	TEXT,'RUC'	TEXT,'Cod_Vendedor'	INTEGER,'Descuento'	REAL,'Cod_Lista_Precios'	INTEGER,'Comentario'	TEXT,'Cod_Zona'	INTEGER,PRIMARY KEY(Cod));");tmpCm.ExecuteNonQuery();
							tmpCm.CommandText=("CREATE TABLE 'Clientes_Contado' ('Num'	INTEGER,'Nombre'	TEXT,'Es_Empresa'	INTEGER,'RUC'	TEXT,'Teléfonos'	TEXT,'Dirección'	TEXT,'Cod_Zona'	INTEGER,'Activo'	INTEGER,'Cod_Lista_Precios'	INTEGER,PRIMARY KEY(Num));");tmpCm.ExecuteNonQuery();
							tmpCm.CommandText=("CREATE TABLE 'Cartera_MC' ('Cod_Módulo'	INTEGER,'IdTipoDoc'	INTEGER,'NoDoc'	INTEGER,'tagCod_Módulo'	INTEGER,'tagIdTipoDoc'	INTEGER,'tagNoDoc'	INTEGER,'Créditos'	REAL,'Pagos'	REAL,'Fecha'	TEXT,PRIMARY KEY(Cod_Módulo,IdTipoDoc,NoDoc,tagCod_Módulo,tagIdTipoDoc,tagNoDoc));");tmpCm.ExecuteNonQuery();
							tmpCm.CommandText=("CREATE INDEX 'ix_productos_marca' ON 'Productos' ('Marca' ASC);");tmpCm.ExecuteNonQuery();
							tmpCm.CommandText=("CREATE INDEX 'ix_productos_descripcion' ON 'Productos' ('Descripción' ASC);");tmpCm.ExecuteNonQuery();
							tmpCm.CommandText=("CREATE INDEX 'ix_productos_cod_producto' ON 'Productos' ('Cod_Producto' ASC);");tmpCm.ExecuteNonQuery();
							tmpCm.CommandText=("CREATE INDEX 'ix_productos_IdProducto' ON 'Productos' ('IdProducto' ASC);");tmpCm.ExecuteNonQuery();
							tmpCm.CommandText=("CREATE INDEX 'ix_clientes_crédito_teléfono' ON 'Clientes_Crédito' ('Teléfonos' ASC);");tmpCm.ExecuteNonQuery();
							tmpCm.CommandText=("CREATE INDEX 'ix_clientes_crédito_nombre' ON 'Clientes_Crédito' ('Nombre' ASC);");tmpCm.ExecuteNonQuery();
							tmpCm.CommandText=("CREATE INDEX 'ix_clientes_crédito_dirección' ON 'Clientes_Crédito' ('Dirección' ASC);");tmpCm.ExecuteNonQuery();
							tmpCm.CommandText=("CREATE INDEX 'ix_clientes_crédito_cod_vendedor' ON 'Clientes_Crédito' ('Cod_Vendedor' ASC);");tmpCm.ExecuteNonQuery();
							tmpCm.CommandText=("CREATE INDEX 'ix_clientes_Crédito_cod_zona' ON 'Clientes_Crédito' ('Cod_Zona' ASC);");tmpCm.ExecuteNonQuery();
							tmpCm.CommandText=("CREATE INDEX 'ix_clientes_Crédito_cod' ON 'Clientes_Crédito' ('Cod' ASC);");tmpCm.ExecuteNonQuery();
							tmpCm.CommandText=("INSERT INTO 'Usuario' VALUES(1,'Leonardo','Leonardo Martín','Martínez Núñez','000000',4);");tmpCm.ExecuteNonQuery();
							tmpCm.CommandText=("INSERT INTO 'ServidorSQL' VALUES(1,'MONTEPLATA','25.47.178.140','SA','SIVC4','monteplata.2014');");tmpCm.ExecuteNonQuery();
							tmpCm.CommandText=("CREATE TABLE 'Informacion' ('ID'	INTEGER,'Empresa'	TEXT,'Oficina'	TEXT,'Dirección'	TEXT,'NoRUC'	TEXT,'Teléfonos'	TEXT,'Email'	TEXT,'IVA'	REAL, 'DISPOSITIVO' TEXT,PRIMARY KEY(ID));");tmpCm.ExecuteNonQuery();
							tmpCm.CommandText=("INSERT INTO Informacion VALUES(1, '', '', '', '', '', '', 15, 'Sin Nombre')");tmpCm.ExecuteNonQuery();

							MensajeError="Base de datos creada exitosamente";
						}
						tmpCn.Close();
						rs=true;
					}
				}
				return rs;
			} catch (Exception ex) {
				MensajeError += ex.Message;
				return false;
			}

		}

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.Main);

			Button cmdIniciarSesion = FindViewById<Button> (Resource.Id.cmdIniciar);
			TextView tMensajeResultado = FindViewById<TextView> (Resource.Id.lblMensaje);
			EditText tUsuario = FindViewById<EditText> (Resource.Id.txtUsuario);
			EditText tClave = FindViewById<EditText> (Resource.Id.txtClave);
			MensajeError = "";


			try {
				objSQL = new clsBDRemotaMSSQL();
				objSQL.ConnectToDatabase();
			} catch (NullReferenceException ex) {
				tMensajeResultado.Text = ex.Message;
			}


			if (AbrirBaseDatos () == true) {
				cmdIniciarSesion.Enabled = true;
				tMensajeResultado.Text = MensajeError;
				cmdIniciarSesion.Click += delegate {
					if ( objUsuario.VerificarUsuario(tUsuario.Text,tClave.Text)==true) {
						objVendedor.Ubicar(objUsuario.Cod_Vendedor);
						StartActivity(typeof(Principal_Actividad));
					} else {
						tMensajeResultado.Text= "Error de inicio de sesión. Verifique Usuario y Contraseña";
					}
				
				};
			} else {
				cmdIniciarSesion.Enabled = false;
				tMensajeResultado.Text = MensajeError;
				cmdIniciarSesion.Click += delegate {
					cmdIniciarSesion.Text = "Error";
				};
			}

		}
	}
}


