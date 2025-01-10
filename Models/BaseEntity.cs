namespace Rentacar.Models
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public string CreatedAt { get; set; } = DateTime.Now.ToString();
        public string UpdatedAt { get; set; } = DateTime.Now.ToString();
    }
}
