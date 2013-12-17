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
			ConnectionList = new ObservableCollection<ConnectionModel>();
			var connections = XDocument.Load(@"Resources/Connection.xml");
			IList<XElement> connectionLists = null;
			if (connections.Element("Connections") != null)
				connectionLists = connections.Element("Connections").Elements().ToList();
			foreach (var connect in connectionLists)
				ConnectionList.Add(new ConnectionModel()
					{
						Alias = connect.Name.LocalName,
						DatabasePath = connect.Attribute("Path").Value,
						Password = connect.Attribute("Pass").Value,
						Login = connect.Attribute("Login").Value,
					});
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

		public static FbConnection Connection { get; set; }

		public ICommand AddDatabase
		{
			get
			{
				return new UserCommand(() =>
					{
						var window = new LoginWindow();
						window.Show();
					});
			}
		}
		public ICommand RemoveDatabase
		{
			get
			{
				return new UserCommand(() =>
				{

				});
			}
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
						dynamic connect = s;
						var builder = new FbConnectionStringBuilder();
						builder.Database = connect.DatabasePath;
						builder.UserID = connect.Login;
						builder.Password = connect.Password;
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
					IsSelect = true;
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
			}
		}

		public ICommand Select
		{
			get
			{
				return new UserCommand(() =>
				{
					IsSelect = true;
				});
			}
		}

	}
}
