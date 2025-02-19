using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABPCourse.Demo1.Messages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABPCourse.Demo1.Configration
{
    public class MessageConfigration: IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {

            builder.Property(x => x.Content).HasMaxLength(500);
            builder.Property(x => x.Content).IsRequired();
            builder.Property(x => x.SentTime).HasDefaultValueSql("getdate()");
        }
    }
}
