namespace ChevronMonthlySub.Domain
{
	using System.Collections.Generic;
	using System.Linq;

	public class StateProductGroup : StateOrderGroup<ProductLine>
	{
		public new IEnumerable<ProductLine> OrderLines { get; set; }

		public int ShipQtySubtotal
		{
			get { return OrderLines.Sum(t => t.ShipQty); }
		}

		public int BoxCountSubtotal
		{
			get { return OrderLines.Sum(t => t.Boxes); }
		}
	}
}