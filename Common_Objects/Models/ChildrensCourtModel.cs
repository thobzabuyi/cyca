using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class ChildrensCourtModel
    {
        public CPR_Childrens_Court_Detail GetSpecificChildresCourtDetailItem(int childrensCourtDetailItemId)
        {
            CPR_Childrens_Court_Detail childrensCourtDetailItem;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var childrensCourtDetailItemList = (from r in dbContext.CPR_Childrens_Court_Detail_Items
                                                    where r.Childrens_Court_Detail_Id.Equals(childrensCourtDetailItemId)
                                                    select r).ToList();

                childrensCourtDetailItem = (from r in childrensCourtDetailItemList
                                            select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return childrensCourtDetailItem;
        }

        public List<CPR_Childrens_Court_Detail> GetListOfChildresCourtDetailItems()
        {
            List<CPR_Childrens_Court_Detail> childrensCourtDetailItems;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var childrensCourtDetailItemList = (from r in dbContext.CPR_Childrens_Court_Detail_Items
                                                        select r).ToList();

                    childrensCourtDetailItems = (from r in childrensCourtDetailItemList
                                                 select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return childrensCourtDetailItems;
        }

        public CPR_Childrens_Court_Detail CreateChildrensCourtDetailItem(int incidentId, bool isForm4Issued, DateTime? dateForm4Issued, DateTime? dateCourtOrderIssued, int? courtId)
        {
            CPR_Childrens_Court_Detail newChildrensCourtDetail;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                var childrensCourtDetail = new CPR_Childrens_Court_Detail()
                {
                    Incident_Id = incidentId,
                    Is_Form4_Issued = isForm4Issued,
                    Date_Form4_Issued = dateForm4Issued,
                    Date_Court_Order_Issued = dateCourtOrderIssued,
                    Court_Id = courtId
                };

                try
                {
                    newChildrensCourtDetail = dbContext.CPR_Childrens_Court_Detail_Items.Add(childrensCourtDetail);
                    dbContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    var Test = ex.Message;
                    return null;
                }
            }

            return newChildrensCourtDetail;
        }

        public CPR_Childrens_Court_Detail EditChildrensCourtDetailItem(int childrensCourtDetailId, int incidentId, bool isForm4Issued, DateTime? dateForm4Issued, DateTime? dateCourtOrderIssued, int? courtId, string Cour_Order_Number)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var editChildrensCourtDetailItem = (from i in dbContext.CPR_Childrens_Court_Detail_Items
                                                    where i.Childrens_Court_Detail_Id.Equals(childrensCourtDetailId)
                                                    select i).FirstOrDefault();

                if (editChildrensCourtDetailItem == null) return null;

                editChildrensCourtDetailItem.Incident_Id = incidentId;
                editChildrensCourtDetailItem.Is_Form4_Issued = isForm4Issued;
                editChildrensCourtDetailItem.Date_Form4_Issued = dateForm4Issued;
                editChildrensCourtDetailItem.Date_Court_Order_Issued = dateCourtOrderIssued;
                editChildrensCourtDetailItem.Court_Id = courtId;
                editChildrensCourtDetailItem.Court_Order_Number = Cour_Order_Number;
                dbContext.SaveChanges();

                return editChildrensCourtDetailItem;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public CPR_Childrens_Court_Detail AddNeedForCareReasons(int childrensCourtDetailItemId, List<int> needForCareReasonItemIds)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var childrensCourtDetailItemToEdit = dbContext.CPR_Childrens_Court_Detail_Items.FirstOrDefault(x => x.Childrens_Court_Detail_Id.Equals(childrensCourtDetailItemId));
                if (childrensCourtDetailItemToEdit == null) return null;

                childrensCourtDetailItemToEdit.Need_for_Care_Reasons.Clear();

                foreach (var needForCareReasonItemId in needForCareReasonItemIds)
                {
                    var reasonToAdd = dbContext.Need_for_Care_Reason_Items.FirstOrDefault(x => x.Need_for_Care_Reason_Id.Equals(needForCareReasonItemId));
                    if (reasonToAdd == null) return null;

                    childrensCourtDetailItemToEdit.Need_for_Care_Reasons.Add(reasonToAdd);
                }

                dbContext.SaveChanges();

                return childrensCourtDetailItemToEdit;
            }
            catch (Exception ex)
            {
                var Test = ex.Message;
                return null;
            }
        }

        public CPR_Childrens_Court_Detail AddCourtOutcomeItems(int childrensCourtDetailItemId, List<int> courtOutcomeItemIds)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var childrensCourtDetailItemToEdit = dbContext.CPR_Childrens_Court_Detail_Items.FirstOrDefault(x => x.Childrens_Court_Detail_Id.Equals(childrensCourtDetailItemId));
                if (childrensCourtDetailItemToEdit == null) return null;

                childrensCourtDetailItemToEdit.Court_Outcomes.Clear();

                foreach (var courtOutcomeItemId in courtOutcomeItemIds)
                {
                    var courtOutcomeToAdd = dbContext.Court_Outcome_Items.FirstOrDefault(x => x.Court_Outcome_Id.Equals(courtOutcomeItemId));
                    if (courtOutcomeToAdd == null) return null;

                    childrensCourtDetailItemToEdit.Court_Outcomes.Add(courtOutcomeToAdd);
                }

                dbContext.SaveChanges();

                return childrensCourtDetailItemToEdit;
            }
            catch (Exception ex)
            {
                var Test = ex.Message;
                return null;
            }
        }

        public CPR_Childrens_Court_Detail AddSection173Items(int childrensCourtDetailItemId, List<int> section173ItemIds)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var childrensCourtDetailItemToEdit = dbContext.CPR_Childrens_Court_Detail_Items.FirstOrDefault(x => x.Childrens_Court_Detail_Id.Equals(childrensCourtDetailItemId));
                if (childrensCourtDetailItemToEdit == null) return null;

                childrensCourtDetailItemToEdit.Section_173_Items.Clear();

                foreach (var section173ItemId in section173ItemIds)
                {
                    var section173ItemToAdd = dbContext.Section_173_Items.FirstOrDefault(x => x.Section_173_Item_Id.Equals(section173ItemId));
                    if (section173ItemToAdd == null) return null;

                    childrensCourtDetailItemToEdit.Section_173_Items.Add(section173ItemToAdd);
                }

                dbContext.SaveChanges();

                return childrensCourtDetailItemToEdit;
            }
            catch (Exception ex)
            {
                var Test = ex.Message;
                return null;
            }
        }

        public CPR_Childrens_Court_Detail AddStatutuoryCourtOutcome(int childrensCourtDetailItemId, int statutoryCourtOutcomeId)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var childrensCourtDetailItemToEdit = dbContext.CPR_Childrens_Court_Detail_Items.FirstOrDefault(x => x.Childrens_Court_Detail_Id.Equals(childrensCourtDetailItemId));
                if (childrensCourtDetailItemToEdit == null) return null;

                childrensCourtDetailItemToEdit.Statutory_Outcome_Items.Clear();

                var section173ItemToAdd = dbContext.Statutory_Outcome_Items.FirstOrDefault(x => x.Statutory_Outcome_Item_Id.Equals(statutoryCourtOutcomeId));
                if (section173ItemToAdd == null) return null;

                childrensCourtDetailItemToEdit.Statutory_Outcome_Items.Add(section173ItemToAdd);

                dbContext.SaveChanges();

                return childrensCourtDetailItemToEdit;
            }
            catch (Exception ex)
            {
                var Test = ex.Message;
                return null;
            }
        }
    }
}
