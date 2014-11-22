namespace NewsBoard.Utils
{
    /// <summary>
    /// This class helps constructing OData queries
    /// </summary>
    public class ODataQueryBuilder
    {
        /// <summary>
        /// Order options
        /// </summary>
        public enum Order
        {
            asc,
            desc
        }

        private string _baseUri;
        private string _expand;
        private string _filter;
        private Order _order;
        private string _orderby;


        private ODataQueryBuilder(string baseUri)
        {
            _baseUri = baseUri;
        }

        public static ODataQueryBuilder From(string baseUri)
        {
            return new ODataQueryBuilder(baseUri);
        }

        public ODataQueryBuilder Expand(string property)
        {
            _expand = property;
            return this;
        }

        public ODataQueryBuilder OrderBy(string property, Order order)
        {
            _orderby = property;
            _order = order;
            return this;
        }

        public ODataQueryBuilder FilterAnd(string predicate)
        {
            if (_filter == null) _filter = predicate;
            else
            {
                _filter += " and " + predicate;
            }

            return this;
        }

        public ODataQueryBuilder FilterAndEq(string property, string value)
        {
            string predicate = property + " eq \\'" + value + "\\'";
            return FilterAnd(predicate);
        }

        public string Build()
        {
            string build = _baseUri + '?';
            if (_expand != null) build += "$expand=" + _expand + "&";
            if (_orderby != null) build += "$orderby=" + _orderby + " " + _order + "&";
            if (_filter != null) build += "$filter=" + _filter;
            return build;
        }
    }
}