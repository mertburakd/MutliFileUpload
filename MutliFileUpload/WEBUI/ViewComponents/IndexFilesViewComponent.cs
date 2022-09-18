using Business.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace WEBUI.ViewComponents
{
    public class IndexFilesViewComponent : ViewComponent
    {
        private readonly IBufferedFileUploadService _bufferedFileUploadService;

        public IndexFilesViewComponent(IBufferedFileUploadService bufferedFileUploadService)
        {
            _bufferedFileUploadService = bufferedFileUploadService;
        }

        public ViewViewComponentResult Invoke()
        {
            var data = _bufferedFileUploadService.GetAll().Data;
            return View(data);
        }
    }
}
