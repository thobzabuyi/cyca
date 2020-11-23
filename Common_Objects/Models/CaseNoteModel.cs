using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class CaseNoteModel
    {
        public Case_Note GetSpecificCaseNote(int caseNoteId)
        {
            Case_Note caseNote;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var caseNoteList = (from r in dbContext.Case_Note_Items
                                    where r.Case_Note_Id.Equals(caseNoteId)
                                    select r).ToList();

                caseNote = (from r in caseNoteList
                            select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return caseNote;
        }

        public List<Case_Note> GetListOfCaseNotes()
        {
            List<Case_Note> caseNotes;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var caseNoteList = (from r in dbContext.Case_Note_Items
                                    select r).ToList();

                caseNotes = (from r in caseNoteList
                             select r).ToList();
            }
            catch (Exception)
            {
                return null;
            }

            return caseNotes;
        }

        public List<Case_Note> GetListOfCaseNotes(int incidentId)
        {
            List<Case_Note> caseNotes;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var caseNoteList = (from r in dbContext.Case_Note_Items
                                    where r.Incident_Id == incidentId
                                    select r).ToList();

                caseNotes = (from r in caseNoteList
                             select r).ToList();
            }
            catch (Exception ex)
            {
                string exVal = ex.Message;
                return null;
            }

            return caseNotes;
        }

        public Case_Note CreateCaseNote(int incidentId, int officeTypeId, DateTime? dateNoteTaken, string caseNoteText)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            var caseNote = new Case_Note() { Incident_Id = incidentId, Office_Type_Id = officeTypeId, Date_Note_Taken = dateNoteTaken, Case_Note_Text = caseNoteText };

            try
            {
                var newCaseNote = dbContext.Case_Note_Items.Add(caseNote);

                dbContext.SaveChanges();

                return newCaseNote;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Case_Note EditCaseNote(int caseNoteId, int incidentId, int officeTypeId, DateTime? dateNoteTaken, string caseNoteText)
        {
            Case_Note editCaseNote;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    editCaseNote = (from a in dbContext.Case_Note_Items
                                    where a.Case_Note_Id.Equals(caseNoteId)
                                    select a).FirstOrDefault();

                    if (editCaseNote == null) return null;

                    editCaseNote.Incident_Id = incidentId;
                    editCaseNote.Office_Type_Id = officeTypeId;
                    editCaseNote.Date_Note_Taken = dateNoteTaken;
                    editCaseNote.Case_Note_Text = caseNoteText;

                    dbContext.SaveChanges();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return editCaseNote;
        }
    }
}
