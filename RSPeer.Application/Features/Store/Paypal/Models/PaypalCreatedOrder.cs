using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace RSPeer.Application.Features.Store.Paypal.Models
{
	public class PaypalCreatedOrder
	{
		public int Id { get; set; }
		public int UserId { get; set; }

		[JsonProperty("id")] public string PaypalId { get; set; }

		public bool IsSandbox { get; set; }

		[JsonProperty("intent")] public string Intent { get; set; }

		[JsonProperty("state")] public string State { get; set; }

		[JsonProperty("payer")] public Payer Payer { get; set; }

		[JsonProperty("transactions")] public List<Transaction> Transactions { get; set; }

		[JsonProperty("note_to_payer")] public string NoteToPayer { get; set; }

		[JsonProperty("create_time")] public DateTimeOffset CreateTime { get; set; }

		[JsonProperty("links")] public List<Link> Links { get; set; }
	}

	public class Link
	{
		[JsonProperty("href")] public string Href { get; set; }

		[JsonProperty("rel")] public string Rel { get; set; }

		[JsonProperty("method")] public string Method { get; set; }
	}
}