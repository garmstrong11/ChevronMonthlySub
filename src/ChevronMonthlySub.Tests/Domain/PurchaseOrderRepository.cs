namespace ChevronMonthlySub.Tests.Domain
{
	using System.Collections.Generic;
	using System.Linq;
	using ChevronMonthlySub.Domain;
	using Reporter;

	public class PurchaseOrderRepository : IPurchaseOrderRepository
	{
		private readonly List<OrderLine> _orderLines;
		private readonly IRecipientRepository _recipientRepository;
	  private readonly IShippingCostService _shippingCostService;
		private readonly ITemplatePathService _templatePathService;

		public PurchaseOrderRepository(
      IExtractor<FlexCelOrderLineDto> extractor, 
      IRecipientRepository recipientRepository, 
      IShippingCostService shippingCostService,
			ITemplatePathService templatePathService
			)
		{
			_orderLines = extractor.Extract()
        .Select(CreateOrderLine)
        .ToList();

			_recipientRepository = recipientRepository;
		  _shippingCostService = shippingCostService;
			_templatePathService = templatePathService;

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
			var siteList = (from line in FreightLines
											group line by new { line.Destination, line.PoNumber } into sites
											select new
											{
												sites.Key.Destination,
												sites.Key.PoNumber,
												BoxCount = sites.Count()
											}).ToList();

			var firstProducts = from line in ProductLines
													group line by line.Destination into shipment
													select shipment.First();

			foreach (var product in firstProducts)
			{
				var matches = siteList
					.FindAll(s => s.Destination == product.Destination && s.PoNumber == product.PoNumber);

				// Make sure at least one box is sent to each destination:
				product.Boxes = matches.Count == 0 ? 1 : matches.Count;
			}
		}

		public IEnumerable<FreightPurchaseOrder> GetFreightPurchaseOrders(string invoiceId)
		{
			var query =
				from line in FreightLines
				group line by new { line.PoNumber, TaxGroup = line.TaxType }
					into orders
					select new FreightPurchaseOrder(new FlexcelChevronReportAdapter(new HardCodedTemplatePathService()))
					{
						PoNumber = orders.Key.PoNumber,
						TaxType = orders.Key.TaxGroup,
						InvoiceNumber = invoiceId,
						Recipient = _recipientRepository.Get("ML"),
						Description = "Initial Description to be filled in later",
						States =
							from order in orders
							group order by order.State
								into states
								select new FreightStateGroup
								{
									StateName = states.Key,
									OrderLines = states.ToList()
								}
					};

			return query.ToList();
		}

    public IEnumerable<ProductPurchaseOrder> GetProductPurchaseOrders(string invoiceId)
    {
      var query =
        from line in ProductLines
        group line by new { line.PoNumber, TaxGroup = line.TaxType }
          into orders
					select new ProductPurchaseOrder(new FlexcelChevronReportAdapter(new HardCodedTemplatePathService()))
					{
            PoNumber = orders.Key.PoNumber,
            TaxType = orders.Key.TaxGroup,
            InvoiceNumber = invoiceId,
            Recipient = _recipientRepository.Get("ML"),
						Description = "Initial Description to be filled in later",
            States =
              from order in orders
              group order by order.State
                into states
                select new ProductStateGroup(_shippingCostService)
                {
                  StateName = states.Key,
                  OrderLines = states.ToList()
                }
          };

      return query.ToList();
    }
	}
}