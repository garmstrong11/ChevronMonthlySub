namespace ChevronMonthlySub.Tests.Extractor
{
	using ChevronMonthlySub.Domain;
	using ChevronMonthlySub.Extractor;
	using FakeItEasy;
	using FluentAssertions;
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
		
		[Test]
	  public void Validation_FileExists()
		{
			var sourcePath = _factory.Create("bonobo.txt");
			A.CallTo(() => _fileOps.Exists(A<string>._)).Returns(false);

			var result = _validator.Validate(sourcePath, ruleSet: "File");
			result.Errors.Should().Contain(e => e.ErrorMessage.Contains("exist"));
		}
  }
}