namespace ChevronMonthlySub.Extractor
{
	public interface ISourcePathFactory<T>
	{
		SourcePath<T> Create(string fullPath);
	}
}