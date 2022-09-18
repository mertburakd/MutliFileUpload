using Business.Abstract;
using Business.Constants;
using Core.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class BufferedFileUploadManager : IBufferedFileUploadService
    {
        private readonly IFileUploadDal _fileUploadDal;

        public BufferedFileUploadManager(IFileUploadDal fileUploadDal)
        {
            _fileUploadDal = fileUploadDal;
        }

        public Core.Results.IResult DeleteAllItems()
        {
            var alldata = _fileUploadDal.GetList();
            if (alldata.Count() >0)
            {
                string fullpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files/");
                ClearFolder(fullpath);
                _fileUploadDal.RemoveRenge(alldata);
                return new SuccessResult(Messages.AllFilesDeleted);
            }

            return new ErrorResult(Messages.FileDeleteSelectError);
        }

        public Core.Results.IResult DeleteItem(int id)
        {
            var data = _fileUploadDal.Get(q => q.Id == id);
            if (data != null)
            {
                string fullpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files/" + data.FileName);
                if (System.IO.File.Exists(fullpath))
                {
                    System.IO.File.Delete(fullpath);
                }
                _fileUploadDal.Delete(data);
                return new SuccessResult(Messages.FileDeleted);
            }
            return new ErrorResult(Messages.FileDeleteSelectError);
        }

        public IDataResult<List<FileUpload>> GetAll()
        {
            return new SuccessDataResult<List<FileUpload>>(_fileUploadDal.GetList());
        }

        public IDataResult<FileUpload> GetOne(int id)
        {
            return new SuccessDataResult<FileUpload>(_fileUploadDal.GetOne());
        }

        public async Task<Core.Results.IResult> UploadFile(List<IFormFile> files)
        {
            var supportedTypes = new[] { "txt", "doc", "docx", "pdf", "xls", "xlsx" ,"jpg","png","jpeg", "TXT", "DOC", "DOCX", "PDF", "XLS", "XLSX", "JPG","PNG", "JPEG" };
            
            List<FileUpload> uploadFiles = new List<FileUpload>();

            if (files != null && files.Count() > 0)
            {
                foreach (var file in files.Where(q => q.Length < (5 * 1024 * 1024))) //Listed just less then 5MB files
                {
                    var fileExt = System.IO.Path.GetExtension(file.FileName).Substring(1);
                    if (supportedTypes.Contains(fileExt))
                    {
                        FileUpload uploadFile = new FileUpload();
                        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files");

                        //create folder if not exist
                        if (!Directory.Exists(path))
                            Directory.CreateDirectory(path);


                        string fileNameWithPath = Path.Combine(path, file.FileName);

                        using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                        uploadFile.FileSize = file.Length;
                        uploadFile.FileName = file.FileName;
                        uploadFile.UploadTime = DateTime.Now;
                        uploadFiles.Add(uploadFile);
                    }

                }
                if (uploadFiles.Count() > 0)
                {
                    _fileUploadDal.AddRenge(uploadFiles);
                }
                else
                {
                    return new ErrorResult(Messages.FileAddNoAnyFile);
                }
                return new SuccessResult(Messages.FileUploadSuccess);
            }
            return new ErrorResult(Messages.FileUploadError);
        }

        private void ClearFolder(string FolderName)
        {
            DirectoryInfo dir = new DirectoryInfo(FolderName);

            foreach (FileInfo fi in dir.GetFiles())
            {
                try
                {
                    fi.Delete();
                }
                catch (Exception) { }
            }

            foreach (DirectoryInfo di in dir.GetDirectories())
            {
                ClearFolder(di.FullName);
                try
                {
                    di.Delete();
                }
                catch (Exception) { }
            }
        }
    }
}
