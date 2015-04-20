using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.EnterpriseServices;

namespace ShadyPines.Models
{
    public enum Quest { [Display(Name = "Very Poor")]VeryPoor, Poor, Fair, Average, Good, [Display(Name = "Very Good")] VeryGood, Excellent }
    public enum Position
    {
        [Display(Name = "Prone Position")]//score of 1
        Prone,
        [Display(Name = "Supline Position")]//score of 1
        Supline,
        [Display(Name = "Lateral Position")]//score of 3
        lateral,
        [Display(Name = "Sims Position")]//score of 3
        sim,
        [Display(Name = "Fowler's Position")]//score of 6
        fp
    }
    public enum NurseEn
    {
        [Display(Name = "CNM A Wilkes")]
        aw,
        [Display(Name = "CNM B Fraser")]
        bf,
        [Display(Name = "SN H Martin")]
        hm,
        [Display(Name = "SN M Houlihan")]
        mh,
        [Display(Name = "CNM G Emmanuel")]
        ge
    }
    public class MedicalQuestion
    {
        public int MedicalQuestionID { get; set; }
        // list of Q's to be asked of patients to access daily health
        // Q's will be a max 10 ??
        // Ask Caroline for her input for relevant Q's


        [Display(Name = "Has the resident fallen today ?")]
        public bool HasFallen { get; set; }
      
        [Display(Name = "How did they sleep last night ?")]
        public Quest Question1 { get; set; }

        [Display(Name = "How is their appetite today ?")]
        public Quest Question2 { get; set; }

        [Display(Name = "How is their awareness today ?")]
        public Quest Question3 { get; set; }

        [Display(Name = "What is their usual resting position whilst in bed ?")]
        public Position Question4 { get; set; }

        [Display(Name = "How do they respond to administrating nurse instructions ?")]
        public Quest Question5 { get; set; }

        [Display(Name = "How is their general state of happiness ?")]
        public Quest Question6 { get; set; }

        [Display(Name = "What is their level of interaction with visitors  ?")]
        public Quest Question7 { get; set; }

        [Display(Name = "What is their level of interaction with other residents  ?")]
        public Quest Question8 { get; set; }

        [Display(Name = "What is their level of interaction with planned activities  ?")]
        public Quest Question9 { get; set; }

        [Display(Name = "Administrating Nurse")]
        public  NurseEn NurseEn { get; set; }

        [Display(Name = "Assessment Date ")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public int DailyTotal { get; set; }

        public int patientID { get; set; }

        public Doctor doctor { get; set; }
        public Nurse nurse { get; set; }
        public virtual Patient patient { get; set; }

    }
}