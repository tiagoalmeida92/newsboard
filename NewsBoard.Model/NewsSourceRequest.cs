namespace NewsBoard.Model
{
    public class NewsSourceRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string RssUrl { get; set; }
        public string Requester { get; set; }
    }
}