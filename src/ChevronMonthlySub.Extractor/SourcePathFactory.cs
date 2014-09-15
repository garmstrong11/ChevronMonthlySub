namespace ChevronMonthlySub.Extractor
{
  using System;
  using Domain;

	public class SourcePathFactory<T> : ISourcePathFactory<T>
	{
	  private readonly IFileOps _fileOps;
	  private readonly IExtractor<T> _extractor;
		
		public SourcePathFactory(IFileOps fileops, IExtractor<T> extractor)
		{
			_fileOps = fileops;
			_extractor = extractor;
		}

    public SourcePath<T> Create(string fullPath)
    {
      if (string.IsNullOrEmpty(fullPath))
      {
        throw new ArgumentNullException("fullPath");
      }

      return new SourcePath<T>(_fileOps, _extractor) {FullPath = fullPath};
    }
  }
}