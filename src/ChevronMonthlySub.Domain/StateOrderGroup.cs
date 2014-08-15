namespace ChevronMonthlySub.Domain
{
	using System.Collections.Generic;

	public class StateOrderGroup<T> where T : OrderLine
	{
		public string StateName { get; set; }
		public IEnumerable<T> OrderLines { get; set; } 
	}
}