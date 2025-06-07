using WebChronicles.Models;

namespace WebChronicles.ViewModels
{
    public class ChapterViewModel
    {
        public  Story Story { get; set; }
        public  Chapter Chapter { get; set; }
        public  int PreviousChapterId { get; set; }
        public  int NextChapterId { get; set; }
    }
}
