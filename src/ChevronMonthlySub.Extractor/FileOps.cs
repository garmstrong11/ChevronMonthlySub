namespace ChevronMonthlySub.Extractor
{
  using System.IO;

  public class FileOps : IFileOps
  {
    public bool Exists(string path)
    {
      return File.Exists(path);
    }

    public string GetExtension(string path)
    {
      var nym = Path.GetExtension(path);
      return nym ?? string.Empty;
    }

    public string GetFileName(string path)
    {
      var nym = Path.GetFileNameWithoutExtension(path);
      return nym ?? string.Empty;
    }
  }
}