namespace MatanotAuctionCreator.Model
{
  public enum eOrderStatus
  {
    /// <summary>
    /// Order hasn't been processed
    /// </summary>
    Unprocessed = 0,

    /// <summary>
    /// Order processed succesfully
    /// </summary>
    Ok = 1,

    /// <summary>
    /// There was an error while processing the order
    /// </summary>
    Error = 2
  }
}
