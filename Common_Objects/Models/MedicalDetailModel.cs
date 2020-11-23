using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class MedicalDetailModel
    {
        public Medical_Detail GetSpecificMedicalDetailItem(int medicalDetailId)
        {
            Medical_Detail medicalDetail;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var medicalDetailItems = (from e in dbContext.Medical_Detail_Items
                                          where e.Medical_Detail_Id.Equals(medicalDetailId)
                                          select e).ToList();

                medicalDetail = (from e in medicalDetailItems
                                 select e).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return null;
            }

            return medicalDetail;
        }

        public List<Medical_Detail> GetListOfMedicalDetailItems()
        {
            List<Medical_Detail> listOfMedicalDetailItems;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var medicalDetailItems = (from e in dbContext.Medical_Detail_Items
                                          select e).ToList();

                listOfMedicalDetailItems = (from e in medicalDetailItems
                                            select e).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }

            return listOfMedicalDetailItems;

        }

        public Medical_Detail CreateMedicalDetail(int incidentId, bool isForm9Completed, bool isJ88Completed, string practitionerName, string practitionerContactNumber, 
            int? treatmentTypeId, int? treatmentGivenById, int? treatmentPlaceId, DateTime? treatmentDate, string treatmentDetails, bool isMedicalFollowUp)
        {
            Medical_Detail newMedicalDetail;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {

                var medicalDetail = new Medical_Detail()
                {
                    Incident_Id = incidentId,
                    Is_Form9_Completed = isForm9Completed,
                    Is_J88_Completed = isJ88Completed,
                    Practitioner_Name = practitionerName,
                    Practitioner_Contact_Number = practitionerContactNumber,
                    Treatment_Type_Id = treatmentTypeId,
                    Treatment_Given_By_Id = treatmentGivenById,
                    Treatment_Place_Id = treatmentPlaceId,
                    Treatment_Date = treatmentDate,
                    Treatment_Details = treatmentDetails,
                    Is_Medical_Followup = isMedicalFollowUp
                };

                try
                {
                    newMedicalDetail = dbContext.Medical_Detail_Items.Add(medicalDetail);
                    dbContext.SaveChanges();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return newMedicalDetail;
        }

        public Medical_Detail EditMedicalDetail(int medicalDetailId, int incidentId, bool isForm9Completed, bool isJ88Completed, string practitionerName, string practitionerContactNumber, 
            int? treatmentTypeId, int? treatmentGivenById, int? treatmentPlaceId, DateTime? treatmentDate, string treatmentDetails, bool isMedicalFollowUp)
        {
            Medical_Detail editMedicalDetail;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    editMedicalDetail = (from e in dbContext.Medical_Detail_Items
                                         where e.Medical_Detail_Id.Equals(medicalDetailId)
                                         select e).FirstOrDefault();

                    if (editMedicalDetail != null)
                    {
                        editMedicalDetail.Incident_Id = incidentId;
                        editMedicalDetail.Is_Form9_Completed = isForm9Completed;
                        editMedicalDetail.Is_J88_Completed = isJ88Completed;
                        editMedicalDetail.Practitioner_Name = practitionerName;
                        editMedicalDetail.Practitioner_Contact_Number = practitionerContactNumber;
                        editMedicalDetail.Treatment_Type_Id = treatmentTypeId;
                        editMedicalDetail.Treatment_Given_By_Id = treatmentGivenById;
                        editMedicalDetail.Treatment_Place_Id = treatmentPlaceId;
                        editMedicalDetail.Treatment_Date = treatmentDate;
                        editMedicalDetail.Treatment_Details = treatmentDetails;
                        editMedicalDetail.Is_Medical_Followup = isMedicalFollowUp;
                        dbContext.SaveChanges();
                    }
                    else
                    {
                        editMedicalDetail = new Medical_Detail();
                        editMedicalDetail.Incident_Id = incidentId;
                        editMedicalDetail.Is_Form9_Completed = isForm9Completed;
                        editMedicalDetail.Is_J88_Completed = isJ88Completed;
                        editMedicalDetail.Practitioner_Name = practitionerName;
                        editMedicalDetail.Practitioner_Contact_Number = practitionerContactNumber;
                        editMedicalDetail.Treatment_Type_Id = treatmentTypeId;
                        editMedicalDetail.Treatment_Given_By_Id = treatmentGivenById;
                        editMedicalDetail.Treatment_Place_Id = treatmentPlaceId;
                        editMedicalDetail.Treatment_Date = treatmentDate;
                        editMedicalDetail.Treatment_Details = treatmentDetails;
                        editMedicalDetail.Is_Medical_Followup = isMedicalFollowUp;
                        dbContext.Medical_Detail_Items.Add(editMedicalDetail);
                        dbContext.SaveChanges();
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return editMedicalDetail;
        }
    }
}
