using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace XmlExtensionAssistant
{
    public static class XmlExtensions
    {
        /// <summary>
        /// Walks all the child nodes of the given root node in an XML structure and performs the specified action against each node
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
                var foundAttribute = x.Attributes?.Cast<XmlAttribute>()
                                                  .FirstOrDefault(attribute => attribute.Name.Equals(attributeName) && 
                                                   attribute.InnerText == attributeValue);
                if (foundAttribute == null)
                {
                    return;
                }

                foundXmlNodes.Add(x);
            });

            return foundXmlNodes;
        }

        /// <summary>
        /// Finds all nodes in an xml based on the specified attribute where clause
        /// </summary>
        /// <param name="xmlDocument">Xml document to search</param>
        /// <param name="whereExression">Where expression to search nodes on</param>
        /// <returns>A collection of xml nodes</returns>
        public static IEnumerable<XmlNode> FindElementsByAttributes(this XmlDocument xmlDocument, Func<XmlAttribute, bool> whereExpression)
        {
            var foundXmlNodes = new List<XmlNode>();
            xmlDocument.FirstChild.ParentNode.TraverseXml(x =>
            {
                var foundAttribute = x.Attributes?.Cast<XmlAttribute>()
                                                  .FirstOrDefault(whereExpression);
                if (foundAttribute == null)
                {
                    return;
                }

                foundXmlNodes.Add(x);
            });

            return foundXmlNodes;
        }

        /// <summary>
        /// Removes all elements based on the specified where clause
        /// </summary>
        /// <param name="xmlDocument">Xml document to search</param>
        /// <param name="whereExpression">Where expressions to removes nodes</param>
        public static void RemoveElements(this XmlDocument xmlDocument, Func<XmlNode, bool> whereExpression)
        {
            var elementsToRemove = xmlDocument.FindElements(whereExpression);
            if (!elementsToRemove.Any())
            {
                return;
            }

            xmlDocument.TraverseXml(x =>
            {
                foreach(XmlNode node in elementsToRemove)
                {
                    if(x != node)
                    {
                        continue;
                    }

                    x.ParentNode.RemoveChild(x);
                }
            });
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

        /// <summary>
        /// Alphabetizes an XMl element's children
        /// </summary>
        /// <param name="xmlDocument">The XML document to reorganize</param>
        /// <param name="whereExpression">The where clause to find the element(s) that need alphabetizing</param>
        public static void AlphabetizeElementChildren(this XmlDocument xmlDocument, Func<XmlNode, bool> whereExpression)
        {
            var elementsToAlphabetize = xmlDocument.FindElements(whereExpression);
            elementsToAlphabetize.ToList().ForEach(x =>
            {
                if (!x.HasChildNodes)
                {
                    return;
                }

                var orderedChildNodes = x.ChildNodes.Cast<XmlNode>().ToList().OrderBy(node => node.Name);
                x.ChildNodes.Cast<XmlNode>().ToList().ForEach(child => x.RemoveChild(child));
                orderedChildNodes.ToList().ForEach(newChild => x.AppendChild(newChild));
            });
        }
    }
}
