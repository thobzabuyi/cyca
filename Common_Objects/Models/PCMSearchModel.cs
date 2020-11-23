using Common_Objects.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Json;
using System.Web.Mvc;

namespace Common_Objects.Models
{
    public class PCMSearchModel
    {
        SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();


        public List<PCMCaseGridMain> GetPCMWorkList()
        {
            // initializing view model
            List<PCMCaseGridMain> caseViewModel = new List<PCMCaseGridMain>();

            // get Adoption cases that have been accepted on adoption module

            var clientAssessments = (from p in db.Intake_Assessments
                                     join client in db.Clients on p.Client_Id equals client.Client_Id
                                     join person in db.Persons on client.Person_Id equals person.Person_Id
                                     join subcat in db.Problem_Sub_Categories on p.Problem_Sub_Category_Id equals subcat.Problem_Sub_Category_Id

                                     join worklist in db.ADOPT_Case_WorkList on p.Intake_Assessment_Id equals worklist.Intake_Assessment_Id//work list

                                     where (subcat.Problem_Sub_Category_Id == 19 /*&& worklist.Adopt_Record_Status_Id != 1*/)

                                     group p by new { client.Client_Id, person.First_Name, person.Last_Name, person.Identification_Number, p.Intake_Assessment_Id, worklist.Adopt_Record_Status_Id }
                                     into g
                                     select new
                                     {
                                         g.Key.Intake_Assessment_Id,
                                         g.Key.Client_Id,
                                         g.Key.First_Name,
                                         g.Key.Last_Name,
                                         g.Key.Identification_Number,
                                         //g.Key.Adopt_Record_Status_Id,
                                         Assessments = g.ToList()

                                     }).ToList();
            ;
            foreach (var item in clientAssessments)
            {
                PCMCaseGridMain obj = new PCMCaseGridMain();

                obj.IntakeAssessmentId = item.Intake_Assessment_Id;
                obj.ClientId = item.Client_Id;
                obj.FirstName = item.First_Name;
                obj.LastName = item.Last_Name;
                obj.IDNumber = item.Identification_Number;
                //obj.RecordStatusDescription = db.apl_Adoption_Record_Status.Find(item.Adopt_Record_Status_Id).Description;
                caseViewModel.Add(obj);
            }
            return caseViewModel;
        }
    }
}
