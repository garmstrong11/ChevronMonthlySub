namespace ChevronMonthlySub.Extractor
{
  public interface ISourcePathFactory<T, TS> where T : ExtractorBase<TS>
  {
    SourcePathBase<T, TS> Create(string fullPath);
  }
}