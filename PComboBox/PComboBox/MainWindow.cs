using System;
using Gtk;

public delegate int MyFunction (int a, int b);

public partial class MainWindow: Gtk.Window
{	
	private ListStore listStore;
	
	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();
		
//		MyFunction f;
//		
//		MyFunction[] functions = new MyFunction[]{suma, resta, multiplica};
//		
//		int random = new Random().Next(3);
//		f= functions[random];
//		
//		Console.WriteLine("f={0}", f(5,3));
		
		treeView.Selection.Mode=SelectionMode.Multiple;
		
		treeView.AppendColumn("Columna uno", new CellRendererText(), "text",0);
		treeView.AppendColumn("Columna dos", new CellRendererText(), "text",1);
		
		CellRenderer cellRenderer = new CellRendererText();
		comboBox.PackStart(cellRenderer, false); //expand = false
		comboBox.AddAttribute (cellRenderer, "text", 0);
		
		CellRenderer cellRenderer2 = new CellRendererText();
		comboBox.PackStart(cellRenderer2, false); //expand = false
		comboBox.AddAttribute (cellRenderer2, "text", 1);
		
		listStore = new ListStore (typeof(string), typeof(string));	
		comboBox.Model = listStore;
		treeView.Model = listStore;
		
		listStore.AppendValues("1", "Uno");
		listStore.AppendValues("2", "Dos");
		
		treeView.Selection.Changed+=delegate{
			int count = treeView.Selection.CountSelectedRows();
			Console.WriteLine("treeView.Selection.Changed CountSelectedRows={0}" + count);
			
			treeView.Selection.SelectedForeach(delegate(TreeModel model, TreePath path, TreeIter iter){
				object value = listStore.GetValue(iter, 0);
				Console.WriteLine("value={0}", value);
		});
		};
		
//		comboBox.Changed += delegate {
//			TreeIter treeIter;
//			if(comboBox.GetActiveIter(out treeIter)){ //Item seleccionado
//				object value = listStore.GetValue (treeIter, 0);
//				Console.WriteLine("comboBoxChanged value={0}", value);
//			}
//		};
		comboBox.Changed += comboBoxChanged;
		
		//comboBox.Changed += delegate {showActiveItem (listStore); };

	}
	
//	private int suma(int a, int b){
//		return a+b;
//	}
//	
//	private int resta(int a, int b){
//		return a-b;
//	}
//	
//	private int multiplica(int a, int b){
//		return a*b;
//	}
	
	private void comboBoxChanged(object obj, EventArgs args){
			TreeIter treeIter;
			if(comboBox.GetActiveIter(out treeIter)){ //Item seleccionado
				object value = listStore.GetValue (treeIter, 0);
				Console.WriteLine("comboBox.Changed value={0}", value);
			}		
	}
	
	protected void OnDeleteEvent (object sender, DeleteEventArgs a){
		Application.Quit ();
		a.RetVal = true;
	}
}
