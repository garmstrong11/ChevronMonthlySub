namespace ChevronMonthlySub.Tests.Domain
{
	using ChevronMonthlySub.Domain;
	using FakeItEasy;
	using FluentAssertions;
	using NUnit.Framework;

	[TestFixture]
	public class OrderKeyTests
	{
		[Test]
		public void CanCreate()
		{
			var dto = new OrderKeyRowDto
				{
				Description = "Oink",
				FreightId = "111",
				ProductId = "222",
				RequestorInitials = "JJ"
				};

			var requestor = new Requestor(69, "JJ", "Jenna Jameson");

			var requestorService = A.Fake<IRequestorService>();
			A.CallTo(() => requestorService.Get(A<string>._)).Returns(requestor);

			IOrderKeyFactory factory = new OrderKeyFactory(requestorService);
			var orderKey = factory.Create(dto);

			orderKey.Requestor.Id.Should().Be(69);
		}
	}
}