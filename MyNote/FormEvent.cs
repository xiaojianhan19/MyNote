using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace MyNote
{
    public partial class FormEvent : Form
    {
        FormDiary diaryForm;
        public FormEvent(FormDiary dForm)
        {
            diaryForm = dForm;
            InitializeComponent();
            InitializeModelEvent();
            InitializeCategory();
            InitializeTimetable();


            dateTimePicker1.Value = DateTime.Today;
            mEvent.RefFlag = false;
        }

        ModelEvent mEvent;
        List<ViewEventItem> viewEvList;
        List<ListItem> viewList;
        private void InitializeModelEvent()
        {
            mEvent = new ModelEvent();
            mEvent.Read();
            viewEvList = new List<ViewEventItem>();
            viewList = new List<ListItem>();
            BindListItem();
        }



        private void LoadCurEvent()
        {
            if (mEvent == null) return;
            DateTime curDate = dateTimePicker1.Value;
            DateTime start = new DateTime(curDate.Year, curDate.Month, 1);
            DateTime end = (new DateTime(curDate.AddMonths(1).Year, curDate.AddMonths(1).Month, 1)).AddDays(-1);

            int ret = mEvent.LoadSummary(start, end, viewEvList);

            viewEvList.Sort(new EventCategoryCompare());
            if (ret == 1)
                BindViewEvents(viewEvList, dataGridViewRefList);
        }

        private void BindViewEvents(List<ViewEventItem> evs, DataGridView DGV)
        {
            DGV.Columns.Clear();
            BindingSource bs = new BindingSource();
            bs.DataSource = evs;
            DGV.DataSource = bs;
            DGV.Columns["Name"].FillWeight = 200;
            DGV.Refresh();
        }

        //Category
        private ModelCategory mCategory;
        private void InitializeCategory()
        {
            mCategory = new ModelCategory();
            mCategory.Read();
        }

        //Timetable
        ModelTimetable mTb;
        TreeNode tn;
        private void InitializeTimetable()
        {
            mTb = new ModelTimetable();
            mTb.ReadTimetable(dateTimePicker1.Value, mCategory);
            tn = new TreeNode(mTb.Timetable.Print());
            tn.Name = mTb.Timetable.Name;
            foreach (var itr in mTb.Timetable.Subs)
            {
                LoadTimetableItemSubs(itr, tn);
            }
            treeView1.Nodes.Clear();
            treeView1.Nodes.Add(tn);
            treeView1.ExpandAll();
        }

        private void LoadCurTimetable()
        {
            if (true)
            {
                mTb.ReadTimetable(dateTimePicker1.Value, mCategory);
                tn = new TreeNode(mTb.Timetable.Print());
                tn.Name = mTb.Timetable.Name;
                foreach (var itr in mTb.Timetable.Subs)
                {
                    LoadTimetableItemSubs(itr, tn);
                }
                treeView1.Nodes.Clear();
                treeView1.Nodes.Add(tn);
                treeView1.ExpandAll();
            }
            mTb.Timetable.Clear();
            foreach (var ev in viewEvList)
            {
                mTb.Timetable.Update(ev.Category, ev.Time, (double)720);
            }
            //DisplayTimetable(mTb.Timetable, tn.Nodes[0]);
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



        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            LoadCurEvent();
            LoadCurTimetable();
        }

        private void FormEvent_FormClosed(object sender, FormClosedEventArgs e)
        {
            diaryForm.Close();
        }

        private void BindListItem()
        {
            dataGridViewList.Columns.Clear();
            BindingSource bs = new BindingSource();
            bs.DataSource = viewList;
            dataGridViewList.DataSource = bs;
            dataGridViewList.Refresh();
        }

        private void dataGridViewRefList_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                DataGridView DGV = (DataGridView)sender;
                if (DGV.SelectedRows.Count == 1)
                {
                    viewList.Clear();
                    string name = DGV.SelectedRows[0].Cells["Name"].Value.ToString();
                    string cty = "";
                    try {
                        cty = DGV.SelectedRows[0].Cells["Category"].Value.ToString();
                    } catch { }
                    foreach (var itr in mEvent.Items)
                    {
                        if (itr.Name == name)
                        {
                            if (String.IsNullOrEmpty(itr.Category) || itr.Category == cty)
                            {
                                
                                foreach (var item in itr.Items)
                                {
                                    viewList.Add(new ListItem(item.Date.ToShortDateString(), item.Time.ToString()));
                                }
                            }
                        }
                    }
                    BindListItem();
                }
            }
            catch { }
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (var vItem in viewEvList)
                {
                    foreach (var itr in mEvent.Items)
                    {
                        if (itr.Id == vItem.Id)
                        {
                            itr.Name = (String.IsNullOrEmpty(vItem.Name)) ? "Unkown" : vItem.Name;
                            itr.Category = (String.IsNullOrEmpty(vItem.Category)) ? "" : vItem.Category;
                            itr.Status = (String.IsNullOrEmpty(itr.Status)) ? "" : vItem.Status;
                            if (itr.Items == null) itr.Items = new List<EventItem>();
                            if (itr.RelatedPIds == null) itr.RelatedPIds = new List<string>();
                            if (itr.RelatedCIds == null) itr.RelatedCIds = new List<string>();
                            if (itr.RelatedFIds == null) itr.RelatedFIds = new List<string>();
                        }
                    }
                }

                for(int i=0; i < mEvent.Items.Count; i++)
                {
                    Event itr = mEvent.Items[i];
                    foreach (var itr2 in mEvent.Items)
                    {
                        if (itr.Id != itr2.Id)
                        {
                            if (itr.Name == itr2.Name && itr.Category == itr2.Category)
                            {
                                foreach(var item in itr2.Items)
                                {
                                    itr.Items.Add(new EventItem(item));
                                }
                                mEvent.Items.Remove(itr2);
                                break;
                            }
                        }
                    }
                    itr.Items.Sort();
                }

                mEvent.Save();

                MessageBox.Show("Update Successed.");
            }
            catch
            {
                MessageBox.Show("Update Failed.");
            }
        }

        private void dataGridViewRefList_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridView DGV = (DataGridView)sender;
                string name = DGV.Rows[e.RowIndex].Cells["Name"].Value.ToString();
                string cty = DGV.Rows[e.RowIndex].Cells["Category"].Value.ToString();
                foreach (var itr in viewEvList)
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (mEvent == null) return;
            DateTime start = dateTimePicker2.Value;
            DateTime end = dateTimePicker3.Value;

            int ret = mEvent.LoadSummary(start, end, viewEvList);

            viewEvList.Sort(new EventCategoryCompare());
            BindViewEvents(viewEvList, dataGridViewRefList);

            LoadCurTimetable();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridViewList.ReadOnly = false;
            LoadCurEvent();
            LoadCurTimetable();
        }

        private void dataGridViewRefList_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView DGV = (DataGridView)sender;
            int nColumnIndex = e.ColumnIndex;
            string column = DGV.Columns[e.ColumnIndex].Name.ToString();

            switch (column)
            {
                case "LastDate":
                    viewEvList.Sort();
                    break;
                case "Category":
                    viewEvList.Sort(new EventCategoryCompare());
                    break;
                default:
                    break;
            }
            BindViewEvents(viewEvList, dataGridViewRefList);

            //LoadCurTimetable();
        }



        private void button3_Click(object sender, EventArgs e)
        {
            Dictionary<string, List<EventItem>> data = new Dictionary<string, List<EventItem>>();

            List<string> titles = new List<string>(){"Learn", "Play" };
            foreach(var title in titles)
            {
                data[title] = new List<EventItem>();
            }

            if (mEvent == null) return;
            DateTime start = dateTimePicker2.Value;
            DateTime end = dateTimePicker3.Value;

            int ret = mEvent.LoadChart(start, end, mTb.Timetable, data);

            DisplayChart(data);
        }

        private void DisplayChart(Dictionary<string, List<EventItem>> data)
        {
            chart1.Series.Clear();
            chart1.ChartAreas.Clear();
            chart1.Titles.Clear();

            Title title1 = new Title("Title1");
            chart1.Titles.Add(title1);

            ChartArea area1 = new ChartArea();
            area1.AxisX.Title = "Title-XAxis";
            area1.AxisX.LabelStyle.Format = "yyyy-MM-dd";
            area1.AxisY.Title = "Title-YAxis";
            chart1.ChartAreas.Add(area1);

            // series
            Random rdm = new Random();
            foreach (var itr in data)
            {
                Series seriesLine = new Series();
                seriesLine.ChartType = SeriesChartType.Line;
                seriesLine.XValueType = ChartValueType.Time;
                seriesLine.YValueType = ChartValueType.Double;
                seriesLine.LegendText = itr.Key;
                seriesLine.BorderWidth = 2;
                int i = 0;
                foreach(var ev in itr.Value)
                {
                    seriesLine.Points.Add(new DataPoint(ev.Date.ToOADate(), ev.Time));
                }
                chart1.Series.Add(seriesLine);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Dictionary<string, List<EventItem>> data = new Dictionary<string, List<EventItem>>();

            List<string> titles = new List<string>() { "Learn", "Play" };
            foreach (var title in titles)
            {
                data[title] = new List<EventItem>();
            }

            if (mEvent == null) return;
            DateTime start = dateTimePicker2.Value;
            DateTime end = dateTimePicker3.Value;

            int ret = mEvent.LoadChart(start, end, mTb.Timetable, data, 2);

            DisplayChart(data);
        }
    }
}
