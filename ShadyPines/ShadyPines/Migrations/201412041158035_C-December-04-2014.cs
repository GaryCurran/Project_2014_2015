namespace ShadyPines.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CDecember042014 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.MedicalQuestions", "patient_PatientID", "dbo.Patients");
            DropIndex("dbo.MedicalQuestions", new[] { "patient_PatientID" });
            RenameColumn(table: "dbo.MedicalQuestions", name: "patient_PatientID", newName: "patientID");
            AlterColumn("dbo.MedicalQuestions", "patientID", c => c.Int(nullable: false));
            CreateIndex("dbo.MedicalQuestions", "patientID");
            AddForeignKey("dbo.MedicalQuestions", "patientID", "dbo.Patients", "PatientID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MedicalQuestions", "patientID", "dbo.Patients");
            DropIndex("dbo.MedicalQuestions", new[] { "patientID" });
            AlterColumn("dbo.MedicalQuestions", "patientID", c => c.Int());
            RenameColumn(table: "dbo.MedicalQuestions", name: "patientID", newName: "patient_PatientID");
            CreateIndex("dbo.MedicalQuestions", "patient_PatientID");
            AddForeignKey("dbo.MedicalQuestions", "patient_PatientID", "dbo.Patients", "PatientID");
        }
    }
}
