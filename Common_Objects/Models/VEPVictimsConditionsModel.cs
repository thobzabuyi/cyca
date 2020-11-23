using System;
using Common_Objects.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.Models
{
    public class VEPVictimsConditionsModel
    {
        public List<VEP_PresentationCondition> gridList()
        {
            List<VEPVictimConditionViewModel> pvm = new List<VEPVictimConditionViewModel>();

            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();

            //var pList = (from VEPVicCon in db.VEP_VictimsConditions
            //                 //where (pcmPre.Client_Id == 27890)
            //             select new
            //             {
            //                 VEPVicCon.VictimsConditionsID,
            //                 VEPVicCon.Caseid,
            //                 VEPVicCon.Conditions,
            //                 VEPVicCon.DateCaptured
            //             }).ToList();

            //foreach (var item in pList)
            //{
            //    VEPVictimConditionViewModel vic = new VEPVictimConditionViewModel();
            //    vic.VictimsConditionsID = item.VictimsConditionsID;
            //    vic.Caseid = item.Caseid;
            //    vic.Excessively_Alert = item.Excessively_Alert;
            //    vic.Bedridden = item.Bedridden;
            //    vic.Fully_Mobile = item.Fully_Mobile;
            //    vic.Easily_Startled = item.Easily_Startled;
            //    vic.Looks_Exhausted = item.Looks_Exhausted;
            //    vic.Poor_Concentration = item.Poor_Concentration;
            //    vic.Disorientated = item.Disorientated;
            //    vic.Confused = item.Confused;
            //    vic.Looks_Afraid = item.Looks_Afraid;
            //    vic.Angry = item.Angry;
            //    vic.Irritable = item.Irritable;
            //    vic.Anxious = item.Anxious;
            //    vic.Panic = item.Panic;
            //    pvm.Add(vic);
            //}
            return db.VEP_PresentationCondition.ToList();
        }

        
    }
}
