namespace ChevronMonthlySub.Extractor
{
  using System.Linq;
  using Domain;

	public class SourcePath<T> : ISourcePath
  {
    private readonly IFileOps _fileOps;
    private readonly IExtractor<T> _extractor;

    internal SourcePath(IFileOps fileOps, IExtractor<T> extractor)
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

    public string FullPath { get; set; }

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