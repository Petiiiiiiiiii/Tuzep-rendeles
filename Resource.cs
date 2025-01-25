using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unalom
{
    internal class Resource
    {
        public string material { get; set; }
        public string unitOfMeasure { get; set; }
        public int price { get; set; }

        public Resource(string line)
        {
            var temporary = line.Split(';');
            this.material = temporary[0];
            this.unitOfMeasure = temporary[1];
            this.price = Convert.ToInt32(temporary[2]);
        }
        public override string ToString()
        {
            return $"{this.material} ({this.unitOfMeasure})";
        }
    }
}
