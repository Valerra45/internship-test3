namespace Task3.Models
{
    public class User
    {
        public string? Name { get; set; }

        public int Rating { get; set; }

        public List<long>? CoinIds { get; set; }
    }
}
