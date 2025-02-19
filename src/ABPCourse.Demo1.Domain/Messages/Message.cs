using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace ABPCourse.Demo1.Messages
{
    public class Message:Entity<Guid>
    {
        public Guid AppointmentId { get; set; }
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public string Content { get; set; }
        public DateTime SentTime { get; set; }
    }
}
