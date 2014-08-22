namespace ChevronMonthlySub.Reporter
{
	using Domain;
	using FlexCel.Report;

	public interface IChevronReport
	{
		bool IsSummary { get; set; }
		void BuildReport(FlexCelReport report, PurchaseOrder po);
		string FilenameFormat { get; }
	}
}