using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalTest.Share.Core.Model
{
    public class Circle : Shape
    {
        public int Radius { get { return this.Size / 2; } }
    }
}
