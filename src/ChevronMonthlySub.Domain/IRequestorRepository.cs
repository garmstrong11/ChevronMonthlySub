namespace ChevronMonthlySub.Domain
{
	using System.Collections.Generic;

	public interface IRequestorRepository
	{
		IEnumerable<Requestor> GetAll();
		Requestor Get(int id);
		Requestor Get(string initials);
	}
}