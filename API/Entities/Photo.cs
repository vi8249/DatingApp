using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("Photos")] // table name
    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
        public int PublicId { get; set; }

        //fully defined relationship
        public AppUser AppUser { get; set; }
        public int AppUserId { get; set; }
    }
}