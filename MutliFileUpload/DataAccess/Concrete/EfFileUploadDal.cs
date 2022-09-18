using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramwork;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete
{
    public class EfFileUploadDal : EfEntityRepositoryBase<FileUpload, MultiFileContext>, IFileUploadDal
    {
        public void AddRenge(List<FileUpload> files)
        {
            using (var context=new MultiFileContext())
            {
                context.AddRange(files);
                context.SaveChanges();
            }
        }

        public void RemoveRenge(List<FileUpload> files)
        {
            using (var context = new MultiFileContext())
            {
                context.RemoveRange(files);
                context.SaveChanges();
            }
        }
    }
}
