namespace ChevronMonthlySub.Domain
{
  using System.Collections.Generic;
  using System.Linq;

	public class FreightPurchaseOrder : PurchaseOrder
	{
		public FreightPurchaseOrder(
			IChevronReportAdapter chevronReportAdapter)
			: base(chevronReportAdapter)
		{
			chevronReportAdapter.Type = GetType();
		}

	  public IEnumerable<FreightStateGroup> States { get; set; } 

		public decimal LineAmountSubtotal
		{
			get { return States.Sum(t => t.LineAmountSubtotal); }
		}

		public decimal TaxAmountSubtotal
		{
			get { return States.Sum(t => t.TaxAmountSubtotal); }
		}

		public decimal Total
		{
			get { return LineAmountSubtotal + TaxAmountSubtotal; }
		}

		public override void UpdateWithOrderKey(Dictionary<string, OrderKey> orderKeys)
		{
			OrderKey key;
			if (!orderKeys.TryGetValue(PoNumber, out key)) return;

			Description = key.Description;
			Requestor = key.Requestor;
			PoNumber = key.FreightId;
		}

		public override string ToString()
	  {
	    return string.Format("{0} {1}", base.ToString(), "FRT.xlsx");
	  }

	  public override void ConfigureReport()
	  {
	    base.ConfigureReport();
      ReportAdapter.AddTable("States", States.OrderBy(s => s.StateName));
      ReportAdapter.SetValue("SubTotal", LineAmountSubtotal);
      ReportAdapter.SetValue("TaxTotal", TaxAmountSubtotal);
	  }

		public override void UpdatePoNumber(string newPo)
		{
			PoNumber = newPo;
			var orderLines = States.SelectMany(p => p.OrderLines);
			foreach (var orderLine in orderLines) {
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