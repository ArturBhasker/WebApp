
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPP.DbModels
{
    /// <summary>
    /// Class for working with user table in DB
    /// </summary>
    public class UserDbModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        public DepartmentDbModel Department { get; set; }
    }
}