using Keyfactor.Models;
using Keyfactor.Engine;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Keyfactor.Controllers
{
    [Route("pagination")]
    public class PaginationController : Controller
    {
        private const string recordCache = "records";
        private readonly IRemoteRecords _remoteRecords;
        private readonly ICache _cache;
        public PaginationController(IRemoteRecords remoteRecords, ICache cache)
        {
            _remoteRecords = remoteRecords;
            _cache = cache;
        }
        [HttpGet("")]
        public DataRecord[] GetRecords(int pageNumber, int resultsPerPage)  
        { 
            // Get the current server time
            // registra clase como singleton para usar como cache (id y fecha de creacion)
            // si no tengo nada ir a buscar lo que necesitemos
            // si tengo en cache, ver si con eso puedo devolver lo que piden (si no, ir a buscar mas y guardar)

            DateTimeOffset timeOffset = DateTimeOffset.UtcNow.UtcDateTime.AddDays(1);
            var records = _cache.GetData<DataRecord[]>(recordCache);
            DataRecord[] remoteRecords;

            if(records == null)
            {
                ServerDateTime startTime = new ServerDateTime().GetMinValue();
                ServerDateTime endTime = startTime.AddMilliseconds(startTime, 2*60*60*1000); // Get from 2 hours

                // Retrieve the data records from the remote server
                remoteRecords = _remoteRecords.GetRemoteRecords(startTime, endTime, resultsPerPage);
                
                _cache.SetData(recordCache, remoteRecords,  timeOffset);
                return remoteRecords;
            }
            else
            {
                int totalPages = (int)records.Length / resultsPerPage;

                if(pageNumber >= 1 && pageNumber <= totalPages)
                {
                    ServerDateTime start = records.LastOrDefault().CreationDate;
                    ServerDateTime end = start.AddMilliseconds(start,2*60*60*1000); // Get another 2 hours
                    remoteRecords = _remoteRecords.GetRemoteRecords(start, end, resultsPerPage - records.Length); 
                    var dataRecord = records.Concat(remoteRecords);
                    _cache.SetData(recordCache, dataRecord, timeOffset);
                    
                    int startIndex = (pageNumber - 1) * resultsPerPage;
                    int endIndex = startIndex + resultsPerPage;

                    DataRecord[] pageRecords = dataRecord.Where(r => r.CreationDate.Value >= start.Value && r.CreationDate.Value <= end.Value)
                                             .Skip(startIndex)
                                             .Take(resultsPerPage)
                                             .ToArray();

                    return (DataRecord[])pageRecords;
                }
                else
                {
                    return records;
                }
            }
        }
    }
}