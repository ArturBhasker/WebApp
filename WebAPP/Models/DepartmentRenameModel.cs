using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPP.Models
{
    /// <summary>
    /// Model to rename department for api request
    /// </summary>
    public class DepartmentRenameModel
    {
        /// <summary>
        /// Name of department to rename.
        /// </summary>
        public string OldName { get; set; }
        /// <summary>
        /// Name of department after renaming.
        /// </summary>
        public string NewName { get; set; }
    }
}
