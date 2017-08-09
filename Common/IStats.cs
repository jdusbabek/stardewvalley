using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace StardewLib
{
    public interface IStats
    {
         FieldInfo[] getFieldList();
    }
}
