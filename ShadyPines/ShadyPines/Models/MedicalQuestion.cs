using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.EnterpriseServices;

namespace ShadyPines.Models
{
    public enum Quest { Very_Poor,Poor, Fair, Average, Good,Very_Good, Excellent}
   
public class MedicalQuestion
    {
        public int MedicalQuestionID { get; set; }


        [Display(Name = "Question 1")]
        public Quest Question1 { get; set; }

        [Display(Name = "Question 2")]
        public Quest Question2 { get; set; }

        [Display(Name = "Nurse")]
        public string NurseTaken { get; set; }

        [Display(Name = "Date")]
        public DateTime Date { get; set; }

        
        public virtual Doctor doctor { get; set; }
        public virtual Nurse nurse { get; set; }
        public virtual Patient patient { get; set; }
        

    }
}