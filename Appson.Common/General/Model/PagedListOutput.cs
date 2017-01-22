using System.Collections.Generic;

namespace JahanJooy.Common.Util.ApiModel.Pagination
{
    public class PagedListOutput<T>
    {
        public List<T> PageItems { get; set; }
        public int PageNumber { get; set; }
        public int TotalNumberOfPages { get; set; }
        public int TotalNumberOfItems { get; set; }
    }
}