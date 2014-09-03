namespace ChevronMonthlySub.Domain
{
  using System.Collections.Generic;
  using System.Linq;

	public class FreightStateGroup : StateGroup
	{
		private IEnumerable<FreightLine> _orderLines;

		public IEnumerable<FreightLine> OrderLines
		{
			get { return _orderLines.OrderBy(p => p.OrderNumber); }
			set { _orderLines = value; }
		}

		public decimal LineAmountSubtotal
		{
			get { return OrderLines.Sum(t => t.LineAmount); }
		}

		public decimal TaxAmountSubtotal
		{
			get { return OrderLines.Sum(t => t.TaxAmount); }
		}

		public int TaxQuantity
		{
			get { return TaxAmountSubtotal > 0.0m ? 1 : 0; }
		}
	}
}