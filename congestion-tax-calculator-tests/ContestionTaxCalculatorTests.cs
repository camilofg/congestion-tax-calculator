using Congestion_Models;
using Congestion_Models.Configs;
using congestion_tax_calculator_netcore;
using Newtonsoft.Json;

namespace congestion_tax_calculator_tests
{

    public class ContestionTaxCalculatorTests
    {

        private readonly ICongestionTaxCalculator sut;
        public ContestionTaxCalculatorTests()
        {
            IEnumerable<DateOnly> _holyDates = new DateOnly[] { new DateOnly(2013,1,1),
            new DateOnly(2013, 3, 28),
            new DateOnly(2013, 3,29),
            new DateOnly(2013,4,1) };

            RangesTax _rangeTaxesList = new RangesTax();
            _rangeTaxesList.HourRangesTaxes = new List<HourRangeTax> {
                new HourRangeTax { StartTime = "06:00", EndTime = "06:29", Tax = 8 },
                new HourRangeTax { StartTime = "06:30", EndTime = "06:59", Tax = 13 },
                new HourRangeTax { StartTime = "07:00", EndTime = "07:59", Tax = 18 },
                new HourRangeTax { StartTime = "08:00", EndTime = "08:29", Tax = 13 },
                new HourRangeTax { StartTime = "08:30", EndTime = "14:59", Tax = 8 },
                new HourRangeTax { StartTime = "15:00", EndTime = "15:29", Tax = 13 },
                new HourRangeTax { StartTime = "15:30", EndTime = "16:59", Tax = 18 },
                new HourRangeTax { StartTime = "17:00", EndTime = "17:59", Tax = 13 },
                new HourRangeTax { StartTime = "18:00", EndTime = "18:29", Tax = 8 },
                new HourRangeTax { StartTime = "18:30", EndTime = "05:59", Tax = 0 }
            };

            List<string> _freeTollVehicles = new List<string> { "Motorcycle", "Tractor", "Emergency", "Diplomat", "Foreign", "Military" };

            sut = new CongestionTaxCalculator(_holyDates, _rangeTaxesList, _freeTollVehicles, 60, 60);
        }

        [Fact]
        public void GetTollFee_ShouldReturnZeroForHoliday()
        {
            var car = new Vehicle("Car");
            var result = sut.GetTollFee(car, new DateTime(2013, 1, 1, 10, 0, 0));
            Assert.Equal(0, result);
        }

        [Fact]
        public void GetTollFee_ShouldReturnZeroForFreeTollVehicle()
        {
            var car = new Vehicle("Motorcycle");
            var result = sut.GetTollFee(car, new DateTime(2013, 10, 1, 10, 0, 0));
            Assert.Equal(0, result);
        }

        [Fact]
        public void GetTollFee_ShouldReturnZeroForJuly()
        {
            var car = new Vehicle("Motorcycle");
            var result = sut.GetTollFee(car, new DateTime(2013, 7, 1, 10, 0, 0));
            Assert.Equal(0, result);
        }


        [Fact]
        public void GetTollFee_ShouldReturnZeroForSaturday()
        {
            var car = new Vehicle("Motorcycle");
            var result = sut.GetTollFee(car, new DateTime(2013, 1, 5, 10, 0, 0));
            Assert.Equal(0, result);
        }

        [Fact]
        public void GetTollFee_ShouldReturnZeroForSunday()
        {
            var car = new Vehicle("Motorcycle");
            var result = sut.GetTollFee(car, new DateTime(2013, 1, 6, 10, 0, 0));
            Assert.Equal(0, result);
        }

        [Fact]
        public void GetTollFee_ShouldReturnEightForCarInDateRange()
        {
            var car = new Vehicle("Car");
            var result = sut.GetTollFee(car, new DateTime(2013, 10, 1, 6, 10, 0));
            Assert.Equal(8, result);
        }

        [Fact]
        public void GetTax_ShouldReturn99ForCarInDateRange()
        {
            var dates = new List<DateTime>();
            dates.Add(new DateTime(2013, 1, 2, 6, 10, 0));   //8
            dates.Add(new DateTime(2013, 1, 2, 6, 15, 0));   //8
            dates.Add(new DateTime(2013, 1, 2, 6, 37, 0));   //13
            dates.Add(new DateTime(2013, 1, 2, 7, 19, 0));   //18
            dates.Add(new DateTime(2013, 3, 11, 8, 10, 0));  //13
            dates.Add(new DateTime(2013, 4, 12, 9, 10, 0));  //8
            dates.Add(new DateTime(2013, 5, 15, 12, 10, 0));  //8
            dates.Add(new DateTime(2013, 9, 1, 16, 10, 0));  //18
            dates.Add(new DateTime(2013, 10, 17, 6, 10, 0));  //8
            dates.Add(new DateTime(2013, 12, 21, 8, 10, 0)); //13

            var car = new Vehicle("Car");
            var result = sut.GetTax(car, dates);
            Assert.Equal(99, result);
        }

        [Fact]
        public void GetTax_ShouldReturn96ForCarInDateRange()
        {
            var dates = new List<DateTime>();
            dates.Add(new DateTime(2013, 1, 2, 6, 10, 0));   //8
            dates.Add(new DateTime(2013, 1, 2, 6, 15, 0));   //8
            dates.Add(new DateTime(2013, 1, 2, 6, 37, 0));   //13
            dates.Add(new DateTime(2013, 1, 2, 7, 19, 0));   //18
            dates.Add(new DateTime(2013, 3, 11, 8, 10, 0));   //13
            dates.Add(new DateTime(2013, 4, 12, 9, 10, 0));   //8
            dates.Add(new DateTime(2013, 5, 15, 12, 10, 0));   //8
            dates.Add(new DateTime(2013, 9, 1, 16, 10, 0));   //18
            dates.Add(new DateTime(2013, 10, 21, 6, 10, 0));   //8
            dates.Add(new DateTime(2013, 10, 21, 7, 10, 0));   //18

            var car = new Vehicle("Car");
            var result = sut.GetTax(car, dates);
            Assert.Equal(96, result);
        }

        [Fact]
        public void GetTax_ShouldReturnMax60ForCarInOneDay()
        {
            var dates = new List<DateTime>();
            dates.Add(new DateTime(2013, 1, 2, 6, 10, 0));   //8
            dates.Add(new DateTime(2013, 1, 2, 6, 15, 0));   //8
            dates.Add(new DateTime(2013, 1, 2, 6, 37, 0));   //13
            dates.Add(new DateTime(2013, 1, 2, 7, 19, 0));   //18
            dates.Add(new DateTime(2013, 1, 2, 8, 10, 0));   //13
            dates.Add(new DateTime(2013, 1, 2, 9, 10, 0));   //8
            dates.Add(new DateTime(2013, 1, 2, 12, 10, 0));  //8
            dates.Add(new DateTime(2013, 1, 2, 16, 10, 0));  //18
            var car = new Vehicle("Car");
            var result = sut.GetTax(car, dates);
            Assert.Equal(60, result);
        }
    }
}