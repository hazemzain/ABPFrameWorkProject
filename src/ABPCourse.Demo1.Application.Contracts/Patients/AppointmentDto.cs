﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABPCourse.Demo1.Patients
{
    public class AppointmentDto
    {

        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public Guid DoctorId { get; set; }
        public DateTime Date { get; set; }
        public string Notes { get; set; }
        public bool IsPaid { get; set; }

    }
}
