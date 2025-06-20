namespace EventDriven.Shared
{
    public class OrderCreated
    {
        public Guid OrderId { get; set; }
        public string CustomerEmail { get; set; }
    }
}
