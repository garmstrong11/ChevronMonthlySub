namespace ChevronMonthlySub.Extractor
{
  using Domain;

  public class OrderLineSourcePath : SourcePathBase<OrderLineExtractor, FlexCelOrderLineDto>
  {
    public OrderLineSourcePath(IFileOps fileOps, OrderLineExtractor extractor) 
      : base(fileOps, extractor)
    {
    }
  }
}