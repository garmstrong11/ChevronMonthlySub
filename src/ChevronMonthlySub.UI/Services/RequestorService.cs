namespace ChevronMonthlySub.UI.Services
{
	using System.Collections.Generic;
	using Domain;

	public class RequestorService : IRequestorService
	{
		 private readonly List<Requestor> _requestors;

		public RequestorService()
		{
			_requestors = new List<Requestor>
				{
				new Requestor(1, "KR", "Katherine Rosales"),
				new Requestor(2, "KH", "Kristen Herman"),
				new Requestor(3, "AA", "Arlita Acuesta"),
				new Requestor(4, "SG", "Stephen Graber"),
				new Requestor(5, "MM", "Mark Matheny"),
				new Requestor(6, "ML", "Maybe Later")
				};
		}
		
		public IEnumerable<Requestor> GetAll()
		{
			return _requestors;
		}

		public Requestor Get(int id)
		{
			return _requestors.Find(r => r.Id.Equals(id));
		}

		public Requestor Get(string initials)
		{
			return _requestors.Find(r => r.Initials.Equals(initials));
		}
	}
}