namespace ChevronMonthlySub.Domain
{
	using System.Collections.Generic;
	using System.Linq;

	public class ProductPurchaseOrder : PurchaseOrder
	{
		public new IEnumerable<ProductStateGroup> States { get; set; }

		public decimal PickPackCharge
		{
			get { return States.Sum(t => t.PickPackCharges); }
		}

		public decimal BoxCharge
		{
			get { return States.Sum(t => t.BoxCharges); }
		}

		public decimal Total
		{
			get { return PickPackCharge + BoxCharge; }
		}
	}
}