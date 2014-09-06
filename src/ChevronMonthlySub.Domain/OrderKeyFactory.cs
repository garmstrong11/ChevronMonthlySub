namespace ChevronMonthlySub.Domain
{
	public class OrderKeyFactory : IOrderKeyFactory
	{
		private readonly IRequestorService _requestorService;

		public OrderKeyFactory(IRequestorService requestorService)
		{
			_requestorService = requestorService;
		}
		
		public OrderKey Create(OrderKeyRowDto dto)
		{
			var orderKey = new OrderKey
				{
				ProductId = dto.ProductId,
				FreightId = dto.FreightId,
				Description = dto.Description,
				Requestor = _requestorService.Get(dto.RequestorInitials)
				};

			return orderKey;
		}
	}
}