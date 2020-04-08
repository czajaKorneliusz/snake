using System.IO;
using System.Windows.Markup;
using System.Windows.Shapes;
using System.Xml;

namespace Snake.Utilities
{
    public static class DeepCloningTool<T>
    {
        internal static object DeepCopy(Rectangle element)
        {
            var xaml = XamlWriter.Save(element);
            var xamlString = new StringReader(xaml);
            var xmlTextReader = new XmlTextReader(xamlString);
            var deepCopyObject = (T) XamlReader.Load(xmlTextReader);
            return deepCopyObject;
        }
    }
}