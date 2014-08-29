﻿namespace ChevronMonthlySub.UI.Infra
{
	using System.Collections.Generic;
	using Domain;

	public class RecipientRepository : IRecipientRepository
	{
		 private readonly List<Recipient> _requestors;

		public RecipientRepository()
		{
			_requestors = new List<Recipient>
				{
				new Recipient(1, "KR", "Katherine Rosales"),
				new Recipient(2, "KH", "Kristen Herman"),
				new Recipient(3, "AA", "Arlita Acuesta"),
				new Recipient(4, "SG", "Stephen Graber"),
				new Recipient(5, "MM", "Mark Matheny"),
				new Recipient(6, "ML", "Maybe Later")
				};
		}
		
		public IEnumerable<Recipient> GetAll()
		{
			return _requestors;
		}

		public Recipient Get(int id)
		{
			return _requestors.Find(r => r.Id.Equals(id));
		}

		public Recipient Get(string initials)
		{
			return _requestors.Find(r => r.Initials.Equals(initials));
		}
	}
}