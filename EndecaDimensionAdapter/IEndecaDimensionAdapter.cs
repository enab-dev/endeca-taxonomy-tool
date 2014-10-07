using System.Collections.Generic;

namespace EndecaDimensionAdapter
{
    public interface IEndecaDimensionAdapter
    {
        List<IDimension> GetAllEndecaDimensions();
        List<int> GetAllDimensionIdsUsed();
        void AddDimensionNodeToDimension(IDimensionNode dimensionNodeToAdd, int dimensionId, int parentDimValIdToAdd);
        void RemoveDimensionNodeFromDimension(int dimensionId, int dimValIdToRemove);
        void SaveAllDimensionFiles();
    }
}
