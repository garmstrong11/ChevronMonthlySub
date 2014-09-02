namespace ChevronMonthlySub.UI.ViewModels
{
	using Caliburn.Micro;

	public class PurchaseOrdersViewModel
	{
		public PurchaseOrdersViewModel()
		{
			Items = new BindableCollection<PurchaseOrderRowViewModel>();
		}

		public BindableCollection<PurchaseOrderRowViewModel> Items { get; set; } 
	}
}