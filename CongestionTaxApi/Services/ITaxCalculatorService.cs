using Congestion_Models;

namespace CongestionTaxApi.Services
{
    public interface ITaxCalculatorService
    {
        int GetTax(Vehicle vehicle, DateTime[] dates);
        int GetTollFee(Vehicle vehicle, DateTime date);
    }
}
