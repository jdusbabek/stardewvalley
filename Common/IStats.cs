using System.Reflection;

namespace StardewLib
{
    public interface IStats
    {
        FieldInfo[] getFieldList();
    }
}
