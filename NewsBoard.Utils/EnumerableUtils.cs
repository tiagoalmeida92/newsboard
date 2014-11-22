using System.Collections.Generic;
using System.Linq;

namespace NewsBoard.Utils
{
    public static class EnumerableUtils
    {

        /// <summary>
        /// Extension method to split a list into chunks
        /// </summary>
        /// <returns>A list of lists in which each list has the size of the parameter</returns>
        public static IEnumerable<IEnumerable<T>> Partition<T>(this IEnumerable<T> items, int partitionSize)
        {
            return items.Select((item, inx) => new {item, inx})
                .GroupBy(x => x.inx/partitionSize)
                .Select(g => g.Select(x => x.item));
        }
    }
}