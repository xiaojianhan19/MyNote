using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyNote
{
    public partial class FormDiary : Form
    {
        //Category Page
        private ModelCategory mCategory;
        private List<CategoryItem> newCategoryItems;
        private List<ViewCategoryObject> hisCategoryList;
        private void InitialCategory()
        {
            mCategory = new ModelCategory();
            mCategory.Read();
            hisCategoryList = new List<ViewCategoryObject>();
            BindCategoryList();
            newCategoryItems = new List<CategoryItem>();
            BindCategoryItem();

            dateTimePickerCategory.Value = DateTime.Today;
        }

        private void BindCategoryList()
        {
            hisCategoryList.Clear();
            foreach (var itr in mCategory.Items)
            {
                hisCategoryList.Add(new ViewCategoryObject(itr.Name, itr.Date.ToShortDateString()));
            }
            dataGridViewCategoryList.Columns.Clear();
            BindingSource bs = new BindingSource();
            bs.DataSource = hisCategoryList;
            dataGridViewCategoryList.DataSource = bs;
            dataGridViewCategoryList.Refresh();
        }

        private void BindCategoryItem()
        {
            dataGridViewCategory.Columns.Clear();
            BindingSource bs = new BindingSource();
            bs.DataSource = newCategoryItems;
            dataGridViewCategory.DataSource = bs;
            dataGridViewCategory.Refresh();
        }

        private void AddCategoryItem(List<CategoryItem> list, Category category, string parent)
        {
            newCategoryItems.Add(new CategoryItem(category.Name, parent));
            foreach (var itr in category.Subs)
            {
                AddCategoryItem(list, itr, category.Name);
            }
        }

        TreeNode cty;
        public void ShowCategory(Category category)
        {
            cty = new TreeNode(category.Name);
            cty.Name = category.Name;
            foreach (var itr in category.Subs)
            {
                LoadCategorySubs(itr, cty);
            }

            treeViewCategory.Nodes.Clear();
            treeViewCategory.Nodes.Add(cty);
            treeViewCategory.ExpandAll();

        }

        private void LoadCategorySubs(Category item, TreeNode node)
        {
            TreeNode n = new TreeNode(item.Name);
            n.Name = item.Name;
            node.Nodes.Add(n);
            foreach (var itr in item.Subs)
            {
                LoadCategorySubs(itr, n);
            }
        }
    }
}