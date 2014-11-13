using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ShadyPines.Models
{
    public class MyDBConnection: DbContext 
    {
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Nurse> Nurses { get; set; }
       
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<MedicalQuestion> MedicalQuestions { get; set; }
        
    }
}

