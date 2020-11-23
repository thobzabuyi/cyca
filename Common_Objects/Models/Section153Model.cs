using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class Section153Model
    {
        public CPR_Section_153 GetSpecificSection153(int section153Id)
        {
            CPR_Section_153 section153;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var section153List = (from s in dbContext.CPR_Section_153s
                                      where s.Section_153_Id.Equals(section153Id)
                                      select s).ToList();

                section153 = (from i in section153List
                              select i).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return section153;
        }

        public List<CPR_Section_153> GetListOfSection153s(bool showInActive, bool showDeleted)
        {
            List<CPR_Section_153> section153s;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var section153List = (from i in dbContext.CPR_Section_153s
                                      select i).ToList();

                section153s = (from i in section153List
                               select i).ToList();
            }
            catch (Exception)
            {
                return null;
            }

            return section153s;
        }

        public CPR_Section_153 CreateSection153(int allegedOffenderId, bool isNoticeGranted, DateTime? dateNoticeGranted, bool isNoticeIssued, DateTime? dateNoticeIssued)
        {
            CPR_Section_153 newSection153;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                var section153 = new CPR_Section_153()
                {
                    Alleged_Offender_Id = allegedOffenderId,
                    Is_Notice_Granted = isNoticeGranted,
                    Date_Notice_Granted = dateNoticeGranted,
                    Is_Notice_Issued = isNoticeIssued,
                    Date_Notice_Issued = dateNoticeIssued
                };

                try
                {
                    newSection153 = dbContext.CPR_Section_153s.Add(section153);
                    dbContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    string exVal = ex.Message;
                    return null;
                }
            }

            return newSection153;
        }

        public CPR_Section_153 EditSection153(int section153Id, int allegedOffenderId, bool isNoticeGranted, DateTime? dateNoticeGranted, bool isNoticeIssued, DateTime? dateNoticeIssued)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var editAddress = (from a in dbContext.CPR_Section_153s
                                   where a.Section_153_Id.Equals(section153Id)
                                   select a).FirstOrDefault();

                if (editAddress == null) return null;

                editAddress.Alleged_Offender_Id = allegedOffenderId;
                editAddress.Is_Notice_Granted = isNoticeGranted;
                editAddress.Date_Notice_Granted = dateNoticeGranted;
                editAddress.Is_Notice_Issued = isNoticeIssued;
                editAddress.Date_Notice_Issued = dateNoticeIssued;

                dbContext.SaveChanges();

                return editAddress;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public CPR_Section_153 AddSection153Item(int cprSection153Id, List<int> section153ItemIds)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var cprSection153ToEdit = dbContext.CPR_Section_153s.FirstOrDefault(x => x.Section_153_Id.Equals(cprSection153Id));
                if (cprSection153ToEdit == null) return null;

                cprSection153ToEdit.Section_153_Items.Clear();

                foreach (var section153ItemId in section153ItemIds)
                {
                    var section153ItemToAdd = dbContext.Section_153_Items.FirstOrDefault(x => x.Section_153_Item_Id.Equals(section153ItemId));
                    if (section153ItemToAdd == null) return null;

                    cprSection153ToEdit.Section_153_Items.Add(section153ItemToAdd);
                }

                dbContext.SaveChanges();

                return cprSection153ToEdit;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
