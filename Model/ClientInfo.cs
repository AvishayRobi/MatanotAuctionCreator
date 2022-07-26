using Newtonsoft.Json;

namespace MatanotAuctionCreator.Model
{
  public class ClientInfo
  {
    [JsonProperty("first_name")]
    public string FirstName { get; set; }

    [JsonProperty("last_name")]
    public string LastName { get; set; }

    [JsonProperty("worker_id")]
    public string WorkerID { get; set; }

    [JsonProperty("email")]
    public string EmailAddress { get; set; }

    [JsonProperty("phone")]
    public string PhoneNumber { get; set; }
  }
}
