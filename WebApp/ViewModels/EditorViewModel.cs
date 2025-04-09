using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels
{
    public class EditorViewModel
    {
        [Display(Name = "Title", Prompt = "Title")]
        public string Title { get; set; } = null!;

        [Display(Name = "Description", Prompt = "description")]

        public string RichTextContent { get; set; } = null!;
    }
}
