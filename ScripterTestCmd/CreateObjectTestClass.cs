using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScripterTestCmd
{
    public class CreateObjectTestClass
    {
        public string Name { get; set; }

        public string Age { get; set; }

        public CreateObjectTestClass(string name)
        {
            Name = name;
        }

        public CreateObjectTestClass()
        {
            
        }
    }
}
