
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPP.DbModels
{
    /// <summary>
    /// Class for working with department table in DB
    /// </summary>
    public class DepartmentDbModel
    {
        [Key]
        public int Id { get; set; }

        [ServiceStack.DataAnnotations.Required]
        [Index(unique: true)]
        public string Name { get; set; }
    }
}
