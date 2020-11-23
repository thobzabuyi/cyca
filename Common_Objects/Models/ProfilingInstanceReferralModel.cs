using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class ProfilingInstanceReferralModel
    {
        public NISIS_Profiling_Instance_Referral GetSpecificProfilingInstanceReferral(int profilingInstanceReferralId)
        {
            NISIS_Profiling_Instance_Referral profilingInstanceReferral;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var profilingInstanceReferralList = (from r in dbContext.NISIS_Profiling_Instance_Referrals
                                                     where r.NISIS_Household_Referral_Id.Equals(profilingInstanceReferralId)
                                                     select r).ToList();

                profilingInstanceReferral = (from r in profilingInstanceReferralList
                                             select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return profilingInstanceReferral;
        }

        public NISIS_Profiling_Instance_Referral GetSpecificProfilingInstanceReferral(int profilingInstanceId, int referralSourceTypeId, int householdMemberId, int serviceId)
        {
            NISIS_Profiling_Instance_Referral profilingInstanceReferral;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var profilingInstanceReferralList = (from r in dbContext.NISIS_Profiling_Instance_Referrals
                                                     where r.Profiling_Instance_Id.Equals(profilingInstanceId)
                                                     where r.Referral_Source_Type_Id.Equals(referralSourceTypeId)
                                                     where r.Household_Member_Id.Equals(householdMemberId)
                                                     where r.Service_Id.Equals(serviceId)
                                                     select r).ToList();

                profilingInstanceReferral = (from r in profilingInstanceReferralList
                                             select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return profilingInstanceReferral;
        }

        public List<NISIS_Profiling_Instance_Referral> GetListOfProfilingInstanceReferrals()
        {
            List<NISIS_Profiling_Instance_Referral> profilingInstanceReferrals;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var profilingInstanceReferralList = (from r in dbContext.NISIS_Profiling_Instance_Referrals
                                                         select r).ToList();

                    profilingInstanceReferrals = (from r in profilingInstanceReferralList
                                                  select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return profilingInstanceReferrals;
        }

        public List<NISIS_Profiling_Instance_Referral> GetListOfProfilingInstanceReferrals(int profilingInstanceId)
        {
            List<NISIS_Profiling_Instance_Referral> profilingInstanceReferrals;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var profilingInstanceReferralList = (from r in dbContext.NISIS_Profiling_Instance_Referrals
                                                     where r.Profiling_Instance_Id.Equals(profilingInstanceId)
                                                     select r).ToList();

                profilingInstanceReferrals = (from r in profilingInstanceReferralList
                                              select r).ToList();
            }
            catch (Exception)
            {
                return null;
            }

            return profilingInstanceReferrals;
        }

        public List<NISIS_Profiling_Instance_Referral> GetListOfProfilingInstanceReferrals(List<int> profilingInstanceIds)
        {
            List<NISIS_Profiling_Instance_Referral> profilingInstanceReferrals;

            var dbContext = new SDIIS_DatabaseEntities();


            try
            {
                var profilingInstanceReferralList = (from r in dbContext.NISIS_Profiling_Instance_Referrals
                                                     where profilingInstanceIds.Contains(r.Profiling_Instance_Id)
                                                     select r).ToList();

                profilingInstanceReferrals = (from r in profilingInstanceReferralList
                                              select r).ToList();
            }
            catch (Exception)
            {
                return null;
            }

            return profilingInstanceReferrals;
        }

        public NISIS_Profiling_Instance_Referral CreateProfilingInstanceReferral(int profilingInstanceId, int referralSourceTypeId, int? householdMemberId, int? serviceId, int? serviceStatusId,
            int? referralPriorityId, string supportingComments, int? responsiblePersonId, int? ownerOrganizationId, int? organizationFacilityId, DateTime? slaDeliveryDate,
            int? externalDeliveryVerificationStatusId, DateTime? deliveryDate, DateTime? expectedDeliveryDate, bool isActive, bool isDeleted,
            DateTime dateCreated, string createdBy)
        {
            NISIS_Profiling_Instance_Referral newReferral;

            var dbContext = new SDIIS_DatabaseEntities();

            var profilingInstanceReferral = new NISIS_Profiling_Instance_Referral()
            {
                Profiling_Instance_Id = profilingInstanceId,
                Referral_Source_Type_Id = referralSourceTypeId,
                Household_Member_Id = householdMemberId,
                Service_Id = serviceId,
                Service_Status_Id = serviceStatusId,
                Referral_Priority_Id = referralPriorityId,
                Supporting_Comments = supportingComments,
                Responsible_Person_Id = responsiblePersonId,
                Owner_Organization_Id = ownerOrganizationId,
                Organization_Facility_Id = organizationFacilityId,
                SLA_Delivery_Date = slaDeliveryDate,
                External_Verification_Status_Id = externalDeliveryVerificationStatusId,
                Delivery_Date = deliveryDate,
                Expected_Delivery_Date = expectedDeliveryDate,
                Is_Active = isActive,
                Is_Deleted = isDeleted,
                Date_Created = dateCreated,
                Created_By = createdBy
            };

            try
            {
                newReferral = dbContext.NISIS_Profiling_Instance_Referrals.Add(profilingInstanceReferral);
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return null;
            }

            return newReferral;
        }

        public NISIS_Profiling_Instance_Referral EditProfilingInstanceReferral(int profilingInstanceReferralId, int profilingInstanceId, int referralSourceTypeId, int? householdMemberId,
            int? serviceId, int? serviceStatusId, int? referralPriorityId, string supportingComments, int? responsiblePersonId, int? ownerOrganizationId, int? organizationFacilityId,
            DateTime? slaDeliveryDate, int? externalDeliveryVerificationStatusId, DateTime? deliveryDate, DateTime? expectedDeliveryDate, bool isActive, bool isDeleted,
            DateTime dateLastModified, string modifiedBy)
        {
            NISIS_Profiling_Instance_Referral editReferral;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                editReferral = (from e in dbContext.NISIS_Profiling_Instance_Referrals
                                where e.NISIS_Household_Referral_Id.Equals(profilingInstanceReferralId)
                                select e).FirstOrDefault();

                if (editReferral == null) return null;

                editReferral.Profiling_Instance_Id = profilingInstanceId;
                editReferral.Referral_Source_Type_Id = referralSourceTypeId;
                editReferral.Household_Member_Id = householdMemberId;
                editReferral.Service_Id = serviceId;
                editReferral.Service_Status_Id = serviceStatusId;
                editReferral.Referral_Priority_Id = referralPriorityId;
                editReferral.Supporting_Comments = supportingComments;
                editReferral.Responsible_Person_Id = responsiblePersonId;
                editReferral.Owner_Organization_Id = ownerOrganizationId;
                editReferral.Organization_Facility_Id = organizationFacilityId;
                editReferral.SLA_Delivery_Date = slaDeliveryDate;
                editReferral.External_Verification_Status_Id = externalDeliveryVerificationStatusId;
                editReferral.Delivery_Date = deliveryDate;
                editReferral.Expected_Delivery_Date = expectedDeliveryDate;
                editReferral.Is_Active = isActive;
                editReferral.Is_Deleted = isDeleted;
                editReferral.Date_Last_Modified = dateLastModified;
                editReferral.Modified_By = modifiedBy;

                dbContext.SaveChanges();
            }
            catch (Exception)
            {
                return null;
            }

            return editReferral;
        }

        public NISIS_Profiling_Instance_Referral UpdateProfilingInstanceReferral(int profilingInstanceReferralId, int serviceStatusId)
        {
            NISIS_Profiling_Instance_Referral editReferral;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                editReferral = (from e in dbContext.NISIS_Profiling_Instance_Referrals
                                where e.NISIS_Household_Referral_Id.Equals(profilingInstanceReferralId)
                                select e).FirstOrDefault();

                if (editReferral == null) return null;

                editReferral.Service_Status_Id = serviceStatusId;

                dbContext.SaveChanges();
            }
            catch (Exception)
            {
                return null;
            }

            return editReferral;
        }
    }
}
