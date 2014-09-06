namespace ChevronMonthlySub.UI.ViewModels
{
	using Caliburn.Micro;
	using Domain;

	public class RequestorListViewModel : PropertyChangedBase
	{
		private readonly IRequestorService _requestorService;
		private BindableCollection<Requestor> _items;
		private Requestor _selectedItem;

		public RequestorListViewModel(IRequestorService requestorService)
		{
			_requestorService = requestorService;
			_items = new BindableCollection<Requestor>(_requestorService.GetAll());
		}

		public BindableCollection<Requestor> Items
		{
			get { return _items; }
			set
			{
				if (Equals(value, _items)) return;
				_items = value;
				NotifyOfPropertyChange();
			}
		}

		public Requestor SelectedItem
		{
			get { return _selectedItem; }
			set
			{
				if (Equals(value, _selectedItem)) return;
				_selectedItem = value;
				NotifyOfPropertyChange();
			}
		}
	}
}