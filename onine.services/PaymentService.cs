using Microsoft.Extensions.Options;
using onine.Repository.Data;
using onine.services.Interfaces;
using onine.services.Settings;
using System.Data.Entity;

namespace onine.services
{
    public class PaymentService : IPaymentService
    {
        private readonly Oninecontext _context;
        private readonly PaymentSettings _paymentSettings;

        public PaymentService(Oninecontext context, IOptions<PaymentSettings> options)
        {
            _context = context;
            _paymentSettings = options.Value;
        }

        public async Task<string> CreatePaymentIntentAsync(int orderId, string buyerEmail)
        {
            var order = await _context.Orders.AsNoTracking().FirstOrDefaultAsync(o => o.Id == orderId && o.BuyerEmail == buyerEmail);
            if (order == null)
            {
                throw new InvalidOperationException("Order not found");
            }
            if (string.IsNullOrEmpty(_paymentSettings.SecretKey))
            {
                throw new InvalidOperationException("Stripe secret key is not configured");
            }
            var amount = order.Subtotal + order.DeliveryCost;
            if (amount <= 0)
            {
                throw new InvalidOperationException("Order amount must be greater than zero");
            }
            var stripeClient = new Stripe.StripeClient(_paymentSettings.SecretKey);   
            var paymentIntentService = new Stripe.PaymentIntentService(stripeClient);
            var options = new Stripe.PaymentIntentCreateOptions
            {
                Amount = (long) Math.Round (amount * 100 , 0,MidpointRounding.AwayFromZero),
                Currency = "usd",
                AutomaticPaymentMethods = new Stripe.PaymentIntentAutomaticPaymentMethodsOptions
                {
                    Enabled = true,
                },
                Metadata = new Dictionary<string, string>
                {
                    { "order_id", order.Id.ToString() },
                    { "buyer_email", order.BuyerEmail }
                }

            };
            var paymentIntent = await paymentIntentService.CreateAsync(options);
           

            return  paymentIntent.ClientSecret

            ?? throw new InvalidOperationException(
                "Strip Client Secret is missing");

        }
    }
}