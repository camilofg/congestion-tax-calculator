namespace Congestion_Models.Dto
{
    public record TollFeeDto
    {
        public Vehicle Vehicle { get; set; }
        public DateTime Date { get; set; }
    }
}
