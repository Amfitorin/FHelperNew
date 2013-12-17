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

namespace FirebirdHelper.ModelViewer
{
	public class DatabaseConnectionListModel: ModelBase
	{
		public DatabaseConnectionListModel()
		{
            var connections = XDocument.Load(@"Resources/Connection.xml");
            IList<XElement> connectionLists = null;
            if (connections.Element("Connections") != null)
                
		}
        ObservableCollection<ConnectionModel> ConnectionList { get; set; }
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
                return new UserCommand(() =>
                {
                    try
                    {
                        if (LoginModel.ConnectionString != null)
                            Connection = new FbConnection(LoginModel.ConnectionString);
                    }
                    catch (Exception ex) { MessageBox.Show(ex.ToString()); }

                });
            }
        }

	}
}
