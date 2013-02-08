using Gtk;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using PGtkNHibernate;
using Serpis.Ad;
using System;
using System.Collections;


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
		
		//updateCategoria(sessionFactory);
			
		//insertCategoria(sessionFactory);
		
		//loadArticulo(sessionFactory);
		
		
		ISession session = sessionFactory.OpenSession();
		ICriteria criteria = sessionFactory.CreateCriteria(typeof(Articulo));
		IList list = criteria.List();
		foreach (Articulo articulo in list)
			Console.WriteLine("Articulo Id={0} Nombre={1} Precio={2}",
			                  articulo.Id, articulo.Nombre, articulo.Precio);
		session.Close ();
		
		sessionFactory.Close();
	}
		
//		ESTA SERIA LA MANERA MÁS CORRECTA DE ESCRIBIRLO
	
//		using (ISession session = sessionFactory.OpenSession()){
//		ICriteria criteria = sessionFactory.CreateCriteria(typeof(Categoria));
//		IList list = criteria.List();
//		foreach (Categoria categoria in list)
//			Console.WriteLine("Categoria Id={0} Nombre={1}",
//			                  categoria.Id, categoria.Nombre);
//		}
//		
//		sessionFactory.Close();
//	}
	
	private void loadArticulo(ISessionFactory sessionFactory){
		using (ISession session = sessionFactory.OpenSession()){
			Articulo articulo = (Articulo) session.Load(typeof(Articulo), 2L);
			Console.WriteLine ("Articulo Id={0} Nombre={1} Precio{2}",
			                   articulo.Id, articulo.Nombre, articulo.Precio);
			if (articulo.Categoria == null)
				Console.WriteLine ("Categoria = null");
			else
				Console.WriteLine("Categoria.Id={0}", articulo.Categoria.Id);
				//Console.WriteLine("Categoria.Nombre={0}", articulo.Categoria.Nombre);
		}
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
			Categoria.Nombre ="Nueva " + DateTime.Now.ToString();
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
