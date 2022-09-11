using Congestion_Models;
using congestion_tax_calculator_netcore;
using CongestionTaxApi.Services;
using Moq;

namespace CongestionTaxApiTests
{
    public class TaxCalculatorServiceTests
    {
        private readonly Mock<ICongestionTaxCalculator> _congestionTaxCalculatorMock;

        public TaxCalculatorServiceTests()
        {
            _congestionTaxCalculatorMock = new Mock<ICongestionTaxCalculator>();
        }

        [Fact]
        public void GetTax_ShouldReturn18()
        {
            _congestionTaxCalculatorMock.Setup(x => x.GetTax(It.IsAny<Vehicle>(), It.IsAny<List<DateTime>>())).Returns(18);
            var sut = new TaxCalculatorService(_congestionTaxCalculatorMock.Object);
            var car = new Vehicle("Car");

            var dates = new List<DateTime>();
            dates.Add(new DateTime(2013, 1, 2, 6, 50, 0));
            dates.Add(new DateTime(2013, 1, 2, 7, 19, 0));
            var result = sut.GetTax(car, dates);
            Assert.Equal(18, result);
        }

        [Fact]
        public void GetTollFee_ShouldReturn18()
        {
            _congestionTaxCalculatorMock.Setup(x => x.GetTollFee(It.IsAny<Vehicle>(), It.IsAny<DateTime>())).Returns(18);
            var sut = new TaxCalculatorService(_congestionTaxCalculatorMock.Object);
            var car = new Vehicle("Car");

            var date = new DateTime(2013, 1, 2, 7, 19, 0);
            var result = sut.GetTollFee(car, date);
            Assert.Equal(18, result);
        }
    }
}