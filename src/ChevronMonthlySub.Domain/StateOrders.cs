namespace ChevronMonthlySub.Domain
{
	using System.Collections.Generic;

	public class StateOrders<T> where T : OrderLine
	{
		public string StateName { get; set; }
		public IEnumerable<T> OrderLines { get; set; } 
	}
}