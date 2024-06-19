using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Order
    {

        public int Id { get; set; }

        public string ShipAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }

        //public string ShipAddress { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.UtcNow;

        [DisplayName("UserID")]
        public int UserID { get; set; }

        [ValidateNever]
        public User User { get; set; }

        //public string Status { get; set; }

        public IEnumerable<OrderDetail> OrderDetails { get; set; }

    }
}
