using Gtk;
using System;-                                                                                                                                                                                                                                                                                                                                                                                                                    

namespace Serpis.Ad
{
	public class DbConnection
	{
		private ApplicationContext ()
		{
		}
		
		private static ApplicationContext instance= new ApplicationContext();
		
		public static ApplicationContext Instace{
			get {return instance;}
		}
		
		private IDbConnection dbConnection;
		public IDbConnection DbConnection {
			get{return dbConnection;}
			set{dbConnection = value;}
		}
	}
}

