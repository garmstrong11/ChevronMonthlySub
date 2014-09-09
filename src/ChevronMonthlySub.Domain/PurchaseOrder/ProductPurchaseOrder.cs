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

		public override void UpdateWithOrderKey(Dictionary<string, OrderKey> orderKeys)
		{
			OrderKey key;
			if (!orderKeys.TryGetValue(PoNumber, out key)) return;

			Description = key.Description;
			Requestor = key.Requestor;
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

		public override void UpdatePoNumber(string newPo)
		{
			PoNumber = newPo;
			var orderLines = States.SelectMany(p => p.OrderLines);
			foreach (var orderLine in orderLines)
			{
				orderLine.PoNumber = newPo;
			}
		}

		public override void RunReports()
		{
			ReportAdapter.OutputFileNameWithoutPrefix = ToString();
			ReportAdapter.Run(false);
			ReportAdapter.Run(true);
		}
	}
}