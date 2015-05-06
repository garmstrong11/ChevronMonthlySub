namespace ChevronMonthlySub.Domain
{
	using System;
	using System.Text;

	public class InvalidStateException : Exception
	{
		private readonly int _rowIndex;
		private readonly string _lineDesc;
		private readonly string _badState;

		public InvalidStateException(int rowIndex, string lineDesc, string badState)
		{
			_rowIndex = rowIndex;
			_lineDesc = lineDesc;
			_badState = badState;
		}

		public override string Message
		{
			get { return FormExceptionMessage(); }
		}

		private string FormExceptionMessage()
		{
			var sb = new StringBuilder();
			sb.AppendFormat("I am unable to extract a valid US state abbreviation from row {0}\n", _rowIndex);
			sb.AppendLine();
			sb.AppendLine("I use the last two characters of the LineDesc column to look up the correct state,");
			sb.AppendFormat("but the value \"{0}\"\n", _badState);
			sb.AppendFormat("from \"{0}\"\n", _lineDesc);
			sb.AppendLine("is not a valid US state abbreviation.");
			sb.AppendLine("Please correct the spreadsheet and try again.");
			sb.AppendLine("It may also be useful to check other rows with the same Order-Number for this problem.");

			return sb.ToString();
		}
	}
}