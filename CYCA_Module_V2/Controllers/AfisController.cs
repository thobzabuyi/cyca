using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Common_Objects.Models;
using Common_Objects.ViewModels;
using Newtonsoft.Json;
using System.Web.Security;
using System.Web.Helpers;
using CYCA_Module_V2.Common_Objects;
using DsdAfis.WebApi;
using System.Threading.Tasks;
using DsdAfis.Core.Messaging;
using System.Net.Http;
using System.Net.Http.Headers;
using DsdAfis.Core.FingerBiometrics;
using System.Text;
using System.Data;
using System.Data.Entity;
using System.Configuration;

namespace CYCA_Module_V2.Controllers
{
    public class AfisController : Controller
    {

       // private WebApiClient webApiClient;
        private DisenrollRequest disenrollRequest = new DisenrollRequest();
        private DisenrollResponse disenrollResponse = new DisenrollResponse();
        private EnrollRequest enrollRequest = new EnrollRequest();
        private EnrollResponse enrollResponse = new EnrollResponse();
        private VerifyRequest verifyRequest = new VerifyRequest();
        private VerifyResponse verifyResponse = new VerifyResponse();
        private IdentifyRequest identifyRequest = new IdentifyRequest();
        private IdentifyResponse identifyResponse = new IdentifyResponse();
        private readonly AffisModel affisModel = new AffisModel();
        public async Task<JsonResult> AfisEntry(FPCaptureRs vm)
        {
            switch (vm.RequestType)
            {
                case "Enroll":
                    //First Do Identity
                    var response = await IdentifyChild(vm);
                    if (response.ErrorCode != 0)
                    {
                        return Json(response, JsonRequestBehavior.AllowGet);
                    }
                    else if (response.FingerprintRecords.Length > 0)
                    {
                        //
                        var uuid = response.FingerprintRecords[0].Uuid;
                        //var record = await db.int_DSD_Afis.Where(a => a.Uid == uuid).SingleOrDefaultAsync();
                        var record = affisModel.GetRecord(uuid);
                        if (record == null)
                        {
                            //Clear from Biometric DB
                            var disEnroll = await DisEnrollChild(response.FingerprintRecords[0].Uuid);
                            if (disEnroll.ErrorCode != 0)
                            {
                                return Json(response, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                //Can Enroll
                            }
                        }
                        else
                        {
                            //Get User
                            var p = affisModel.GetUserByRecord(record.Person_Id);
                            //Already exsits in DB
                            response.Error = "These biometrics are registered to (" + p.First_Name + " " + p.Last_Name + ")";
                            response.ErrorCode = 99;
                            response.ErrorLink = "/Client/Index/" + p.Person_Id.ToString();
                            return Json(response, JsonRequestBehavior.AllowGet);
                        }
                    }
                    //Enroll User
                    var enrollResponse = await EnrollChild(vm);
                    if (enrollResponse.ErrorCode != 0)
                    {
                        return Json(response, JsonRequestBehavior.AllowGet);
                    }
                    //Add user to db
                    int_DSD_Afis afisRecord = new int_DSD_Afis();
                    afisRecord.Person_Id = vm.PersonId;
                    afisRecord.Uid = enrollResponse.FingerprintRecords[0].Uuid;
                    afisRecord.CreatedTimestamp = DateTime.Now;
                    afisRecord.Is_Verified = true;

                    await affisModel.AddAffis(afisRecord);
               
                    return Json(response, JsonRequestBehavior.AllowGet);
                case "DisEnroll":
                    var disResp = await DisEnrollChild(vm.UuId);
                    if (disResp.ErrorCode != 0)
                    {
                        return Json(disResp, JsonRequestBehavior.AllowGet);
                    }
                    //Remove from Afis         
                    await affisModel.RemoveAffis(vm.UuId);
                    return Json(disResp, JsonRequestBehavior.AllowGet);
                case "Identify":
                    //First Do Identity
                    var responseIdentify = await IdentifyChild(vm);
                    return Json(responseIdentify, JsonRequestBehavior.AllowGet);
            }
            return null;
        }
        public async Task<EnrollResponse> EnrollChild(FPCaptureRs vm)
        {
            EnrollResponse returnModel = new EnrollResponse();
            EnrollRequest request = new EnrollRequest();
            request.Id = Guid.NewGuid();
            request.FingerprintRecord = new DsdAfis.Core.FingerBiometrics.FingerprintRecord();
            request.FingerprintRecord.Id = 0;
            request.FingerprintRecord.Active = false;
            request.FingerprintRecord.DateTime = DateTime.Now;
            request.FingerprintRecord.FingerprintSet = new DsdAfis.Core.FingerBiometrics.FingerprintSet();
            request.FingerprintRecord.FingerprintSet.Dpi = 500;
            request.FingerprintRecord.FingerprintSet.ImageEncoding = "WSQ";
            request.FingerprintRecord.FingerprintSet.ImageHeight = 512;
            request.FingerprintRecord.FingerprintSet.ImageWidth = 512;
            List<Fingerprint> fingers = new List<Fingerprint>();
            Fingerprint blankFinger = null;
            fingers.Add(blankFinger);
            foreach (Finger f in vm.Fingers)
            {
                if(f.Sequence>0)
                {
                    Fingerprint finger = new Fingerprint();
                    switch (f.Name)
                    {
                        case "1":
                            finger.Code = FingerCode.RightThumb;
                            break;
                        case "6":
                            finger.Code = FingerCode.LeftThumb;
                            break;
                    }
                    finger.EncodedImage = f.Print;
                    fingers.Add(finger);
                }
                else
                {
                    fingers.Add(null);
                }
            }
            request.FingerprintRecord.FingerprintSet.Fingerprints = fingers.ToArray();
            request.FingerprintRecord.Person = new DsdAfis.Core.Person();
            request.FingerprintRecord.Person.Gender = DsdAfis.Core.PersonGender.Unknown;


            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["WebApiUrl"]);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.PostAsync(
                "api/afis/enroll", new StringContent(request.Json, Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    return MessageBase.Deserialize<EnrollResponse>(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    return returnModel;
                }
            }
        }
        public async Task<DisenrollResponse> DisEnrollChild(Guid uuid)
        {
            DisenrollResponse returnModel = new DisenrollResponse();
            DisenrollRequest request = new DisenrollRequest();

            request.Id = Guid.NewGuid();
            request.FingerprintRecord = new DsdAfis.Core.FingerBiometrics.FingerprintRecord();
            request.FingerprintRecord.Id = 0;
            request.FingerprintRecord.Active = false;
            request.FingerprintRecord.DateTime = DateTime.Now;
            request.FingerprintRecord.Uuid = uuid;

            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["WebApiUrl"]);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.PostAsync(
                "api/afis/disenroll", new StringContent(request.Json, Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                        return MessageBase.Deserialize<DisenrollResponse>(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    return returnModel;
                }
            }
        }
        public JsonResult VerifyChild(CYCAAdmissionViewModel vm)
        {
            return null;
        }
        public async Task<IdentifyResponse> IdentifyChild(FPCaptureRs vm)
        {
            IdentifyResponse returnModel = new IdentifyResponse();
            IdentifyRequest request = new IdentifyRequest();

            request.Id = Guid.NewGuid();
            request.FingerprintRecord = new DsdAfis.Core.FingerBiometrics.FingerprintRecord();
            request.FingerprintRecord.Id = 0;
            request.FingerprintRecord.Active = false;
            request.FingerprintRecord.DateTime = DateTime.Now;
            request.FingerprintRecord.FingerprintSet = new DsdAfis.Core.FingerBiometrics.FingerprintSet();
            request.FingerprintRecord.FingerprintSet.Dpi = 500;
            request.FingerprintRecord.FingerprintSet.ImageEncoding = "WSQ";
            request.FingerprintRecord.FingerprintSet.ImageHeight = 512;
            request.FingerprintRecord.FingerprintSet.ImageWidth = 512;
            List<Fingerprint> fingers = new List<Fingerprint>();
            Fingerprint blankFinger = null;
            fingers.Add(blankFinger);
            foreach (Finger f in vm.Fingers)
            {
                if (f.Sequence > 0)
                {
                    Fingerprint finger = new Fingerprint();
                    switch (f.Name)
                    {
                        case "1":
                            finger.Code = FingerCode.RightThumb;
                            break;
                        case "6":
                            finger.Code = FingerCode.LeftThumb;
                            break;
                    }
                    finger.EncodedImage = f.Print;
                    fingers.Add(finger);
                }
                else
                {
                    fingers.Add(null);
                }
            }
            request.FingerprintRecord.FingerprintSet.Fingerprints = fingers.ToArray();
           
            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["WebApiUrl"]);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.PostAsync(
                "api/afis/identify", new StringContent(request.Json, Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                        return MessageBase.Deserialize<IdentifyResponse>(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    return returnModel;
                }
            }
        }

        }
}