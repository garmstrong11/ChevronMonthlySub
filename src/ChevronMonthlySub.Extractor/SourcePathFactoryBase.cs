namespace ChevronMonthlySub.Extractor
{
  public abstract class SourcePathFactoryBase<T, TS>  where T : ExtractorBase<TS>
  {
    protected readonly IFileOps Fileops;
    protected readonly ExtractorBase<TS> Extractor;

    protected SourcePathFactoryBase(IFileOps fileops, ExtractorBase<TS> extractor)
    {
      Fileops = fileops;
      Extractor = extractor;
    }

  }
}