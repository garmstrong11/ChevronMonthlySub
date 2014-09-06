namespace ChevronMonthlySub.Extractor
{
	using System.Collections.Generic;
	using Domain;

	public class OrderKeyExtractor : ExtractorBase<OrderKeyRowDto>, IExtractor<OrderKeyRowDto>
	{
		public override IList<OrderKeyRowDto> Extract()
		{
			var result = base.Extract();

			for (var row = 2; row <= Xls.RowCount; row++)
			{
				var dto = new OrderKeyRowDto
				{
					ProductId = ExtractString(row, 1),
					FreightId = ExtractString(row, 2),
					Description = ExtractString(row, 3),
					RequestorInitials = ExtractString(row, 4)
				};

				result.Add(dto);
			}

			return result;
		}
	}
}