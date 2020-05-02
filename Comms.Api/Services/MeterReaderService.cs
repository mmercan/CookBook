using System;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Google.Protobuf.WellKnownTypes;

namespace Comms.Api
{
    public class MeterReaderService : MeterReader.MeterReaderBase
    {

        private readonly ILogger<MeterReaderService> _logger;

        public MeterReaderService(ILogger<MeterReaderService> logger)
        {
            _logger = logger;
        }
        public override Task<StatusMessage> AddReading(ReadingPackage request, ServerCallContext context)
        {
            var date = DateTime.Now;
            return Task.FromResult(new StatusMessage
            {
                Message = "new message 123",
                Success = ReadingStatus.Success,
                StatusTime = Timestamp.FromDateTime(DateTime.UtcNow)
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