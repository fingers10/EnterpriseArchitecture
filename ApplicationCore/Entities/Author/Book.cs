using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fingers10.EnterpriseArchitecture.ApplicationCore.Entities.Author
{
    public class Book
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(1500)]
        public string Description { get; set; }

        [ForeignKey("AuthorId")]
        public Author Author { get; set; }

        public long AuthorId { get; set; }
    }
}
