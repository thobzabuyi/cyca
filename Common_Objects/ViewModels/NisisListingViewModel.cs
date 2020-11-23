using Common_Objects.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace Common_Objects.ViewModels
{
    public class NisisListingViewModel
    {
        public int Segment_Count { get; set; }

        public List<NISIS_Listing> NISIS_Listings { get; set; }

        public int Site_EA_Id { get; set; }

        public int Selected_Segment_Id { get; set; }

        [Display(Name = "EA Segment")]
        public SelectList Segment_List
        {
            get
            {
                var segmentModel = new NisisSiteEASegmentModel();
                var listOfSegments = segmentModel.GetListOfNisisSiteEASegments(false, false, Site_EA_Id);

                var segmentList = (from c in listOfSegments
                                   select new SelectListItem()
                                   {
                                       Text = c.EA_Code_With_Segment,
                                       Value = c.Segment_Id.ToString(CultureInfo.InvariantCulture),
                                       Selected = c.Segment_Id.Equals(Selected_Segment_Id)
                                   }).ToList();

                if (segmentList.Count == 1)
                {
                    segmentList[0].Selected = true;
                    Selected_Segment_Id = int.Parse(segmentList[0].Value);
                }

                var selectList = new SelectList(segmentList, "Value", "Text", Selected_Segment_Id);

                return selectList;
            }
            set { }
        }
    }
}
