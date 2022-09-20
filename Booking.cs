namespace TaxaBooking;

public class Booking
{
    public int Id { get; set; }

    public DateTime Tidsstempel { get; set; }

    public string? Startadresse { get; set; }

    public string? Destinationsadresse { get; set; }

    public DateTime Starttidspunkt { get; set; }
}
