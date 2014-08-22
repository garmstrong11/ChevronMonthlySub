namespace ChevronMonthlySub.Domain
{
	using System.Collections.Generic;
	using System.Linq;

	public class FreightStateGroup : StateGroup
	{
		public IEnumerable<FreightLine> OrderLines { get; set; }

		public decimal LineAmountSubtotal
		{
			get { return OrderLines.Sum(t => t.LineAmount); }
		}

		public decimal TaxAmountSubtotal
		{
			get { return OrderLines.Sum(t => t.TaxAmount); }
		}
	}
}