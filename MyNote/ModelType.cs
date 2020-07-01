using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNote
{
    class ModelType : ModelBase<Type>
    {
        public ModelType()
        {
            this.fileName = "Type.xml";
        }
    }

    public class Type
    {
        public string Name { set; get; }
        public List<string> Items;
        public Type() { this.Items = new List<string>(); }
        public Type(string Name) : this() { this.Name = Name; }
    }
}
