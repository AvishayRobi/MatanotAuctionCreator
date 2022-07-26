using System.Collections.Generic;
using MatanotAuctionCreator.Model;
using MatanotAuctionCreator.Files;
using WallaShops.Utils;

namespace MatanotAuctionCreator.BL
{
  public class MatanotAcFtpManager
  {
    #region Data Members
    private string ftpFileContent { get; set; }
    private string ftpFileName { get; set; }
    private FtpClient ftpClient { get; }
    #endregion

    #region Ctor
    public MatanotAcFtpManager()
    {
      this.ftpFileContent = string.Empty;
      this.ftpClient = new FtpClient();
      this.ftpFileName = string.Empty;
    }
    #endregion

    public MatanotAcFtpManager SetUpFileContent()
    {
      this.ftpFileName = getFirstFileName();
      this.ftpFileContent = getRemoteFileContent();

      return this;
    }

    public ICollection<MatanotOrder> GetOrders()
    {      
      ICollection<MatanotOrder> orders = extractOrders();
      deleteRemoteOrders();

      return orders;
    }

    public string GetFtpFileContent()
      =>
      this.ftpFileContent;

    public string GetFileName()
      =>
      this.ftpFileName;

    private string getFirstFileName()
      =>
      this.ftpClient
      .GetFirstFileName();

    private string getRemoteFileContent()
      =>
      this.ftpClient
      .GetRemoteFileContent(this.ftpFileName);

    private ICollection<MatanotOrder> extractOrders()
      =>
      WSJsonConverter
      .DeserializeObject<ICollection<MatanotOrder>>(this.ftpFileContent);

    private void deleteRemoteOrders()
      =>
      this.ftpClient
      .DeleteRemoteFile();
  }
}
