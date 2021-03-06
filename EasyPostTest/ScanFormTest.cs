﻿using EasyPost;

using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EasyPostTest {
    [TestClass]
    public class ScanFormTest
    {
        [TestInitialize]
        public void Initialize()
        {
            ClientManager.SetCurrent("NvBX2hFF44SVvTPtYjF0zQ");
        }

        [TestMethod]
        public void TestScanFormList() {
            Dictionary<string, object> dict = new Dictionary<string, object>() { { "page_size", 1 } };
            ScanFormList scanFormList = ScanForm.List(null, dict);
            Assert.AreNotEqual(null, scanFormList.scan_forms[0].batch_id);
            Assert.AreNotEqual(0, scanFormList.scan_forms.Count);
            ScanFormList nextScanFormList = scanFormList.Next(null);
            Assert.AreNotEqual(scanFormList.scan_forms[0].id, nextScanFormList.scan_forms[0].id);
        }

        [TestMethod]
        public void TestScanFormCreateAndRetrieve() {
            ScanForm scanForm = ScanForm.Retrieve(null, "sf_e35ae7fc59bb4482ae32efc663267104");
            Assert.AreEqual("sf_e35ae7fc59bb4482ae32efc663267104", scanForm.id);
        }
    }
}
