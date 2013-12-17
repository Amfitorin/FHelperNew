using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.IO;
using System.Xml.Linq;

namespace FirebirdHelper.Models
{
	public class ModelBase : INotifyPropertyChanged
	{
		static readonly string resourceDirectory = "Resources";
		public static readonly string ConnectionsFile = @"Resources/Connection.xml";
		public ModelBase()
		{
			Directory.CreateDirectory(resourceDirectory);
			if (!File.Exists(ConnectionsFile))
			{
				var document = new XDocument();
				document.Add(new XElement("Connections"));
				document.Save(ConnectionsFile);
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected void FirePropertyChanged(string name)
		{
			try
			{
				if (PropertyChanged == null)
					return;
				PropertyChanged(this, new PropertyChangedEventArgs(name));
			}
			catch
			{
				throw;
			}
		}
		protected virtual void FirePropertyChanged<T>(System.Linq.Expressions.Expression<Func<T>> property)
		{
			var expression = property.Body as System.Linq.Expressions.MemberExpression;
			if (expression == null)
				throw new NotSupportedException("Invalid expression passed. Only property member should be selected.");

			FirePropertyChanged(expression.Member.Name);
		}
	}
}
