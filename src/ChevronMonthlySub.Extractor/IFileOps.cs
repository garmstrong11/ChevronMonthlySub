namespace ChevronMonthlySub.Extractor
{
  public interface IFileOps
  {
    bool Exists(string path);
    string GetExtension(string path);
    string GetFileName(string path);
  }
}