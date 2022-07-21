using Pharmacy.Models.Helper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pharmacy.Models.ResponseDTO
{
    public class Response<T>
    {
        public Response(IQueryable<T> query, PagingDTO pagingDTO)
        {
            Pageing = new PageingDetails();

            Pageing.totalRows = query.Count();
            Pageing.totalPages = (int)Math.Ceiling((double)Pageing.totalRows / pagingDTO.RowCount);
            Pageing.curPage = pagingDTO.PageNumber;
            Pageing.hasNextPage = Pageing.curPage < Pageing.totalPages;
            Pageing.hasPrevPage = Pageing.curPage > 1;

            data = query.Skip((pagingDTO.PageNumber - 1) * pagingDTO.RowCount).Take(pagingDTO.RowCount).ToList();
        }

        public Boolean success { get; set; }
        public String message { get; set; } = "";
        public int code { get; set; } = 0;
        public List<T> data { get; set; }
        public PageingDetails Pageing { get; set; }

    }
}
