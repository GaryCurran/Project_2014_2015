using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShadyPines.Models
{
    public class Doctor
    {
        public int DoctorID { get; set; }
        public String Name { get; set; }


       // one doctor has many patients
        public virtual IEnumerable<Patient> patient { get; set; }

    }
}