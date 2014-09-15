namespace ChevronMonthlySub.Extractor
{
  public interface ISourcePath
  {
    bool Exists { get; }
    bool ColumnNamesMatchExtractorColumnDictionary { get; }
    string FullPath { get; }
    string Extension { get; }
    string FileName { get; }
  }
}