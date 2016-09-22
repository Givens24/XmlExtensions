﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace XmlExtensionAssistant
{
    public static class XmlExtensions
    {
        /// <summary>
        /// Walks all the child nodes of the given root node in an XML strucuture and performs the specified action against each node
        /// </summary>
        /// <param name="rootXmlNode">XML root node to walk down</param>
        /// <param name="action">Method to perfom against each XML node</param>
        public static void TraverseXml(this XmlNode rootXmlNode, Action<XmlNode> action)
        {
            rootXmlNode.ChildNodes.Cast<XmlNode>().ToList().ForEach(x =>
            {
                action(x);
                TraverseXml(x, action);
            });
        }

        /// <summary>
        /// Finds all nodes in an xml based on the specified where clause
        /// </summary>
        /// <param name="xmlDocument">Xml document to search</param>
        /// <param name="whereExression">Where expression to search nodes on</param>
        /// <returns>A collection of xml nodes</returns>
        public static IEnumerable<XmlNode> FindElements(this XmlDocument xmlDocument, Func<XmlNode, bool> whereExression)
        {
            var foundXmlNodes = new List<XmlNode>();
            xmlDocument.FirstChild.ParentNode.TraverseXml(x =>
            {
                foundXmlNodes.Add(x);
            });

            return foundXmlNodes.Where(whereExression);
        }

        /// <summary>
        /// Finds all nodes in an xml that have the specified attribute name and value
        /// </summary>
        /// <param name="xmlDocument">Xml document to search</param>
        /// <param name="attributeName">Attribute name to search for</param>
        /// <param name="attributeValue">Attribute value to search for</param>
        /// <returns>A collection of xml nodes</returns>
        public static IEnumerable<XmlNode> FindElementsByAttributeValue(this XmlDocument xmlDocument,
            string attributeName, string attributeValue)
        {
            var foundXmlNodes = new List<XmlNode>();
            xmlDocument.FirstChild.ParentNode.TraverseXml(x =>
            {
                var foundAttribute =
                    x.Attributes?.Cast<XmlAttribute>()
                        .FirstOrDefault(
                            attribute => attribute.Name.Equals(attributeName) && attribute.InnerText == attributeValue);
                if (foundAttribute == null)
                {
                    return;
                }

                foundXmlNodes.Add(x);
            });

            return foundXmlNodes;
        }

        /// <summary>
        /// Flattens an XML document into a collection of nodes
        /// </summary>
        /// <param name="xmlDocument">XML document to flatten</param>
        /// <returns>A collection of all the nodes in an xml</returns>
        public static IEnumerable<XmlNode> Flatten(this XmlDocument xmlDocument)
        {
            var flattenedXml = new List<XmlNode>();
            xmlDocument.FirstChild.ParentNode.TraverseXml(x =>
            {
                flattenedXml.Add(x);
            });

            return flattenedXml;
        }
    }
}