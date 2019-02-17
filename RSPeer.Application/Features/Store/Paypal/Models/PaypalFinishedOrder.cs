using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace RSPeer.Application.Features.Store.Paypal.Models
{
	public class PaypalFinishedOrder
	{
		[JsonProperty("id")] public string Id { get; set; }

		[JsonProperty("intent")] public string Intent { get; set; }

		[JsonProperty("state")] public string State { get; set; }

		[JsonProperty("cart")] public string Cart { get; set; }

		[JsonProperty("payer")] public Payer Payer { get; set; }

		[JsonProperty("transactions")] public List<Transaction> Transactions { get; set; }

		[JsonProperty("create_time")] public DateTimeOffset CreateTime { get; set; }

		[JsonProperty("links")] public List<Link> Links { get; set; }

		public string UserId { get; set; }

		public bool Processed { get; set; }
	}

	public class PayerInfo
	{
		[JsonProperty("email")] public string Email { get; set; }

		[JsonProperty("first_name")] public string FirstName { get; set; }

		[JsonProperty("last_name")] public string LastName { get; set; }

		[JsonProperty("payer_id")] public string PayerId { get; set; }

		[JsonProperty("shipping_address")] public ShippingAddress ShippingAddress { get; set; }

		[JsonProperty("country_code")] public string CountryCode { get; set; }
	}

	public class TransactionAmount
	{
		[JsonProperty("total")] public string Total { get; set; }

		[JsonProperty("currency")] public string Currency { get; set; }

		[JsonProperty("details")] public PurpleDetails Details { get; set; }
	}

	public class PurpleDetails
	{
		[JsonProperty("subtotal")] public string Subtotal { get; set; }

		[JsonProperty("tax")] public string Tax { get; set; }

		[JsonProperty("shipping")] public string Shipping { get; set; }

		[JsonProperty("insurance")] public string Insurance { get; set; }

		[JsonProperty("handling_fee")] public string HandlingFee { get; set; }
	}

	public class Payee
	{
		[JsonProperty("merchant_id")] public string MerchantId { get; set; }

		[JsonProperty("email")] public string Email { get; set; }
	}

	public class RelatedResource
	{
		[JsonProperty("sale")] public Sale Sale { get; set; }
	}

	public class Sale
	{
		[JsonProperty("id")] public string Id { get; set; }

		[JsonProperty("state")] public string State { get; set; }

		[JsonProperty("amount")] public SaleAmount Amount { get; set; }

		[JsonProperty("payment_mode")] public string PaymentMode { get; set; }

		[JsonProperty("protection_eligibility")]
		public string ProtectionEligibility { get; set; }

		[JsonProperty("protection_eligibility_type")]
		public string ProtectionEligibilityType { get; set; }

		[JsonProperty("transaction_fee")] public TransactionFee TransactionFee { get; set; }

		[JsonProperty("parent_payment")] public string ParentPayment { get; set; }

		[JsonProperty("create_time")] public DateTimeOffset CreateTime { get; set; }

		[JsonProperty("update_time")] public DateTimeOffset UpdateTime { get; set; }

		[JsonProperty("links")] public List<Link> Links { get; set; }
	}

	public class SaleAmount
	{
		[JsonProperty("total")] public string Total { get; set; }

		[JsonProperty("currency")] public string Currency { get; set; }

		[JsonProperty("details")] public FluffyDetails Details { get; set; }
	}

	public class FluffyDetails
	{
		[JsonProperty("subtotal")] public string Subtotal { get; set; }
	}

	public class TransactionFee
	{
		[JsonProperty("value")] public string Value { get; set; }

		[JsonProperty("currency")] public string Currency { get; set; }
	}
}