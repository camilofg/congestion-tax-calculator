using System;
using Congestion_Models;

namespace congestion_tax_calculator_netcore
{
    public interface ICongestionTaxCalculator
    {
        int GetTax(Vehicle vehicle, DateTime[] dates);
        int GetTollFee(Vehicle vehicle, DateTime date);
    }
}
