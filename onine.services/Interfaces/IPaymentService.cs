namespace onine.services.Interfaces
{
	public interface IPaymentService
	{
		Task<string> CreatePaymentIntentAsync(int orderId, string buyerEmail);
	}
}