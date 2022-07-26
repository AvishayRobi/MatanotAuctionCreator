using Newtonsoft.Json;

namespace MatanotAuctionCreator.Model
{
  public class ShippingInfo
  {
    [JsonProperty("city")]
    public string City { get; set; }

    [JsonProperty("street")]
    public string StreetName { get; set; }

    [JsonProperty("street_number")]
    public string StreetNumber { get; set; }

    [JsonProperty("zip_code")]
    public string ZipCode { get; set; }
  }
}
