using System;

namespace Pharmacy.Models.Helper
{
    public class PageingDetails
    {

        public int totalRows { get; set; }
        public int totalPages { get; set; }
        public int curPage { get; set; }
        public Boolean hasNextPage { get; set; }
        public Boolean hasPrevPage { get; set; }
        public String NextPageUrl { get; set; } = "";
        public String PrevPageUrl { get; set; } = "";
    }
}
