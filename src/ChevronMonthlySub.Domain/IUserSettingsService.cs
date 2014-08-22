namespace ChevronMonthlySub.Domain
{
	public interface IUserSettingsService
	{
		/// <summary>
		/// The charge for each box shipped.
		/// </summary>
		decimal BoxFee { get; }

		/// <summary>
		/// The charge for handling and packing each product.
		/// </summary>
		decimal PickPackFee { get; }
	}
}