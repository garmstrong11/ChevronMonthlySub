namespace ChevronMonthlySub.Tests.Extractor
{
	using System.Collections.Generic;
	using System.Linq;
	using ChevronMonthlySub.Domain;
	using ChevronMonthlySub.Extractor;
	using FakeItEasy;
	using FluentValidation;
	using NUnit.Framework;
	using UI.Validators;

	[TestFixture]
  public class OrderLineSourcePathValidatorTests
  {
	  private OrderLineSourcePathValidator _validator;
		private IExtractor<FlexCelOrderLineDto> _extractor;
		private ISourcePathFactory<FlexCelOrderLineDto> _factory; 
		private IFileOps _fileOps;
			
		[SetUp]
	  public void Init()
		{
			_fileOps = A.Fake<IFileOps>();
			_extractor = A.Fake<IExtractor<FlexCelOrderLineDto>>();
			_validator = new OrderLineSourcePathValidator(_fileOps);
			_factory = new SourcePathFactory<FlexCelOrderLineDto>(_fileOps, _extractor);
		}

		[TestCase(true, Result = false)]
		[TestCase(false, Result = true)]
		public bool FileValidation_FileExists_ContainsErrors(bool fileExist)
		{
			var sourcePath = _factory.Create("bozo.txt");
			A.CallTo(() => _fileOps.Exists(A<string>._)).Returns(fileExist);

			var result = _validator.Validate(sourcePath, ruleSet: "File");

			return result.Errors.Any(e => e.ErrorMessage.Contains("not exist"));
		}

		[TestCase(".txt", Result = true)]
		[TestCase(".xls", Result = false)]
		[TestCase(".xlsx", Result = false)]
		public bool FileValidation_ExcelFile_ContainsErrors(string ext)
		{
			var sourcePath = _factory.Create("bozo.txt");
			A.CallTo(() => _fileOps.GetExtension(A<string>._)).Returns(ext);

			var result = _validator.Validate(sourcePath, ruleSet: "File");

			return result.Errors.Any(e => e.ErrorMessage.Contains("Excel file"));
		}

		[TestCase("2222222", Result = false)]
		[TestCase("boogaloo", Result = true)]
		[TestCase("54", Result = true)]
		public bool NameValidation_InvoiceNumber_ContainsErrors(string name)
		{
			var sourcePath = _factory.Create("bozo.txt");
			A.CallTo(() => _fileOps.GetFileName(A<string>._)).Returns(name);

			var result = _validator.Validate(sourcePath, ruleSet: "Name");

			return result.Errors.Any(e => e.ErrorMessage.Contains("invoice"));
		}

		[TestCase("Two", Result = false)]
		[TestCase("Three", Result = true)]
		public bool StructureValidation_MismatchedColumnNameLists_ContainsErrors(string column)
		{
			var dict = new Dictionary<string, int> { {"One", 1}, {"Two", 2} };
			var list = new List<string> {"One", column};

			var sourcePath = _factory.Create("bozo.xlsx");
			A.CallTo(() => _extractor.ColumnDictionary).Returns(dict);
			A.CallTo(() => _extractor.ExtractHeaderNames()).Returns(list);

			var result = _validator.Validate(sourcePath, ruleSet: "Structure");

			return result.Errors.Any(e => e.ErrorMessage.Contains("extractor"));
		}
  }
}