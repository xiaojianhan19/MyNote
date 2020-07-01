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

        private void BindListToDGV(List<Object> list, DataGridView DGV)
        {
            DGV.Columns.Clear();
            BindingSource bs = new BindingSource();
            bs.DataSource = list;
            DGV.DataSource = bs;
            DGV.Columns["Name"].FillWeight = 200;
            DGV.Refresh();
        }

        //Event
        private ModelEvent mEvent;
        List<InputEventItem> curEvs;
        List<ViewEventItem> refEvs;
        private void InitializeModelEvent()
        {
            mEvent = new ModelEvent();
            mEvent.Read();
            curEvs = new List<InputEventItem>();
            refEvs = new List<ViewEventItem>();

        }

        private void BindViewEventItems(List<ViewEventItem> evs, DataGridView DGV)
        {
            DGV.Columns.Clear();
            BindingSource bs = new BindingSource();
            bs.DataSource = evs;
            DGV.DataSource = bs;
            DGV.Columns["Id"].Visible = false;
            DGV.Columns["LastDate"].Visible = false;
            DGV.Columns["Name"].FillWeight = 200;
            DGV.Refresh();
        }

        private void BindInputEventItems(List<InputEventItem> evs, DataGridView DGV)
        {
            DGV.Columns.Clear();
            BindingSource bs = new BindingSource();
            bs.DataSource = evs;
            DGV.DataSource = bs;
            DGV.Columns["Date"].Visible = false;
            DGV.Columns["Name"].FillWeight = 200;
            DGV.Columns["Memo"].FillWeight = 600;
            DGV.Refresh();
        }

        private void LoadCurEvent()
        {
            if (mEvent == null) return;
            int ret = mEvent.LoadCurEvent(dateTimePicker1.Value, curEvs, refEvs);

            BindInputEventItems(curEvs, dataGridViewEvent);
            if (ret == 1)
                BindViewEventItems(refEvs, dataGridView1);

        }

        private void AddSelecteEventsToListEvent(DataGridView SourceDGV, List<ViewEventItem> SourceList, DataGridView TargetDGV, List<InputEventItem> TargetList)
        {
            try
            {
                int n = SourceDGV.SelectedRows.Count - 1;
                for (int i = 0; i < SourceDGV.SelectedRows.Count; i++)
                {
                    string name = SourceDGV.SelectedRows[n - i].Cells["Name"].Value.ToString();
                    string cty = "";
                    try
                    {
                        cty = SourceDGV.SelectedRows[n - i].Cells["Category"].Value.ToString();
                    }
                    catch { }

                    if (String.IsNullOrEmpty(name))
                        continue;
                    foreach (var itr in SourceList)
                    {
                        if (itr.Name == name)
                        {
                            if (cty != "" && !String.IsNullOrEmpty(itr.Category) && itr.Category != cty)
                                continue;

                            bool exist = false;
                            foreach (var evt in TargetList)
                            {
                                if (evt.Name == itr.Name && evt.Category == itr.Category)
                                {
                                    exist = true;
                                    break;
                                }
                            }
                            if (!exist)
                            {
                                InputEventItem newEv = new InputEventItem(itr);
                                TargetList.Add(newEv);
                                //if (TargetDGV.Name == "dataGridViewCurResult")
                                LoadCurTimetable();
                            }
                            break;
                        }
                    }
                }
                BindInputEventItems(TargetList, TargetDGV);
            }
            catch { }
        }

        //Timetable
        ModelTimetable mTb;
        TreeNode tn;
        private void InitializeTimetable()
        {
            mTb = new ModelTimetable();
        }

        private void LoadCurTimetable()
        {
            DateTime date = dateTimePicker1.Value;
            if (date < mTb.StartTime || date >= mTb.EndTime)
            {
                mTb.ReadTimetable(date, mCategory);
                tn = new TreeNode(mTb.Timetable.Print());
                tn.Name = mTb.Timetable.Name;
                foreach (var itr in mTb.Timetable.Subs)
                {
                    LoadTimetableItemSubs(itr, tn);
                }
                treeView1.Nodes.Clear();
                treeView1.Nodes.Add(tn);
                treeView1.ExpandAll();

                ViewEventItem.InitCategoryCompare(mTb.Timetable);
            }

            mTb.Timetable.Clear();
            foreach (var ev in curEvs)
            {
                mTb.Timetable.Update(ev.Category, UltilityConvert.ToDoble(ev.Time), (double)24);
            }
            DisplayTimetable(mTb.Timetable, tn);
        }

        private void DisplayTimetable(TimetableItem item, TreeNode node)
        {
            if (item.Name == node.Name)
            {
                node.Text = item.Print();
            }
            foreach (var itr in item.Subs)
            {
                foreach (var n in node.Nodes)
                {
                    TreeNode child = ((TreeNode)n);
                    if (itr.Name == child.Name)
                        DisplayTimetable(itr, child);
                }
            }
        }

        private void LoadTimetableItemSubs(TimetableItem item, TreeNode node)
        {
            TreeNode n = new TreeNode(item.Print());
            n.Name = item.Name;
            node.Nodes.Add(n);
            foreach (var itr in item.Subs)
            {
                LoadTimetableItemSubs(itr, n);
            }
        }
    }
}