using Newtonsoft.Json;

namespace MatanotAuctionCreator.Model
{
  public class MatanotOrderItem
  {
    [JsonProperty("product_name")]
    public string Name { get; set; }

    [JsonProperty("sell_price")]
    public int Price { get; set; }

    [JsonProperty("walla_price")]
    public int WallaPrice { get; set; }

    [JsonProperty("product_auction")]
    public string AuctionID { get; set; }

    [JsonProperty("product_sku")]
    public string PfId { get; set; }

    [JsonProperty("qty")]
    public int Quantity { get; set; }

    [JsonProperty("shop_id")]
    public int ShopID { get; set; }
  }
}
