using System.Linq;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using WallaShops.Utils;

namespace MatanotAuctionCreator.Files
{
  public class SftpMed
  {
    #region Data Members
    private SftpClient client { get; }
    private string folderPath { get; }
    #endregion

    #region Ctor Dtor
    public SftpMed()
    {
      this.folderPath = WSGeneralUtils.GetAppSettings("---");
      string userName = WSGeneralUtils.GetAppSettings("---");
      string password = WSGeneralUtils.GetAppSettings("---");
      string host = WSGeneralUtils.GetAppSettings("---");

      this.client = new SftpClient(host, userName, password);
      this.client.Connect();
    }

    ~SftpMed()
    {
      this.client.Disconnect();
      this.client.Dispose();
    }
    #endregion

    public string GetFirstFileName()
    {
      this.client
        .ChangeDirectory("/");

      return this.client
        .ListDirectory(this.folderPath)
        .Select(getFileName)
        .FirstOrDefault(isFileTypeJson)
        .LastAppearanceAfter('/');
    }

    public string ReadFile(string fileName)
    {
      this.client
        .ChangeDirectory(this.folderPath);

      return this.client
        .ReadAllText(this.folderPath + "/" + fileName);
    }

    public void DeleteRemoteFile(string fileName)
      =>
      this.client
      .DeleteFile(this.folderPath + "/" + fileName);

    private string getFileName(SftpFile file)
      =>
      file.FullName;

    private bool isFileTypeJson(string fileName)
      =>
      fileName.Contains("json");
  }
}
