namespace ChevronMonthlySub.Domain
{
	using System.Collections.Generic;
	using System.Linq;

	public class ProductPurchaseOrder : PurchaseOrder
	{
		public ProductPurchaseOrder(
			IChevronReportAdapter chevronReportAdapter)
			: base(chevronReportAdapter)
		{
			chevronReportAdapter.Type = GetType();
		}

		public IEnumerable<ProductStateGroup> States { get; set; }

		public decimal PickPackCharge
		{
			get { return States.Sum(t => t.PickPackCharges); }
		}

		public decimal BoxCharge
		{
			get { return States.Sum(t => t.BoxCharges); }
		}

		public decimal Total
		{
			get { return PickPackCharge + BoxCharge; }
		}

		public override string ToString()
		{
			return string.Format("{0}{1}", base.ToString(), ".xlsx");
		}

		public override void ConfigureReport()
		{
			base.ConfigureReport();
			ReportAdapter.AddTable("States", States.OrderBy(s => s.StateName));
			ReportAdapter.SetValue("PickPackTotal", PickPackCharge);
			ReportAdapter.SetValue("BoxTotal", BoxCharge);
		}

		public void RunReports()
		{
			ReportAdapter.OutputFileNameWithoutPrefix = ToString();
			ReportAdapter.Run(false);
			ReportAdapter.Run(true);
		}
	}
}