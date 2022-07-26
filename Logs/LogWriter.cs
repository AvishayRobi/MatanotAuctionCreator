using WallaShops.Common.Logs.BL;

namespace MatanotAuctionCreator.Logs
{
  public static class LogWriter
  {
    public static void WriteLogIfDebugMode(string message)
      =>
      LogHandler.WriteToConsoleIfDebugMode(message);
  }
}
