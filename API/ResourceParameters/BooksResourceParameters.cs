namespace Fingers10.EnterpriseArchitecture.API.ResourceParameters
{
    /// <summary>
    /// Books Resource Parameters
    /// </summary>
    public class BooksResourceParameters
    {
        private const int _maxPageSize = 20;
        private int _pageSize = 10;

        /// <summary>
        /// Search Title
        /// </summary>
        public string SearchTitle { get; set; }

        /// <summary>
        /// Page Number
        /// </summary>
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// Page Size
        /// </summary>
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > _maxPageSize) ? _maxPageSize : value;
        }
    }
}
