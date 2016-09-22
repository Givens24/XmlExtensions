using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using XmlExtensionAssistant;


namespace XmlHelper.Tests
{
    [TestClass]
    public class XmlExtensionsTests
    {
        [TestMethod]
        public void TraverseXml_No_Results_With_No_Child_Nodes()
        {
            var xmlRequestInput = "<trip></trip>";
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlRequestInput);

            var childNodes = new List<XmlNode>();
            xmlDoc.FirstChild.TraverseXml(x =>
            {
                childNodes.Add(x);
            });

            Assert.IsFalse(childNodes.Any());
        }

        [TestMethod]
        public void TraverseXml_Successfully_Traverse_Tree_And_Change_Node_Inner_Text_Values()
        {
            var xmlRequestInput =
                "<trip><route number=\"1\"><stop stopid=\"1\"><city>Apple Valley</city></stop><stop stopid=\"2\"><city>Plymouth</city><country>!*)</country></stop>" +
                "</route><route number=\"2\"><stop stopid=\"1\"><city>St. Paul</city></stop><stop stopid=\"2\"><city>St. Cloud</city><country>US</country></stop></route></trip>";
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlRequestInput);

            xmlDoc.FirstChild.TraverseXml(x =>
            {
                if (x.Name.Equals("country") && x.InnerText.Equals("!*)"))
                {
                    x.FirstChild.Value = "";
                }
            });

            Assert.IsTrue(!xmlDoc.FindElements(x => x.Name.Equals("country") && x.InnerText.Equals("!*)")).Any());
        }

        [TestMethod]
        public void Flatten_Xml_Structure()
        {
            var xmlRequestInput =
                "<trip><route number=\"1\"><stop stopid=\"1\"><city>Apple Valley</city></stop><stop stopid=\"2\"><city>Plymouth</city><country>!*)</country></stop>" +
                "</route><route number=\"2\"><stop stopid=\"1\"><city>St. Paul</city></stop><stop stopid=\"2\"><city>St. Cloud</city><country>US</country></stop></route></trip>";
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlRequestInput);

            var flattenedXml = xmlDoc.Flatten();

            Assert.IsTrue(flattenedXml.Any(x => x.Name.Equals("city") && x.InnerText.Equals("Apple Valley")));
        }

        [TestMethod]
        public void FindElementsByAttributeValue_Get_Xml_Elements_By_Attribute_And_Attribute_Value()
        {
            var xmlRequestInput =
                "<trip><route number=\"1\"><stop stopid=\"1\"><city>Apple Valley</city></stop><stop stopid=\"2\"><city>Plymouth</city><country>!*)</country></stop>" +
                "</route><route number=\"2\"><stop stopid=\"1\"><city>St. Paul</city></stop><stop stopid=\"2\"><city>St. Cloud</city><country>US</country></stop></route></trip>";
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlRequestInput);

            var elementsFound = xmlDoc.FindElementsByAttributeValue("stopid", "2");

            Assert.IsTrue(elementsFound.Count() == 2);
        }

        [TestMethod]
        public void FindElementsByAttributeValue_No_Elements_Returned_By_Attribute_And_Attribute_Value()
        {
            var xmlRequestInput =
                "<trip><route number=\"1\"><stop stopid=\"1\"><city>Apple Valley</city></stop><stop stopid=\"2\"><city>Plymouth</city><country>!*)</country></stop>" +
                "</route><route number=\"2\"><stop stopid=\"1\"><city>St. Paul</city></stop><stop stopid=\"2\"><city>St. Cloud</city><country>US</country></stop></route></trip>";
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlRequestInput);

            var elementsFound = xmlDoc.FindElementsByAttributeValue("stopid", "3");

            Assert.IsFalse(elementsFound.Any());
        }

        [TestMethod]
        public void FindElements_Find_Elements_By_Specific_Where_Clause()
        {
            var xmlRequestInput =
                "<trip><route number=\"1\"><stop stopid=\"1\"><city>Apple Valley</city></stop><stop stopid=\"2\"><city>Plymouth</city><country>!*)</country></stop>" +
                "</route><route number=\"2\"><stop stopid=\"1\"><city>St. Paul</city></stop><stop stopid=\"2\"><city>St. Cloud</city><country>US</country></stop></route></trip>";
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlRequestInput);

            var elementsFound = xmlDoc.FindElements(x => x.Name.Equals("city") || x.Name.Equals("country"));

            Assert.IsTrue(elementsFound.Count() == 6);
        }

        [TestMethod]
        public void FindElements_Get_No_Elements_With_A_Specific_Name_And_Inner_Text_Value()
        {
            var xmlRequestInput =
                "<trip><route number=\"1\"><stop stopid=\"1\"><city>Apple Valley</city></stop><stop stopid=\"2\"><city>Plymouth</city><country>US</country></stop>" +
                "</route><route number=\"2\"><stop stopid=\"1\"><city>St. Paul</city></stop><stop stopid=\"2\"><city>St. Cloud</city><country>US</country></stop></route></trip>";
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlRequestInput);

            var foundElements = xmlDoc.FindElements(x => x.Name.Equals("bad") && x.InnerText.Equals("1"));

            Assert.IsFalse(foundElements.Any());
        }
    }
}
