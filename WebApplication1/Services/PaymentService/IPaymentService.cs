using WebApplication1.Models;

namespace WebApplication1.Services
{
    public interface IPaymentService
    {
        Task<IEnumerable<Payment>> GetPayments();

        Task<Payment?> GetPaymentById(int id);

        Task<Payment> AddPayment(Payment payment);

        Task<Payment> UpdatePayment(Payment payment);

        Task<bool> DeletePayment(int id);
    }
}