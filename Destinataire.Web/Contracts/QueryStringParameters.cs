using System.Collections.Generic;

namespace Destinataire.Web.Contracts
{
    public abstract class QueryStringParameters
    {
        public string Order { get; set; }
        public string Search { get; set; }

        private const int maxPageSize = 50;

        public int PageIndex { get; set; } = 0;
        private int _pageSize = 10;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = maxPageSize < value ? maxPageSize : value;
        }

        public virtual Dictionary<string, string> ToDictionary()
        {
            var dic =  new Dictionary<string, string>
            {
                {nameof(PageSize), PageSize.ToString()},
                {nameof(PageIndex), PageIndex.ToString()},
            };

            if (!string.IsNullOrWhiteSpace(Search))
            {
                dic.Add(nameof(Search), Search);
            }
            
            if (!string.IsNullOrWhiteSpace(Order))
            {
                dic.Add(nameof(Order), Order);
            }

            return dic;

        }
    }
}