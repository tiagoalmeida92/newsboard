using System;
using System.Collections.Generic;

namespace NewsBoard.Model
{
    public class NewsCategory
    {
        public int Id { get; set; }

        public String Name { get; set; } //TODO Name

        public virtual ICollection<CategoryData> SubCategories { get; set; } //TODO remove
    }

    public class CategoryData
    {
        public CategoryData()
        {
        }

        public CategoryData(string name)
        {
            Name = name;
        }

        public int Id { get; set; }

        public string Name { get; private set; }
    }
}