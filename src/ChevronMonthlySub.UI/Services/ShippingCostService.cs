namespace ChevronMonthlySub.UI.Services
{
	using Domain;

	public class ShippingCostService : IShippingCostService
	{
		public decimal BoxFee
		{
			get { return Properties.Settings.Default.BoxFee; }
		}

		public decimal PickPackFee
		{
			get { return Properties.Settings.Default.PickFee; }
		}
	}
}