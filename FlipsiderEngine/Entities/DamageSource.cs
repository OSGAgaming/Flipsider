using Flipsider.Localization;

namespace Flipsider.Entities
{
    public class DamageSource
    {
        public DamageSource(double amount, object sender, LocalizedText message)
        {
            Sender = sender;
            Message = message;
            Amount = amount;
        }

        public DamageSource(DamageSource cloneFrom)
        {
            Sender = cloneFrom.Sender;
            Message = cloneFrom.Message;
            Amount = cloneFrom.Amount;
        }

        public object Sender { get; }
        public LocalizedText Message { get; }
        public double Amount { get; set; }
    }
}
