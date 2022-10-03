using System.Data;
using MatanotAuctionCreator.Model;
using WallaShops.Data;
using WallaShops.Objects;

namespace MatanotAuctionCreator.DAL
{
  public class MatanotAcDal : WSSqlHelper
  {
    #region Ctor
    public MatanotAcDal() : base(WSPlatforms.WallaShops)
    {
    }
    #endregion

    public void BurnOrder(MatanotOrder order)
    {
      WSSqlParameters spParams = new WSSqlParameters();
      spParams.AddInputParameter("@order_id", order.OrderID);
      spParams.AddInputParameter("@total_price", order.TotalPrice);
      spParams.AddInputParameter("@order_date", order.OrderDate);
      spParams.AddInputParameter("@client_first_name", order.ClientInfo.FirstName);
      spParams.AddInputParameter("@client_last_name", order.ClientInfo.LastName);
      spParams.AddInputParameter("@worker_id", order.ClientInfo.WorkerID);
      spParams.AddInputParameter("@client_email", order.ClientInfo.EmailAddress);
      spParams.AddInputParameter("@client_phone", order.ClientInfo.PhoneNumber);
      spParams.AddInputParameter("@shipping_city", order.ShipmentInfo.City);
      spParams.AddInputParameter("@shipping_street", order.ShipmentInfo.StreetName);
      spParams.AddInputParameter("@shipping_street_num", order.ShipmentInfo.StreetNumber);
      spParams.AddInputParameter("@zip_code", order.ShipmentInfo.ZipCode);

      base.ExecuteNonQuery("matanot_ac_burn_orders", ref spParams);
    }

    public void BurnOrderItem(MatanotOrderItem item, int orderID)
    {
      WSSqlParameters spParams = new WSSqlParameters();
      spParams.AddInputParameter("@order_id", orderID);
      spParams.AddInputParameter("@product_name", item.Name);
      spParams.AddInputParameter("@price", item.Price);
      spParams.AddInputParameter("@walla_price", item.WallaPrice);
      spParams.AddInputParameter("@sku", item.SKU);
      spParams.AddInputParameter("@quantity", item.Quantity);
      spParams.AddInputParameter("@shop_id", item.ShopID);

      base.ExecuteNonQuery("matanot_ac_burn_order_item", ref spParams);
    }   

    public DataTable GetUnprocessedAuctionItems(int orderID)
    {
      WSSqlParameters spParams = new WSSqlParameters();
      spParams.AddInputParameter("@order_id", orderID);

      return base.GetDataTable("matanot_ac_get_previous_unprocessed_order_items", ref spParams);
    }

    public void UpdateOrderStatus(int orderID, int status, string failureReason = "")
    {
      WSSqlParameters spParams = new WSSqlParameters();
      spParams.AddInputParameter("@order_id", orderID);
      spParams.AddInputParameter("@status", status);
      spParams.AddInputParameter("@failure_reason", failureReason);

      base.ExecuteNonQuery("matanot_ac_update_order_status", ref spParams);
    }

    public void AttachFileToOrders(int orderID, string virtualPath)
    {
      WSSqlParameters spParams = new WSSqlParameters();
      spParams.AddInputParameter("@order_id", orderID);
      spParams.AddInputParameter("@virtual_path", virtualPath);

      base.ExecuteNonQuery("matanot_ac_attach_file_to_order", ref spParams);
    }

    public DataTable GetUnprocessedAuctions()
     =>
     base.GetDataTable("matanot_ac_get_previous_unprocessed_orders");
  }
}
