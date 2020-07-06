using System.Threading.Tasks;
using ZeroOne.Extension;

namespace ZeroOne.Application
{
    public interface IMapLocationService : IHttpService
    {
        Task<MapDirectionDrive> GetDirectionDrive(string origin, string destination, EDriveDistanceType driveDistanceType);
    }
}
