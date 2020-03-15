using System;
using System.Collections.Generic;
using System.Text;

namespace Destinataire.Core.Helpers
{
    public class PagedList<T>
    {
        public Pagination Pagination { get; private set; } = new Pagination();
        public string Sorting { get; set; }

        internal PagedList()
        {
        }

        public PagedList(List<T> items, int totalCount, int pageIndex, int pageSize)
        {
            Pagination.CurrentCount = items.Count;
            Pagination.TotalCount = totalCount;
            Pagination.PageIndex = pageIndex;
            Pagination.PageSize = pageSize;
            Pagination.TotalPages = (int) Math.Ceiling(totalCount / (double) pageSize);


            Items.AddRange(items);
        }

        public List<T> Items { get; set; } = new List<T>();
    }
}