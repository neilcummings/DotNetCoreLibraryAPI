namespace WebApi.Helpers
{
    public class AuthorsResourceParameters
    {
        const int MaxPageSize = 20;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 10;

        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        }

        public string Genre { get; set; }
        public string SearchQuery { get; set; }
    }
}