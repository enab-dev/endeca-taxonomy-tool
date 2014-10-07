namespace EndecaDimensionAdapter
{
    public interface IEndecaDimensionFileAdapter
    {
        IEndecaDimensionXml GetEndecaDimensionsFromFile(string file);
        void WriteOutEndecaDimensionFile(string file, IEndecaDimensionXml dimensionFileXml);
    }
}
