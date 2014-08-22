namespace ChevronMonthlySub.Domain
{
	using System.Collections.Generic;
	using System.Linq;

	public class FreightPurchaseOrder : PurchaseOrder
	{
		public new IEnumerable<FreightStateGroup> States { get; set; }

		public decimal LineAmountSubtotal
		{
			get { return States.Sum(t => t.LineAmountSubtotal); }
		}

		public decimal TaxAmountSubtotal
		{
			get { return States.Sum(t => t.TaxAmountSubtotal); }
		}

		public decimal Total
		{
			get { return LineAmountSubtotal + TaxAmountSubtotal; }
		}
	}
}