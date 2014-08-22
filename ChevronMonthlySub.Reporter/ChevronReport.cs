namespace ChevronMonthlySub.Reporter
{
	using System;
	using Domain;
	using FlexCel.Report;

	public abstract class ChevronReport<T> : IChevronReport<T> where T : PurchaseOrder
	{
		private string _outputDir;
		public bool IsSummary { get; set; }
		public FlexCelReport Report { get; set; }
		public T PurchaseOrder { get; set; }

		public virtual void BuildReport()
		{
			Report.AddTable("States", PurchaseOrder.States);
			Report.SetValue("TaxType", PurchaseOrder.TaxType);
			Report.SetValue("PoNumber", PurchaseOrder.PoNumber);
			Report.SetValue("InvoiceNumber", PurchaseOrder.InvoiceNumber);
			Report.SetValue("Recipient", PurchaseOrder.Recipient);
		}

		public abstract string OutputFilename { get; }

		public string Prefix
		{
			get { return IsSummary ? "Summary" : "Chevron FG"; }
		}

		public string Suffix
		{
			get
			{
				if (PurchaseOrder == null) {
					throw new InvalidOperationException("PurchaseOrder not set");
				}

				var po = PurchaseOrder as FreightPurchaseOrder;
				return po != null ? " FRT.xlsx" : ".xlsx";
			}
		}

		public string OutputDir
		{
			get { return _outputDir; }
		}
	}
}