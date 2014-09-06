namespace ChevronMonthlySub.Domain
{
	public interface IOrderKeyFactory
	{
		OrderKey Create(OrderKeyRowDto dto);
	}
}