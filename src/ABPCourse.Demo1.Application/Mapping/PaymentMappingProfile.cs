using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABPCourse.Demo1.Payment;
using AutoMapper;

namespace ABPCourse.Demo1.Mapping
{
    public class PaymentMappingProfile:Profile
    {
        public PaymentMappingProfile()
        {
            CreateMap<payment, PaymentDto>();
            CreateMap<PaymentDto, payment>();
        }
    }
}
