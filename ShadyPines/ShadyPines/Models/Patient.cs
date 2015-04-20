using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShadyPines.Models
{
    public enum Gender { Male, Female }
    public enum Wing { Athena, Cronus, Dinlas, Morpheus, Persephone }
    public class Patient
    {
        public int PatientID { get; set; }

        public String Name { get; set; }

        [Display(Name = "Doctors Name ")]
        public String DoctorName { get; set; }

        public Gender Gender { get; set; }

        [Display(Name = "Medical Card No: ")]
        public String MedicalCard { get; set; }
        
        [Display(Name = "Care Home Wing ")]
        //[Required(ErrorMessage = "Care home wing is required")]
        public Wing Wing { get; set; }

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