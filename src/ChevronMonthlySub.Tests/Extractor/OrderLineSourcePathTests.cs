namespace ChevronMonthlySub.Tests.Extractor
{
  using ChevronMonthlySub.Domain;
  using ChevronMonthlySub.Extractor;
  using FakeItEasy;
  using NUnit.Framework;

  [TestFixture]
  public class OrderLineSourcePathTests
  {
    [Test]
    public void CanCreate()
    {
      var fileOps = A.Fake<IFileOps>();
      var extractor = A.Fake<ExtractorBase<FlexCelOrderLineDto>>();
      var source = new OrderLineSourcePath(fileOps, extractor);
    }
  }
}