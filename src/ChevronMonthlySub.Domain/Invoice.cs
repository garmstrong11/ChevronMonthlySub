namespace ChevronMonthlySub.Domain
{
	using System.Collections.Generic;
	using System.Linq;

	public class Invoice
	{
		private readonly IEnumerable<FreightLine> _freightLines;
		private readonly IEnumerable<ProductLine> _productLines; 
 
		public Invoice(IEnumerable<OrderLine> orderLines)
		{
			var enumerable = orderLines as IList<OrderLine> ?? orderLines.ToList();
			_freightLines = enumerable.OfType<FreightLine>().ToList();
			_productLines = enumerable.OfType<ProductLine>().ToList();
		}
		
		public string InvoiceNumber { get; set; }

		public IEnumerable<PurchaseOrderGroup<FreightLine>> FreightOrders
		{
			get { return GetOrders(_freightLines); }
		}

		public IEnumerable<PurchaseOrderGroup<ProductLine>> ProductOrders
		{
			get { return GetOrders(_productLines); }
		}

		private IEnumerable<PurchaseOrderGroup<T>> GetOrders<T>(IEnumerable<T> lines) where T : OrderLine
		{
			var query = from line in lines
				group line by line.PoNumber
				into orders
				select new PurchaseOrderGroup<T>
					{
					PoNumber = orders.Key,
					TaxGroups = from order in orders
						group order by order.TaxGroup
						into taxGroups
						select new TaxGroup<T>
							{
							GroupName = taxGroups.Key,
							StateOrderGroups = from taxGroup in taxGroups
								group taxGroup by taxGroup.State
								into states
								select new StateOrderGroup<T>
									{
									StateName = states.Key,
									OrderLines = states.ToList()
									}
							}
					};

			return query.ToList();
		} 
	}
}