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

    public IEnumerable<TaxGroup<FreightLine>> FreightOrders
    {
      get { return GetOrders(_freightLines); }
    }

    public IEnumerable<TaxGroup<ProductLine>> ProductOrders
    {
      get { return GetOrders(_productLines); }
    }

    private static IEnumerable<TaxGroup<T>> GetOrders<T>(IEnumerable<T> lines) where T : OrderLine
    {
      var query = 
        from line in lines
        group line by new { line.PoNumber, line.TaxGroup }
        into orders
        select new TaxGroup<T>
               {
                 PoNumber = orders.Key.PoNumber,
                 GroupName = orders.Key.TaxGroup,
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