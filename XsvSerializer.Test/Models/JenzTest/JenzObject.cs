using System;
using Tiberhealth.XsvSerializer;

namespace XsvSerializer.Test.Models.JenzTest
{
    internal class JenzObject
    {
        public string FirstName { get; set; }
        public string MidName { get; set; }
        public string LastName { get; set; }
        [Xsv("Student_ID")] public string StudentId { get; set; }
        [Xsv("Application_Year")] public int? ApplicationYear { get; set; }
        [Xsv("Application_Term")] public string ApplicationTerm { get; set; }
        [Xsv("Last_Admission_Stage")] public string LastAdminissionStage { get; set; }
        [Xsv("Stage_Date")] public DateTime? StageDate { get; set; }
        [Xsv("Previous_Admission_Stage")] public string PreviousAdmissionStage { get; set; }
        public string Location { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        [Xsv("Last_4_SSN")] public string Last4Ssn { get; set; }
        [Xsv("CITY")] public string City { get; set; }
        [Xsv("STATE")] public string State { get; set; }
        [Xsv("COUNTRY")] public string Country { get; set; }
        [Xsv("DOB")] public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        [Xsv("Undergrad_GPA")] public decimal? UndergradGpa { get; set; }
        [Xsv("Undergrad_SGPA")] public decimal? UndergradSgpa { get; set; }
        [Xsv("Master_Inst")] public string MasterInstitution { get; set; }
        [Xsv("Master_GPA")] public decimal? MasterGpa { get; set; }
        [Xsv("Master_SGPA")] public decimal? MasterSgpa { get; set; }
        [Xsv("MCAT_Score")] public decimal? McatScore { get; set; }
        [Xsv("MCATC")] public decimal? Mcatc { get; set; }
        [Xsv("Program_Code")] public string ProgramCode { get; set; }
        public string Enrollment { get; set; }
    }
}
