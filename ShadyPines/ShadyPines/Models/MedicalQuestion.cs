using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.EnterpriseServices;

namespace ShadyPines.Models
{
    public enum Quest { [Display(Name = "Very Poor")]VeryPoor, Poor, Fair, Average, Good, [Display(Name = "Very Good")] VeryGood, Excellent }
   
public class MedicalQuestion
    {
        public int MedicalQuestionID { get; set; }


        // list of Q's to be asked of patients to access daily health
        // Q's will be a max 10 ??
        // Ask Caroline for her input for relevant Q's
        [Display(Name = "How did the resident sleep last night")]
        public Quest Question1 { get; set; }

        [Display(Name = "How's the residents appetite this morning")]
        public Quest Question2 { get; set; }

        [Display(Name = "Administrating Nurse")]
        public string NurseTaken { get; set; }

        [Display(Name = "Assessment Date ")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public int DailyTotal { get; set; }

        public int patientID { get; set; }
        
        public  Doctor doctor { get; set; }
        public  Nurse nurse { get; set; }
        public  virtual Patient patient { get; set; }
       
    }
}