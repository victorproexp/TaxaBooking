using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text.Json;

namespace TaxaBooking.Controllers;

[ApiController]
[Route("[controller]")]
public class BookingController : ControllerBase
{
    private readonly ILogger<BookingController> _logger;

    private IConnection connection;

    private static int nextId;

    public BookingController(ILogger<BookingController> logger)
    {
        _logger = logger;

        //var factory = new ConnectionFactory() { HostName = "localhost" };
        var factory = new ConnectionFactory() { HostName = "172.17.0.2" };
        connection = factory.CreateConnection();
    }

    [HttpPost]
    public Booking CreateBooking(Booking booking)
    {
        booking.Id = nextId++;
        booking.Tidsstempel = DateTime.Now;

        using(var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: "hello",
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

            var body = JsonSerializer.SerializeToUtf8Bytes(booking);

            channel.BasicPublish(exchange: "",
                                routingKey: "hello",
                                basicProperties: null,
                                body: body);
            Console.WriteLine(" [x] Sent {0}", booking);
        }

        return booking;
    }
}
