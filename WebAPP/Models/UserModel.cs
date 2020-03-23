using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPP.Models
{
    /// <summary>
    /// Model of user for api request.
    /// </summary>
    public class UserModel
    {
        public string Name { get; set; }
        public string DepartmentName { get; set; }
    }
}
