using System;
namespace stripeTest.Models
{
	public class StripeSettings
	{
		public StripeSettings(){}

		public string SecretKey { get; set; }
		public string PublicKey { get; set; }
	}
}

