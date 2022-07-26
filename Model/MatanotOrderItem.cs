using Newtonsoft.Json;

namespace MatanotAuctionCreator.Model
{
  public class MatanotOrderItem
  {
    [JsonProperty("product_name")]
    public string Name { get; set; }

    [JsonProperty("product_price")]
    public int Price { get; set; }

    [JsonProperty("walla_price")]
    public int WallaPrice { get; set; }

    [JsonProperty("product_sku")]
    public string SKU { get; set; }

    [JsonProperty("qty")]
    public int Quantity { get; set; }

    [JsonProperty("shop_id")]
    public int ShopID { get; set; }
  }
}
