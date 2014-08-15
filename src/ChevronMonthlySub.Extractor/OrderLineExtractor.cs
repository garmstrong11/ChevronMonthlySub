namespace ChevronMonthlySub.Extractor
{
	using System;
	using System.Collections.Generic;
	using Domain;
	using FlexCel.XlsAdapter;

	public class OrderLineExtractor : IExtractor<FlexCelOrderLineDto>
	{
		private readonly XlsFile _xls;

		public OrderLineExtractor(XlsFile xls)
		{
			_xls = xls;
		}
		
		public IList<FlexCelOrderLineDto> Extract()
		{
			var result = new List<FlexCelOrderLineDto>();
			
			for (var row = 2; row <= _xls.RowCount; row++) {
				var dto = new FlexCelOrderLineDto
					{
						DateShipped = ExtractDateShipped(row),
						PoNumber = ExtractPoNumber(row),
						OrderNumber = ExtractOrderNumber(row),
						InventoryItemId = ExtractInventoryItemId(row),
						LineDesc = ExtractLineDesc(row),
						LineAmount = ExtractLineAmount(row),
						TaxAmount = ExtractTaxAmount(row),
						QtyShipped = ExtractQtyShipped(row),
						LineDistribution = ExtractLineDistribution(row),
						FreightLineCount = ExtractFreightLineCount(row),
						OrderLineCount = ExtractOrderLineCount(row)
					};

				result.Add(dto);
			}

			return result;
		}

		private DateTime ExtractDateShipped(int rowIndex)
		{
			const int columnIndex = 1;
			var val = _xls.GetStringFromCell(rowIndex, columnIndex).Value;
			return DateTime.Parse(val);
		}

		private string ExtractPoNumber(int rowIndex)
		{
			const int columnIndex = 2;
			return ExtractString(rowIndex, columnIndex);
		}

		private int ExtractOrderNumber(int rowIndex)
		{
			const int columnIndex = 3;
			return ExtractIntFromString(rowIndex, columnIndex);
		}

		private string ExtractInventoryItemId(int rowIndex)
		{
			const int columnIndex = 4;
			return ExtractString(rowIndex, columnIndex);
		}

		private string ExtractLineDesc(int rowIndex)
		{
			const int columnIndex = 5;
			var extracted = ExtractString(rowIndex, columnIndex);
			return extracted.Replace("\n", " ");
		}

		private decimal ExtractLineAmount(int rowIndex)
		{
			const int columnIndex = 6;
			return ExtractDecimal(rowIndex, columnIndex);
		}

		private decimal ExtractTaxAmount(int rowIndex)
		{
			const int columnIndex = 7;
			return ExtractDecimal(rowIndex, columnIndex);
		}

		private int ExtractQtyShipped(int rowIndex)
		{
			const int columnIndex = 8;
			return ExtractInt(rowIndex, columnIndex);
		}

		private string ExtractLineDistribution(int rowIndex)
		{
			const int columnIndex = 9;
			return ExtractString(rowIndex, columnIndex);
		}

		private int ExtractFreightLineCount(int rowIndex)
		{
			const int columnIndex = 10;
			return ExtractInt(rowIndex, columnIndex);
		}

		private int ExtractOrderLineCount(int rowindex)
		{
			const int columnIndex = 11;
			return ExtractInt(rowindex, columnIndex);
		}

		private string ExtractString(int rowIndex, int columnIndex)
		{
			var val = _xls.GetCellValue(rowIndex, columnIndex);

			return val != null ? val.ToString() : string.Empty;
		}

		private int ExtractInt(int rowIndex, int columnIndex)
		{
			var extract = _xls.GetCellValue(rowIndex, columnIndex);
			if (extract == null) return default(int);

			if (!(extract is double)) return default (int);

			return Convert.ToInt32(extract);
		}

		private int ExtractIntFromString(int rowIndex, int columnIndex)
		{
			var val = default(int);
			var extract = _xls.GetCellValue(rowIndex, columnIndex);
			if (extract == null) return val;

			int.TryParse(extract.ToString(), out val);
			return val;
		}

		private decimal ExtractDecimal(int rowIndex, int columnIndex)
		{
			var extract = _xls.GetCellValue(rowIndex, columnIndex);
			if (extract == null) return default(decimal);

			if (!(extract is double)) return default(decimal);

			return Convert.ToDecimal(extract);
		}
	}
}