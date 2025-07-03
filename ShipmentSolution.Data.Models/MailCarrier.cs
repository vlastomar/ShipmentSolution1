using ShipmentSolution.Data.Models;

public class MailCarrier
{
    public int MailCarrierId { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int RouteId { get; set; }
    public string CurrentLocation { get; set; } = null!;
    public string Status { get; set; } = null!;
    public bool IsDeleted { get; set; }


    public ICollection<Route> Routes { get; set; } = new List<Route>();

    public ICollection<Delivery> Deliveries { get; set; } = new List<Delivery>();
}
