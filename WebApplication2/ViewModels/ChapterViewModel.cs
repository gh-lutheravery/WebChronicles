using WebChronicles.Models;

namespace WebChronicles.ViewModels
{
    public class ChapterViewModel
    {
        public required Story Story { get; set; }
        public required Chapter Chapter { get; set; }
        public required Chapter PreviousChapter { get; set; }
        public required Chapter NextChapter { get; set; }
    }
}
