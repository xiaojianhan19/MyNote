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
        //Fantasy
        ModelFantasy mFantasy;
        TreeNode tnFantasy;
        TreeNode tnFantasyItem;
        private void InitializeFantasy()
        {
            mFantasy = new ModelFantasy();
            mFantasy.Read();
            tnFantasyItem = new TreeNode("Fantasy");
        }

        private void LoadcurFantasy()
        {
            DateTime date = dateTimePicker1.Value;
            if (date < mFantasy.StartTime || date >= mFantasy.EndTime)
            {
                mFantasy.ReadCategory(date, mCategory);
                tnFantasy = new TreeNode(mFantasy.Category.Name);
                tnFantasy.Name = mFantasy.Category.Name;
                foreach (var itr in mFantasy.Category.Subs)
                {
                    LoadCategorySubs(itr, tnFantasy);
                }
                treeViewFantasy.Nodes.Clear();
                treeViewFantasy.Nodes.Add(tnFantasy);
                treeViewFantasy.ExpandAll();
            }
        }


        private void curFantasyClear()
        {
            textBoxFId.Text = "";
            textBoxFName.Text = "";
            textBoxFMemo.Text = "";
            textBoxFName2.Text = "";
            textBoxFName3.Text = "";
            textBoxFLevel.Text = "";


        }
        private void curFantasyItemClear()
        {
            textBoxFItemIndex.Text = "";
            textBoxFItemName.Text = "";
            textBoxFItemMemo.Text = "";
            textBoxFItemName2.Text = "";
            textBoxFItemName3.Text = "";

        }
        private void UpdateRelatedFId(Collection newFantasy)
        {
            bool updateFlg = false;
            foreach (var itr in mEvent.Items)
            {
                if (itr.Name.Contains(newFantasy.Name))
                {
                    if (itr.RelatedFIds == null)
                        itr.RelatedFIds = new List<string>();
                    bool isFound = false;
                    foreach (var rId in itr.RelatedFIds)
                    {
                        if (rId == newFantasy.Id)
                            isFound = true;
                    }
                    if (!isFound)
                    {
                        itr.RelatedFIds.Add(newFantasy.Id);
                        updateFlg = true;
                    }
                }
            }
            if (updateFlg)
            {
                mEvent.Save();
                LoadCurEvent();
            }
        }

        private void LoadcurFantasy(string node)
        {
            foreach (var itr in mFantasy.Items)
            {
                if (itr.Id == node)
                {
                    textBoxFId.Text = itr.Id;
                    textBoxFName.Text = itr.Name;
                    textBoxFCategory.Text = itr.Category;
                    textBoxFStatus.Text = itr.Status;
                    textBoxFMemo.Text = itr.Memo;
                    textBoxFName2.Text = itr.Name2;
                    textBoxFName3.Text = itr.Name3;
                    textBoxFLevel.Text = itr.Level.ToString();
                }
            }
        }

        private void LoadcurFantasyItems(string node)
        {
            tnFantasyItem.Nodes.Clear();
            tnFantasyItem.Name = "";
            tnFantasyItem.Text = node;

            foreach (var itr in mFantasy.Items)
            {
                if (itr.Category == node)
                {
                    string name = (!String.IsNullOrEmpty(itr.Name)) ? itr.Name : (!String.IsNullOrEmpty(itr.Name2)) ? itr.Name2 : itr.Name3;
                    TreeNode parent = new TreeNode(name);
                    parent.Name = itr.Id;
                    tnFantasyItem.Nodes.Add(parent);

                    if (itr.Paragraph.Count > 0)
                    {
                        TreeNode list1 = new TreeNode("Paragraph");
                        list1.Name = "Paragraph";
                        parent.Nodes.Add(list1);
                        foreach (var item in itr.Paragraph)
                        {
                            TreeNode tn = new TreeNode(item.Index.ToString() + " " + item.Name);
                            tn.Name = item.Id;
                            list1.Nodes.Add(tn);
                        }
                    }
                    if (itr.Character.Count > 0)
                    {
                        TreeNode list2 = new TreeNode("Character");
                        list2.Name = "Character";
                        parent.Nodes.Add(list2);
                        foreach (var item in itr.Character)
                        {
                            TreeNode tn = new TreeNode(item.Index.ToString() + " " + item.Name);
                            tn.Name = item.Id;
                            list2.Nodes.Add(tn);
                        }
                    }
                    if (itr.Setting.Count > 0)
                    {
                        TreeNode list3 = new TreeNode("Setting");
                        list3.Name = "Setting";
                        parent.Nodes.Add(list3);
                        foreach (var item in itr.Setting)
                        {
                            TreeNode tn = new TreeNode(item.Index.ToString() + " " + item.Name);
                            tn.Name = item.Id;
                            list3.Nodes.Add(tn);
                        }
                    }
                    break;
                }
            }
            treeViewFantasyDetail.Nodes.Clear();
            treeViewFantasyDetail.Nodes.Add(tnFantasyItem);
            treeViewFantasyDetail.ExpandAll();
        }

        private void DisplayFantasy(Collection itr)
        {
            textBoxFId.Text = itr.Id;
            textBoxFName.Text = itr.Name;
            textBoxFCategory.Text = itr.Category;
            textBoxFStatus.Text = itr.Status;
            textBoxFMemo.Text = itr.Memo;
            textBoxFName2.Text = itr.Name2;
            textBoxFName3.Text = itr.Name3;
            textBoxFLevel.Text = itr.Level.ToString();


            checkedListBoxFTags.ClearSelected();
            foreach (var tag in itr.Tags)
            {
                int i = 0;
                foreach (var obj in checkedListBoxFTags.Items)
                {
                    string col = obj.ToString();
                    if (col == tag)
                    {
                        checkedListBoxFTags.SetItemCheckState(i, CheckState.Checked);
                        break;
                    }
                    i++;
                }
            }
        }
        private void DisplayFantasyItem(CollectionItem item)
        {
            textBoxFItemId.Text = item.Id;
            textBoxFItemIndex.Text = item.Index.ToString();
            textBoxFItemName.Text = item.Name;
            textBoxFItemMemo.Text = item.Memo;
            if (item is Character)
            {

                textBoxFItemName2.Text = ((Character)item).Name2;
                textBoxFItemName3.Text = ((Character)item).Name3;
            }

        }
    }
}