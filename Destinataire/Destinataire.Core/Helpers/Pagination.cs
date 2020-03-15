namespace Destinataire.Core.Helpers
{
    public class Pagination
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public int CurrentCount { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
    }
}