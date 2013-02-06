using System;
using Gtk;

using NHibernate;

using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

using PGtkNHibernate;

public partial class MainWindow: Gtk.Window
{	
	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();
		
		Configuration configuration = new Configuration();
		configuration.Configure();
		configuration.SetProperty(NHibernate.Cfg.Environment.Hbm2ddlKeyWords, "none");
		configuration.AddAssembly(typeof(Categoria).Assembly);
		new SchemaExport(configuration).Execute(true, false, false);
		
		ISessionFactory sessionFactory = configuration.BuildSessionFactory();
		
		updateCategoria(sessionFactory);
			
		insertCategoria(sessionFactory);
		
		sessionFactory.Close();
	}
	
	private void updateCategoria(ISessionFactory sessionFactory){
		ISession session = sessionFactory.OpenSession();//Esta linea debe estar dentro ya que si hay alguna excepción no se ejecutará y el close siempre se ejecutará.
		try{
			Categoria categoria = (Categoria)session.Load(typeof(Categoria), 2L);
			Console.WriteLine("Categoria Id={0} Nombre={1}");		
			categoria.Nombre = DateTime.Now.ToString();		
			session.SaveOrUpdate (categoria);		
			session.Flush();
		}finally{//Lo que está dentro del finally SIEMPRE se va a ejecutar
		session.Close();
		}
	}
	
	//Usando el using; realizas la versión corta del try{ }finally{ } (Implementa la interfaz disposable)
	private void insertCategoria(ISessionFactory sessionFactory){
		using (ISession session = sessionFactory.OpenSession()){
			Categoria categoria = new Categoria();
			Categoria.Nombre = "Nueva " + DateTime.Now.ToString();
			session.SaveOrUpdate (categoria);		
			session.Flush();	
		}
	}
	
//	private void insertCategoria(ISessionFactory sessionFactory){
//		ISession session = sessionFactory.OpenSession();
//		try{
//			Categoria categoria = new Categoria();
//			Categoria.Nombre = "Nueva " + DateTime.Now.ToString();
//			session.SaveOrUpdate (categoria);		
//			session.Flush();	
//		}finally{	
//		session.Close();
//		}
//	}
	
	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}
}
