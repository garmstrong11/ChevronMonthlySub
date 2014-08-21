namespace ChevronMonthlySub.Domain
{
  using System.Collections.Generic;
  using System.Linq;

  public class Invoice
  {
    private readonly IEnumerable<FreightLine> _freightLines;
    private readonly IEnumerable<ProductLine> _productLines;

    public Invoice(IEnumerable<OrderLine> orderLines)
    {
      var enumerable = orderLines as IList<OrderLine> ?? orderLines.ToList();
      _freightLines = enumerable.OfType<FreightLine>().ToList();
      _productLines = enumerable.OfType<ProductLine>().ToList();
    }

    public string InvoiceNumber { get; set; }

    public IEnumerable<ReportData<FreightLine>> FreightOrders
    {
      get { return GetReportData(_freightLines); }
    }

    public IEnumerable<ReportData<ProductLine>> ProductOrders
    {
      get { return GetReportData(_productLines); }
    }

    private static IEnumerable<ReportData<T>> GetReportData<T>(IEnumerable<T> lines) where T : OrderLine
    {
      var query = 
        from line in lines
        group line by new { line.PoNumber, TaxGroup = line.TaxType }
        into orders
        select new ReportData<T>
               {
                 PoNumber = orders.Key.PoNumber,
                 TaxType = orders.Key.TaxGroup,
                 States = 
                   from order in orders
                   group order by order.State
                   into states
                   select new StateOrderGroup<T>
                          {
                            StateName = states.Key,
                            OrderLines = states.ToList()
                          }
               };


      return query.ToList();
    }
  }
}