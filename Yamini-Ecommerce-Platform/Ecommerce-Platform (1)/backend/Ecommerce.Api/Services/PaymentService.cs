
namespace Ecommerce.Api.Services
{
    public interface IPaymentService
    {
        // returns a dummy payment confirmation id
        string ProcessPayment(decimal amount, string currency = "USD");
    }

    // Mock payment service that simulates Stripe/PayPal processing.
    public class MockPaymentService : IPaymentService
    {
        public string ProcessPayment(decimal amount, string currency = "USD")
        {
            // In real project, call Stripe/PayPal SDKs here (test mode).
            return $"PAY-{Guid.NewGuid().ToString().Substring(0,8)}";
        }
    }
}
