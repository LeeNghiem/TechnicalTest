using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalTest.Share.Core.Model;

namespace TechnicalTest.Share.Core
{
    public interface IPatternCache
    {
        Pattern GetColorPattern();
        Pattern GetImagePattern();
    }
}
