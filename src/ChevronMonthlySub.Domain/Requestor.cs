namespace ChevronMonthlySub.Domain
{
	public class Requestor
	{
		public int Id { get; set; }
		public string Initials { get; set; }
		public string Name { get; set; }

		protected bool Equals(Requestor other)
		{
			return string.Equals(Name, other.Name) 
				&& string.Equals(Initials, other.Initials) 
				&& Id == other.Id;
		}

		public Requestor(int id, string initials, string name)
		{
			Name = name;
			Initials = initials;
			Id = id;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			return obj.GetType() == GetType() && Equals((Requestor) obj);
		}

		public override int GetHashCode()
		{
			unchecked {
				var hashCode = Name.GetHashCode();
				hashCode = (hashCode * 397) ^ Initials.GetHashCode();
				hashCode = (hashCode * 397) ^ Id;
				return hashCode;
			}
		}

		public static bool operator ==(Requestor left, Requestor right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(Requestor left, Requestor right)
		{
			return !Equals(left, right);
		}

		public override string ToString()
		{
			return Initials;
		}
	}
}