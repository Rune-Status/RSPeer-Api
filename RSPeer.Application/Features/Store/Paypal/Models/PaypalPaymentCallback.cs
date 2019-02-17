using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace RSPeer.Application.Features.Store.Paypal.Models
{
	public class PaypalPaymentCallback
	{
		[JsonProperty("id")] public string Id { get; set; }

		[JsonProperty("event_version")] public string EventVersion { get; set; }

		[JsonProperty("create_time")] public DateTimeOffset CreateTime { get; set; }

		[JsonProperty("resource_type")] public string ResourceType { get; set; }

		[JsonProperty("event_type")] public string EventType { get; set; }

		[JsonProperty("summary")] public string Summary { get; set; }

		[JsonProperty("resource")] public Resource Resource { get; set; }

		[JsonProperty("links")] public List<Link> Links { get; set; }
	}

	public class Resource
	{
		[JsonProperty("id")] public string Id { get; set; }

		[JsonProperty("state")] public string State { get; set; }

		[JsonProperty("amount")] public Amount Amount { get; set; }

		[JsonProperty("payment_mode")] public string PaymentMode { get; set; }

		[JsonProperty("protection_eligibility")]
		public string ProtectionEligibility { get; set; }

		[JsonProperty("protection_eligibility_type")]
		public string ProtectionEligibilityType { get; set; }

		[JsonProperty("transaction_fee")] public TransactionFee TransactionFee { get; set; }

		[JsonProperty("invoice_number")] public string InvoiceNumber { get; set; }

		[JsonProperty("parent_payment")] public string ParentPayment { get; set; }

		[JsonProperty("create_time")] public DateTimeOffset CreateTime { get; set; }

		[JsonProperty("update_time")] public DateTimeOffset UpdateTime { get; set; }

		[JsonProperty("links")] public List<Link> Links { get; set; }
	}
}