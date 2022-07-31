using System.Collections.Generic;
using System.Linq;
using MatanotAuctionCreator.Model;

namespace MatanotAuctionCreator.BL
{
  public class MatanotOrderSplitter
  {
    #region Data Members
    private IEnumerable<MatanotOrder> splittedOrders { get; set; }
    #endregion

    #region Ctor
    public MatanotOrderSplitter()
    {
      this.splittedOrders = Enumerable.Empty<MatanotOrder>();
    }
    #endregion

    public MatanotOrderSplitter FlatOrders(IEnumerable<MatanotOrder> orders)
    {
      this.splittedOrders = getFlattedOrders(orders);

      return this;
    }

    public MatanotOrderSplitter GroupOrdersByShop()
    {
      this.splittedOrders = getGroupedOrders();

      return this;
    }

    public IEnumerable<MatanotOrder> GetOrders()
      =>
      this.splittedOrders;

    private IEnumerable<MatanotOrder> getFlattedOrders(IEnumerable<MatanotOrder> orders)
      =>
      orders
      .SelectMany(o => o.Items.Select(i => new MatanotOrder
      {
        Items = new List<MatanotOrderItem> { i },
        ShipmentInfo = o.ShipmentInfo,
        ClientInfo = o.ClientInfo,
        OrderDate = o.OrderDate,
        TotalPrice = i.Price,
        OrderID = o.OrderID
      }));

    private IEnumerable<MatanotOrder> getGroupedOrders()
      =>
      this.splittedOrders
      .GroupBy(o => new
      {
        o.Items.First().ShopID,
        o.OrderID
      })
      .Select(g => new MatanotOrder()
      {
        TotalPrice = g.SelectMany(i => i.Items).Sum(i => i.Price),
        Items = g.SelectMany(i => i.Items).ToList(),
        ShipmentInfo = g.First().ShipmentInfo,
        ClientInfo = g.First().ClientInfo,
        OrderDate = g.First().OrderDate,
        OrderID = g.First().OrderID
      });
  }
}
