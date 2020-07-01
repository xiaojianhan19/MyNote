using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.CheckedListBox;

namespace MyNote
{
    public partial class FormDiary : Form
    {
        public FormDiary()
        {
            InitializeComponent();
            UltilityIO.AutoBackUp();
            InitializeModelEvent();

            InitialCategory();
            InitializeTimetable();
            InitializePerson();
            InitializeCollection();
            InitializeFantasy();
            dateTimePicker1.Value = Convert.ToDateTime(DateTime.Today.ToShortDateString());

            LoadCurEvent();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            LoadCurEvent();
            LoadCurTimetable();
            LoadCurPerson();
            LoadcurCollection();
            LoadcurFantasy();
        }

        // ------------------
        // Event Controller
        // ------------------
        private void buttonEventSave_Click(object sender, EventArgs e)
        {
            mEvent.Save(curEvs, dateTimePicker1.Value);
            LoadCurEvent();
        }

        private void buttonAddSelectRows_Click(object sender, EventArgs e)
        {
            AddSelecteEventsToListEvent(dataGridView1, refEvs, dataGridViewEvent, curEvs);
        }

        private void buttonEventView_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormEvent fEv = new FormEvent(this);
            fEv.Show();
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            mEvent.UpdateId();
            //mEvent.UpdateCollection();
            mEvent.Save();
        }

        private void buttonBackup_Click(object sender, EventArgs e)
        {
            UltilityIO.FullBackUp();
        }

        private void dataGridViewEvent_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridView DGV = (DataGridView)sender;
                string name = DGV.Rows[e.RowIndex].Cells["Name"].Value.ToString();
                string cty = DGV.Rows[e.RowIndex].Cells["Category"].Value.ToString();
                foreach (var itr in curEvs)
                {
                    if (itr.Name == name && itr.Category == cty)
                    {
                        itr.AnalysisCategoryStr(cty, mTb.Timetable);
                        itr.Translate();
                    }
                }
                LoadCurTimetable();
            }
            catch { }
        }

        // ------------------
        // Collection Controller
        // ------------------
        private void textBoxCCategory_Leave(object sender, EventArgs e)
        {
            try
            {
                TextBox textBox = (TextBox)sender;
                string name = textBox.Text;
                int n = Convert.ToInt32(name);
                List<int> list = new List<int>();
                while (n >= 10)
                {
                    list.Add(n % 10);
                    n = n / 10;
                }
                list.Add(n);
                list.Reverse();
                Category cur = mCollection.Category;
                foreach (var i in list)
                {
                    cur = cur.Subs[i - 1];
                }
                textBox.Text = cur.Name;
            }
            catch { }
        }

        private void textBoxCStatus_Leave(object sender, EventArgs e)
        {
            try
            {
                TextBox textBox = (TextBox)sender;
                string name = textBox.Text;
                int n = Convert.ToInt32(name);
                switch (n)
                {
                    case 1:
                        textBox.Text = "OnProcess";
                        break;
                    case 2:
                        textBox.Text = "Current";
                        break;
                    case 3:
                        textBox.Text = "Finished";
                        break;
                    case 4:
                        textBox.Text = "GiveUp";
                        break;
                    case 5:
                        textBox.Text = "Miss";
                        break;
                    case 6:
                        textBox.Text = "LookBack";
                        break;
                    case 7:
                        textBox.Text = "Complete";
                        break;
                    default:
                        textBox.Text = "Finished";
                        break;
                }
            }
            catch { }
        }

        private void treeViewCollection_AfterSelect(object sender, TreeViewEventArgs e)
        {            
            LoadcurCollectionItems(e.Node.Name);
        }
        private void treeViewCollectionDetail_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                string id = e.Node.Name;
                if (String.IsNullOrEmpty(id))
                    return;
                string type = id.Substring(0, 1);
                if (type == "C")
                {
                    foreach (var itr in mCollection.Items)
                    {
                        if (itr.Id == id)
                        {
                            DisplayCollection(itr);
                        }
                    }
                }
                else
                {
                    foreach (var itr in mCollection.Items)
                    {
                        foreach (var item in itr.Character)
                        {
                            if (item.Id == id)
                            {
                                DisplayCollection(itr);
                                DisplayCollectionItem(item);
                                radioButton1.Checked = true;
                            }
                        }
                        foreach (var item in itr.Paragraph)
                        {
                            if (item.Id == id)
                            {
                                DisplayCollection(itr);
                                DisplayCollectionItem(item);
                                radioButton2.Checked = true;
                            }
                        }
                        foreach (var item in itr.Setting)
                        {
                            if (item.Id == id)
                            {
                                DisplayCollection(itr);
                                DisplayCollectionItem(item);
                                radioButton3.Checked = true;
                            }
                        }
                    }
                }
            }
            catch { }
        }
      
        private void buttonCollectionClear_Click(object sender, EventArgs e)
        {
            curCollectionClear();
            textBoxCName.Focus();
        }

        private void buttonCollectionAdd_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> Tags = new List<string>();
                for (int i = 0; i < checkedListBoxCTags.Items.Count; i++)
                {
                    if (checkedListBoxCTags.GetItemChecked(i))
                    {
                        Tags.Add(checkedListBoxCTags.GetItemText(checkedListBoxCTags.Items[i]));
                    }
                }
                DateTime InputDate = UltilityConvert.ToDateMin(textBoxCInputDate.Text);
                DateTime ReleaseDate = UltilityConvert.ToDateMin(textBoxCReleaseDate.Text);
                Collection curCollection = mCollection.Add(textBoxCId.Text,
                            textBoxCName.Text,
                            textBoxCCategory.Text,
                            textBoxCStatus.Text,
                            textBoxCMemo.Text,
                            textBoxCName2.Text,
                            textBoxCName3.Text,
                            UltilityConvert.ToInt(textBoxCLevel.Text),
                            InputDate,
                            ReleaseDate,
                            Tags
                            );
                mCollection.Save();
                LoadcurCollectionItems(curCollection.Category);
                curCollectionClear();
                UpdateRelatedCId(curCollection);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void buttonCollectionUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> Tags = new List<string>();
                for (int i = 0; i < checkedListBoxCTags.Items.Count; i++)
                {
                    if (checkedListBoxCTags.GetItemChecked(i))
                    {
                        Tags.Add(checkedListBoxCTags.GetItemText(checkedListBoxCTags.Items[i]));
                    }
                }
                DateTime InputDate = UltilityConvert.ToDateMin(textBoxCInputDate.Text);
                DateTime ReleaseDate = UltilityConvert.ToDateMin(textBoxCReleaseDate.Text);
                if (InputDate == DateTime.MinValue)
                    InputDate = ReleaseDate;
                Collection curCollection = mCollection.Update(textBoxCId.Text,
                            textBoxCName.Text,
                            textBoxCCategory.Text,
                            textBoxCStatus.Text,
                            textBoxCMemo.Text,
                            textBoxCName2.Text,
                            textBoxCName3.Text,
                            UltilityConvert.ToInt(textBoxCLevel.Text),
                            InputDate,
                            ReleaseDate,
                            Tags
                            );
                mCollection.Save();
                LoadcurCollectionItems(curCollection.Category);
                LoadcurCollection(curCollection.Id);
                UpdateRelatedCId(curCollection);


            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void buttonCollectionDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if ((int)MessageBox.Show("Delete the Collection ?", "返回值 确定1 取消2", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != 1)
                    return;
                string category = textBoxCCategory.Text;
                mCollection.Delete(textBoxCId.Text);
                mCollection.Save();
                curCollectionClear();
                LoadcurCollectionItems(category);

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void buttonCItemAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string cty = textBoxCCategory.Text;
                string id = textBoxCId.Text;
                if (String.IsNullOrEmpty(id))
                    throw new Exception("Please select the Collecion first.");
                foreach(var itr in mCollection.Items)
                {
                    if(itr.Id == id)
                    {
                        CollectionItem curItem;
                        if (radioButton1.Checked)
                        {
                            Character newItem = new Character(textBoxCItemId.Text, UltilityConvert.ToInt(textBoxCItemIndex.Text),
                                textBoxCItemTitle.Text, textBoxCItemMemo.Text, textBoxCItemName2.Text, textBoxCItemName3.Text);
                            itr.Character.Add(newItem);
                            curItem = newItem;
                        }
                        else if (radioButton2.Checked)
                        {
                            CollectionItem newItem = new CollectionItem(textBoxCItemId.Text, UltilityConvert.ToInt(textBoxCItemIndex.Text),
                                textBoxCItemTitle.Text, textBoxCItemMemo.Text);
                            itr.Paragraph.Add(newItem);
                            curItem = newItem;
                        }
                        else
                        {
                            CollectionItem newItem = new CollectionItem(textBoxCItemId.Text, UltilityConvert.ToInt(textBoxCItemIndex.Text),
                                textBoxCItemTitle.Text, textBoxCItemMemo.Text);
                            itr.Setting.Add(newItem);
                            curItem = newItem;
                        }
                        mCollection.Save();
                        LoadcurCollectionItems(cty);
                        curCollectionItemClear();
                        break;
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void buttonCItemUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                string cty = textBoxCCategory.Text;
                string id = textBoxCId.Text;
                string itemId = textBoxCItemId.Text;
                if (String.IsNullOrEmpty(id) || String.IsNullOrEmpty(itemId))
                    throw new Exception("Please select the Collecion first.");
                bool exist = false;
                foreach (var itr in mCollection.Items)
                {
                    if (itr.Id == id)
                    {
                        foreach(var item in itr.Character)
                        {
                            if(item.Id == itemId)
                            {
                                item.Index = UltilityConvert.ToInt(textBoxCItemIndex.Text);
                                item.Name = textBoxCItemTitle.Text;
                                item.Memo = textBoxCItemMemo.Text;
                                item.Name2 = textBoxCItemName2.Text;
                                item.Name3 = textBoxCItemName3.Text;
                                exist = true;
                                break;
                            }
                        }
                        foreach (var item in itr.Paragraph)
                        {
                            if (item.Id == itemId)
                            {
                                item.Index = UltilityConvert.ToInt(textBoxCItemIndex.Text);
                                item.Name = textBoxCItemTitle.Text;
                                item.Memo = textBoxCItemMemo.Text;
                                exist = true;
                                break;
                            }
                        }
                        foreach (var item in itr.Setting)
                        {
                            if (item.Id == itemId)
                            {
                                item.Index = UltilityConvert.ToInt(textBoxCItemIndex.Text);
                                item.Name = textBoxCItemTitle.Text;
                                item.Memo = textBoxCItemMemo.Text;
                                exist = true;
                                break;
                            }
                        }
                        break;
                    }
                }
                if(exist)
                {
                    mCollection.Save();
                    LoadcurCollectionItems(cty);
                }
                else
                {
                    throw new Exception("Data not found.");
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void buttonCItemDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string cty = textBoxCCategory.Text;
                string id = textBoxCId.Text;
                string itemId = textBoxCItemId.Text;
                if (String.IsNullOrEmpty(id) || String.IsNullOrEmpty(itemId))
                    throw new Exception("Please select the Collecion first.");
                bool exist = false;
                foreach (var itr in mCollection.Items)
                {
                    if (itr.Id == id)
                    {
                        foreach (var item in itr.Character)
                        {
                            if (item.Id == itemId)
                            {
                                itr.Character.Remove(item);
                                exist = true;
                                break;
                            }
                        }
                        foreach (var item in itr.Paragraph)
                        {
                            if (item.Id == itemId)
                            {
                                itr.Paragraph.Remove(item);
                                exist = true;
                                break;
                            }
                        }
                        foreach (var item in itr.Setting)
                        {
                            if (item.Id == itemId)
                            {
                                itr.Setting.Remove(item);
                                exist = true;
                                break;
                            }
                        }
                        break;
                    }
                }
                if (exist)
                {
                    mCollection.Save();
                    LoadcurCollectionItems(cty);
                    curCollectionItemClear();
                }
                else
                {
                    throw new Exception("Data not found.");
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void buttonCItemClear_Click(object sender, EventArgs e)
        {
            curCollectionItemClear();
        }

        // ------------------
        // Fantasy Controller
        // ------------------
        private void buttonFAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string status = textBoxFStatus.Text;
                List<string> Tags = new List<string>();
                for (int i = 0; i < checkedListBoxFTags.Items.Count; i++)
                {
                    if (checkedListBoxFTags.GetItemChecked(i))
                    {
                        Tags.Add(checkedListBoxFTags.GetItemText(checkedListBoxFTags.Items[i]));
                    }
                }
                DateTime InputDate = UltilityConvert.ToDateMin(textBoxFInputDate.Text);
                DateTime ReleaseDate = UltilityConvert.ToDateMin(textBoxFReleaseDate.Text);
                Collection curFantasy = mFantasy.Add(textBoxFId.Text,
                            textBoxFName.Text,
                            textBoxFCategory.Text,
                            textBoxFStatus.Text,
                            textBoxFMemo.Text,
                            textBoxFName2.Text,
                            textBoxFName3.Text,
                            UltilityConvert.ToInt(textBoxFLevel.Text),
                            InputDate,
                            ReleaseDate,
                            Tags
                            );
                mFantasy.Save();
                LoadcurFantasyItems(curFantasy.Category);
                curFantasyClear();
                UpdateRelatedFId(curFantasy);
                textBoxFStatus.Text = status;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void buttonFUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                string status = textBoxFStatus.Text;
                List<string> Tags = new List<string>();
                for (int i = 0; i < checkedListBoxFTags.Items.Count; i++)
                {
                    if (checkedListBoxFTags.GetItemChecked(i))
                    {
                        Tags.Add(checkedListBoxFTags.GetItemText(checkedListBoxFTags.Items[i]));
                    }
                }
                DateTime InputDate = UltilityConvert.ToDateMin(textBoxFInputDate.Text);
                DateTime ReleaseDate = UltilityConvert.ToDateMin(textBoxFReleaseDate.Text);
                Collection curFantasy = mFantasy.Update(textBoxFId.Text,
                            textBoxFName.Text,
                            textBoxFCategory.Text,
                            textBoxFStatus.Text,
                            textBoxFMemo.Text,
                            textBoxFName2.Text,
                            textBoxFName3.Text,
                            UltilityConvert.ToInt(textBoxFLevel.Text),
                            InputDate,
                            ReleaseDate,
                            Tags
                            );
                mFantasy.Save();
                LoadcurFantasyItems(curFantasy.Category);
                curFantasyClear();
                LoadcurFantasy();
                UpdateRelatedFId(curFantasy);


            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void buttonFDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string category = textBoxFCategory.Text;
                mFantasy.Delete(textBoxFId.Text);
                mFantasy.Save();
                curFantasyClear();
                LoadcurFantasyItems(category);

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void buttonFItemAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string cty = textBoxFCategory.Text;
                string id = textBoxFId.Text;
                if (String.IsNullOrEmpty(id))
                    throw new Exception("Please select the Collecion first.");
                foreach (var itr in mFantasy.Items)
                {
                    if (itr.Id == id)
                    {
                        CollectionItem curItem;
                        if (radioButton4.Checked)
                        {
                            Character newItem = new Character(textBoxFItemId.Text, UltilityConvert.ToInt(textBoxFItemIndex.Text),
                                textBoxFItemName.Text, textBoxFItemMemo.Text, textBoxFItemName2.Text, textBoxFItemName3.Text);
                            itr.Character.Add(newItem);
                            curItem = newItem;
                        }
                        else if (radioButton5.Checked)
                        {
                            CollectionItem newItem = new CollectionItem(textBoxFItemId.Text, UltilityConvert.ToInt(textBoxFItemIndex.Text),
                                textBoxFItemName.Text, textBoxFItemMemo.Text);
                            itr.Paragraph.Add(newItem);
                            curItem = newItem;
                        }
                        else
                        {
                            CollectionItem newItem = new CollectionItem(textBoxFItemId.Text, UltilityConvert.ToInt(textBoxFItemIndex.Text),
                                textBoxFItemName.Text, textBoxFItemMemo.Text);
                            itr.Setting.Add(newItem);
                            curItem = newItem;
                        }
                        mFantasy.Save();
                        LoadcurFantasyItems(cty);
                        curFantasyItemClear();
                        break;
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void buttonFItemUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                string cty = textBoxFCategory.Text;
                string id = textBoxFId.Text;
                string itemId = textBoxFItemId.Text;
                if (String.IsNullOrEmpty(id) || String.IsNullOrEmpty(itemId))
                    throw new Exception("Please select the Collecion first.");
                bool exist = false;
                foreach (var itr in mFantasy.Items)
                {
                    if (itr.Id == id)
                    {
                        foreach (var item in itr.Character)
                        {
                            if (item.Id == itemId)
                            {
                                item.Index = UltilityConvert.ToInt(textBoxFItemIndex.Text);
                                item.Name = textBoxFItemName.Text;
                                item.Memo = textBoxFItemMemo.Text;
                                item.Name2 = textBoxFItemName2.Text;
                                item.Name3 = textBoxFItemName3.Text;
                                exist = true;
                                break;
                            }
                        }
                        foreach (var item in itr.Paragraph)
                        {
                            if (item.Id == itemId)
                            {
                                item.Index = UltilityConvert.ToInt(textBoxFItemIndex.Text);
                                item.Name = textBoxFItemName.Text;
                                item.Memo = textBoxFItemMemo.Text;
                                exist = true;
                                break;
                            }
                        }
                        foreach (var item in itr.Setting)
                        {
                            if (item.Id == itemId)
                            {
                                item.Index = UltilityConvert.ToInt(textBoxFItemIndex.Text);
                                item.Name = textBoxFItemName.Text;
                                item.Memo = textBoxFItemMemo.Text;
                                exist = true;
                                break;
                            }
                        }
                        break;
                    }
                }
                if (exist)
                {
                    mFantasy.Save();
                    LoadcurFantasyItems(cty);
                    curFantasyItemClear();
                }
                else
                {
                    throw new Exception("Data not found.");
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void buttonFItemDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string cty = textBoxFCategory.Text;
                string id = textBoxFId.Text;
                string itemId = textBoxFItemId.Text;
                if (String.IsNullOrEmpty(id) || String.IsNullOrEmpty(itemId))
                    throw new Exception("Please select the Collecion first.");
                bool exist = false;
                foreach (var itr in mFantasy.Items)
                {
                    if (itr.Id == id)
                    {
                        foreach (var item in itr.Character)
                        {
                            if (item.Id == itemId)
                            {
                                itr.Character.Remove(item);
                                exist = true;
                                break;
                            }
                        }
                        foreach (var item in itr.Paragraph)
                        {
                            if (item.Id == itemId)
                            {
                                itr.Paragraph.Remove(item);
                                exist = true;
                                break;
                            }
                        }
                        foreach (var item in itr.Setting)
                        {
                            if (item.Id == itemId)
                            {
                                itr.Setting.Remove(item);
                                exist = true;
                                break;
                            }
                        }
                        break;
                    }
                }
                if (exist)
                {
                    mFantasy.Save();
                    LoadcurFantasyItems(cty);
                    curFantasyItemClear();
                }
                else
                {
                    throw new Exception("Data not found.");
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void buttonFClear_Click(object sender, EventArgs e)
        {
            curFantasyClear();
            textBoxFName.Focus();
        }

        private void treeViewFantasy_AfterSelect(object sender, TreeViewEventArgs e)
        {
            LoadcurFantasy(e.Node.Name);
            LoadcurFantasyItems(e.Node.Name);
        }

        private void treeViewFantasyDetail_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                string id = e.Node.Name;
                if (String.IsNullOrEmpty(id))
                    return;
                string type = id.Substring(0, 1);
                if (type == "C")
                {
                    foreach (var itr in mFantasy.Items)
                    {
                        if (itr.Id == id)
                        {
                            DisplayFantasy(itr);
                        }
                    }
                }
                else
                {
                    foreach (var itr in mFantasy.Items)
                    {
                        foreach (var item in itr.Character)
                        {
                            if (item.Id == id)
                            {
                                DisplayFantasy(itr);
                                DisplayFantasyItem(item);
                                radioButton4.Checked = true;
                            }
                        }
                        foreach (var item in itr.Paragraph)
                        {
                            if (item.Id == id)
                            {
                                DisplayFantasy(itr);
                                DisplayFantasyItem(item);
                                radioButton5.Checked = true;
                            }
                        }
                        foreach (var item in itr.Setting)
                        {
                            if (item.Id == id)
                            {
                                DisplayFantasy(itr);
                                DisplayFantasyItem(item);
                                radioButton6.Checked = true;
                            }
                        }
                    }
                }
            }
            catch { }
        }

        private void textBoxFCategory_Leave(object sender, EventArgs e)
        {
            try
            {
                TextBox textBox = (TextBox)sender;
                string name = textBox.Text;
                int n = Convert.ToInt32(name);
                List<int> list = new List<int>();
                while (n >= 10)
                {
                    list.Add(n % 10);
                    n = n / 10;
                }
                list.Add(n);
                list.Reverse();
                Category cur = mFantasy.Category;
                foreach (var i in list)
                {
                    cur = cur.Subs[i - 1];
                }
                textBox.Text = cur.Name;
            }
            catch { }
        }

        private void textBoxFStatus_Leave(object sender, EventArgs e)
        {
            try
            {
                TextBox textBox = (TextBox)sender;
                string name = textBox.Text;
                int n = Convert.ToInt32(name);
                switch (n)
                {
                    case 1:
                        textBox.Text = "OnProcess";
                        break;
                    case 2:
                        textBox.Text = "Current";
                        break;
                    case 3:
                        textBox.Text = "Finished";
                        break;
                    case 4:
                        textBox.Text = "GiveUp";
                        break;
                    case 5:
                        textBox.Text = "Miss";
                        break;
                    case 6:
                        textBox.Text = "LookBack";
                        break;
                    case 7:
                        textBox.Text = "Complete";
                        break;
                    default:
                        textBox.Text = "Finished";
                        break;
                }
            }
            catch { }
        }


        // ------------------
        // Person Controller
        // ------------------
        private void buttonPersonAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string status = textBoxPersonStatus.Text;
                curPerson = mPerson.Add(textBoxPersonId.Text,
                            textBoxPersonName.Text,
                            textBoxPersonGroup.Text,
                            textBoxPersonStatus.Text,
                            textBoxPersonMemo.Text,
                            textBoxPersonName2.Text,
                            textBoxPersonName3.Text,
                            textBoxPersonAddress.Text,
                            dateTimePicker1.Value);
                mPerson.Save();
                LoadCurPersonGroup(curPerson.Category);
                CurPersonClear();
                UpdateRelatedPId(curPerson);
                textBoxPersonStatus.Text = status;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void buttonPersonUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                string status = textBoxPersonStatus.Text;

                curPerson = mPerson.Update(textBoxPersonId.Text,
                            textBoxPersonName.Text,
                            textBoxPersonGroup.Text,
                            textBoxPersonStatus.Text,
                            textBoxPersonMemo.Text,
                            textBoxPersonName2.Text,
                            textBoxPersonName3.Text,
                            textBoxPersonAddress.Text);
                mPerson.Save();
                LoadCurPersonGroup(curPerson.Category);
                CurPersonClear();
                LoadCurPerson();
                UpdateRelatedPId(curPerson);


            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void textBoxPCategory_Leave(object sender, EventArgs e)
        {
            try
            {
                TextBox textBox = (TextBox)sender;
                string name = textBox.Text;
                int n = Convert.ToInt32(name);
                List<int> list = new List<int>();
                while (n >= 10)
                {
                    list.Add(n % 10);
                    n = n / 10;
                }
                list.Add(n);
                list.Reverse();
                Category cur = mPerson.Category;
                foreach (var i in list)
                {
                    cur = cur.Subs[i - 1];
                }
                textBox.Text = cur.Name;
            }
            catch { }
        }

        private void textBoxPersonStatus_Leave(object sender, EventArgs e)
        {
            try
            {
                TextBox textBox = (TextBox)sender;
                string name = textBox.Text;
                int n = Convert.ToInt32(name);
                switch (n)
                {
                    case 1:
                        textBox.Text = "Close";
                        break;
                    case 2:
                        textBox.Text = "Far";
                        break;
                    case 3:
                        textBox.Text = "Miss";
                        break;
                    case 4:
                        textBox.Text = "Gone";
                        break;
                    default:
                        textBox.Text = "Far";
                        break;
                }
            }
            catch { }
        }

        private void treeViewPerson_AfterSelect(object sender, TreeViewEventArgs e)
        {
            LoadCurPersonGroup(e.Node.Name);
        }

        private void dataGridViewPersonGroup_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                DataGridView DGV = (DataGridView)sender;
                if (DGV.SelectedRows.Count == 1)
                {
                    string id = DGV.SelectedRows[0].Cells["Id"].Value.ToString();

                    foreach (var itr in mPerson.Items)
                    {
                        if (itr.Id == id)
                        {
                            textBoxPersonId.Text = itr.Id;
                            textBoxPersonName.Text = itr.Name;
                            textBoxPersonGroup.Text = itr.Category;
                            textBoxPersonStatus.Text = itr.Status;
                            textBoxPersonMemo.Text = itr.Memo;
                            textBoxPersonName2.Text = itr.Name2;
                            textBoxPersonName3.Text = itr.Name3;
                            textBoxPersonAddress.Text = itr.Address;
                        }
                    }
                }
            }
            catch { }
        }

        private void buttonPersonClear_Click(object sender, EventArgs e)
        {
            CurPersonClear();
            textBoxPersonName.Focus();
        }

        private void buttonPersonDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string category = textBoxPersonGroup.Text;
                mPerson.Delete(textBoxPersonId.Text);
                mPerson.Save();
                CurPersonClear();
                LoadCurPersonGroup(category);

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        // ------------------
        // Category Controller
        // ------------------
        private void buttonAddCategory_Click(object sender, EventArgs e)
        {
            if (mCategory.AddCategory(textBoxCategoryName.Text, dateTimePickerCategory.Value, textBoxCategoryParent.Text, newCategoryItems))
            {
                BindCategoryList();
            }
            else
            {
                MessageBox.Show("Category Data already exist.");
            }
        }

        private void buttonUpdateCategory_Click(object sender, EventArgs e)
        {
            if (mCategory.UpdateCategory(textBoxCategoryName.Text, dateTimePickerCategory.Value, textBoxCategoryParent.Text, newCategoryItems))
            {
                BindCategoryList();
            }
            else
            {
                MessageBox.Show("Category Data not found.");
            }
        }

        private void dataGridViewCategoryList_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                DataGridView DGV = (DataGridView)sender;
                if (DGV.SelectedRows.Count == 1)
                {
                    string name = DGV.SelectedRows[0].Cells["Name"].Value.ToString();
                    string date = DGV.SelectedRows[0].Cells["Date"].Value.ToString();
                    foreach (var itr in mCategory.Items)
                    {
                        if (itr.Name == name && itr.Date.ToShortDateString() == date)
                        {
                            dateTimePickerCategory.Value = itr.Date;
                            textBoxCategoryName.Text = itr.Name;
                            textBoxCategoryParent.Text = itr.Category.Name;

                            newCategoryItems.Clear();
                            foreach (var item in itr.Category.Subs)
                            {
                                AddCategoryItem(newCategoryItems, item, itr.Category.Name);
                            }
                            BindCategoryItem();

                            ShowCategory(itr.Category);
                            break;
                        }
                    }
                }
            }
            catch { }
        }

        private void buttonTagAdd_Click(object sender, EventArgs e)
        {
            string newTag = textBoxTagAdd.Text;
            if (!String.IsNullOrWhiteSpace(newTag))
            {
                if(!tCollectionTags.Items.Contains(newTag))
                {
                    tCollectionTags.Items.Add(newTag);
                }
            }
            mType.Save();
            RefreshTags(tCollectionTags.Items, checkedListBoxCTags);
        }

        private void buttonEventSearch_Click(object sender, EventArgs e)
        {
            if (mEvent == null) return;
            int ret = mEvent.SearchRefEvent(textBoxEventSearch.Text, refEvs);
            if (ret == 1)
                BindViewEventItems(refEvs, dataGridView1);
        }
    }
}
