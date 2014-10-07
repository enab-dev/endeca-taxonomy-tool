using Castle.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EndecaDimensionAdapter
{
    public class EndecaDimensionAdapter : IEndecaDimensionAdapter
    {
        private readonly IEndecaDimensionFileAdapter _fileReader;
        private readonly Dictionary<string, IEndecaDimensionXml> _endecaDimensionFilesDictionary;
        private readonly List<IDimension> _endecaDimensions;

        public ILogger Log { get; set; }

        public EndecaDimensionAdapter(IEndecaDimensionFileAdapter fileReader, IEnumerable<string> files)
        {
            if (files == null || !files.Any()) throw new ArgumentNullException("No file list provided to EndecaDimensionAdapter!!!");

            _fileReader = fileReader;
            _endecaDimensionFilesDictionary = files.ToDictionary(f => f, f => _fileReader.GetEndecaDimensionsFromFile(f));
            _endecaDimensions = _endecaDimensionFilesDictionary.Values.SelectMany(f => f.Dimensions).ToList();
        }

        public List<IEndecaDimensionXml> GetAllEndecaDimensionXmls()
        {
            return _endecaDimensionFilesDictionary.Values.ToList();
        }

        public List<IDimension> GetAllEndecaDimensions()
        {
            return _endecaDimensions;
        }

        public List<int> GetAllDimensionIdsUsed()
        {
            var idList = new HashSet<int>();

            foreach (var dim in _endecaDimensions)
            {
                idList.Add(dim.DimId.Id);
                idList.AddRange(ProcessDimensionNode(dim.DimNode));
            }

            return idList.ToList();
        }

        public void AddDimensionNodeToDimension(IDimensionNode dimensionNodeToAdd,
            int dimensionId, int parentDimValId)
        {
            var dimension = GetDimensionById(dimensionId);
            if (dimension == null) return;
            
            var parentNode = GetDimensionNodeById(dimension.DimNode, parentDimValId);
            if (parentNode == null) return;

            var nodes = parentNode.DimNodes == null ? new List<IDimensionNode>() : parentNode.DimNodes.ToList();
            nodes.Add(dimensionNodeToAdd);
            parentNode.DimNodes = nodes.ToArray();
        }

        public void RemoveDimensionNodeFromDimension(int dimensionId, int dimValIdToRemove)
        {
            var dimension = GetDimensionById(dimensionId);
            if (dimension == null) return;

            RemoveDimensionNodeById(dimension.DimNode, dimValIdToRemove);
        }

        public void SaveAllDimensionFiles()
        {
            foreach (var endecaDimensionFile in _endecaDimensionFilesDictionary)
            {
                var dimensionIdsInFile = endecaDimensionFile.Value.Dimensions.Select(d => d.DimId.Id);
                endecaDimensionFile.Value.Dimensions =
                    _endecaDimensions.Where(d => dimensionIdsInFile.Contains(d.DimId.Id)).ToArray();

                _fileReader.WriteOutEndecaDimensionFile(endecaDimensionFile.Key, endecaDimensionFile.Value);
            }
        }

        private IDimension GetDimensionById(int dimensionId)
        {
            return _endecaDimensions.FirstOrDefault(d => d.DimId.Id == dimensionId);
        }

        private static void RemoveDimensionNodeById(IDimensionNode node, int id)
        {
            if (node.DimNodes == null || node.DimNodes.Length == 0) return;

            var nodes = node.DimNodes.ToList();
            node.DimNodes = nodes.Where(n => n.DimVal.DimValId.Id != id).ToArray();
            foreach(var dimNode in node.DimNodes)
                RemoveDimensionNodeById(dimNode, id);
        }

        private static IDimensionNode GetDimensionNodeById(IDimensionNode node, int id)
        {
            if (node.DimVal.DimValId.Id == id) return node;

            if (node.DimNodes == null || node.DimNodes.Length == 0) return null;

            return
                node.DimNodes.Select(dimNode => GetDimensionNodeById(dimNode, id))
                    .FirstOrDefault(targetNode => targetNode != null);
        }

        private static IEnumerable<int> ProcessDimensionNode(IDimensionNode dimNode)
        {
            var idList = new HashSet<int> { dimNode.DimVal.DimValId.Id };

            if (dimNode.DimNodes == null) return idList;

            foreach (var node in dimNode.DimNodes.Where(node => node != null))
            {
                idList.AddRange(ProcessDimensionNode(node));
            }

            return idList;
        }
    }
}
