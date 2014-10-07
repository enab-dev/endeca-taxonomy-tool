using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EndecaDimensionAdapter.Tests
{
    [TestClass]
    public class EndecaDimensionAdapterTests
    {
        private static List<EndecaDimensionProps> _mockEndecaDimensionProps; 
        private static List<IEndecaDimensionXml> _mockEndecaDimensionFiles;
        private static List<IDimension> _mockEndecaDimensions;
        private static List<IDimensionNode> _mockEndecaDimensionNodes; 
        private static Mock<IEndecaDimensionFileAdapter> _mockEndecaDimensionFileAdapter;
        private static Dictionary<string, IEndecaDimensionXml> _mockSavedFiles;
        
        [TestInitialize]
        public void SetUpMockEndecaDimensions()
        {
            InitializeMockEndecaDimensionProps();
            InitializeMockEndecaDimensionNodes();
            InitializeMockDimensions();
            InitializeMockDimensionFiles();
            InitializeMockDimensionFileAdapter();

            _mockSavedFiles = new Dictionary<string, IEndecaDimensionXml>();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IfFileListIsNullAnExceptionIsThrown()
        {
            var adapter = new EndecaDimensionAdapter(_mockEndecaDimensionFileAdapter.Object, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IfFileListIsEmptyAnExceptionIsThrown()
        {
            var adapter = new EndecaDimensionAdapter(_mockEndecaDimensionFileAdapter.Object, new List<string>());
        }

        [TestMethod]
        public void CanGetEndecaDimensions()
        {
            var expected = _mockEndecaDimensions;
            var actual = new EndecaDimensionAdapter(_mockEndecaDimensionFileAdapter.Object, new[] { "1", "2" }).GetAllEndecaDimensions();

            Assert.AreEqual(expected.Count, actual.Count);

            for (var i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(expected[i].DimId.Id, actual[i].DimId.Id);
                Assert.AreEqual(expected[i].DimNode.DimVal.Properties[0].Pval, actual[i].DimNode.DimVal.Properties[0].Pval);
                Assert.AreEqual(expected[i].DimNode.DimVal.Synonyms[0].Value, actual[i].DimNode.DimVal.Synonyms[0].Value);
                Assert.AreEqual(expected[i].DimNode.DimNodes == null, actual[i].DimNode.DimNodes == null);
                
                if (expected[i].DimNode.DimNodes == null) continue;
                
                Assert.AreEqual(expected[i].DimNode.DimNodes.Length, actual[i].DimNode.DimNodes.Length);
            }
        }

        [TestMethod]
        public void CanGetEndecaDimensionIdsUsed()
        {
            var expected = new List<int> {1, 7, 4, 31, 40, 38};
            var actual = new EndecaDimensionAdapter(_mockEndecaDimensionFileAdapter.Object, new[] { "1", "2" }).GetAllDimensionIdsUsed();

            Assert.AreEqual(expected.Count, actual.Count);

            foreach (var id in expected)
            {
                Assert.IsTrue(actual.Contains(id));
            }
        }

        [TestMethod]
        public void CanAddDimensionValueToDimension()
        {
            var adapter = new EndecaDimensionAdapter(_mockEndecaDimensionFileAdapter.Object, new[] {"1", "2"});
            var newDimNode =
                MockEndecaDimensionGenerator.GetMockEndecaDimensionNode(new EndecaDimensionProps
                {
                    Id = 79,
                    Name = "New Dim Node",
                    NameFrench = "New Dim Node Fr"
                });
            adapter.AddDimensionNodeToDimension(newDimNode, 1, 38);
            var actual =
                adapter.GetAllEndecaDimensions()
                    .FirstOrDefault(d => d.DimId.Id == 1)
                    .DimNode.DimNodes.FirstOrDefault(n => n.DimVal.DimValId.Id == 38)
                    .DimNodes;

            Assert.IsTrue(actual.Length == 1);

            var insertedNode = actual.FirstOrDefault(n => n.DimVal.DimValId.Id == 79);

            Assert.IsNotNull(insertedNode);
            Assert.AreEqual(insertedNode, newDimNode);
        }

        [TestMethod]
        public void CanRemoveDimensionValueFromDimension()
        {
            var adapter = new EndecaDimensionAdapter(_mockEndecaDimensionFileAdapter.Object, new[] { "1", "2" });
            adapter.RemoveDimensionNodeFromDimension(1, 40);

            var actual = adapter.GetAllEndecaDimensions().FirstOrDefault(d => d.DimId.Id == 1);

            Assert.IsTrue(actual.DimNode.DimNodes.Length == 1);
            Assert.IsTrue(actual.DimNode.DimNodes[0].DimVal.DimValId.Id == 38);
        }

        [TestMethod]
        public void CanSaveDimensionsToFiles()
        {
            var adapter = new EndecaDimensionAdapter(_mockEndecaDimensionFileAdapter.Object, new[] { "1", "2" });
            var newDimNode =
            MockEndecaDimensionGenerator.GetMockEndecaDimensionNode(new EndecaDimensionProps
            {
                Id = 79,
                Name = "New Dim Node",
                NameFrench = "New Dim Node Fr"
            });
            adapter.AddDimensionNodeToDimension(newDimNode, 1, 38);
            adapter.SaveAllDimensionFiles();
            var actual = adapter.GetAllEndecaDimensionXmls();

            Assert.AreEqual(_mockSavedFiles.Count(), actual.Count);
            Assert.IsTrue(actual.ScrambledEquals(_mockSavedFiles.Values));
        }

        private static List<IEndecaDimensionXml> GetMockEndecaDimensionFiles(IEnumerable<List<EndecaDimensionProps>> listOfEndecaDimensionPropLists)
        {
            return listOfEndecaDimensionPropLists.Select(MockEndecaDimensionGenerator.GetMockEndecaDimensionFile).ToList();
        }

        private static void InitializeMockEndecaDimensionProps()
        {
            _mockEndecaDimensionProps = new List<EndecaDimensionProps>
            {
                new EndecaDimensionProps {Id = 1, Name = "First Dimension", NameFrench = "First Dimension Fr"},
                new EndecaDimensionProps {Id = 7, Name = "Seventh Dimension", NameFrench = "Seventh Dimension Fr"},
                new EndecaDimensionProps {Id = 4, Name = "Fourth Dimension", NameFrench = "Fourth Dimension Fr"},
                new EndecaDimensionProps {Id = 31, Name = "Thirty First Dimension", NameFrench = "Thirty First Dimension Fr"},                
            };
        }

        private static void InitializeMockDimensionFiles()
        {
            var dimensionPropsForFile1 = _mockEndecaDimensionProps.Take(2).ToList();
            var dimensionPropsForFile2 = _mockEndecaDimensionProps.Skip(2).Take(2).ToList();

            _mockEndecaDimensionFiles =
                GetMockEndecaDimensionFiles(new List<List<EndecaDimensionProps>>
                {
                    dimensionPropsForFile1,
                    dimensionPropsForFile2
                });

            _mockEndecaDimensionFiles[0].Dimensions[0].DimNode.DimNodes = _mockEndecaDimensionNodes.ToArray();
        }

        private static void InitializeMockDimensionFileAdapter()
        {
            _mockEndecaDimensionFileAdapter = new Mock<IEndecaDimensionFileAdapter>();
            _mockEndecaDimensionFileAdapter.Setup(r => r.GetEndecaDimensionsFromFile(It.Is<string>(s => s.Equals("1"))))
                .Returns(_mockEndecaDimensionFiles[0]);
            _mockEndecaDimensionFileAdapter.Setup(r => r.GetEndecaDimensionsFromFile(It.Is<string>(s => s.Equals("2"))))
                .Returns(_mockEndecaDimensionFiles[1]);
            _mockEndecaDimensionFileAdapter.Setup(
                r => r.WriteOutEndecaDimensionFile(It.IsAny<string>(), It.IsAny<IEndecaDimensionXml>()))
                .Callback<string, IEndecaDimensionXml>((f, d) => _mockSavedFiles.Add(f, d));
        }

        private static void InitializeMockDimensions()
        {
            _mockEndecaDimensions = new List<IDimension>();
            foreach (var dimProps in _mockEndecaDimensionProps)
            {
                _mockEndecaDimensions.Add(MockEndecaDimensionGenerator.GetMockEndecaDimension(dimProps));
            }
            _mockEndecaDimensions[0].DimNode.DimNodes = _mockEndecaDimensionNodes.ToArray();
        }

        private static void InitializeMockEndecaDimensionNodes()
        {
            _mockEndecaDimensionNodes = new List<IDimensionNode>
            {
                MockEndecaDimensionGenerator.GetMockEndecaDimensionNode(new EndecaDimensionProps
                {
                    Id = 40,
                    Name = "Fourtieth Dimension",
                    NameFrench = "Fourtieth Dimension Fr"
                }),
                MockEndecaDimensionGenerator.GetMockEndecaDimensionNode(new EndecaDimensionProps
                {
                    Id = 38,
                    Name = "Thirty Eight Dimension",
                    NameFrench = "Thirty Eight Dimension Fr"
                })
            };
        }
    }
}
