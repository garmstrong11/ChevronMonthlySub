namespace ChevronMonthlySub.UI.ViewModels
{
	using Caliburn.Micro;

	public class ErrorWindowViewModel : Screen
	{
		private string _errors;

		public ErrorWindowViewModel()
		{
			DisplayName = "File Import Failed";
		}

		public string Errors
		{
			get { return _errors; }
			set
			{
				if (value == _errors) return;
				_errors = value;
				NotifyOfPropertyChange();
			}
		}

		public void Ok()
		{
			TryClose();
		}
	}
}