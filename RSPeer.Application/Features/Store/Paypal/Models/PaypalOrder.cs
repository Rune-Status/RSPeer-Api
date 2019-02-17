using System.Collections.Generic;
using Newtonsoft.Json;

namespace RSPeer.Application.Features.Store.Paypal.Models
{
	public class PaypalOrder
	{
		[JsonProperty("intent")] public string Intent { get; set; }

		[JsonProperty("payer")] public Payer Payer { get; set; }

		[JsonProperty("transactions")] public List<Transaction> Transactions { get; set; }

		[JsonProperty("note_to_payer")] public string NoteToPayer { get; set; }

		[JsonProperty("redirect_urls")] public RedirectUrls RedirectUrls { get; set; }
	}

	public class Payer
	{
		[JsonProperty("payment_method")] public string PaymentMethod { get; set; }
	}

	public class RedirectUrls
	{
		[JsonProperty("return_url")] public string ReturnUrl { get; set; }

		[JsonProperty("cancel_url")] public string CancelUrl { get; set; }
	}

	public class Transaction
	{
		[JsonProperty("amount")] public Amount Amount { get; set; }

		[JsonProperty("description")] public string Description { get; set; }

		[JsonProperty("custom")] public string Custom { get; set; }

		[JsonProperty("invoice_number")] public string InvoiceNumber { get; set; }

		[JsonProperty("payment_options")] public PaymentOptions PaymentOptions { get; set; }

		[JsonProperty("soft_descriptor")] public string SoftDescriptor { get; set; }

		[JsonProperty("item_list")] public ItemList ItemList { get; set; }
	}

	public class Amount
	{
		[JsonProperty("total")] public string Total { get; set; }

		[JsonProperty("currency")] public string Currency { get; set; }

		[JsonProperty("details")] public Details Details { get; set; }
	}

	public class Details
	{
		[JsonProperty("subtotal")] public string Subtotal { get; set; }

		[JsonProperty("tax")] public string Tax { get; set; }

		[JsonProperty("shipping")] public string Shipping { get; set; }

		[JsonProperty("handling_fee")] public string HandlingFee { get; set; }

		[JsonProperty("shipping_discount")] public string ShippingDiscount { get; set; }

		[JsonProperty("insurance")] public string Insurance { get; set; }
	}

	public class ItemList
	{
		[JsonProperty("items")] public List<Item> Items { get; set; }

		[JsonProperty("shipping_address")] public ShippingAddress ShippingAddress { get; set; }
	}

	public class Item
	{
		[JsonProperty("name")] public string Name { get; set; }

		[JsonProperty("description")] public string Description { get; set; }

		[JsonProperty("quantity")] public string Quantity { get; set; }

		[JsonProperty("price")] public string Price { get; set; }

		[JsonProperty("tax")] public string Tax { get; set; }

		[JsonProperty("sku")] public string Sku { get; set; }

		[JsonProperty("currency")] public string Currency { get; set; }
	}

	public class ShippingAddress
	{
		[JsonProperty("recipient_name")] public string RecipientName { get; set; }

		[JsonProperty("line1")] public string Line1 { get; set; }

		[JsonProperty("line2")] public string Line2 { get; set; }

		[JsonProperty("city")] public string City { get; set; }

		[JsonProperty("country_code")] public string CountryCode { get; set; }

		[JsonProperty("postal_code")] public string PostalCode { get; set; }

		[JsonProperty("phone")] public string Phone { get; set; }

		[JsonProperty("state")] public string State { get; set; }
	}

	public class PaymentOptions
	{
		[JsonProperty("allowed_payment_method")]
		public string AllowedPaymentMethod { get; set; }
	}
}