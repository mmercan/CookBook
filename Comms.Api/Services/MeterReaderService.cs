using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace Comms.Api
{
    public class MeterReaderService : MeterReader.MeterReaderBase
    {

        private readonly ILogger<MeterReaderService> _logger;

        public MeterReaderService(ILogger<MeterReaderService> logger)
        {
            _logger = logger;
        }
        public override Task<StatusMessage> AddReading(ReadingMessage request, ServerCallContext context)
        {

            return Task.FromResult(new StatusMessage
            {
                Message = "new message 123",
                Success = ReadingStatus.Success
            });
            //return new Comms.Api.StatusMessage();
            // return new StatusMessage();
        }


        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}