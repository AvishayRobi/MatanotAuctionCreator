using System;
using System.Collections.Generic;
using System.Linq;
using MatanotAuctionCreator.Files;
using MatanotAuctionCreator.Logs;
using MatanotAuctionCreator.Model;

namespace MatanotAuctionCreator.BL
{
  public class MatanotAuctionCreatorHandler
  {
    #region Data Members
    private MatanotAcSftpManager sftpManager { get; }
    private MatanotAcDalManager dalManager { get; }
    #endregion

    #region Ctor
    public MatanotAuctionCreatorHandler()
    {
      this.sftpManager = new MatanotAcSftpManager();
      this.dalManager = new MatanotAcDalManager();
    }
    #endregion

    public void Exec()
    {
      try
      {
        startProcess();
      }
      catch (Exception ex)
      {
        LogWriter.WriteLogIfDebugMode($"Exception: {ex.Message} \nStackTrace: {ex.StackTrace}");
      }
    }

    private void startProcess()
    {
      IEnumerable<MatanotOrder> orders = getOrders();
      handleFile(orders);
      burnOrders(orders);
      createWsAuctions(orders);
    }

    private void handleFile(IEnumerable<MatanotOrder> orders)
    {
      backUpOrders(orders);
      deleteRemoteFile();
    }

    private void backUpOrders(IEnumerable<MatanotOrder> orders)
    {
      string virtualPath = backUpFile();
      attachFileToOrders(orders, virtualPath);
    }

    private string backUpFile()
    {
      string ftpFileContent = getSftpFileContent();
      string ftpFileName = getSftpFileName();
      string virtualPath = uploadToBlob(ftpFileContent, ftpFileName);

      return virtualPath;
    }

    private IEnumerable<MatanotOrder> getOrders()
    {
      IEnumerable<MatanotOrder> orders = getOrdersToProcess();
      IEnumerable<MatanotOrder> splittedOrders = splitOrders(orders);

      return splittedOrders;
    }

    private IEnumerable<MatanotOrder> getOrdersToProcess()
    {
      ICollection<MatanotOrder> newOrders = getNewOrders();
      LogWriter.WriteLogIfDebugMode($"New orders count: {newOrders.Count}");
      IEnumerable<MatanotOrder> previousUnprocessedOrders = getPreviousUnprocessedOrders();

      return newOrders.Concat(previousUnprocessedOrders);
    }

    private ICollection<MatanotOrder> getNewOrders()
      =>
      this.sftpManager
      .SetUpFileContent()
      .GetOrders();

    private string getSftpFileContent()
      =>
      this.sftpManager
      .GetFileContent();

    private string getSftpFileName()
      =>
      this.sftpManager
      .GetFileName();

    private void deleteRemoteFile()
      =>
      this.sftpManager
      .DeleteRemoteFile();

    private IEnumerable<MatanotOrder> getPreviousUnprocessedOrders()
      =>
      this.dalManager
      .GetPreviousUnprocessedOrders();

    private void burnOrders(IEnumerable<MatanotOrder> orders)
      =>
      this.dalManager
      .BurnOrders(orders);

    private string uploadToBlob(string fileContent, string fileName)
      =>
      new BlobUploader()
      .ConvertFileContent(fileContent)
      .GenerateFullPath(fileName)
      .Upload();

    private void createWsAuctions(IEnumerable<MatanotOrder> orders)
      =>
      new MatanotWsAuctionManager()
      .SetDalManager(this.dalManager)
      .CreateWsAuctions(orders);

    private IEnumerable<MatanotOrder> splitOrders(IEnumerable<MatanotOrder> orders)
      =>
      new MatanotOrderSplitter()
      .FlatOrders(orders)
      .GroupOrdersByShop()
      .GetOrders();

    private void attachFileToOrders(IEnumerable<MatanotOrder> orders, string virtualPath)
      =>
      this.dalManager
      .AttachFileToOrders(orders, virtualPath);
  }
}
