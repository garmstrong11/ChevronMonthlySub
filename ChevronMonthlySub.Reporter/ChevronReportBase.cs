namespace ChevronMonthlySub.Reporter
{
	using Domain;
	using FlexCel.Report;

	public abstract class ChevronReportBase : IChevronReport
	{
		public bool IsSummary { get; set; }
		public virtual string FilenameFormat { get; private set; }

		public virtual void BuildReport(FlexCelReport report, PurchaseOrder po)
		{
			report.AddTable("States", po.States);
			report.SetValue("TaxType", po.TaxType);
			report.SetValue("PoNumber", po.PoNumber);
			report.SetValue("InvoiceNumber", po.InvoiceNumber);
			report.SetValue("Recipient", po.Recipient);
		}
	}
}