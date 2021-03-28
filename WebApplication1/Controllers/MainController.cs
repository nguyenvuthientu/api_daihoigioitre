using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClassLibrary1;

namespace WebApplication1.Controllers
{
    [EnableCors(origins:"*",headers:"*",methods:"*")]
    [Route("api/[controller]")]
    [ApiController]
    public class MainController : ControllerBase
    {
        string bucketName = Cresdential.bucketName;
        string awsAccessKey = Cresdential.awsAccessKey;
        string awsSecretKey = Cresdential.awsSecretKey;
        static int currentRequestIndex = 0;

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                IAmazonS3 client = new AmazonS3Client(awsAccessKey, awsSecretKey, RegionEndpoint.APSoutheast1);
                ListObjectsRequest request = new ListObjectsRequest
                {
                    BucketName = bucketName,
                    Prefix = ""
                };

                ListObjectsResponse response = client.ListObjects(request);

                List<string> keyLst = new List<string>();

                foreach (S3Object obj in response.S3Objects)
                {
                    keyLst.Add(obj.Key);
                }

                var currentUrl = "";
                currentRequestIndex += 1;
                if (currentRequestIndex > keyLst.Count)
                {
                    currentRequestIndex = 1;
                }

                currentUrl = keyLst[currentRequestIndex - 1];

                return Ok(currentUrl);
            }
            catch (Exception ex)
            {
                return BadRequest($"Lỗi: Không truy cập được dữ liệu. Lý do: {ex.Message}");
            }
        }
    }
}
