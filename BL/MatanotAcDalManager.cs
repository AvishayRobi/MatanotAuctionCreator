using System.Collections.Generic;
using System.Data;
using System.Linq;
using MatanotAuctionCreator.DAL;
using MatanotAuctionCreator.Model;
using WallaShops.Utils;

namespace MatanotAuctionCreator.BL
{
  public class MatanotAcDalManager: IOrderStatusUpdatable
  {
    #region Data Members
    private MatanotAcDal dal { get; }
    #endregion

    #region Ctor
    public MatanotAcDalManager()
    {
      this.dal = new MatanotAcDal();
    }
    #endregion

    public IEnumerable<MatanotOrder> GetPreviousUnprocessedOrders()
    {
      DataTable dt = this.dal.GetUnprocessedAuctions();

      return from DataRow dr
             in dt.Rows
             select new MatanotOrder()
             {
               OrderDate = WSStringUtils.ToString(dr["order_date"]),
               TotalPrice = WSStringUtils.ToInt(dr["total_price"]),
               OrderID = WSStringUtils.ToInt(dr["order_id"]),
               ClientInfo = new ClientInfo()
               {
                 FirstName = WSStringUtils.ToString(dr["client_first_name"]),
                 EmailAddress = WSStringUtils.ToString(dr["client_email"]),
                 LastName = WSStringUtils.ToString(dr["client_last_name"]),
                 PhoneNumber = WSStringUtils.ToString(dr["client_phone"]),
                 WorkerID = WSStringUtils.ToString(dr["worker_id"])
               },
               ShipmentInfo = new ShippingInfo()
               {
                 StreetNumber = WSStringUtils.ToString(dr["shipping_street_num"]),
                 StreetName = WSStringUtils.ToString(dr["shipping_street"]),
                 City = WSStringUtils.ToString(dr["shipping_city"]),
                 ZipCode = WSStringUtils.ToString(dr["zip_code"])
               },
               Items = getItems(WSStringUtils.ToInt(dr["order_id"])).ToList()
             };
    }

    public void UpdateOrderStatus(int orderID, eOrderStatus orderStatus, string failureReason = "")
      =>
      this.dal
      .UpdateOrderStatus(orderID, (int)orderStatus, failureReason);

    public void AttachFileToOrders(IEnumerable<MatanotOrder> orders, string virtualPath)
      =>
      orders
      .Select(i => i.OrderID)
      .ApplyEach(i =>
      {
        this.dal.AttachFileToSingleOrder(i, virtualPath);
      });    

    public void BurnOrders(IEnumerable<MatanotOrder> orders)
      =>
      orders
      .ApplyEach(o =>
      {
        this.dal.BurnOrder(o);
        burnOrderItems(o);
      });

    private void burnOrderItems(MatanotOrder order)
      =>
      order
      .Items
      .ForEach(i =>
      {
        this.dal.BurnOrderItem(i, order.OrderID);
      });

    private IEnumerable<MatanotOrderItem> getItems(int orderID)
    {
      DataTable dt = this.dal.GetUnprocessedAuctionItems(orderID);

      return from DataRow dr
             in dt.Rows
             select new MatanotOrderItem()
             {
               AuctionID = WSStringUtils.ToString(dr["auction_id"]),
               WallaPrice = WSStringUtils.ToInt(dr["walla_price"]),
               Name = WSStringUtils.ToString(dr["product_name"]),
               Quantity = WSStringUtils.ToInt(dr["quantity"]),
               ShopID = WSStringUtils.ToInt(dr["shop_id"]),
               PfId = WSStringUtils.ToString(dr["pf_id"]),
               Price = WSStringUtils.ToInt(dr["price"])
             };
    }    
  }
}
