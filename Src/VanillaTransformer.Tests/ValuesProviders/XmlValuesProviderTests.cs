﻿using System.Linq;
using NUnit.Framework;
using VanillaTransformer.Core.Configuration;
using VanillaTransformer.Core.ValuesProviders;

namespace VanillaTransformer.Tests.ValuesProviders
{
    [TestFixture]
    public class XmlValuesProviderTests
    {
        [Test]
        public void should_be_able_to_read_properly_xml_file_structure()
        {
            //ARRANGE
            const string testFileName = "test.xml";
            var textFileReaderMock = TextFileReaderTestsHelpers.GetTextFileReaderMock(testFileName, @"<root><Var1>Val1</Var1></root>");
            var xmlValueProvider = new XmlFileConfigurationValuesProvider(testFileName, new XmlTextFileReader(textFileReaderMock));
            
            //ACT
            var result = xmlValueProvider.GetValues();

            //ASSERT
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            var firstPair = result.First();
            Assert.AreEqual("Var1", firstPair.Key);
        }

        [Test]
        public void should_be_able_to_read_simple_value_from_xml_file()
        {
            //ARRANGE
            const string testFileName = "test.xml";
            var textFileReader = TextFileReaderTestsHelpers.GetTextFileReaderMock(testFileName, @"<root><Var1>Val1</Var1></root>");
            var xmlValueProvider = new XmlFileConfigurationValuesProvider(testFileName, new XmlTextFileReader(textFileReader));

            
            //ACT
            var result = xmlValueProvider.GetValues();

            //ASSERT
            var firstPair = result.First();
            Assert.AreEqual("Val1", firstPair.Value);
        }

        [Test]
        public void should_be_able_read_value_with_xml_content()
        {
             //ARRANGE
            const string testFileName = "test.xml";
            var textFileReader = TextFileReaderTestsHelpers.GetTextFileReaderMock(testFileName, @"<root><Var1><InsideXml>Sample</InsideXml></Var1></root>");
            var xmlValueProvider = new XmlFileConfigurationValuesProvider(testFileName, new XmlTextFileReader(textFileReader));

            //ACT
            var result = xmlValueProvider.GetValues();

            //ASSERT
            var firstPair = result.First();
            Assert.AreEqual("<InsideXml>Sample</InsideXml>", firstPair.Value);

        }


        [Test]
        public void should_be_able_read_value_with_xml_content_and_preserve_whitespaces()
        {
             //ARRANGE
            const string testFileName = "test.xml";
            var textFileReader = TextFileReaderTestsHelpers.GetTextFileReaderMock(testFileName, @"<root>
    <Var1>
        <InsideXml>Sample</InsideXml>
        <InsideXml>Sample2</InsideXml>
    </Var1>
</root>");
            var xmlValueProvider = new XmlFileConfigurationValuesProvider(testFileName, new XmlTextFileReader(textFileReader));

            
            //ACT
            var result = xmlValueProvider.GetValues();

            //ASSERT
            var firstPair = result.First();
            Assert.AreEqual(
@"
        <InsideXml>Sample</InsideXml>
        <InsideXml>Sample2</InsideXml>
    ", firstPair.Value);

        }

        [Test]
        public void should_be_able_read_value_with_xml_attribute()
        {
            //ARRANGE
            const string testFileName = "test.xml";
            var textFileReader = TextFileReaderTestsHelpers.GetTextFileReaderMock(testFileName, @"<root><Var1>attribute=""val""</Var1></root>");
            var xmlValueProvider = new XmlFileConfigurationValuesProvider(testFileName, new XmlTextFileReader(textFileReader));

            //ACT
            var result = xmlValueProvider.GetValues();

            //ASSERT
            var firstPair = result.First();
            Assert.AreEqual(@"attribute=""val""", firstPair.Value);
        }
    }
}
