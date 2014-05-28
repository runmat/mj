using System;
using System.Dynamic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using GeneralTools.Services;

namespace CkgDomainLogic.General.Models
{
    public class DynamicXml : DynamicObject
    {
        readonly XElement _root;
        private DynamicXml(XElement root)
        {
            _root = root;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = null;

            var att = _root.Attribute(binder.Name);
            if (att != null)
            {
                result = att.Value;
                return true;
            }

            var nodes = _root.Elements(binder.Name);
            if (nodes.Count() > 1)
            {
                result = nodes.Select(n => new DynamicXml(n)).ToList();
                return true;
            }

            var node = _root.Element(binder.Name);
            if (node != null)
            {
                if (node.HasElements)
                {
                    result = new DynamicXml(node);
                }
                else
                {
                    result = node.Value;
                }
                return true;
            }

            return true;
        }

        public static DynamicXml Parse(string xmlString)
        {
            return new DynamicXml(XDocument.Parse(xmlString).Root);
        }

        public static DynamicXml Load(string filename)
        {
            return new DynamicXml(XDocument.Load(filename).Root);
        }

        public static DynamicXml Convert(XmlNode[] xmlNodes)
        {
            if (xmlNodes == null)
                return null;

            var root = new XElement("root");
            for (var i = 0; i < xmlNodes.Length; i++)
            {
                var node = xmlNodes[i];
                root.Add(new XElement(node.Name, node.InnerXml));
            }
            return new DynamicXml(root);
        }

        public static dynamic GetDynamicObject(XmlNode[] xmlNodes)
        {
            if (xmlNodes == null)
                return null;

            var builder = DynamicTypeBuilder.CreateTypeBuilder("Dynamic Type Assembly", "Dynamic Type Module", "Dynamic Type from XML");
            for (var i = 0; i < xmlNodes.Length; i++)
            {
                var node = xmlNodes[i];
                DynamicTypeBuilder.CreateAutoImplementedProperty(builder, node.Name, typeof(string));
            }

            var type = builder.CreateType();
            var dynamicObject = Activator.CreateInstance(type);

            for (var i = 0; i < xmlNodes.Length; i++)
            {
                var node = xmlNodes[i];
                type.GetProperty(node.Name).SetValue(dynamicObject, node.InnerXml, null);
            }

            return dynamicObject;
        }
    }
}
