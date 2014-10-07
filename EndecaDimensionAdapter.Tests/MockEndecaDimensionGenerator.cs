using System.Collections.Generic;
using System.Linq;

namespace EndecaDimensionAdapter.Tests
{
    public class MockEndecaDimensionGenerator
    {
        public static IEndecaDimensionXml GetMockEndecaDimensionFile(List<EndecaDimensionProps> dimPropsList)
        {
            var dimensions = dimPropsList.Select(GetMockEndecaDimension).ToArray();

            return new EndecaDimensionXml
            {
                Version = "1.0.0",
                Dimensions = dimensions
            };
        }

        public static IDimension GetMockEndecaDimension(EndecaDimensionProps dimProps)
        {
            return new Dimension
            {
                DimId = new DimensionId { Id = dimProps.Id },
                DimNode = GetMockEndecaDimensionNode(dimProps)
            };
        }

        public static IDimensionNode GetMockEndecaDimensionNode(EndecaDimensionProps dimProps)
        {
            return new DimensionNode
            {
                DimVal = new Dval
                {
                    DimValId = new DvalId {Id = dimProps.Id},
                    Properties =
                        new IProp[] {new Prop {Name = "localization_fr", Pval = dimProps.NameFrench}},
                    Synonyms =
                        new ISynonym[]
                        {
                            new Synonym
                            {
                                Classify = "TRUE",
                                Display = "TRUE",
                                Search = "TRUE",
                                Value = dimProps.Name
                            }
                        },
                    Type = "Exact"
                }
            };
        }
    }
}
