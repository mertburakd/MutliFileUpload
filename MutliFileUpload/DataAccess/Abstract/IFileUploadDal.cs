using Core.DataAccess;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IFileUploadDal : IEntityRepository<FileUpload>
    {
        void AddRenge(List<FileUpload> files);
        void RemoveRenge(List<FileUpload> files);
    }
}
