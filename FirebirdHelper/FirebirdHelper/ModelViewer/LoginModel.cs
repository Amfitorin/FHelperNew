using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using FirebirdHelper.Helpers;
using FirebirdSql.Data.FirebirdClient;
using Microsoft.Win32;
using File = System.IO;
using FirebirdHelper.Models;
using System.Xml.Linq;

namespace FirebirdHelper.ModelViewer
{
	public class LoginModel : ConnectionModel
	{
		readonly string[] _extensions = new string[] { ".fdb", ".gdb", ".FDB", ".GDB" };

		public static string ConnectionString { get; set; }

		public bool IsConnect { get { return Password != null && Login != null && _extensions.Contains(File.Path.GetExtension(DatabasePath)); } }

		public ICommand Connect
		{
			get 
			{
				return new UserCommand((o)=>
				{
					var builder = new FbConnectionStringBuilder();
					builder.Database = DatabasePath;
					builder.UserID = Login;
					builder.Password = Password;
					ConnectionString = builder.ToString();
                    var document = XDocument.Load(@"Resources/Connect.xml");
                    if (document == null)
                        document = new XDocument();
				});
			}
		}

		public ICommand View
		{
			get
			{
				return new UserCommand(() =>
				{
					var dialog = new OpenFileDialog();
					dialog.InitialDirectory = @"C:\";
					dialog.Filter = ("DB files|*.fdb;*.gdb;*.FDB;*.GDB");
					if (dialog.ShowDialog().Value)
					{
						DatabasePath = dialog.FileName;
					}
				});
			}
		}

    }
}