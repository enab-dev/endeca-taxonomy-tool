using System;
using System.Xml.Serialization;

namespace EndecaDimensionAdapter
{
    [XmlRoot("DIMENSIONS")]
    public class EndecaDimensionXml : IEndecaDimensionXml
    {
        [XmlElement("DIMENSION")]
        public IDimension[] Dimensions { get; set; }
        [XmlAttribute("VERSION")]
        public string Version { get; set; }

        public override bool Equals(Object obj)
        {
            var endecaDimensionXml = (EndecaDimensionXml) obj;
            return ((endecaDimensionXml.Dimensions == null && Dimensions == null) ||
                    endecaDimensionXml.Dimensions.ScrambledEquals(Dimensions)) &&
                    ((string.IsNullOrEmpty(endecaDimensionXml.Version) && string.IsNullOrEmpty(Version)) ||
                   endecaDimensionXml.Version.Equals(Version));
        }
    }

    public class Dimension : IDimension
    {
        [XmlElement("DIMENSION_ID")]
        public IDimensionId DimId { get; set; }

        [XmlElement("DIMENSION_NODE")]
        public IDimensionNode DimNode { get; set; }
        [XmlAttribute("NAME")]
        public string Name { get; set; }

        [XmlAttribute("SRC_TYPE")]
        public string SrcType { get; set; }

        public override bool Equals(Object obj)
        {
            var dimension = (Dimension) obj;
            return ((dimension.DimId == null && DimId == null) || dimension.DimId.Equals(DimId)) &&
                   ((dimension.DimNode == null && DimNode == null) || dimension.DimNode.Equals(DimNode)) &&
                   ((string.IsNullOrEmpty(dimension.Name) && string.IsNullOrEmpty(Name)) || dimension.Name.Equals(Name)) &&
                   ((string.IsNullOrEmpty(dimension.SrcType) && string.IsNullOrEmpty(SrcType)) ||
                    dimension.SrcType.Equals(SrcType));
        }
    }

    public class DimensionId : IDimensionId
    {
        [XmlAttribute("ID")]
        public int Id { get; set; }

        public override bool Equals(Object obj)
        {
            var dimensionId = (DimensionId) obj;
            return dimensionId.Id == Id;
        }
    }

    public class DimensionNode : IDimensionNode
    {
        [XmlElement("DVAL")]
        public IDval DimVal { get; set; }
        [XmlElement("DIMENSION_NODE")]
        public IDimensionNode[] DimNodes { get; set; }

        public override bool Equals(Object obj)
        {
            var dimensionNode = (DimensionNode) obj;
            return ((dimensionNode.DimVal == null && DimVal == null) || dimensionNode.DimVal.Equals(DimVal)) &&
                   ((dimensionNode.DimNodes == null && DimNodes == null) ||
                    dimensionNode.DimNodes.ScrambledEquals(DimNodes));
        }
    }

    public class Dval : IDval
    {
        [XmlAttribute("TYPE")]
        public string Type { get; set; }
        [XmlElement("DVAL_ID")]
        public IDvalId DimValId { get; set; }
        [XmlElement("SYN")]
        public ISynonym[] Synonyms { get; set; }
        [XmlElement("PROP")]
        public IProp[] Properties { get; set; }

        public override bool Equals(Object obj)
        {
            var dval = (Dval) obj;
            return ((string.IsNullOrEmpty(dval.Type) && string.IsNullOrEmpty(Type)) || dval.Type.Equals(Type)) &&
                   ((dval.DimValId == null && DimValId == null) || dval.DimValId.Equals(DimValId)) &&
                   ((dval.Synonyms == null && Synonyms == null) || dval.Synonyms.ScrambledEquals(Synonyms)) &&
                   ((dval.Properties == null && Properties == null) || dval.Properties.ScrambledEquals(Properties));
        }
    }

    public class DvalId : IDvalId
    {
        [XmlAttribute("ID")]
        public int Id { get; set; }

        public override bool Equals(Object obj)
        {
            var dvalId = (DvalId) obj;
            return dvalId.Id == Id;
        }
    }

    public class Synonym : ISynonym
    {
        [XmlAttribute("CLASSIFY")]
        public string Classify { get; set; }
        [XmlAttribute("DISPLAY")]
        public string Display { get; set; }
        [XmlAttribute("SEARCH")]
        public string Search { get; set; }
        [XmlText]
        public string Value { get; set; }

        public override bool Equals(Object obj)
        {
            var synonym = (Synonym) obj;
            return ((string.IsNullOrEmpty(synonym.Classify) && string.IsNullOrEmpty(Classify)) || synonym.Classify.Equals(Classify)) &&
                   ((string.IsNullOrEmpty(synonym.Display) && string.IsNullOrEmpty(Display)) || synonym.Display.Equals(Display)) &&
                   ((string.IsNullOrEmpty(synonym.Search) && string.IsNullOrEmpty(Search)) || synonym.Search.Equals(Search)) &&
                   ((string.IsNullOrEmpty(synonym.Value) && string.IsNullOrEmpty(Value)) || synonym.Value.Equals(Value));
        }
    }

    public class Prop: IProp
    {
        [XmlAttribute("NAME")]
        public string Name { get; set; }
        [XmlElement("PVAL")]
        public string Pval { get; set; }

        public override bool Equals(Object obj)
        {
            var property = (Prop) obj;
            return ((string.IsNullOrEmpty(property.Name) && string.IsNullOrEmpty(Name)) || property.Name.Equals(Name)) && 
                   ((string.IsNullOrEmpty(property.Pval) && string.IsNullOrEmpty(Pval)) || property.Pval.Equals(Pval));
        }
    }
}
