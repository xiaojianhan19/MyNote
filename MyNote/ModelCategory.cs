using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNote
{
    public class CategoryItem
    {
        public string Name { set; get; }
        public string Parent { set; get; }
        public CategoryItem() { }
        public CategoryItem(string Name, string Parent)
        {
            this.Name = Name;
            this.Parent = Parent;
        }
        public CategoryItem(CategoryItem item)
        {
            this.Name = item.Name;
            this.Parent = item.Parent;
        }
    }

    public class Category
    {
        public string Name { set; get; }
        public List<Category> Subs { set; get; }
        public Category() { Subs = new List<Category>(); }
        public Category(string Name) : this() { this.Name = Name; }

        public Category(Category category)
        {
            this.Name = category.Name;
            if (this.Subs == null)
                this.Subs = new List<Category>();
            if (category.Subs == null) return;
            foreach (var itr in category.Subs)
            {
                this.Subs.Add(new Category(itr));
            }
        }

        //public void CopyCategorySubs(Category category)
        //{
        //    this.Name = category.Name;
        //    foreach (var itr in category.Subs)
        //    {
        //        CopyCategorySubs(itr);
        //    }
        //}
    }

    public class CategoryObject
    {
        public string Name;
        public DateTime Date;
        public Category Category;

        public CategoryObject() { }
        public CategoryObject(string Name, DateTime Date)
        {
            this.Name = Name;
            this.Date = Date;
            this.Category = new Category();
        }

        public CategoryObject(string Name, DateTime Date, string Parent, List<CategoryItem> items)
        {
            this.Name = Name;
            this.Date = Date;
            Category = new Category();
            Category.Name = Parent;

            List<CategoryItem> loopList = new List<CategoryItem>();
            foreach (var itr in items)
            {
                if(!String.IsNullOrEmpty(itr.Parent))
                    loopList.Add(new CategoryItem(itr));
            }
            for (int i= 0; i < loopList.Count; i++)
            {
                CategoryItem item = loopList[i];
                Category newItem = new Category(item.Name);
                bool ret = AddItems(newItem, item.Parent, Category);
                if (ret == false && i < 1000)
                {
                    loopList.Add(new CategoryItem(item));
                    i++;
                }
            }
        }

        public bool AddItems(Category newItem, String Parent, Category category)
        {
            if (category.Name == Parent)
            {
                category.Subs.Add(newItem);
                return true;
            }
            else
            {
                foreach (var itr in category.Subs)
                {
                    if (AddItems(newItem, Parent, itr))
                        return true;
                }
            }
            return false;
        }
    }

    public class ViewCategoryObject
    {
        public string Name { set; get; }
        public string Date { set; get; }
        public ViewCategoryObject() { }
        public ViewCategoryObject(string Name, string Date)
        {
            this.Name = Name;
            this.Date = Date;
        }
        public ViewCategoryObject(ViewCategoryObject obj)
        {
            this.Name = obj.Name;
            this.Date = obj.Date;
        }
        public ViewCategoryObject(string Name, DateTime Date)
        {
            this.Name = Name;
            this.Date = Date.ToShortDateString();
        }
    }

    public class ModelCategory : ModelBase<CategoryObject>
    {
        public ModelCategory()
        {
            fileName = "Category.xml";
        }

        public bool AddCategory(string name, DateTime date, string parent, List<CategoryItem> newCategoryItems)
        {
            if (name == "")
            {
                return false;
            }

            bool isFound = false;
            int index = 0;
            foreach (var itr in this.Items)
            {
                if (itr.Date > date)
                {
                    index++;
                }
                if (itr.Name == name && itr.Date == date)
                {
                    this.Items.Remove(itr);
                    isFound = true;
                    break;
                }
            }
            if (isFound)
            {
                return false;
            }
            else
            {
                this.Items.Insert(index, new CategoryObject(name, date, parent, newCategoryItems));
                this.Save();
                return true;
            }
        }

        public bool UpdateCategory(string name, DateTime date, string parent, List<CategoryItem> newCategoryItems)
        {
            if (name == "")
            {
                return false;
            }

            bool isFound = false;
            int index = 0;
            foreach (var itr in this.Items)
            {
                if (itr.Date > date)
                {
                    index++;
                }
                if (itr.Name == name && itr.Date == date)
                {
                    this.Items.Remove(itr);
                    isFound = true;
                    break;
                }
            }
            if (isFound)
            {
                this.Items.Insert(index, new CategoryObject(name, date, parent, newCategoryItems));
                this.Save();
                return true;
            }
            else
            {
                return false;
            }
        }

    }


}
