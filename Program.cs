using MatanotAuctionCreator.BL;

namespace MatanotAuctionCreator
{
  public class Program
  {
    public static void Main(string[] args)
    {
      MatanotAuctionCreatorHandler handler = new MatanotAuctionCreatorHandler();

      handler.Exec();
    }
  }
}
