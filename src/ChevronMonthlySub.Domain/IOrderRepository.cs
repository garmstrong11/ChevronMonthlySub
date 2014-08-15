namespace ChevronMonthlySub.Domain
{
	using System.Collections.Generic;

	public interface IOrderRepository
	{
		IEnumerable<FreightLine> FreightLines { get; }
		IEnumerable<ProductLine> ProductLines { get; } 
	}
}