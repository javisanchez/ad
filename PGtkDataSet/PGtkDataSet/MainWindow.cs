using Gtk;
using Npgsql;
using System;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;

public partial class MainWindow: Gtk.Window
{	
	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();
	}
	
	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Gtk.Application.Quit ();
		a.RetVal = true;
	}
	
	protected void OnExecuteActionActivated (object sender, System.EventArgs e)
	{
		string connectionString = "Server=localhost; Database=dbprueba; Id=javi; password=sistemas";
		IDbConnection dbConnection = new NpgsqlConnection(connectionString);
		IDbCommand selectCommand = dbConnection.CreateCommand();
		selectCommand.CommandText = "select * from articulo";
		IDbDataAdapter dbDataAdapter = new NpgsqlDataAdapter();
		dbDataAdapter.SelectCommand = selectCommand;
		
		DataSet dataSet = new DataSet();
		
		dbDataAdapter.Fill (dataSet);
		
		Console.WriteLine("Tables.Count={0}, dataSet.Tables.Count");
	}
}
