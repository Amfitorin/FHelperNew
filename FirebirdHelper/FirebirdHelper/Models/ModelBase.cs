using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace FirebirdHelper.Models
{
	public class ModelBase : INotifyPropertyChanged
	{
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
