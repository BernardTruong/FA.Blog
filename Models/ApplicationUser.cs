using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.JustBlog.Core.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int? age { get; set; }
        public string? aboutMe { get; set; }
    }
}
