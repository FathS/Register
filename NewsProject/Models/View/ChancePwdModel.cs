using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NewsProject.Models.View
{
    public class ChancePwdModel
    { 
        [Required]
        public string OldPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
        
    }
}
