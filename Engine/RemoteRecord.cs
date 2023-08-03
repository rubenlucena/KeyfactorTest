using Keyfactor.Models;

namespace Keyfactor.Engine
{
    public interface IRemoteRecords
    {
        public DataRecord[] GetRemoteRecords(ServerDateTime notBefore, ServerDateTime notAfter, int recordLimit); 

    }
    public class RemoteRecords : IRemoteRecords
    {
        public DataRecord[] GetRemoteRecords(ServerDateTime notBefore, ServerDateTime notAfter, int recordLimit)
        {
            throw new NotImplementedException();
        }
    }
}