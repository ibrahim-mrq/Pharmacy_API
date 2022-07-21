using System;

namespace Pharmacy.Models.Helper
{
    public class PagingDTO
    {
        private int rowCount = 10;

        public int RowCount { get => rowCount; set => rowCount = Math.Min(30, value); }
        public int PageNumber { get; set; } = 1;

    }
}
