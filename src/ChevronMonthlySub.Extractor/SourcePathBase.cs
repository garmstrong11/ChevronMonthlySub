namespace ChevronMonthlySub.Extractor
{
  using System.Linq;

  public abstract class SourcePathBase<T, TS> : ISourcePath where T : ExtractorBase<TS>
  {
    private readonly IFileOps _fileOps;
    private readonly T _extractor;

    protected SourcePathBase(IFileOps fileOps, T extractor)
    {
      _fileOps = fileOps;
      _extractor = extractor;
    }

    public bool Exists
    {
      get { return _fileOps.Exists(FullPath); }
    }

    public bool ColumnNamesMatchExtractorColumnDictionary
    {
      get
      {
        _extractor.SourcePath = FullPath;
        return _extractor.ExtractHeaderNames().SequenceEqual(_extractor.ColumnDictionary.Keys);
      }
    }

    public string FullPath { get; internal set; }

    public string Extension
    {
      get { return _fileOps.GetExtension(FullPath); }
    }

    public string FileName
    {
      get { return _fileOps.GetFileName(FullPath); }
    }
  }
}