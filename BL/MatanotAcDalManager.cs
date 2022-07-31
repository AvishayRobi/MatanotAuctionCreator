using System.Collections.Generic;
using System.Data;
using System.Linq;
using MatanotAuctionCreator.DAL;
using MatanotAuctionCreator.Model;
using WallaShops.Utils;

namespace MatanotAuctionCreator.BL
{
  public class MatanotAcDalManager
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

    public void AttachFileToOrders(IEnumerable<MatanotOrder> orders, string virtualPath)
      =>
      orders
      .Select(getOrderID)
      .ApplyEach(i => attachFileToSingleOrder(i, virtualPath));

    public void UpdateOrderStatus(int orderID, eOrderStatus orderStatus, string failureReason = "")
      =>
      this.dal
      .UpdateOrderStatus(orderID, (int)orderStatus, failureReason);

    public void BurnOrders(IEnumerable<MatanotOrder> orders)
      =>
      orders
      .ApplyEach(burnOrder);

    private void burnOrder(MatanotOrder order)
    {
      this.dal.BurnOrder(order);
      burnOrderItems(order);
    }

    private IEnumerable<MatanotOrderItem> getItems(int orderID)
    {
      DataTable dt = this.dal.GetUnprocessedAuctionItems(orderID);

      return from DataRow dr
             in dt.Rows
             select new MatanotOrderItem()
             {
               WallaPrice = WSStringUtils.ToInt(dr["walla_price"]),
               Name = WSStringUtils.ToString(dr["product_name"]),
               Quantity = WSStringUtils.ToInt(dr["quantity"]),
               ShopID = WSStringUtils.ToInt(dr["shop_id"]),
               Price = WSStringUtils.ToInt(dr["price"]),
               SKU = WSStringUtils.ToString(dr["sku"])
             };
    }

    private void burnOrderItems(MatanotOrder order)
      =>
      order
      .Items
      .ForEach(i =>
      {
        this.dal.BurnOrderItem(i, order.OrderID);
      });

    private void attachFileToSingleOrder(int orderID, string virtualPath)
      =>
      this.dal
      .AttachFileToOrders(orderID, virtualPath);

    private int getOrderID(MatanotOrder order)
      =>
      order.OrderID;
  }
}
