namespace ChevronMonthlySub.Extractor
{
	using System.Collections.Generic;
	using Domain;
	using FlexCel.XlsAdapter;

	public class TaxGroupExtractor : IExtractor<TaxGroup>
	{
		private readonly XlsFile _xl;

		public TaxGroupExtractor(XlsFile xl)
		{
			_xl = xl;
		}
		
		public IList<TaxGroup> Extract()
		{
			var result = new List<TaxGroup>();

			for (var i = 2; i <= _xl.RowCount; i++) {
				result.Add(
					new TaxGroup {
						State = _xl.GetCellValue(i, 1).ToString(),
						Id = _xl.GetCellValue(i, 2).ToString(),
						GroupName = _xl.GetCellValue(i, 3).ToString()
					});
			}

			return result;
		}
	}
}