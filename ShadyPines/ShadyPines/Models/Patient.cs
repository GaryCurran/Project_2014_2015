using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShadyPines.Models
{
    public enum Gender { Male,Female}
    public class Patient
    {
        public int PatientID { get; set; }

        public String Name { get; set; }

        public String MedicalCard { get; set; }

        public Gender Gender { get; set; }

        public String DoctorName { get; set; }

        // array list of medical questions
        public List<MedicalQuestion> questions = new List<MedicalQuestion>();
        

        // one patient has many nurses
        public virtual IEnumerable<Nurse> nurse { get; set; }


        // one patient has one doctor
        public virtual Doctor doctor { get; set; }

        // one patient has one list of Medical Questions
        public virtual IEnumerable<MedicalQuestion> med { get; set; }
    }
}