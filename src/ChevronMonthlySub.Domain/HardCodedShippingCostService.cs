namespace ChevronMonthlySub.Domain
{
	public class HardCodedShippingCostService : IShippingCostService
	{
		public decimal BoxFee
		{
			get { return 2.50m; }
		}

		public decimal PickPackFee
		{
			get { return 0.50m; }
		}
	}
}