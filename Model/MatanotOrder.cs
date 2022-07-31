using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using WallaShops.Utils;
using WSOrderCreator.Model;

namespace MatanotAuctionCreator.Model
{
  public class MatanotOrder : ICasualOrderConvertible
  {
    [JsonProperty("shopper_id")]
    public string ShopperID { get; set; }

    [JsonProperty("order_id")]
    public int OrderID { get; set; }

    [JsonProperty("total_amount")]
    public int TotalPrice { get; set; }

    [JsonProperty("order_date")]
    public string OrderDate { get; set; }

    [JsonProperty("client_info")]
    public ClientInfo ClientInfo { get; set; }

    [JsonProperty("shipping_info")]
    public ShippingInfo ShipmentInfo { get; set; }

    [JsonProperty("products_info")]
    public List<MatanotOrderItem> Items { get; set; }

    public CasualOrder ConvertToCasualOrder()
      =>
      new CasualOrder()
      {
        AffiliateName = WSGeneralUtils.GetAppSettings("affiliateName"),
        ShippingDetails = getShippingDetails(),
        ShopperDetails = getShopperDetails(),
        TotalOrderPrice = this.TotalPrice,
        DateCreated = this.OrderDate,
        Items = getConvertedItems(),
        ShopperID = this.ShopperID,
        EntryID = this.OrderID,
        ShopID = getShopID()
      };

    private ShippingDetails getShippingDetails()
      =>
      new ShippingDetails()
      {
        ZipCode = int.Parse(this.ShipmentInfo.ZipCode),
        StreetNumber = this.ShipmentInfo.StreetNumber,
        StreetName = this.ShipmentInfo.StreetName,
        City = this.ShipmentInfo.City,
        Enterance = string.Empty,
        Apratment = 0,
        Floor = 0
      };

    private ShopperDetails getShopperDetails()
      =>
      new ShopperDetails()
      {
        Idz = WSGeneralUtils.GetAppSettings("defaultShopperIdz"),
        FirstName = this.ClientInfo.FirstName,
        Email = this.ClientInfo.EmailAddress,
        LastName = this.ClientInfo.LastName,
        PhoneNumber = getPhoneNumber(),
        Birthdate = DateTime.Today
      }
      .ValidateIdz();

    private string getPhoneNumber()
      =>
      string.IsNullOrEmpty(this.ClientInfo.PhoneNumber)
      ? WSGeneralUtils.GetAppSettings("defaultShopperPhoneNumber")
      : this.ClientInfo.PhoneNumber;

    private int getShopID()
      =>
      this.Items
      .First()
      .ShopID;

    private List<OrderItem> getConvertedItems()
      =>
      this.Items
      .ConvertAll(generateItem);

    private OrderItem generateItem(MatanotOrderItem productInfo)
      =>
      new OrderItem()
      {
        AuctionID = int.TryParse(productInfo.SKU, out int x) ? int.Parse(productInfo.SKU) : 0,
        PriceBeforeDiscount = productInfo.Price,
        CouponAmount = productInfo.WallaPrice,
        Quantity = productInfo.Quantity,
        Description = productInfo.Name,
        FinalPrice = productInfo.Price,
        ShopID = productInfo.ShopID
      };
  }
}
