using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class User
    {
        [Key]
        [DisplayName("User-ID")]
        public int Id { get; set; }

        [Required]
        [DisplayName("Name")]
        public string Name { get; set; }

        [Required]
        [DisplayName("User-Name")]
        public string UserName { get; set; }

        [Required]
        [DisplayName("Email")]
        public string Email { get; set; }

        [Required]
        [DisplayName("Password")]
        public string Password { get; set; }

        //[Required]
        [DisplayName("Address")]
        public string? Address { get; set; }


        [DisplayName("Register Date")]
        public DateTime RegisterDate { get; set; }

        [DisplayName("Role")]
        public int RoleID { get; set; }

        [ForeignKey("RoleID")]
        [ValidateNever]
        public Role Role { get; set; }

    }
}
