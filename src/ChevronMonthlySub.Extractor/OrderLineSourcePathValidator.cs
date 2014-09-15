namespace ChevronMonthlySub.Extractor
{
  using FluentValidation;

  public class OrderLineSourcePathValidator : AbstractValidator<OrderLineSourcePath>
  {
    private readonly IFileOps _fileOps;

    public OrderLineSourcePathValidator(IFileOps fileOps)
    {
      _fileOps = fileOps;

      RuleSet("File", () =>
      {
        RuleFor(x => x.FullPath).NotEmpty();

        RuleFor(x => x.FullPath)
          .Must(s => _fileOps.Exists(s))
          .WithMessage("File does not exist");

        RuleFor(x => x.Extension)
          .Matches(@"\.xlsx?")
          .WithMessage("File is not an Excel file.");
      });

      RuleSet("Name", () => RuleFor(x => x.FileName)
        .Matches(@"\d{6,}")
        .WithMessage("File name does not contain an invoice number."));

      RuleSet("Structure", () => RuleFor(x => x.ColumnNamesMatchExtractorColumnDictionary)
        .Equal(true)
        .WithMessage("Column Names in this file do not match the column names for this extractor."));
    }
  }
}