using System;
using System.Collections.Generic;
using System.Linq;
using MatanotAuctionCreator.Logs;
using MatanotAuctionCreator.Model;
using WSOrderCreator;

namespace MatanotAuctionCreator.BL
{
  public class MatanotWsAuctionManager
  {
    #region Data Members
    private MatanotAcDalManager dalManager { get; set; }
    #endregion

    #region Ctor
    public MatanotWsAuctionManager()
    {
      this.dalManager = null;
    }
    #endregion

    public MatanotWsAuctionManager SetDalManager(MatanotAcDalManager dalManager)
    {
      this.dalManager = dalManager;

      return this;
    }

    public void CreateWsAuctions(IEnumerable<MatanotOrder> orders)
    {
      foreach (MatanotOrder order in orders)
      {
        try
        {
          processMacOrder(order);
        }
        catch (Exception ex)
        {
          handleMacException(order.OrderID, ex);
        }
      }
    }

    private void processMacOrder(MatanotOrder order)
    {
      validateItems(order.Items);
      createSingleWsAuction(order);
      updateOrderStatus(order.OrderID, eOrderStatus.Ok);
    }

    private void handleMacException(int orderID, Exception ex)
    {
      LogWriter.WriteLogIfDebugMode($"Exception: {ex.Message} \nStackTrace: {ex.StackTrace}");
      updateOrderStatus(orderID, eOrderStatus.Error, ex.Message);
    }

    private void validateItems(IEnumerable<MatanotOrderItem> items)
    {
      MatanotOrderItem itemWithoutAuctionId = items.FirstOrDefault(auctionIdNotExist);
      bool isAllItemsContainsAuctionID = itemWithoutAuctionId == null;

      if (!isAllItemsContainsAuctionID)
      {
        throw new Exception($"Empty AuctionID for Item: {itemWithoutAuctionId.Name}");
      }
    }

    private void createSingleWsAuction(MatanotOrder order)
      =>
      new WSOrderCreatorHandler()
      .CreateWsAuction(order);

    private bool auctionIdNotExist(MatanotOrderItem item)
      =>
      string.IsNullOrEmpty(item.AuctionID);

    private void updateOrderStatus(int orderID, eOrderStatus orderStatus, string failureReason = "")
      =>
      this.dalManager
      .UpdateOrderStatus(orderID, orderStatus, failureReason);
  }
}
