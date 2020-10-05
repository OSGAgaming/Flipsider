namespace Flipsider.Worlds.Entities
{
    public class DamageSource
    {
        public DamageSource(object sender, string message, double amount)
        {
            Sender = sender;
            Message = message;
            Amount = amount;
        }

        public object Sender { get; }
        public string Message { get; }
        public double Amount { get; set; }
    }
}
