using Business.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using WEBUI.Models;

namespace WEBUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBufferedFileUploadService _bufferedFileUploadService;
        public HomeController( IBufferedFileUploadService bufferedFileUploadService)
        {
            _bufferedFileUploadService = bufferedFileUploadService;
        }

        public IActionResult Index(MultipleFilesModel model)
        {
            return View(model);
        }
        [HttpGet]
        public IActionResult DeleteItem(int id)
        {
            MultipleFilesModel model = new MultipleFilesModel();
           var result= _bufferedFileUploadService.DeleteItem(id);
            model.IsResponse = true;
            if (result != null)
            {
                model.IsSuccess = result.Success;
                model.Message = result.Message;
            }
            else
            {
                model.IsSuccess = true;
                model.Message = "File Deleted";
            }
            return RedirectToAction("Index", model);
        }     

        public IActionResult DeleteAllItem()
        {
            MultipleFilesModel model = new MultipleFilesModel();
           var result= _bufferedFileUploadService.DeleteAllItems();
            model.IsResponse = true;
            if (result != null)
            {
                model.IsSuccess = result.Success;
                model.Message = result.Message;
            }
            else
            {
                model.IsSuccess = true;
                model.Message = "Deleted All Files";
            }
           
            return RedirectToAction("Index", model);
        }

        [HttpPost]
        [RequestSizeLimit(1024 * 1024 * 1024)] //Total Max files size is 1 GB
        public IActionResult MultiUpload(MultipleFilesModel model)
        {
            if (model == null)
            {
                MultipleFilesModel multipleFilesModel = new MultipleFilesModel { IsSuccess = false, Message = "Invalid element!", IsResponse = false };
                return RedirectToAction("Index", multipleFilesModel);
            }
            model.IsResponse = true;


            var result = _bufferedFileUploadService.UploadFile(model.Files);
            if (result.AsyncState!=null)
            {
                model.IsSuccess = result.Result.Success;
                model.Message = result.Result.Message;
            }
            else
            {
                model.IsSuccess = true;
                model.Message = "Upload is Success";
            }
          

            return RedirectToAction("Index", model);
        }
        private Dictionary<string,string> GetMimeTypes()
        {
            var supportedTypes = new[] { "txt", "doc", "docx", "pdf", "xls", "xlsx", "jpg", "png", "jpeg", "TXT", "DOC", "DOCX", "PDF", "XLS", "XLSX", "JPG", "PNG", "JPEG" };

            return new Dictionary<string, string>
            {
                {".txt","text/plan" },
                {".pdf","application/pdf" },
                {".doc","application/vnd.ms-word"},
                {".docx","application/vnd.ms-word"},
                {".xls","application/vnd.ms-excel"},
                {".xlsx","application/vnd.openxmlformats-officedocument.spereadsheetml.sheet" },
                {".png","image/png" },
                {".jpg","image/jpeg" },
                {".jpeg","image/jpeg" },
                {".TXT","text/plan" },
                {".PDF","application/pdf" },
                {".DOC","application/vnd.ms-word"},
                {".DOCX","application/vnd.ms-word"},
                {".XLS","application/vnd.ms-excel"},
                {".XLSX","application/vnd.openxmlformats-officedocument.spereadsheetml.sheet" },
                {".PNG","image/png" },
                {".JPG","image/jpeg" },
                {".JPEG","image/jpeg" }
            };
        }
        public FileResult Download(string fileName)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files/") + fileName;
            byte[] bytes = System.IO.File.ReadAllBytes(path);
            return File(bytes, GetMimeTypes()["." + fileName.Split(".")[1]], fileName);
        }
    }
}