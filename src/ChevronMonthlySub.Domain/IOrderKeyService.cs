namespace ChevronMonthlySub.Domain
{
	using System.Collections.Generic;

	public interface IOrderKeyService
	{
		Dictionary<string, OrderKey> AcquireOrderKeys();
	}
}