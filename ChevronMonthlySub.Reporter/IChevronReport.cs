namespace ChevronMonthlySub.Reporter
{
	using Domain;
	using FlexCel.Report;

	public interface IChevronReport<T> where T : PurchaseOrder
	{
		bool IsSummary { get; set; }
		FlexCelReport Report { get; set; }
		T PurchaseOrder { get; set; }
		void BuildReport();
		string OutputFilename { get; }
		string Prefix { get; }
		string Suffix { get; }
		string OutputDir { get; }
	}
}