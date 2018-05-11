using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Xunit;

namespace FMData.Xml.Tests
{
    public class GeneralTests
    {
        [Fact]
        public void Sample_fmresultset_FieldExtraction()
        {
            //arrange 
            var local = XmlResponses.GrammarSample_fmresultset;
            var xdoc = XDocument.Load(new StringReader(local));
            var ns = XNamespace.Get("http://www.filemaker.com/xml/fmresultset");

            // act
            var dict = new Dictionary<string, string>();
            var records = xdoc
                .Descendants(ns + "resultset")
                .Elements(ns + "record")
                .Select(r => new Record()
                {
                    RecordId = Convert.ToInt32(r.Attribute("record-id").Value),
                    ModId = Convert.ToInt32(r.Attribute("mod-id").Value),
                    FieldData = r.Elements(ns + "field")
                    .ToDictionary(
                        k => k.Attribute("name").Value, 
                        v => v.Value
                        )
                });

            //assert
            Assert.Contains(records, r => r.RecordId == 14);
            Assert.Contains(records.SelectMany(f => f.FieldData), r => r.Key == "Title");
            Assert.Contains(records.SelectMany(f => f.FieldData), r => r.Value == "Spring in Giverny 3");
        }
    }
}