using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rmanager.Models
{
    public class BookingDetailsDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid RoomId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
