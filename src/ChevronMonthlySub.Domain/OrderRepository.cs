namespace ChevronMonthlySub.Domain
{
	using System.Collections.Generic;
	using System.Linq;

	public class OrderRepository : IOrderRepository
	{
		private readonly IEnumerable<OrderLine> _orderLines;

		public OrderRepository(IEnumerable<OrderLine> orderLines)
		{
			_orderLines = orderLines;
		}
		
		public IEnumerable<FreightLine> FreightLines
		{
			get { return _orderLines.OfType<FreightLine>(); }
		}

		public IEnumerable<ProductLine> ProductLines
		{
			get { return _orderLines.OfType<ProductLine>(); }
		} 
	}
}