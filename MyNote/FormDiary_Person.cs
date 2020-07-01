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
        //Person
        ModelPerson mPerson;
        List<ViewPerson> curGroup;
        Person curPerson;
        TreeNode tnPerson;
        private void InitializePerson()
        {
            mPerson = new ModelPerson();
            mPerson.Read();
            curGroup = new List<ViewPerson>();
        }

        private void LoadCurPerson()
        {
            DateTime date = dateTimePicker1.Value;
            if (date < mPerson.StartTime || date >= mPerson.EndTime)
            {
                mPerson.ReadCategory(date, mCategory);
                tnPerson = new TreeNode(mPerson.Category.Name);
                tnPerson.Name = mPerson.Category.Name;
                foreach (var itr in mPerson.Category.Subs)
                {
                    LoadCategorySubs(itr, tnPerson);
                }
                treeViewPerson.Nodes.Clear();
                treeViewPerson.Nodes.Add(tnPerson);
                treeViewPerson.ExpandAll();
            }

            if (curPerson == null)
            {
                curPerson = new Person();
            }

            textBoxPersonId.Text = curPerson.Id;
            textBoxPersonName.Text = curPerson.Name;
            textBoxPersonGroup.Text = curPerson.Category;
            textBoxPersonStatus.Text = curPerson.Status;
            textBoxPersonMemo.Text = curPerson.Memo;
            textBoxPersonName2.Text = curPerson.Name2;
            textBoxPersonName3.Text = curPerson.Name3;
            textBoxPersonAddress.Text = curPerson.Address;
        }

        private void CurPersonClear()
        {
            textBoxPersonId.Text = "";
            textBoxPersonName.Text = "";
            textBoxPersonMemo.Text = "";
            textBoxPersonName2.Text = "";
            textBoxPersonName3.Text = "";
            textBoxPersonAddress.Text = "";
        }

        private void UpdateRelatedPId(Person newPerson)
        {
            bool updateFlg = false;
            foreach (var itr in mEvent.Items)
            {
                if (itr.Name.Contains(newPerson.Name))
                {
                    if (itr.RelatedPIds == null)
                        itr.RelatedPIds = new List<string>();
                    bool isFound = false;
                    foreach (var rId in itr.RelatedPIds)
                    {
                        if (rId == newPerson.Id)
                            isFound = true;
                    }
                    if (!isFound)
                    {
                        itr.RelatedPIds.Add(newPerson.Id);
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


        private void LoadCurPersonGroup(string node)
        {
            curGroup.Clear();
            foreach (var itr in mPerson.Items)
            {
                if (itr.Category == node)
                {
                    curGroup.Add(new ViewPerson(itr.Id, itr.Name, itr.Name2, itr.Name3));
                }
            }
            curGroup.Sort();

            BindPersonGroup();
        }

        private void BindPersonGroup()
        {
            dataGridViewPersonGroup.Columns.Clear();
            BindingSource bs = new BindingSource();
            bs.DataSource = curGroup;
            dataGridViewPersonGroup.DataSource = bs;
            dataGridViewPersonGroup.Columns["Id"].Visible = false;
            dataGridViewPersonGroup.Refresh();
        }

    }
}