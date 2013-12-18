using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using FirebirdHelper.Helpers;
using FirebirdSql.Data.FirebirdClient;
using System.Windows;
using FirebirdHelper.Models;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using System.IO;

namespace FirebirdHelper.ModelViewer
{
	public class DatabaseConnectionListModel : ModelBase
	{

		public DatabaseConnectionListModel()
		{
			Directory.CreateDirectory("Resources");
			Connections = new Dictionary<string, Lookup<string, string>>();
			ConnectionList = new List<ConnectionModel>();
			var connections = XDocument.Load(@"Resources/Connection.xml");
			IList<XElement> connectionLists = null;
			if (connections.Element("Connections") != null)
				connectionLists = connections.Element("Connections").Elements().ToList();
			foreach (var connect in connectionLists)
			{
				ConnectionList.Add(new ConnectionModel()
					{
						Alias = connect.Name.LocalName,
						DatabasePath = connect.Attribute("Path").Value,
						Password = connect.Attribute("Pass").Value,
						Login = connect.Attribute("Login").Value,
					});
				Connections[connect.Name.LocalName] = null;
			}

		}

		IList<ConnectionModel> _connectionList;
		public IList<ConnectionModel> ConnectionList
		{
			get { return _connectionList; }
			set
			{
				_connectionList = value;
				FirePropertyChanged("ConnectionList");
			}
		}

		Dictionary<string,Lookup<string,string>> _connections;
		public Dictionary<string, Lookup<string, string>> Connections
		{
			get { return _connections; }
			set
			{
				_connections = value;
				FirePropertyChanged("Connections");
			}
		}

		public static FbConnection Connection { get; set; }

		public ICommand AddDatabase
		{
			get
			{
				return new UserCommand(() =>
					{
						var window = new LoginWindow();
						window.ShowDialog();
						var document = XDocument.Load(ConnectionsFile);
						if (document.Element("Connections").Elements().Count() != 0)
						{
							ConnectionList = new List<ConnectionModel>();
							foreach (var item in document.Element("Connections").Elements())
								ConnectionList.Add(new ConnectionModel
								{
									Alias = item.Name.LocalName,
									DatabasePath = item.Attribute("Path").Value,
									Password = item.Attribute("Pass").Value,
									Login = item.Attribute("Login").Value
								});
						}
					});
			}
		}
		public ICommand RemoveDatabase
		{
			get
			{
				return new UserCommand((s) =>
				{
					ConnectionList.Remove(SelectedConnection);
					Connections.Remove(SelectedConnection.Alias);
					SelectedConnection = null;
					IsSelect = false;
					UpdateConnectionFile();
				});
			}
		}
		void UpdateConnectionFile()
		{
			var document = new XDocument();
			var connections = new XElement("Connections");
			foreach (var item in ConnectionList)
				connections.Add(new XElement(item.Alias, null, new XAttribute("Login", item.Login), new XAttribute("Pass", item.Password), new XAttribute("Path", item.DatabasePath)));
			document.Add(connections);
			File.Delete(ConnectionsFile);
			document.Save(ConnectionsFile);
		}

		public ICommand ConfigDatabase
		{
			get
			{
				return new UserCommand(() =>
				{

				});
			}
		}

		public ICommand Connect
		{
			get
			{
				return new UserCommand((s) =>
				{
					try
					{
						var builder = new FbConnectionStringBuilder();
						builder.Database = SelectedConnection.DatabasePath;
						builder.UserID = SelectedConnection.Login;
						builder.Password = SelectedConnection.Password;
						Connection = new FbConnection(builder.ToString());
						IsConnect = true;
						IsSelect = false;
					}
					catch (Exception ex) { MessageBox.Show(ex.ToString()); }

				});
			}
		}

		public ICommand Disconnect
		{
			get
			{
				return new UserCommand((s) =>
				{
					IsConnect = false;
					IsSelect = false;
					Connection.Dispose();
				});
			}
		}
		bool _isSelect = false;
		public bool IsSelect
		{
			get { return _isSelect; }
			set
			{
				_isSelect = value;
				FirePropertyChanged("IsSelect");
				FirePropertyChanged("IsRemove");
			}
		}

		bool _isConnect = false;
		public bool IsConnect
		{
			get { return _isConnect; }
			set
			{
				_isConnect = value;
				FirePropertyChanged("IsConnect");
				FirePropertyChanged("IsRemove");
			}
		}

		public bool IsRemove
		{
			get { return IsSelect && !IsConnect; }
		}

		ConnectionModel SelectedConnection;


		public ICommand Select
		{
			get
			{
				return new UserCommand((s) =>
				{
					dynamic item = s;
					SelectedConnection = ConnectionList.First(con=>con.Alias == item.Key);
					IsSelect = true;
				});
			}
		}


	}
}
