public class Booking
{
    public int Id { get; set; }
    public string CoupleName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public DateTime WeddingDate { get; set; }
    public string Service { get; set; }
    public DateTime RequestedOn { get; set; }
    public decimal Budget { get; set; }
    public int GuestCount { get; set; }
    public string Status { get; set; }
    public string SpecialRequests { get; set; }
    public string Theme { get; set; }
    public string Venue { get; set; }
    public string Priority { get; set; }
    public string Timeline { get; set; }
    public int? WeddingId { get; set; }
    public Wedding Wedding { get; set; }
}
