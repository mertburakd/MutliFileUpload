using Core.Results;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IBufferedFileUploadService
    {
        Task<Core.Results.IResult> UploadFile(List<IFormFile> files);
        IDataResult<List<FileUpload>> GetAll();
        Core.Results.IResult DeleteItem(int item);
        Core.Results.IResult DeleteAllItems();
        IDataResult<FileUpload> GetOne(int id);
    }
}
