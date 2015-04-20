using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShadyPines.Models
{
    public enum NurseLevel { Staff, CNM }
    public class Nurse
    {
        public int NurseID { get; set; }
        public String Name { get; set; }
        public NurseLevel NurseLevel { get; set; }
        
        // one nurse has many patients
        public virtual Patient patient { get; set; }
    }
}