namespace ChevronMonthlySub.Domain
{
	using System;

	public class ReportTemplate
	{
		public bool IsSummary { get; set; }
		public Type Type { get; set; }
		public string Path { get; set; }
	}
}