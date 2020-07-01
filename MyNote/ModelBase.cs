using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNote
{
    public class ModelBase<T>
    {
        public List<T> Items;
        protected string fileName = "BaseItem.xml";
        private XmlIO<List<T>> xmlIo = new XmlIO<List<T>>();

        public ModelBase() { }

        public virtual List<T> Read()
        {
            if (Items == null)
            {
                Items = new List<T>();
                Items = xmlIo.ReadFile(Items, fileName);
            }           
            return Items;
        }

        public virtual void Save()
        {
            if (Items.Count != 0)
            {
                xmlIo.SaveFile(Items, fileName);
            }
        }
    }


    //public class BaseItem
    //{
    //    public string Memo { set; get; }
    //    public BaseItem() { }
    //    public BaseItem(string Memo)
    //    {
    //        this.Memo = Memo;
    //    }
    //}

    public class BaseParent
    {
        public string Id { set; get; }
        public string Name { set; get; }
        public string Category { set; get; }
        public string Status { set; get; }
        public string Memo { set; get; }
        public BaseParent() {
            Id = "";
            Name = "";
            Category = "";
            Status = "";
            Memo = "";
        }
        public BaseParent(string Name, string Category, string Status, string Memo) 
        {
            this.Name = Name;
            this.Category = Category;
            this.Status = Status;
            this.Memo = Memo;
        }
        public BaseParent(string Id, string Name, string Category, string Status, string Memo)
        {
            this.Id = Id;
            this.Name = Name;
            this.Category = Category;
            this.Status = Status;
            this.Memo = Memo;
        }
    }

    public class BaseChild
    {
        public DateTime Date { set; get; }
        public double Time { set; get; }
        public string Memo { set; get; }
        public BaseChild() { }
        public BaseChild(DateTime Date, double Time, string Memo) 
        {
            this.Date = Date;
            this.Time = Time;
            this.Memo = Memo;
        }
    }

    public class ListItem
    {
        public string Name { set; get; }
        public string Value { set; get; }
        public ListItem() { }
        public ListItem(string Name, string Parent)
        {
            this.Name = Name;
            this.Value = Parent;
        }
        public ListItem(ListItem item)
        {
            this.Name = item.Name;
            this.Value = item.Value;
        }
    }

    public class BaseItem
    {
        public static int LastId;
        public string Id { set; get; }
        public int Index { set; get; }
        public string Name { set; get; }
        public string Memo { set; get; }
        public DateTime Date { set; get; }
        public BaseItem() { }
        public BaseItem(string Id, int Index, string Name, string Memo)
        {
            this.Id = Id;
            if (String.IsNullOrEmpty(Id))
            {
                this.Id = "I" + String.Format("{0:D7}", ++BaseItem.LastId);
            }          
            this.Index = Index;
            this.Name = Name;
            this.Memo = Memo;
            this.Date = UltilityDate.Today();
        }
    }
}
