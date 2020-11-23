using System.Collections.Generic;
using System.Linq;
using Common_Objects.Models;
using System.Web;
using System.Web.Mvc;
using ZXing;
using ZXing.Common;
using System.Drawing.Imaging;
using System.IO;
using System;

namespace Common_Objects
{
    public class Helpers
    {
        public static void SetAuthorizedRolesVisibility(ref List<Menu_Item> menuItems, List<Role> authorizedRoles)
        {
            foreach (var item in menuItems)
            {
                if (item.Is_Active == false)
                {
                    item.Is_Visible = false;
                }else
                {
                    item.Is_Visible = true;
                }
                

                if (item.Sub_Menu_Items.Any())
                {
                    var subMenuItems = item.Sub_Menu_Items.Where(x => x.Is_Active == true).ToList();

                    SetAuthorizedRolesVisibility(ref subMenuItems, authorizedRoles);

                    // Set Parent Item Invisible if all SubItems are Invisible
                    item.Is_Visible = item.Sub_Menu_Items.Count(i => i.Is_Visible.Equals(false)) != item.Sub_Menu_Items.Count();
                }
                else
                {
                    var isAuthorized = false;

                    if (item.Module_Action != null)
                    {
                        if (item.Module_Action.Roles.Any())
                        {
                            foreach (var role in item.Module_Action.Roles)
                            {
                                if (authorizedRoles.Count(ar => ar.Role_Id.Equals(role.Role_Id)) > 0)
                                    isAuthorized = true;
                            }
                        }
                        //else
                        //{
                        //    // If no roles are specified for a Menu Item, we assume it's visible to all
                        //    //isAuthorized = true;
                        //}
                    }

                    item.Is_Visible = isAuthorized;
                }
            }
        }

        public static List<LookupTableItem> GetLookupDataItems()
        {
            var items = new List<LookupTableItem>()
            {
                new LookupTableItem() { Description = "Abuse Types", Value = 1 },
                new LookupTableItem() { Description = "Allergies", Value = 2 },
                new LookupTableItem() { Description = "Chronic Illnesses", Value = 3 },
                new LookupTableItem() { Description = "Client Types", Value = 4 },
                new LookupTableItem() { Description = "Disabilities", Value = 5 },
                new LookupTableItem() { Description = "Eye Colors", Value = 6 },
                new LookupTableItem() { Description = "Genders", Value = 7 },
                new LookupTableItem() { Description = "Grades", Value = 8 },
                new LookupTableItem() { Description = "Hair Colors", Value = 9 },
                new LookupTableItem() { Description = "Idenfication Types", Value = 10 },
                new LookupTableItem() { Description = "Income Ranges", Value = 11 },
                new LookupTableItem() { Description = "Injury Types", Value = 12 },
                new LookupTableItem() { Description = "Job Positions", Value = 13 },
                new LookupTableItem() { Description = "Languages", Value = 14 },
                new LookupTableItem() { Description = "Marital Statusses", Value = 15 },
                new LookupTableItem() { Description = "Nationalities", Value = 16 },
                new LookupTableItem() { Description = "Occupations", Value = 17 },
                new LookupTableItem() { Description = "Offence Categories", Value = 18 },
                new LookupTableItem() { Description = "Population Groups", Value = 19 },
                new LookupTableItem() { Description = "Preferred Contact Types", Value = 20 },
                new LookupTableItem() { Description = "Races", Value = 21 },
                new LookupTableItem() { Description = "Reception Action Types", Value = 22 },
                new LookupTableItem() { Description = "Reception Visit Types", Value = 23 },
                new LookupTableItem() { Description = "Referral Focus Area", Value = 24 },
                new LookupTableItem() { Description = "Relationship Types", Value = 25 },
                new LookupTableItem() { Description = "Religions", Value = 26 },
                new LookupTableItem() { Description = "School Types", Value = 27 },
                new LookupTableItem() { Description = "Another Lookup", Value = 28 }
            }.ToList();

            return items;
        }

        public static List<LookupTableItem> GetMedicalConditionTypes()
        {
            var items = new List<LookupTableItem>()
            {
                new LookupTableItem() { Description = "Allergy", Value = 1 },
                new LookupTableItem() { Description = "Chronic Illness", Value = 2 },
                new LookupTableItem() { Description = "Disease ", Value = 3 }
            }.ToList();

            return items;
        }

        public static List<LookupTableItem> GetRelationTypes()
        {
            var items = new List<LookupTableItem>()
            {
                new LookupTableItem() { Description = "Adoptive Parents", Value = 1 },
                new LookupTableItem() { Description = "Biological Parents", Value = 2 },
                new LookupTableItem() { Description = "Family Members", Value = 3},
                new LookupTableItem() { Description = "Foster Parents", Value = 4},
                new LookupTableItem() { Description = "Caregivers", Value = 5},
                new LookupTableItem() { Description = "Prospective Adoptive Parents", Value = 6}
            }.ToList();

            return items;
        }
    }

    public static class QRHelper
    {
        public static IHtmlString GenerateQrCode(this HtmlHelper html, string url, string alt = "QR code", int height = 500, int width = 500, int margin = 0)
        {
            var qrWriter = new BarcodeWriter();
            qrWriter.Format = BarcodeFormat.QR_CODE;
            qrWriter.Options = new EncodingOptions() { Height = height, Width = width, Margin = margin };

            using (var q = qrWriter.Write(url))
            {
                using (var ms = new MemoryStream())
                {
                    q.Save(ms, ImageFormat.Png);
                    var img = new TagBuilder("img");
                    img.Attributes.Add("src", String.Format("data:image/png;base64,{0}", Convert.ToBase64String(ms.ToArray())));
                    img.Attributes.Add("alt", alt);
                    return MvcHtmlString.Create(img.ToString(TagRenderMode.SelfClosing));
                }
            }
        }
    }

    public class LookupTableItem
    {
        public string Description { get; set; }
        public int Value { get; set; }
    }
}
