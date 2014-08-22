namespace ChevronMonthlySub.Domain
{
	using System.Collections.Generic;

	public interface IRecipientRepository
	{
		IEnumerable<Recipient> GetAll();
		Recipient Get(int id);
		Recipient Get(string initials);
	}
}