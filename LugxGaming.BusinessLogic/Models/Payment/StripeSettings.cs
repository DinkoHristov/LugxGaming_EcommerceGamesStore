namespace LugxGaming.BusinessLogic.Models.Payment
{
    public class StripeSettings
    {
        public string SecretKey { get; set; }

        public string PublicKey { get; set; }

        public string WebhookSecret { get; set; }
    }
}