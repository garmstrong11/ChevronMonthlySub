namespace ChevronMonthlySub.Domain
{
	using System.Collections.Generic;

	public class PurchaseOrderGroup<T> where T : OrderLine
	{
		public string PoNumber { get; set; }
		public IEnumerable<TaxGroup<T>> TaxGroups { get; set; }
	}
}