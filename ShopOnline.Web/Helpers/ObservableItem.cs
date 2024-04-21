using System.ComponentModel;

namespace ShopOnline.Web.Helpers
{
	public class ObservableItem<T> : INotifyPropertyChanged where T : class
	{
		private T _value;

		public T Value
		{
			get { return _value; }
			set
			{
				if (!Equals(_value, value))
				{
					_value = value;
					OnPropertyChanged(nameof(Value));
				}
			}
		}
		public ObservableItem(T value)
		{
			Value = value;		
		}
		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}

}
