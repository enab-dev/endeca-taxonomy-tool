namespace EndecaDimensionAdapter
{
    public interface IEndecaDimensionXml
    {
        IDimension[] Dimensions { get; set; }
        string Version { get; set; }
    }

    public interface IDimension
    {
        IDimensionId DimId { get; set; }
        IDimensionNode DimNode { get; set; }
        string Name { get; set; }
        string SrcType { get; set; }
    }

    public interface IDimensionId
    {
        int Id { get; set; }
    }

    public interface IDimensionNode
    {
        IDval DimVal { get; set; }
        IDimensionNode[] DimNodes { get; set; }
    }

    public interface IDval
    {
        string Type { get; set; }
        IDvalId DimValId { get; set; }
        ISynonym[] Synonyms { get; set; }
        IProp[] Properties { get; set; }
    }

    public interface IDvalId
    {
        int Id { get; set; }
    }

    public interface ISynonym
    {
        string Classify { get; set; }
        string Display { get; set; }
        string Search { get; set; }
        string Value { get; set; }
    }

    public interface IProp
    {
        string Name { get; set; }
        string Pval { get; set; }
    }
}
