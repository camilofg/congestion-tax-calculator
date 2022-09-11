using Congestion_Models;
using Congestion_Models.Configs;


namespace congestion_tax_calculator_netcore
{
    public class CongestionTaxCalculator : ICongestionTaxCalculator
    {
        /**
       * Calculate the total toll fee for one a vehicle
       *
       * @param vehicle - the vehicle
       * @param dates   - date and time of all passes of one vehicle
       * @return - the total congestion tax for that vehicle
       */

        private readonly IEnumerable<DateOnly> _holyDates;
        private readonly RangesTax _rangeTaxesList;
        private readonly List<string> _freeTollVehicles;

        public CongestionTaxCalculator(IEnumerable<DateOnly> holyDates, RangesTax rangeTaxesList, List<string> freeTollVehicles)
        {
            _holyDates = holyDates;
            _rangeTaxesList = rangeTaxesList;
            _freeTollVehicles = freeTollVehicles;
        }

        #region Public methods
        public int GetTax(Vehicle vehicle, DateTime[] dates)
        {
            var daysFee = new List<Tuple<DateOnly, int>>();
            int totalFee = 0;
            var datesByDay = dates.GroupBy(x => DateOnly.FromDateTime(x.Date));

            foreach (var day in datesByDay)
            {
                int dayFee = 0;
                if (!IsTollFreeDate(day.Key.ToDateTime(TimeOnly.Parse("12:00 AM"))))
                {
                    var pivotDay = day.Select(x => x).ToList();
                    while (pivotDay.Count() > 0)
                    {
                        var tollsInPeriod = pivotDay.Where(x => x <= pivotDay.FirstOrDefault().AddMinutes(60));
                        var hourFeePivot = 0;
                        foreach (var toll in tollsInPeriod)
                        {
                            hourFeePivot = Math.Max(hourFeePivot, GetTimeFee(TimeOnly.FromDateTime(toll)));
                        }
                        dayFee += hourFeePivot;
                        if (dayFee >= 60)
                        {
                            dayFee = 60;
                            pivotDay.Clear();
                        }
                        pivotDay = pivotDay.Where(x => !tollsInPeriod.Contains(x)).ToList();
                        Console.WriteLine(pivotDay.Count());
                    }
                    daysFee.Add(Tuple.Create(day.Key, dayFee));
                    totalFee += dayFee;
                }
            }
            return totalFee;
        }

        public int GetTollFee(Vehicle vehicle, DateTime date)
        {
            if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle)) return 0;

            TimeOnly time = TimeOnly.FromDateTime(date);
            return GetTimeFee(time);
        }

        #endregion

        #region Private methods
        private int GetTimeFee(TimeOnly time)
        {
            return _rangeTaxesList.HourRangesTaxes
                            .Where(x => time.IsBetween(x.StartTimeOnly, x.EndTimeOnly))
                            .Select(x => x.Tax).FirstOrDefault();
        }

        private bool IsTollFreeVehicle(Vehicle vehicle)
        {
            if (vehicle == null) return false;
            string vehicleType = vehicle.VehicleType;

            return _freeTollVehicles.Any(x => x.Equals(vehicleType, StringComparison.OrdinalIgnoreCase));
        }

        private bool IsTollFreeDate(DateTime date)
        {
            return (IsWeekendOrJuly(date))
                ? true : _holyDates.Any(d => d == DateOnly.FromDateTime(date));
        }

        private static bool IsWeekendOrJuly(DateTime date)
        {
            return date.Month == 7 || date.ToString("dddd") == "Saturday" || date.ToString("dddd") == "Sunday";
        }
        #endregion
    }
}
