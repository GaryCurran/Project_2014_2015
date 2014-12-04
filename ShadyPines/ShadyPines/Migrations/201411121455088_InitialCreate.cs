namespace ShadyPines.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Doctors",
                c => new
                    {
                        DoctorID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.DoctorID);
            
            CreateTable(
                "dbo.MedicalQuestions",
                c => new
                    {
                        MedicalQuestionID = c.Int(nullable: false, identity: true),
                        Question1 = c.Int(nullable: false),
                        Question2 = c.Int(nullable: false),
                        NurseTaken = c.String(),
                        Date = c.DateTime(nullable: false),
                        doctor_DoctorID = c.Int(),
                        nurse_NurseID = c.Int(),
                        patient_PatientID = c.Int(),
                    })
                .PrimaryKey(t => t.MedicalQuestionID)
                .ForeignKey("dbo.Doctors", t => t.doctor_DoctorID)
                .ForeignKey("dbo.Nurses", t => t.nurse_NurseID)
                .ForeignKey("dbo.Patients", t => t.patient_PatientID)
                .Index(t => t.doctor_DoctorID)
                .Index(t => t.nurse_NurseID)
                .Index(t => t.patient_PatientID);
            
            CreateTable(
                "dbo.Nurses",
                c => new
                    {
                        NurseID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        NurseLevel = c.Int(nullable: false),
                        patient_PatientID = c.Int(),
                    })
                .PrimaryKey(t => t.NurseID)
                .ForeignKey("dbo.Patients", t => t.patient_PatientID)
                .Index(t => t.patient_PatientID);
            
            CreateTable(
                "dbo.Patients",
                c => new
                    {
                        PatientID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        MedicalCard = c.String(),
                        Gender = c.Int(nullable: false),
                        DoctorName = c.String(),
                        doctor_DoctorID = c.Int(),
                    })
                .PrimaryKey(t => t.PatientID)
                .ForeignKey("dbo.Doctors", t => t.doctor_DoctorID)
                .Index(t => t.doctor_DoctorID);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.MedicalQuestions", "patient_PatientID", "dbo.Patients");
            DropForeignKey("dbo.MedicalQuestions", "nurse_NurseID", "dbo.Nurses");
            DropForeignKey("dbo.Nurses", "patient_PatientID", "dbo.Patients");
            DropForeignKey("dbo.Patients", "doctor_DoctorID", "dbo.Doctors");
            DropForeignKey("dbo.MedicalQuestions", "doctor_DoctorID", "dbo.Doctors");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Patients", new[] { "doctor_DoctorID" });
            DropIndex("dbo.Nurses", new[] { "patient_PatientID" });
            DropIndex("dbo.MedicalQuestions", new[] { "patient_PatientID" });
            DropIndex("dbo.MedicalQuestions", new[] { "nurse_NurseID" });
            DropIndex("dbo.MedicalQuestions", new[] { "doctor_DoctorID" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Patients");
            DropTable("dbo.Nurses");
            DropTable("dbo.MedicalQuestions");
            DropTable("dbo.Doctors");
        }
    }
}
