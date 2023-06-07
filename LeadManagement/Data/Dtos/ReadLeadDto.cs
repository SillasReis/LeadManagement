namespace LeadManagement.Data.Dtos
{
    public class ReadLeadDto
    {
        public int Id { get; set; }

        public string ContactFirstName { get; set; }

        public string Category { get; set; }

        public string Description { get; set; }

        public string Suburb { get; set; }

        public DateTime CreatedAt { get; set; }

        public decimal Price { get; set; }

        public string Status { get; set; }
    }
}
