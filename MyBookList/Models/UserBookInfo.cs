namespace MyBookList.Models
{
    public class UserBookInfo
    {
        public string BookId { get; set; }
        public string Status { get; set; }
        public int PagesRead { get; set; }
        public int ReReadCount { get; set; }
    }
}