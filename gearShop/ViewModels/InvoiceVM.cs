using Domain.Entities;

namespace GearShopWeb.ViewModels;

public class InvoiceVM
{
    public Order Order { get; set; }
    public IEnumerable<OrderDetail> OrderDetail { get; set; }
}
