namespace ChevronMonthlySub.Tests.Extractor
{
	using System;
	using ChevronMonthlySub.Domain;
	using ChevronMonthlySub.Extractor;
	using FakeItEasy;
	using FluentAssertions;
	using NUnit.Framework;

	[TestFixture]
	public class SourcePathFactoryTests
	{
		private IFileOps _fileOps;
		private IExtractor<FlexCelOrderLineDto> _extractor;
		private SourcePathFactory<FlexCelOrderLineDto> _factory;
			
		[SetUp]
		public void Setup()
		{
			_fileOps = A.Fake<IFileOps>();
			_extractor = A.Fake<IExtractor<FlexCelOrderLineDto>>();
			_factory = new SourcePathFactory<FlexCelOrderLineDto>(_fileOps, _extractor);
		}
		
		[Test]
		public void Create_NotNullStringParam_CreatesSourcePathObject()
		{
			var source = _factory.Create(@"\\NLS_SERVER\Global_User\garmstrong\Chevron\ServiceOrderKey.xls");
			source.Should().NotBeNull();
		}

		[Test]
		public void Create_NullStringParam_ThrowsArgumentNullException()
		{
			Action act = () => _factory.Create("");

			act.ShouldThrow<ArgumentNullException>().WithMessage("*fullPath*");
		}
	}
}