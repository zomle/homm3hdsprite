using System;
using System.Windows.Input;

namespace SASpriteGen.ViewModel
{
	public class Command : ICommand
	{
		public Action Action { get; set; }

		public string DisplayName { get; set; }

		public void Execute(object parameter)
		{
			if (Action != null)
				Action();
		}

		public bool CanExecute(object parameter)
		{
			return IsEnabled;
		}

		private bool _isEnabled = true;
		public bool IsEnabled
		{
			get { return _isEnabled; }
			set
			{
				_isEnabled = value;
				if (CanExecuteChanged != null)
					CanExecuteChanged(this, EventArgs.Empty);
			}
		}

		public event EventHandler CanExecuteChanged;

		public Command(Action action)
		{
			Action = action;
		}
	}

	public class Command<T> : ICommand
	{
		public Action<T> Action { get; set; }

		public void Execute(object parameter)
		{
			if (Action != null && parameter is T)
				Action((T)parameter);
		}

		public bool CanExecute(object parameter)
		{
			return IsEnabled;
		}

		private bool _isEnabled = true;
		public bool IsEnabled
		{
			get { return _isEnabled; }
			set
			{
				_isEnabled = value;
				if (CanExecuteChanged != null)
					CanExecuteChanged(this, EventArgs.Empty);
			}
		}

		public event EventHandler CanExecuteChanged;

		public Command(Action<T> action)
		{
			Action = action;
		}
	}
}
