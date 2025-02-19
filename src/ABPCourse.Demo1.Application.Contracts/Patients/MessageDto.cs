using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABPCourse.Demo1.Patients
{
    public class MessageDto
    {
        public Guid Id { get; set; }
        public Guid AppointmentId { get; set; }
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public string Content { get; set; }
        public DateTime SentTime { get; set; }
    }
}
