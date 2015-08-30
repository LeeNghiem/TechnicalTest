using System.Threading.Tasks;
using TechnicalTest.Share.Core.Model;

namespace TechnicalTest.Share.Core
{
    public interface IMyHttpClient
    {
        Task<Pattern> DownloadPatternAsync();

        Task<Pattern> DownloadColorAsync();
    }
}