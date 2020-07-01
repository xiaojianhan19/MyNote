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
        //Collection
        ModelCollection mCollection;
        TreeNode tnCollection;
        TreeNode tnCollectionItem;
        private void InitializeCollection()
        {
            mCollection = new ModelCollection();
            mCollection.Read();
            tnCollectionItem = new TreeNode("Collection");

            InitializeType();
        }

        private void LoadcurCollection()
        {
            DateTime date = dateTimePicker1.Value;
            if (date < mCollection.StartTime || date >= mCollection.EndTime)
            {
                mCollection.ReadCategory(date, mCategory);
                tnCollection = new TreeNode(mCollection.Category.Name);
                tnCollection.Name = mCollection.Category.Name;
                foreach (var itr in mCollection.Category.Subs)
                {
                    LoadCategorySubs(itr, tnCollection);
                }
                treeViewCollection.Nodes.Clear();
                treeViewCollection.Nodes.Add(tnCollection);
                treeViewCollection.ExpandAll();
            }
        }


        private void curCollectionClear()
        {
            textBoxCId.Text = "";
            textBoxCName.Text = "";
            textBoxCMemo.Text = "";
            textBoxCName2.Text = "";
            textBoxCName3.Text = "";
            textBoxCLevel.Text = "";
            textBoxCStatus.Text = "";
            textBoxCInputDate.Text = "";
            textBoxCReleaseDate.Text = "";

        }
        private void curCollectionItemClear()
        {
            textBoxCItemId.Text = "";
            textBoxCItemIndex.Text = "";
            textBoxCItemTitle.Text = "";
            textBoxCItemMemo.Text = "";
            textBoxCItemName2.Text = "";
            textBoxCItemName3.Text = "";
        }
        private void UpdateRelatedCId(Collection newCollection)
        {
            bool updateFlg = false;
            foreach (var itr in mEvent.Items)
            {
                if (itr.Name.Contains(newCollection.Name))
                {
                    if (itr.RelatedCIds == null)
                        itr.RelatedCIds = new List<string>();
                    bool isFound = false;
                    foreach (var rId in itr.RelatedCIds)
                    {
                        if (rId == newCollection.Id)
                            isFound = true;
                    }
                    if (!isFound)
                    {
                        itr.RelatedCIds.Add(newCollection.Id);
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

        private void LoadcurCollection(string id)
        {
            foreach (var itr in mCollection.Items)
            {
                if (itr.Id == id)
                {
                    textBoxCId.Text = itr.Id;
                    textBoxCName.Text = itr.Name;
                    textBoxCCategory.Text = itr.Category;
                    textBoxCStatus.Text = itr.Status;
                    textBoxCMemo.Text = itr.Memo;
                    textBoxCName2.Text = itr.Name2;
                    textBoxCName3.Text = itr.Name3;
                    textBoxCLevel.Text = itr.Level.ToString();
                    textBoxCInputDate.Text = itr.InputDate.ToShortDateString();
                    textBoxCReleaseDate.Text = itr.ReleaseDate.ToShortDateString();
                }
            }
        }

        private void LoadcurCollectionItems(string node)
        {
            tnCollectionItem.Nodes.Clear();
            tnCollectionItem.Name = "";
            tnCollectionItem.Text = node;

            foreach (var itr in mCollection.Items)
            {
                if (itr.Category == node)
                {
                    string name = (!String.IsNullOrEmpty(itr.Name)) ? itr.Name : (!String.IsNullOrEmpty(itr.Name2)) ? itr.Name2 : itr.Name3;
                    TreeNode parent = new TreeNode(name);
                    parent.Name = itr.Id;
                    tnCollectionItem.Nodes.Add(parent);

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
                }
            }
            treeViewCollectionDetail.Nodes.Clear();
            treeViewCollectionDetail.Nodes.Add(tnCollectionItem);
            treeViewCollectionDetail.ExpandAll();
        }

        private void DisplayCollection(Collection itr)
        {
            textBoxCId.Text = itr.Id;
            textBoxCName.Text = itr.Name;
            textBoxCCategory.Text = itr.Category;
            textBoxCStatus.Text = itr.Status;
            textBoxCMemo.Text = itr.Memo;
            textBoxCName2.Text = itr.Name2;
            textBoxCName3.Text = itr.Name3;
            textBoxCLevel.Text = itr.Level.ToString();
            textBoxCInputDate.Text = itr.InputDate.ToShortDateString();
            textBoxCReleaseDate.Text = itr.ReleaseDate.ToShortDateString();

            for(int i= 0; i < checkedListBoxCTags.Items.Count; i++)
            {
                checkedListBoxCTags.SetItemCheckState(i, CheckState.Unchecked);
            }
            foreach (var tag in itr.Tags)
            {
                int i = 0;
                foreach (var obj in checkedListBoxCTags.Items)
                {
                    string col = obj.ToString();
                    if (col == tag)
                    {
                        checkedListBoxCTags.SetItemCheckState(i, CheckState.Checked);
                        break;
                    }
                    i++;
                }
            }
        }
        private void DisplayCollectionItem(CollectionItem item)
        {
            textBoxCItemId.Text = item.Id;
            textBoxCItemIndex.Text = item.Index.ToString();
            textBoxCItemTitle.Text = item.Name;
            textBoxCItemMemo.Text = item.Memo;
            if (item is Character)
            {

                textBoxCItemName2.Text = ((Character)item).Name2;
                textBoxCItemName3.Text = ((Character)item).Name3;
            }

        }

        //Type
        ModelType mType;
        Type tCollectionTags;
        public void InitializeType()
        {
            mType = new ModelType();
            mType.Read();
            foreach (var itr in mType.Items)
            {
                if(itr.Name == "CollectionTag")
                {
                    tCollectionTags = itr;
                    RefreshTags(tCollectionTags.Items, checkedListBoxCTags);
                }
            }
            if(tCollectionTags == null)
            {
                tCollectionTags = new Type("CollectionTag");
                mType.Items.Add(tCollectionTags);
            }
        }

        public void RefreshTags(List<string> tTags, CheckedListBox tListBox)
        {
            foreach (var itr in tTags)
            {
                if(!tListBox.Items.Contains(itr))
                {
                    tListBox.Items.Add(itr);
                }
            }
        }

    }
}