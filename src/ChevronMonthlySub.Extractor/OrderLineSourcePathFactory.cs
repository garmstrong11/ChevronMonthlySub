namespace ChevronMonthlySub.Extractor
{
  using System;
  using Domain;

  public class OrderLineSourcePathFactory : 
    SourcePathFactoryBase<OrderLineExtractor, FlexCelOrderLineDto>
  {
    public OrderLineSourcePathFactory(IFileOps fileops, OrderLineExtractor extractor) 
      : base(fileops, extractor)
    {
    }

    public OrderLineSourcePath Create(string fullPath)
    {
      if (string.IsNullOrEmpty(fullPath))
      {
        throw new ArgumentNullException("fullPath");
      }

      return new OrderLineSourcePath(Fileops, Extractor) {FullPath = fullPath};
    }
  }
}