namespace ChevronMonthlySub.Domain
{
	using System.Collections.Generic;

	public class TaxGroup<T> where T : OrderLine 
	{
		public string GroupName { get; set; }
    public string PoNumber { get; set; }
		public IEnumerable<StateOrderGroup<T>> States { get; set; }
	}
}