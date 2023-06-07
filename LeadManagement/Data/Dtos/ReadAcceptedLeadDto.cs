namespace LeadManagement.Data.Dtos
{
    public class ReadAcceptedLeadDto
    {
        public int Id { get; set; }

        public string ContactFullName { get; set; }

        public string ContactPhoneNumber { get; set; }

        public string ContactEmail { get; set; }

        public string Category { get; set; }

        public string Description { get; set; }

        public string Suburb { get; set; }

        public DateTime CreatedAt { get; set; }

        public decimal Price { get; set; }

        public decimal FinalPrice { get; set; }

        public string Status { get; set; }
    }
}
