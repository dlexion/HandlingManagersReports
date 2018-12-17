using System.Threading.Tasks;

namespace ReportHandler.BLL.Interfaces
{
    public interface IFileHandler
    {
        Task ParseFile(string path, string folderForProcessedFile);
    }
}