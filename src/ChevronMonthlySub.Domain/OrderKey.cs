namespace ChevronMonthlySub.Domain
{
	public class OrderKey
	{
		internal OrderKey()
		{}

		public string ProductId { get; internal set; }
		public string FreightId { get; internal set; }
		public string Description { get; internal set; }
		public Requestor Requestor { get; internal set; }
	}
}