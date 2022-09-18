using Business.Abstract;
using Business.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete;
using Entities.Concrete;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using System.Reflection;
using WEBUI.Controllers;
using WEBUI.Models;

namespace Test_FileUpload
{
    public class UnitTest1
    {

        [Fact]
        public void Test_Home_Check()
        {
            var mockRepo = new Mock<IBufferedFileUploadService>();
            mockRepo.Setup(repo => repo.GetAll());
            var controller = new HomeController(null);
            var result = controller.Index(new MultipleFilesModel());
            var viewResult = Assert.IsType<ViewResult>(result);

            var model = Assert.IsAssignableFrom<MultipleFilesModel>(viewResult.ViewData.Model);
            Assert.NotNull(model);
        }

        [Fact]
        public void Test_Delete_All()
        {
            var mockRepo = new Mock<IBufferedFileUploadService>();
            mockRepo.Setup(repo => repo.DeleteAllItems());
            var controller = new HomeController(mockRepo.Object);
            var result = controller.DeleteAllItem();
          
            Assert.NotNull(result);
        }
        [Fact]
        public void Test_Delete_SingleFile()
        {
            var mockRepo = new Mock<IBufferedFileUploadService>();
            mockRepo.Setup(re => re.DeleteItem(1018));
            var controller = new HomeController(mockRepo.Object);
            var result = controller.DeleteItem(1018);
            Assert.NotNull(result);
        }
        [Fact]
        public void Test_Upload_File()
        {
            MultipleFilesModel multipleFilesModel = new MultipleFilesModel();
            var upload = new List<IFormFile>();
            var mockRepo = new Mock<IBufferedFileUploadService>();
            //"C:\Users\Mert Dervisoglu\Documents\GitHub\MutliFileUpload\MutliFileUpload\WEBUI\wwwroot\"
            ReadOnlySpan<char> appPath = Assembly.GetEntryAssembly().Location.Replace("Test_FileUpload", "WEBUI");
            var dir = Path.GetDirectoryName(appPath.Slice(0, appPath.IndexOf("bin\\")));
            string path = Path.Combine(dir.ToString(), $"wwwroot\\TestData", "P8212049.JPG");
            string pathRec = Path.Combine(dir.ToString(), "wwwroot\\Files", "P8212049.JPG");
            using var stream = new MemoryStream(File.ReadAllBytes(path).ToArray());
            var sss = path.Split(@"\\").Last();
            var formFile = new FormFile(stream, 0, stream.Length, "Files", "P8212049.JPG")
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpeg"
            };
            using (var streams = new FileStream(pathRec, FileMode.Create))
            {
                formFile.CopyTo(streams);
            }
            upload.Add(formFile);
            multipleFilesModel.Files= upload;
            
            mockRepo.Setup(repo => repo.UploadFile(upload));
            var controller = new HomeController(mockRepo.Object);
            var result = controller.MultiUpload(new MultipleFilesModel());
            //var viewResult = Assert.IsType<ViewResult>(result);

            var mocresult = mockRepo.Object.UploadFile(upload);
            //var model = Assert.IsAssignableFrom<MultipleFilesModel>(viewResult.ViewData.Model);
            Assert.Equal(mocresult.Id!=null?true:false, result!=null?true:false);
        }
    

    }
}