using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNote
{
    public class ModelFantasy : ModelBase<Collection>
    {
        public DateTime StartTime { set; get; }
        public DateTime EndTime { set; get; }
        public Category Category { set; get; }
        public ModelFantasy()
        {
            fileName = "Fantasy.xml";
        }

        public override List<Collection> Read()
        {
            base.Read();
            GetLastId();
            return this.Items;
        }

        public Category ReadCategory(DateTime curDate, ModelCategory mCategory)
        {
            EndTime = DateTime.MaxValue;
            foreach (var itr in mCategory.Items)
            {
                if (itr.Name == "Fantasy" && itr.Date > curDate)
                {
                    EndTime = itr.Date;
                }
                else if (itr.Name == "Fantasy" && itr.Date <= curDate)
                {
                    Category = new Category(itr.Category);
                    StartTime = itr.Date;
                    break;
                }
            }
            if (Category == null)
            {
                Category = new Category("Fantasy");
                //throw new Exception("No Timetable is found in \"Category.xml\".");
            }
            return Category;
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
                    if (curId > Collection.LastId)
                        Collection.LastId = curId;
                }
                foreach (var item in itr.Paragraph)
                {
                    if (!String.IsNullOrEmpty(item.Id))
                    {
                        string tmp = item.Id.Substring(1);
                        int curId = 0;
                        try { curId = Convert.ToInt32(tmp); } catch { continue; }
                        if (curId > CollectionItem.LastId)
                            CollectionItem.LastId = curId;
                    }
                }
                foreach (var item in itr.Character)
                {
                    if (!String.IsNullOrEmpty(item.Id))
                    {
                        string tmp = item.Id.Substring(1);
                        int curId = 0;
                        try { curId = Convert.ToInt32(tmp); } catch { continue; }
                        if (curId > CollectionItem.LastId)
                            CollectionItem.LastId = curId;
                    }
                }
                foreach (var item in itr.Setting)
                {
                    if (!String.IsNullOrEmpty(item.Id))
                    {
                        string tmp = item.Id.Substring(1);
                        int curId = 0;
                        try { curId = Convert.ToInt32(tmp); } catch { continue; }
                        if (curId > CollectionItem.LastId)
                            CollectionItem.LastId = curId;
                    }
                }
            }
        }


        public Collection Add(string Id, string Name, string Category, string Status, string Memo,
            string Name2, string Name3, int Level, DateTime InputeDate, DateTime ReleaseDate, List<string> Tags)
        {
            Collection nP = new Collection(Id, Name, Category, Status, Memo, Name2, Name3, Level, InputeDate, ReleaseDate, Tags);
            if (String.IsNullOrEmpty(nP.Id))
            {
                nP.Id = "C" + String.Format("{0:D7}", ++Collection.LastId);
            }
            if (String.IsNullOrEmpty(nP.Status))
            {
                nP.Status = "Far";
            }
            if (String.IsNullOrEmpty(nP.Memo)) Memo = "";

            if (String.IsNullOrEmpty(nP.Id) || String.IsNullOrEmpty(nP.Category)
                || (String.IsNullOrEmpty(nP.Name) && String.IsNullOrEmpty(nP.Name2) && String.IsNullOrEmpty(nP.Name3)))
            {
                throw new Exception("Column required.");
            }
            foreach (var itr in this.Items)
            {
                if (nP.Id == itr.Id || (nP.Category == itr.Category && (nP.Name == itr.Name || nP.Name2 == itr.Name2 || nP.Name3 == itr.Name3)))
                    throw new Exception("Item already exist.");
            }
            this.Items.Add(nP);
            return nP;
        }

        public Collection Update(string Id, string Name, string Category, string Status, string Memo,
            string Name2, string Name3, int Level, DateTime InputDate, DateTime ReleaseDate, List<string> Tags)
        {
            Collection nP = new Collection();
            bool exist = false;
            foreach (var itr in this.Items)
            {
                if (Id == itr.Id)
                {
                    exist = true;
                    nP = itr;
                    break;
                }
            }
            if (!exist)
                throw new Exception("Data not found.");

            nP.Id = Id;
            nP.Name = Name;
            nP.Category = Category;
            nP.Status = Status;
            nP.Memo = Memo;
            nP.Name2 = Name2;
            nP.Name3 = Name3;
            nP.Level = Level;
            nP.InputDate = InputDate;
            nP.ReleaseDate = ReleaseDate;
            nP.Tags.Clear();
            foreach (var tag in Tags)
            {
                nP.Tags.Add(tag);
            }
            return nP;
        }

        public void Delete(string id)
        {
            bool exist = false;
            foreach (var itr in this.Items)
            {
                if (id == itr.Id)
                {
                    exist = true;
                    this.Items.Remove(itr);
                    break;
                }
            }
            if (!exist)
                throw new Exception("Data not found.");
        }
    }

    public class ViewFantasy : IComparable<ViewFantasy>
    {
        public string Id { set; get; }
        public string Name { set; get; }
        public string Name2 { set; get; }
        public string Name3 { set; get; }

        public ViewFantasy() { }
        public ViewFantasy(string id, string name, string name2, string name3)
        {
            this.Id = id;
            this.Name = name;
            this.Name2 = name2;
            this.Name3 = name3;
        }

        public int CompareTo(ViewFantasy other)
        {
            if (null == other)
            {
                return 1;//空值比较大，返回1
            }
            return this.Id.CompareTo(other.Id);//Up
            //return other.LastDate.CompareTo(this.LastDate);//Down
        }
    }


}
