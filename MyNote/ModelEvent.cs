using System;
using System.Collections.Generic;

namespace MyNote
{   
    public class EventItem : BaseChild, IComparable<EventItem>
    {
        public EventItem() { }
        //public EventItem(string Name, string Category, string Memo, DateTime Date, double Time)
        //    : base(Name, Category, Memo, Date, Time)
        //{
        //}

        public EventItem(EventItem EvItm) : base(EvItm.Date, EvItm.Time, EvItm.Memo)
        {
        }

        public EventItem(DateTime date, double time, string memo) : base(date, time, memo)
        {
        }

        public EventItem(InputEventItem InEv, DateTime CurDate)
        {
            this.Date = CurDate;
            this.Time = UltilityConvert.ToDoble(InEv.Time);
            this.Memo = InEv.Memo;
        }

        //重写的CompareTo方法，根据Date排序
        public int CompareTo(EventItem other)
        {
            if (null == other)
            {
                return 1;//空值比较大，返回1
            }
            //return this.LastDate.CompareTo(other.LastDate);//Up
            return other.Date.CompareTo(this.Date);//Down
        }
    }

    public class Event : BaseParent
    {
        public static int LastId = 0;
        public List<EventItem> Items { get; set; }
        public List<string> RelatedPIds{ get; set; }
        public List<string> RelatedCIds { get; set; }
        public List<string> RelatedFIds { get; set; }
        public string Sort { get; set; }

        public Event()
        {
            if (Items == null) Items = new List<EventItem>();
            if (RelatedPIds == null) RelatedPIds = new List<string>();
            if (RelatedCIds == null) RelatedCIds = new List<string>();
            if (RelatedFIds == null) RelatedFIds = new List<string>();
        }
        public Event(string Name, string Category, string Status, string Memo) : base(Name, Category, Status, Memo)
        {
            if (Items == null) Items = new List<EventItem>();
            if (RelatedPIds == null) RelatedPIds = new List<string>();
            if (RelatedCIds == null) RelatedCIds = new List<string>();
            if (RelatedFIds == null) RelatedFIds = new List<string>();
        }

        public Event(Event Ev) : this(Ev.Name, Ev.Category, Ev.Status, Ev.Memo)
        {
            this.Sort = Ev.Sort;
            foreach(var child in Ev.Items)
            {
                this.Items.Add(new EventItem(child));
            }
        }

        public Event(InputEventItem InEv, DateTime CurDate) : this(InEv.Name, InEv.Category, InEv.Status, InEv.Memo)
        {
            if (Event.LastId != 0)
            {
                this.Id = "E" + String.Format("{0:D7}",++LastId);
            }

            DateTime date = CurDate;
            if (!String.IsNullOrEmpty(InEv.Date))
            {
                date = UltilityConvert.ToDate(InEv.Date);
            }
            Items.Add(new EventItem(InEv, date));
        }
    }

    public class InputEventItem : IComparable<InputEventItem>
    {
        public string Name { set; get; }
        public string Category { set; get; }
        public string Status { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Memo { get; set; }

        public InputEventItem() { }
        public InputEventItem(Event Ev)
        {
            this.Name = Ev.Name;
            this.Category = Ev.Category;
            this.Status = Ev.Status;
        }

        public InputEventItem(Event Ev, EventItem EvItm)
        {
            this.Name = Ev.Name;
            this.Category = Ev.Category;
            this.Status = Ev.Status;
            this.Date = EvItm.Date.ToShortDateString();
            this.Time = EvItm.Time.ToString();
            this.Memo = EvItm.Memo;
        }

        public InputEventItem(ViewEventItem EvItm)
        {
            this.Name = EvItm.Name;
            this.Category = EvItm.Category;
            this.Status = EvItm.Status;
        }

        public void AnalysisCategoryStr(string name, TimetableItem tb)
        {
            try
            {
                int n = Convert.ToInt32(name);
                List<int> list = new List<int>();
                while (n >= 10)
                {
                    list.Add(n % 10);
                    n = n / 10;
                }
                list.Add(n);
                list.Reverse();
                TimetableItem cur = tb;
                foreach (var i in list)
                {
                    cur = cur.Subs[i - 1];
                }
                this.Category = cur.Name;
            }
            catch { }
        }

        public void Translate()
        {
            //Status
            if (!String.IsNullOrEmpty(this.Status))
            {
                int statusCode = Convert.ToInt32(UltilityConvert.ToDoble(this.Status));
                switch (statusCode)
                {
                    case 0:
                        break;
                    case 1:
                        this.Status = "OnProcess";
                        break;
                    case 2:
                        this.Status = "Current";
                        break;
                    case 3:
                        this.Status = "Finished";
                        break;
                    case 4:
                        this.Status = "GiveUp";
                        break;
                    case 6:
                        this.Status = "LookBack";
                        break;
                    case 7:
                        this.Status = "Completed";
                        break;
                    case 8:
                        this.Status = "Extra";
                        break;
                    case 9:
                        this.Status = "Regular";
                        break;
                    default:
                        this.Status = statusCode.ToString() + "%";
                        break;
                }
            }

            //Time
            string s = this.Time;
            double time = 0.0;
            double cnt = 1;
            if(s.Length == 2)
            {
                cnt = UltilityConvert.ToDoble(s.Substring(0,1));
                s = s.Substring(1, 1);
            }
            if (s == "h" || s == "H")
            {
                time = 0.5 * cnt;
            }
            else if (s == "q" || s == "Q")
            {
                time = 0.25 * cnt;
            }
            else if (s == "t" || s == "T")
            {
                time = 0.1 * cnt;
            }
            if(time != 0.0)
            {
                this.Time = time.ToString();
            }
        }
        public int CompareTo(InputEventItem other)
        {

            if (null == other)
            {
                return 1;//空值比较大，返回1
            }

            InputEventItem x = this;
            InputEventItem y = other;
            if (x.Category == null && y.Category != null)
                return -1;
            else if (x.Category != null && y.Category == null)
                return 1;
            else if (x.Category == null && y.Category == null)
                return 0;

            if (x.Category != y.Category)
            {
                if (ViewEventItem.CategoryIndexes != null && ViewEventItem.CategoryIndexes.ContainsKey(x.Category) && ViewEventItem.CategoryIndexes.ContainsKey(y.Category))

                    return ViewEventItem.CategoryIndexes[x.Category].CompareTo(ViewEventItem.CategoryIndexes[y.Category]);
                else
                    return x.Category.CompareTo(y.Category);
            }
            else if (this.Status != null && this.Status != other.Status)
            {
                return this.Status.CompareTo(other.Status);//Up
            }
            else
                return this.Name.CompareTo(other.Name);//Up
        }
    }

    public class ViewEventItem : IComparable<ViewEventItem>
    {
        public static Dictionary<string, string> CategoryIndexes;

        public string Id { set; get; }
        public string Name { set; get; }
        public string Category { set; get; }
        public string Status { get; set; }
        public double Time { get; set; }
        public int Duration { get; set; }
        public DateTime LastDate { get; set; }
        public ViewEventItem() { }

        public ViewEventItem(Event Ev)
        {
            this.Id = Ev.Id;
            this.Name = Ev.Name;
            this.Category = Ev.Category;
            this.Status = Ev.Status;
            this.Duration = Ev.Items.Count;
            foreach (var child in Ev.Items)
            {
                this.Time += child.Time;
                if (child.Date > LastDate)
                    LastDate = child.Date;
            }
        }

        public ViewEventItem(Event Ev, EventItem Item)
        {
            this.Id = Ev.Id;
            this.Name = Ev.Name;
            this.Category = Ev.Category;
            this.Status = Ev.Status;
            this.Duration = 1;
            this.Time = Item.Time;
            this.LastDate = Item.Date;
        }

        public void AnalysisCategoryStr(string name, TimetableItem tb)
        {
            try
            {
                int n = Convert.ToInt32(name);
                List<int> list = new List<int>();
                while (n >= 10)
                {
                    list.Add(n % 10);
                    n = n / 10;
                }
                list.Add(n);
                list.Reverse();
                TimetableItem cur = tb;
                foreach (var i in list)
                {
                    cur = cur.Subs[i - 1];
                }
                this.Category = cur.Name;
            }
            catch { }
        }

        public void Translate()
        {
            //Status
            if (!String.IsNullOrEmpty(this.Status))
            {
                int statusCode = Convert.ToInt32(UltilityConvert.ToDoble(this.Status));
                switch (statusCode)
                {
                    case 0:
                        break;
                    case 1:
                        this.Status = "OnProcess";
                        break;
                    case 2:
                        this.Status = "Current";
                        break;
                    case 3:
                        this.Status = "Finished";
                        break;
                    case 4:
                        this.Status = "GiveUp";
                        break;
                    case 6:
                        this.Status = "LookBack";
                        break;
                    case 7:
                        this.Status = "Completed";
                        break;
                    case 8:
                        this.Status = "Extra";
                        break;
                    case 9:
                        this.Status = "Regular";
                        break;
                    default:
                        this.Status = statusCode.ToString() + "%";
                        break;
                }
            }
        }

        public static void InitCategoryCompare(TimetableItem category)
        {
            if (ViewEventItem.CategoryIndexes == null)
            {
                ViewEventItem.CategoryIndexes = new Dictionary<string, string>();
                //CategoryIndexes.Add(category.Name, "0");
                int i = 0;
                foreach (var sub in category.Subs)
                {
                    AddCategoryIndex(sub, (++i).ToString(), ViewEventItem.CategoryIndexes);
                }
            }
        }

        public static void AddCategoryIndex(TimetableItem category, string prex, Dictionary<string, string> CategoryIndexes)
        {
            int i = 0;
            foreach (var sub in category.Subs)
            {
                CategoryIndexes.Add(sub.Name, prex + (++i).ToString());
                AddCategoryIndex(sub, prex + i.ToString(), CategoryIndexes);
            }
        }


        //重写的CompareTo方法，根据Date排序
        public int CompareTo(ViewEventItem other)
        {
            //ViewEventItem x = this;
            //ViewEventItem y = other;
            //if (x.Category == null && y.Category != null)
            //    return -1;
            //else if (x.Category != null && y.Category == null)
            //    return 1;
            //else if (x.Category == null && y.Category == null)
            //    return 0;

            //if (x.Category != y.Category)
            //{
            //    if (ViewEventItem.CategoryIndexes != null && ViewEventItem.CategoryIndexes.ContainsKey(x.Category) && ViewEventItem.CategoryIndexes.ContainsKey(y.Category))

            //        return ViewEventItem.CategoryIndexes[x.Category].CompareTo(ViewEventItem.CategoryIndexes[y.Category]);
            //    else
            //        return x.Category.CompareTo(y.Category);
            //}
            //else
            //{
            //    return x.LastDate.CompareTo(y.LastDate);
            //}

            if (null == other)
            {
                return 1;//空值比较大，返回1
            }
            //return this.LastDate.CompareTo(other.LastDate);//Up
            return other.LastDate.CompareTo(this.LastDate);//Down
        }
    }

    public class ModelEvent : ModelBase<Event>
    {
        public bool RefFlag = true;
        public ModelEvent()
        {
            fileName = "Event.xml";
        }

        override public List<Event> Read()
        {
            base.Read();
            GetLastId();
            return this.Items;
        }

        public void Save(List<InputEventItem> CurEvs, DateTime CurDate)
        {
            foreach (var itr in CurEvs)
            {
                DateTime date;
                if (itr.Date == null)
                    date = CurDate;
                else
                    date = UltilityConvert.ToDate(itr.Date);
                double time = UltilityConvert.ToDoble(itr.Time);
                if (String.IsNullOrEmpty(itr.Status))
                    itr.Status = "Current";
                //event exist check
                bool isFound = false;
                bool mergeFlag = false;
                foreach (var ev in this.Items)
                {
                    if (ev.Name == itr.Name)
                    {
                        if (String.IsNullOrEmpty(ev.Category))
                        {
                            ev.Category = itr.Category;
                            mergeFlag = true;
                        }
                        if (ev.Category != itr.Category) continue;

                        isFound = true;
                        if (ev.Status != itr.Status)
                        {
                            ev.Status = itr.Status;
                        }
                        bool isFoundItem = false;
                        foreach (var item in ev.Items)
                        {
                            if (item.Date == date)
                            {
                                isFoundItem = true;
                                if (time == -1.0)
                                {
                                    ev.Items.Remove(item);
                                    break;
                                }
                                else
                                {
                                    item.Time = time;
                                    item.Memo = itr.Memo;
                                }
                            }
                        }
                        if (!isFoundItem && time != 0.0)
                        {
                            ev.Items.Add(new EventItem(itr, CurDate));
                            ev.Items.Sort();
                        }
                        else
                        {
                            if (ev.Items.Count == 0)
                            {
                                this.Items.Remove(ev);
                            }
                        }
                        break;
                    }
                }
                if (mergeFlag)
                {
                    Event first = null;
                    bool firstFlag = true;
                    foreach (var ev in this.Items)
                    {
                        if (ev.Name == itr.Name)
                        {
                            if (firstFlag)
                            {
                                first = ev;
                                firstFlag = false;
                            }
                            else
                            {
                                if (String.IsNullOrEmpty(ev.Category) || ev.Category == first.Category)
                                {
                                    foreach (var item in ev.Items)
                                    {
                                        first.Items.Add(new EventItem(item));
                                    }
                                    first.Items.Sort();
                                    Items.Remove(ev);
                                    break;
                                }
                            }
                        }
                    }
                }
                if (!isFound && time != 0.0)
                {
                    Event newEv = new Event(itr, CurDate);
                    this.Items.Add(newEv);
                }
            }
            this.Save();
        }

        public int LoadCurEvent(DateTime CurDate, List<InputEventItem> CurEvs, List<ViewEventItem> RefEvs)
        {
            //Event only
            CurEvs.Clear();
            foreach (var itr in this.Items)
            {
                foreach (var child in itr.Items)
                {
                    if (child.Date == CurDate)
                    {
                        CurEvs.Add(new InputEventItem(itr, child));
                    }
                }
            }

            DateTime dt = Convert.ToDateTime(DateTime.Today.AddDays(-3).ToShortDateString());
            if (CurDate > Convert.ToDateTime(DateTime.Today.AddDays(-3).ToShortDateString()))
            {
                foreach (var itr in this.Items)
                {
                    if (String.IsNullOrEmpty(itr.Status) || itr.Status == "Regular" || itr.Status == "OnProcess" || itr.Status == "LookBack" || itr.Status.Contains("%"))
                    {
                        bool isFound = false;
                        foreach (var item in CurEvs)
                        {
                            if (item.Name == itr.Name && item.Category == itr.Category)
                            {
                                isFound = true;
                                break;
                            }
                        }
                        if (!isFound)
                            CurEvs.Add(new InputEventItem(itr));
                    }
                }
            }
            CurEvs.Sort();

            //ReferenceList
            int refresh = 0;
            if (RefFlag)
            {
                RefFlag = false;
                refresh = 1;
                RefEvs.Clear();
                foreach (var itr in this.Items)
                {
                    RefEvs.Add(new ViewEventItem(itr));
                }
                RefEvs.Sort();
            }

            return refresh;
        }


        public int LoadSummary(DateTime start, DateTime end, List<ViewEventItem> viewEvList)
        {
            viewEvList.Clear();
            foreach (var Ev in this.Items)
            {
                foreach (var itr in Ev.Items)
                {
                    if (itr.Date < start || itr.Date > end)
                        continue;

                    bool isFound = false;
                    for (int i = 0; i < viewEvList.Count; i++)
                    {
                        if (Ev.Name == viewEvList[i].Name)
                        {
                            if (String.IsNullOrEmpty(Ev.Category) || Ev.Category == viewEvList[i].Category)
                            {
                                ViewEventItem tmp = viewEvList[i];
                                tmp.Duration++;
                                tmp.Time += itr.Time;
                                if (tmp.LastDate < itr.Date)
                                {
                                    tmp.LastDate = itr.Date;
                                }
                                isFound = true;
                                break;
                            }
                        }
                    }
                    if (!isFound)
                    {
                        viewEvList.Insert(0, new ViewEventItem(Ev, itr));
                    }
                }
            }
            viewEvList.Sort();
            return 1;
        }

        public void GetLastId()
        {
            foreach (var itr in Items)
            {
                if (!String.IsNullOrEmpty(itr.Id))
                {
                    string tmp = itr.Id.Substring(1);
                    int curId = 0;
                    try { curId = Convert.ToInt32(tmp); } catch { continue; }
                    if (curId > Event.LastId)
                        Event.LastId = curId;
                }
            }
        }

        public void UpdateId()
        {
            GetLastId();

            foreach (var itr in Items)
            {
                if (String.IsNullOrEmpty(itr.Id))
                {
                    Event.LastId++;
                    itr.Id = "E" + String.Format("{0:D7}", Event.LastId);
                }
            }
        }

        //It's a little complex, Give up
        //public void UpdateCollection()
        //{
        //    //GetLastId();

        //    //foreach (var itr in Items)
        //    //{
        //    //    string cty = itr.Category;
        //    //    //if(  cty)
                

        //    //}
        //}

        public int SearchRefEvent(string content, List<ViewEventItem> RefEvs)
        {
            //ReferenceList
            RefFlag = false;
            RefEvs.Clear();
            foreach (var itr in this.Items)
            {
                if(itr.Name.Contains(content))
                {
                    RefEvs.Add(new ViewEventItem(itr));
                }
            }
            RefEvs.Sort();

            return 1;
        }

        public int LoadChart(DateTime start, DateTime end, TimetableItem timetable, Dictionary<string, List<EventItem>> data, int mode = 0)
        {
            //viewEvList.Clear();
            timetable.Clear();
            for (DateTime date = start; date <= end; date = date.AddDays(1))
            {
                if(mode != 2 ) timetable.Clear();
                foreach (var Ev in this.Items)
                {
                    foreach (var itr in Ev.Items)
                    {
                        if (itr.Date == date)
                        {
                            timetable.Update(Ev.Category, itr.Time, (double)24);
                        }
                    }
                }

                foreach(var cat in data)
                {
                    double time = 0;
                    timetable.GetTime(cat.Key, ref time);
                    data[cat.Key].Add(new EventItem(date, time, ""));
                }
            }

            return 1;
        }


    }

    public class EventCategoryCompare : IComparer<ViewEventItem>
    {
        public int Compare(ViewEventItem x, ViewEventItem y)
        {

            if (x.Category == null && y.Category != null)
                return -1;
            else if (x.Category != null && y.Category == null)
                return 1;
            else if (x.Category == null && y.Category == null)
                return 0;

            if (x.Category != y.Category)
            {
                if(ViewEventItem.CategoryIndexes.ContainsKey(x.Category) && ViewEventItem.CategoryIndexes.ContainsKey(y.Category))

                    return ViewEventItem.CategoryIndexes[x.Category].CompareTo(ViewEventItem.CategoryIndexes[y.Category]);
                else
                    return x.Category.CompareTo(y.Category);
            }
            else
            {
                return x.LastDate.CompareTo(y.LastDate);
            }
        }
    }

    public class ImportEventItem : EventItem, IComparable<ImportEventItem>
    {
        public int parentId { get; set; }
        public ImportEventItem() { }
        public ImportEventItem(EventItem e, int parentId) : base(e.Date, e.Time, e.Memo)
        {
            this.parentId = parentId;
        }

        public int CompareTo(ImportEventItem other)
        {
            if (null == other)
            {
                return 1;//空值比较大，返回1
            }
            return this.Date.CompareTo(other.Date);//Up
        }
    }

}
