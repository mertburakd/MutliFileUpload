using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WEBUI.Models
{
    //[RequestSizeLimit(5 * 1024 * 1024)]
    public class MultipleFilesModel : ReponseModel
    {
        [Required(ErrorMessage = "Please select files")]
        //[SizeValidator(sizeInBytes: 5 * 1024 * 1024,ErrorMessage = "Image filesize should be smaller than 5 MB")]
        public List<IFormFile> Files { get; set; }
    }
}
