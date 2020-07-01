using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNote
{
    public class TimetableItem : Category
    {
        public double Time { set; get; }
        public int Percent { set; get; }
        public bool UpdateFlag { set; get; }
        public new List<TimetableItem> Subs { set; get; }
        public TimetableItem(string Name) : base(Name) {
            this.Subs = new List<TimetableItem>();
        }
        public TimetableItem(Category Cty)
        {
            this.Name = Cty.Name;
            this.Subs = new List<TimetableItem>();
            this.CreateSubs(Cty);
        }

        public void CreateSubs(Category Cty)
        {
            foreach (var itr in Cty.Subs)
            {
                TimetableItem item = new TimetableItem(itr.Name);
                this.Subs.Add(item);
                item.CreateSubs(itr);
            }
        }

        public void Clear()
        {
            this.Time = 0;
            this.Percent = 0;
            foreach (var itr in Subs)
            {
                itr.Clear();
            }
        }

        public void Update(string cty, double time, double total)
        {
            if (this.Name == cty)
            {
                this.Time += time;
                this.Percent = Convert.ToInt32(this.Time / total * 100);
                UpdateFlag = true;
            }
            else
            {
                if (this.Subs.Count > 0)
                {
                    this.Time = 0;
                    foreach (var itr in this.Subs)
                    {
                        itr.Update(cty, time, total);
                        this.Time += itr.Time;
                        this.Percent = Convert.ToInt32(this.Time / total * 100);
                    }
                }
            }
        }

        public void GetTime(string cty, ref double time)
        {
            if (this.Name == cty)
            {
                time = this.Time;
            }
            else
            {
                if (this.Subs.Count > 0)
                {
                    foreach (var itr in this.Subs)
                    {
                        itr.GetTime(cty, ref time);
                    }
                }
            }
        }

        public string Print()
        {
            string str = this.Name;
            while (str.Length < 14) str += " ";
            str += this.Time.ToString();
            while (str.Length < 24) str += " ";
            str += this.Percent.ToString() + "%";

            return str;
        }
    }

    public class ModelTimetable
    {
        public DateTime StartTime { set; get; }
        public DateTime EndTime { set; get; }
        public TimetableItem Timetable { set; get; }
        public ModelTimetable()
        {
        }

        //Timetable Read
        public TimetableItem ReadTimetable(DateTime curDate, ModelCategory mCategory)
        {
            EndTime = DateTime.MaxValue;
            foreach (var itr in mCategory.Items)
            {
                if (itr.Name == "Timetable" && itr.Date > curDate)
                {
                    EndTime = itr.Date;
                }
                else if (itr.Name == "Timetable" && itr.Date <= curDate)
                {
                    Timetable = new TimetableItem(itr.Category);
                    StartTime = itr.Date;
                    break;
                }
            }
            if (Timetable == null)
            {
                Timetable = new TimetableItem("Timetable");
                //throw new Exception("No Timetable is found in \"Category.xml\".");
            }
            return Timetable;
        }
    }
}
