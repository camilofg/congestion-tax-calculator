using Congestion_Models;
using System;

namespace congestion.calculator
{
    public interface ICongestionTaxCalculator
    {
        int GetTax(Vehicle vehicle, DateTime[] dates);
        int GetTollFee(Vehicle vehicle, DateTime date);
    }
}
