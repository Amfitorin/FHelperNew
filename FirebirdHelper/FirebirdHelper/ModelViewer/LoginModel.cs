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
using Io = System.IO;

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
				return new UserCommand((o) =>
				{
					var builder = new FbConnectionStringBuilder();
					builder.Database = DatabasePath;
					builder.UserID = Login;
					builder.Password = Password;
					ConnectionString = builder.ToString();
					var document = XDocument.Load(@"Resources/Connection.xml");
					var connections = document.Element("Connections");
					if (connections.Element(Alias) != null)
						connections.Element(Alias).Remove();
					connections.Add(UpdateConnection());
					Io.File.Delete(ConnectionsFile);
					document.Save(ConnectionsFile);
				});
			}
		}

		XElement UpdateConnection()
		{
			var connection = new XElement(Alias, null, new XAttribute("Login", Login), new XAttribute("Pass", Password), new XAttribute("Path", DatabasePath));
			return connection;
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