using System;
using System.Collections.Generic;

namespace DeltaDNA
{
	// Token: 0x0200000F RID: 15
	public class ProductBuilder
	{
		// Token: 0x0600007E RID: 126 RVA: 0x0000484C File Offset: 0x00002A4C
		public ProductBuilder AddRealCurrency(string currencyType, int currencyAmount)
		{
			if (this.realCurrency != null)
			{
				throw new InvalidOperationException("A Product may only have one real currency");
			}
			this.realCurrency = new Dictionary<string, object>
			{
				{
					"realCurrencyType",
					currencyType
				},
				{
					"realCurrencyAmount",
					currencyAmount
				}
			};
			return this;
		}

		// Token: 0x0600007F RID: 127 RVA: 0x0000489C File Offset: 0x00002A9C
		public ProductBuilder AddVirtualCurrency(string currencyName, string currencyType, int currencyAmount)
		{
			if (this.virtualCurrencies == null)
			{
				this.virtualCurrencies = new List<Dictionary<string, object>>();
			}
			this.virtualCurrencies.Add(new Dictionary<string, object>
			{
				{
					"virtualCurrency",
					new Dictionary<string, object>
					{
						{
							"virtualCurrencyName",
							currencyName
						},
						{
							"virtualCurrencyType",
							currencyType
						},
						{
							"virtualCurrencyAmount",
							currencyAmount
						}
					}
				}
			});
			return this;
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00004910 File Offset: 0x00002B10
		public ProductBuilder AddItem(string itemName, string itemType, int itemAmount)
		{
			if (this.items == null)
			{
				this.items = new List<Dictionary<string, object>>();
			}
			this.items.Add(new Dictionary<string, object>
			{
				{
					"item",
					new Dictionary<string, object>
					{
						{
							"itemName",
							itemName
						},
						{
							"itemType",
							itemType
						},
						{
							"itemAmount",
							itemAmount
						}
					}
				}
			});
			return this;
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00004984 File Offset: 0x00002B84
		public Dictionary<string, object> ToDictionary()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			if (this.realCurrency != null)
			{
				dictionary.Add("realCurrency", this.realCurrency);
			}
			if (this.virtualCurrencies != null)
			{
				dictionary.Add("virtualCurrencies", this.virtualCurrencies);
			}
			if (this.items != null)
			{
				dictionary.Add("items", this.items);
			}
			return dictionary;
		}

		// Token: 0x0400004E RID: 78
		private Dictionary<string, object> realCurrency;

		// Token: 0x0400004F RID: 79
		private List<Dictionary<string, object>> virtualCurrencies;

		// Token: 0x04000050 RID: 80
		private List<Dictionary<string, object>> items;
	}
}
