using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VetDesk.Repository
{
    public class ListFetchOptions
    {
        public string FilterText { get; set; } = null;
        public int PageNum { get; set; } = 1;
        public int RowsPerPage { get; set; } = 0;

        public static readonly ListFetchOptions DefaultOptions = new ListFetchOptions();
    }
}
