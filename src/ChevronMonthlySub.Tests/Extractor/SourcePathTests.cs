namespace ChevronMonthlySub.Tests.Extractor
{
	using System.Collections.Generic;
	using ChevronMonthlySub.Domain;
	using ChevronMonthlySub.Extractor;
  using FakeItEasy;
	using FluentAssertions;
	using NUnit.Framework;

  [TestFixture]
  public class SourcePathTests
  {
	  private IFileOps _fileOps;
	  private IExtractor<FlexCelOrderLineDto> _extractor;
	  private SourcePathFactory<FlexCelOrderLineDto> _factory;

	  [SetUp]
	  public void Init()
	  {
		  _fileOps = A.Fake<IFileOps>();
		  _extractor = A.Fake<IExtractor<FlexCelOrderLineDto>>();
			_factory = new SourcePathFactory<FlexCelOrderLineDto>(_fileOps, _extractor);
	  }
		
		[TestCase(true, Result = true)]
		[TestCase(false, Result = false)]
    public bool TestExists(bool exists)
    {
	    A.CallTo(() => _fileOps.Exists(A<string>._)).Returns(exists);

      var source = _factory.Create(@"F:\Mularkey.txt");

	    return source.Exists;
    }

		[Test]
	  public void ColumnNameMatch_NotSame_ReturnsFalse()
	  {
		  var dict = new Dictionary<string, int> { {"ColumnOne", 1}, {"ColumnTwo", 2} };
		  var list = new List<string> {"ColumnOne", "ColumnThree"};

			A.CallTo(() => _extractor.ColumnDictionary).Returns(dict);
		  A.CallTo(() => _extractor.ExtractHeaderNames()).Returns(list);

		  var source = _factory.Create("babar.xlsx");

		  source.ColumnNamesMatchExtractorColumnDictionary.Should().BeFalse();
	  }

		[Test]
		public void ColumnNameMatch_Same_ReturnsTrue()
		{
			var dict = new Dictionary<string, int> { { "ColumnOne", 1 }, { "ColumnTwo", 2 } };
			var list = new List<string> { "ColumnOne", "ColumnTwo" };

			A.CallTo(() => _extractor.ColumnDictionary).Returns(dict);
			A.CallTo(() => _extractor.ExtractHeaderNames()).Returns(list);

			var source = _factory.Create("babar.xlsx");

			source.ColumnNamesMatchExtractorColumnDictionary.Should().BeTrue();
		}
  }
}