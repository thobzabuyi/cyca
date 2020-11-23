using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using Json;
namespace Common_Objects.Models
{
   public class FileConverterModel
    {
        //  public void ConvertFileToBase64(baseImage img111)
        //  {
        //    string base64String = string.Empty;
        // Convert Image to Base64
        //   using (var img = System.Drawing.Image.FromFile(img111.Image)) // Image Path from File Upload Controller
        //    {
        //     using (var memStream = new MemoryStream())
        //     {
        //        img.Save(memStream, img.RawFormat);
        //        byte[] imageBytes = memStream.ToArray();

        // Convert byte[] to Base64 String
        //       base64String = Convert.ToBase64String(imageBytes);

        //   return base64String;
        //  }
        // }

        //        at View side
        //<img src="data:image/jpg;base64,@ViewBag.Image64" width="80" height="80"/>

        //  or

        //  Base64StringToImage(base64String);
        // Convert Base64 to Image 


        //  }

        public Image Base64StringToImage(string base64String)
        {
            byte[] imageBytes = Convert.FromBase64String(base64String);
            var memStream = new MemoryStream(imageBytes, 0, imageBytes.Length);

            memStream.Write(imageBytes, 0, imageBytes.Length);
            var image = System.Drawing.Image.FromStream(memStream);
            return image;
        }

        public void ConvertFilesToJson(string info)
        {
           // var son = new JsonParser();
          //  son.
          //  string output = JsonConvert.SerializeObject(jsonStr);
        }
    }
}
