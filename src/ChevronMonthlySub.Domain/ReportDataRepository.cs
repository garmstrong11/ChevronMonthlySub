namespace ChevronMonthlySub.Domain
{
	using System.Collections.Generic;
	using System.Linq;

	public class ReportDataRepository : IReportDataRepository
	{
		private readonly List<OrderLine> _orderLines;

		public ReportDataRepository(IExtractor<FlexCelOrderLineDto> extractor)
		{
			_orderLines = extractor.Extract().Select(CreateOrderLine).ToList();

			AssignBoxCountsToProductLines();
		}
		
		public IEnumerable<FreightLine> FreightLines
		{
			get { return _orderLines.OfType<FreightLine>(); }
		}

		public IEnumerable<ProductLine> ProductLines
		{
			get { return _orderLines.OfType<ProductLine>(); }
		}

		public IEnumerable<ReportData<ProductLine>> GetProductReports(string invoiceId)
		{
			return GetReportData(ProductLines, invoiceId);
		}

		public IEnumerable<ReportData<FreightLine>> GetFreightReports(string invoiceId)
		{
			return GetReportData(FreightLines, invoiceId);
		}

		public IEnumerable<ReportData<OrderLine>> GetAllReports(string invoiceId )
		{
			return GetAllReports(_orderLines, invoiceId);
		}

		private static OrderLine CreateOrderLine(FlexCelOrderLineDto dto)
		{
			if (!string.IsNullOrWhiteSpace(dto.InventoryItemId))
			{
				return new ProductLine(dto);
			}

			return new FreightLine(dto);
		}

		private void AssignBoxCountsToProductLines()
		{
			var siteDict = (from line in FreightLines
											group line by line.Destination into sites
											select new
											{
												sites.Key,
												BoxCount = sites.Count()
											}).ToDictionary(k => k.Key, v => v.BoxCount);

			var firstProducts = from line in ProductLines
													group line by line.Destination into shipment
													select shipment.First();

			foreach (var product in firstProducts)
			{
				var count = 0;

				// Make sure at least one box is sent to each destination:
				if (!siteDict.TryGetValue(product.Destination, out count))
				{
					count = 1;
				}

				product.Boxes = count;
			}
		}

		private static IEnumerable<ReportData<T>> GetReportData<T>(IEnumerable<T> lines, string invoiceId) where T : OrderLine
		{
			var query =
				from line in lines
				group line by new { line.PoNumber, TaxGroup = line.TaxType }
					into orders
					select new ReportData<T>
					{
						PoNumber = orders.Key.PoNumber,
						TaxType = orders.Key.TaxGroup,
						InvoiceNumber = invoiceId,
						States =
							from order in orders
							group order by order.State
								into states
								select new StateOrderGroup<T>
								{
									StateName = states.Key,
									OrderLines = states.ToList()
								}
					};


			return query.ToList();
		}

		private static IEnumerable<ReportData<OrderLine>> GetAllReports(IEnumerable<OrderLine> lines, string invoiceId)
		{
			var query =
				from line in lines
				group line by new { line.PoNumber, TaxGroup = line.TaxType }
					into orders
					select new ReportData<OrderLine>
					{
						PoNumber = orders.Key.PoNumber,
						TaxType = orders.Key.TaxGroup,
						InvoiceNumber = invoiceId,
						States =
							from order in orders
							group order by order.State
								into states
								select new StateOrderGroup<OrderLine>
								{
									StateName = states.Key,
									OrderLines = states
								}
					};


			return query.ToList();
		}
	}
}