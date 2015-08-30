using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalTest.Share.Core.Model
{

    public enum PatternType { Color, Image }

    public class Pattern
    {
        public PatternType PatternType { get; set; }
        public byte[] ImageBytes { get; set; }
        public Tuple<int,int,int,int> ColorARBG { get; set; }
    }
}
