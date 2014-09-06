namespace ChevronMonthlySub.Extractor
{
	using System.Collections.Generic;
	using Domain;

	public class OrderLineExtractor : ExtractorBase<FlexCelOrderLineDto>, IExtractor<FlexCelOrderLineDto>
	{
		public override IList<FlexCelOrderLineDto> Extract()
		{
			var result = base.Extract();
			
			for (var row = 2; row <= Xls.RowCount; row++) {
				var dto = new FlexCelOrderLineDto
					{
						DateShipped = ExtractDateTime(row, 1),
						PoNumber = ExtractString(row, 2),
						OrderNumber = ExtractIntFromString(row, 3),
						InventoryItemId = ExtractString(row, 4),
						LineDesc = ExtractLineDesc(row, 5),
						LineAmount = ExtractDecimal(row, 6),
						TaxAmount = ExtractDecimal(row, 7),
						QtyShipped = ExtractInt(row, 8),
						LineDistribution = ExtractString(row, 9),
						FreightLineCount = ExtractInt(row, 10),
						OrderLineCount = ExtractInt(row, 11)
					};

				result.Add(dto);
			}

			return result;
		}

		private string ExtractLineDesc(int rowIndex, int columnIndex)
		{
			var extracted = ExtractString(rowIndex, columnIndex);
			return extracted.Replace("\n", " ");
		}
	}
}