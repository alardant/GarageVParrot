namespace GarageVParrot.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Grade { get; set; }
        public bool Accepted { get; set; }
        public DateTime datePublished { get; set; } = DateTime.Now;
    }
}
