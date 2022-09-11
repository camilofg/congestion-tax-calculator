namespace Congestion_Models.Configs
{
    public record HourRangeTax
    {
        private string _startTime;
        private string _endTime;
        public string StartTime
        {
            set
            {
                _startTime = value;
                StartTimeOnly = ConvertStrToTimeOnly(value);
            }
        }
        public TimeOnly StartTimeOnly { get; set; }
        public string EndTime
        {
            set
            {
                _endTime = value;
                EndTimeOnly = ConvertStrToTimeOnly(value);
            }
        }
        public TimeOnly EndTimeOnly { get; set; }
        public int Tax { get; set; }

        private TimeOnly ConvertStrToTimeOnly(string input)
        {
            if (input == null)
                throw new ArgumentNullException(input);

            var splited = input.Split(":");
            return new TimeOnly(int.Parse(splited[0]), int.Parse(splited[1]));
        }
    }
}
