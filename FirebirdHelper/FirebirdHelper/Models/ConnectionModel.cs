using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FirebirdHelper.Models
{
    public class ConnectionModel : ModelBase
    {
        string _databasePath;
        public string DatabasePath
        {
            get { return _databasePath; }
            set
            {
                _databasePath = value;
                FirePropertyChanged("DatabasePath");
                FirePropertyChanged("IsConnect");
            }
        }

        string _login;
        public string Login
        {
            get { return _login; }
            set
            {
                _login = value;
                FirePropertyChanged("Login");
                FirePropertyChanged("IsConnect");
            }
        }

        string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                FirePropertyChanged("Password");
                FirePropertyChanged("IsConnect");
            }
        }

		string _alias;
		public string Alias
		{
			get { return _alias; }
			set
			{
				_alias = value;
				FirePropertyChanged("Alias");
				FirePropertyChanged("IsConnect");
			}
		}

    }
}
