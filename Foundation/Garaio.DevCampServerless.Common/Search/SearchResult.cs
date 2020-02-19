using System;
using System.Collections.Generic;
using System.Text;

namespace Garaio.DevCampServerless.Common.Search
{
    public class SearchResult
    {
        public SearchResultType Type { get; set; }

        /// <summary>
        /// Either entity id or intend. Currently known intends are: "ListPersons", "ListProjects" and "ListTechnologies"
        /// </summary>
        public string ResultKey { get; set; }
        public string ResultText { get; set; }
    }
}
