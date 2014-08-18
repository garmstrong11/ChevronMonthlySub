namespace ChevronMonthlySub.Domain
{
	using System.Collections.Generic;
	using System.Linq;

	public class LineFactory
	{		
		public OrderLine Create(FlexCelOrderLineDto dto)
		{
			if (!string.IsNullOrWhiteSpace(dto.InventoryItemId)) {
				return new ProductLine(dto);
			}

			return new FreightLine(dto);
		}

		public void AssignBoxCount(List<OrderLine> lines)
		{
			var freightLines = lines.OfType<FreightLine>();
			var productLines = lines.OfType<ProductLine>();

			var siteDict = (from line in freightLines
											group line by line.Destination into sites
											select new
											{
												sites.Key,
												BoxCount = sites.Count()
											}).ToDictionary(k => k.Key, v => v.BoxCount);

			var firstProducts = from line in productLines
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
	}
}