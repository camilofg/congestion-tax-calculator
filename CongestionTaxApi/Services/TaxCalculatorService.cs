using Congestion_Models;
using System.Resources;
using System.Reflection;
using congestion_tax_calculator_netcore;

namespace CongestionTaxApi.Services
{
    public class TaxCalculatorService : ITaxCalculatorService
    {
        private readonly ICongestionTaxCalculator _congestionTaxCalculator;
       
        public TaxCalculatorService(ICongestionTaxCalculator congestionTaxCalculator)
        {
            _congestionTaxCalculator = congestionTaxCalculator;
        }

        #region public methods
        public int GetTax(Vehicle vehicle, DateTime[] dates)
        {
            return _congestionTaxCalculator.GetTax(vehicle, dates);
        }

        public int GetTollFee(Vehicle vehicle, DateTime date)
        {
            return _congestionTaxCalculator.GetTollFee(vehicle, date);
        }
        #endregion
    }
}
