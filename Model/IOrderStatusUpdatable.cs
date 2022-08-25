namespace MatanotAuctionCreator.Model
{
  public interface IOrderStatusUpdatable
  {
    void UpdateOrderStatus(int orderID, eOrderStatus orderStatus, string failureReason = "");
  }
}
