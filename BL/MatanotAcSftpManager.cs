using System.Collections.Generic;
using MatanotAuctionCreator.Files;
using MatanotAuctionCreator.Model;
using WallaShops.Utils;

namespace MatanotAuctionCreator.BL
{
  public class MatanotAcSftpManager
  {
    #region Data Members
    private string sftpFileContent { get; set; }
    private string sftpFileName { get; set; }
    private SftpMed sftpMed { get; }
    #endregion

    #region Ctor
    public MatanotAcSftpManager()
    {
      this.sftpFileContent = string.Empty;
      this.sftpFileName = string.Empty;
      this.sftpMed = new SftpMed();
    }
    #endregion

    public MatanotAcSftpManager SetUpFileContent()
    {
      this.sftpFileName = getFirstFileName();
      this.sftpFileContent = getRemoteFileContent();

      return this;
    }

    public ICollection<MatanotOrder> GetOrders()
      =>
      extractOrders();

    public string GetFileContent()
      =>
      this.sftpFileContent;

    public string GetFileName()
      =>
      this.sftpFileName;

    public void DeleteRemoteFile()
      =>
      this.sftpMed
      .DeleteRemoteFile(this.sftpFileName);

    private ICollection<MatanotOrder> extractOrders()
      =>
      WSJsonConverter
      .DeserializeObject<ICollection<MatanotOrder>>(this.sftpFileContent);

    private string getFirstFileName()
      =>
      this.sftpMed
      .GetFirstFileName();

    private string getRemoteFileContent()
      =>
      this.sftpMed
      .ReadFile(this.sftpFileName);
  }
}
