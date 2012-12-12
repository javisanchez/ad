using Gtk;
using Npgsql;
using PArticulo;
using Serpis.Ad;
using System;
using System.Collections.Generic;
using System.Data;


public partial class MainWindow: Gtk.Window
{
	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();
		
		string connectionString = "Server=localhost;Database=dbprueba;User Id=javi;Password=sistemas";
		ApplicationContext.Instance.DbConnection = new NpgsqlConnection(connectionString);
		ApplicationContext.Instance.DbConnection.Open ();
		
		IDbCommand dbCommand = ApplicationContext.Instance.DbConnection.CreateCommand ();
		dbCommand.CommandText = 
			"select a.id, a.nombre, a.precio, c.nombre as categoria " +
			"from articulo a left join categoria c " +
			"on a.categoria = c.id";
		
		IDataReader dataReader = dbCommand.ExecuteReader ();
		
		TreeViewExtensions.Fill (treeView, dataReader);
		dataReader.Close ();
		
		dataReader = dbCommand.ExecuteReader ();
		TreeViewExtensions.Fill (treeView, dataReader);
		dataReader.Close ();
		
	}
	
	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		ApplicationContext.Instance.DbConnection.Close ();

		Application.Quit ();
		a.RetVal = true;
	}

	protected void OnClearActionActivated (object sender, System.EventArgs e)
	{
		ListStore listStore = (ListStore)treeView.Model;
		listStore.Clear ();
	}

	protected void OnEditActionActivated (object sender, System.EventArgs e)
	{
		long id = getSelectedId();
		ArticuloView articuloView = new ArticuloView( id );
		articuloView.Show ();
	}
	
	private long getSelectedId() {
		TreeIter treeIter;
		treeView.Selection.GetSelected(out treeIter);
		
		ListStore listStore = (ListStore)treeView.Model;
		return long.Parse (listStore.GetValue (treeIter, 0).ToString ()); 
	}

	protected void OnNewActionActivated (object sender, System.EventArgs e)
	{
		throw new System.NotImplementedException ();
	}
}
