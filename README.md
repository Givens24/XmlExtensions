# XmlExtensions

# Setup and Usage for XmlExtensions
* All you need to do in order to use the extensions is reference the **XmlExtensionAssistant** dll (which you can download through nuget) and create an instance of an XmlDocument to extend on. The only method that does not extend on the Xmldocument object is **"TraverseXml"**. This method is an extension for **XmlNode**.
* The XmlExtensions library contains 4 methods that include **"TraverseXml"**, **"FindElements"**, **"FindElementsByAttributeValue"** and **"Flatten"**
* The following examples bellow will demonstrate how to call each of the four methods above.

## TraverseXml
* Traverse Xml will start at a specific node and perform the action that you pass in as a parameter.
```C#
var xmlRequestInput = "<trip><route number=\"1\"><stop stopid=\"1\"><city>Apple Valley</city></stop><stop stopid=\"2\">" +<city>Plymouth</city><country>!*)</country></stop></route><route number=\"2\"><stop stopid=\"1\"><city>St. Paul</city>" +</stop><stop stopid=\"2\"><city>St. Cloud</city><country>US</country></stop></route></trip>";

var xmlDoc = new XmlDocument();
xmlDoc.LoadXml(xmlRequestInput);

xmlDoc.FirstChild.TraverseXml(x =>
{
     if (x.Name.Equals("country") && x.InnerText.Equals("!*)"))
     {
         x.FirstChild.Value = "";
     }
});
```

## FindElements
* FindElements will return a collection of Xml nodes based on the where clause parameter that is passed in. The where class is a Func<XmlNode, bool>.
```C#
var xmlRequestInput = "<trip><route number=\"1\"><stop stopid=\"1\"><city>Apple Valley</city></stop><stop stopid=\"2\">" +<city>Plymouth</city><country>!*)</country></stop></route><route number=\"2\"><stop stopid=\"1\"><city>St. Paul</city>" +</stop><stop stopid=\"2\"><city>St. Cloud</city><country>US</country></stop></route></trip>";

var xmlDoc = new XmlDocument();
xmlDoc.LoadXml(xmlRequestInput);

var elementsFound = xmlDoc.FindElements(x => x.Name.Equals("city") || x.Name.Equals("country"));
``` 

## FindElementsByAttributeValue
* FindElementsByAttributeValue will return a collection of Xml nodes that meet the attribute name and attribute value that are passed in.
```C#
var xmlRequestInput = "<trip><route number=\"1\"><stop stopid=\"1\"><city>Apple Valley</city></stop><stop stopid=\"2\">" +<city>Plymouth</city><country>!*)</country></stop></route><route number=\"2\"><stop stopid=\"1\"><city>St. Paul</city>" +</stop><stop stopid=\"2\"><city>St. Cloud</city><country>US</country></stop></route></trip>";

var xmlDoc = new XmlDocument();
xmlDoc.LoadXml(xmlRequestInput);

var elementsFound = xmlDoc.FindElementsByAttributeValue("stopid", "2");
```

## Flatten
* Flatten returns a collection of Xml nodes in the entire XmlDocument.
```C#
var xmlRequestInput = "<trip><route number=\"1\"><stop stopid=\"1\"><city>Apple Valley</city></stop><stop stopid=\"2\">" +<city>Plymouth</city><country>!*)</country></stop></route><route number=\"2\"><stop stopid=\"1\"><city>St. Paul</city>" +</stop><stop stopid=\"2\"><city>St. Cloud</city><country>US</country></stop></route></trip>";

var xmlDoc = new XmlDocument();
xmlDoc.LoadXml(xmlRequestInput);

var allXmlNodesFlattened = xmlDoc.Flatten();
```

## AlphabetizeElementChildren
* AlphabetizeElementChildren alphabetizes all the child elements inside of a particular element. The **whereExpression** parameter determines what element(s) children you want alphabetized.
```C#
var xml = "<shelf>" +
"<book><title>Test Book 1</title><author>Judy Blume</author><copyright>2016</copyright></book>" +
"<book><title>Test Book 2</title><copyright>2002</copyright><author>John Doe</author></book>" +
"<book><copyright>2014</copyright><title>Test Book 3</title><author>Tom Clancy</author></book>" +
"</shelf>";

var xmlDocument = new XmlDocument();
xmlDocument.LoadXml(xml);

xmlDocument.AlphabetizeElementChildren(x => x.Name.Equals("book"));
var bookNumberOne = xmlDocument.FindElements(x => x.Name.Equals("book")).FirstOrDefault();
```
