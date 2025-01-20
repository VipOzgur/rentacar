namespace Rentacar.Models
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public string CreateDate { get; set; } = DateTime.Now.ToString();
        public string UpdateDate { get; set; } = DateTime.Now.ToString();
    }


}
