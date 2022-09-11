namespace Congestion_Models.Dto
{
    public record TaxDto
    {
        public Vehicle Vehicle { get; set; }
        public List<DateTime> Dates { get; set; }
    }
}
