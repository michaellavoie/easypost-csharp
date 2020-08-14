using EasyPost;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace EasyPostTest {
    [TestClass]
    public class AddressTest {
        Address address;

        [TestInitialize]
        public void Initialize() {
            ClientManager.SetCurrent("NvBX2hFF44SVvTPtYjF0zQ");
            address = new Address() {
                company = "Simpler Postage Inc",
                street1 = "164 Townsend Street",
                street2 = "Unit 1",
                city = "San Francisco",
                state = "CA",
                country = "US",
                zip = "94107"
            };
        }

        [TestMethod]
        [ExpectedException(typeof(HttpException))]
        public void TestRetrieveInvalidId() {
            Address.Retrieve(null, "not-an-id");
        }

        [TestMethod]
        public void TestCreateAndRetrieve() {
            Dictionary<string, object> parameters = new Dictionary<string, object>() {
                { "company", "Simpler Postage Inc" },
                { "street1", "164 Townsend Street" },
                { "street2", "Unit 1" },
                { "city", "San Francisco" },
                { "state", "CA" },
                { "country", "US" },
                { "zip", "94107" }
            };
            Address address = Address.Create(null, parameters);
            Assert.IsNotNull(address.id);
            Assert.AreEqual(address.company, "Simpler Postage Inc");
            Assert.IsNull(address.name);

            Address retrieved = Address.Retrieve(null, address.id);
            Assert.AreEqual(address.id, retrieved.id);
        }

        [TestMethod]
        public void TestCreateWithVerifications() {
            Dictionary<string, object> parameters = new Dictionary<string, object>() {
                { "company", "Simpler Postage Inc" },
                { "street1", "164 Townsend Street" },
                { "street2", "Unit 1" },
                { "city", "San Francisco" },
                { "state", "CA" },
                { "country", "US" },
                { "zip", "94107" },
                { "verifications", new List<string>() { "delivery", "zip4" } }
            };

            Address address = Address.Create(null, parameters);
            Assert.IsNotNull(address.verifications.delivery);
            Assert.IsNotNull(address.verifications.zip4);

            parameters = new Dictionary<string, object>() {
                { "company", "Simpler Postage Inc" },
                { "street1", "123 Fake Street" },
                { "zip", "94107" },
                { "verifications", new List<string>() { "delivery", "zip4" } }
            };

            address = Address.Create(null, parameters);
            Assert.AreEqual(address.verifications.delivery.success, false);
        }

        [TestMethod]
        public void TestInstanceCreateWithVerifications() {
            Address address = new Address() {
                company = "Simpler Postage Inc",
                street1 = "164 Townsend Street",
                street2 = "Unit 1",
                city = "San Francisco",
                state = "CA",
                country = "US",
                zip = "94107",
                verify = new List<string>() { "delivery", "zip4" }
            };

            address.Create(null);
            Assert.IsNotNull(address.verifications.delivery);
            Assert.IsNotNull(address.verifications.zip4);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpException))]
        public void TestCreateWithStrictVerifications() {
            Dictionary<string, object> parameters = new Dictionary<string, object>() {
                { "company", "Simpler Postage Inc" },
                { "street1", "123 Fake Street" },
                { "zip", "94107" },
                { "strict_verifications", new List<string>() { "delivery", "zip4" } }
            };

            Address address = Address.Create(null, parameters);
        }

        [TestMethod]
        public void TestCreateInstance() {
            address.Create(null);
            Assert.IsNotNull(address.id);
        }

        [TestMethod]
        public void TestInstanceCreateWithVerificationParams() {
            address.Create(null, new List<string>() { "delivery", "zip4" });
            Assert.IsNotNull(address.verifications.delivery);
            Assert.IsNotNull(address.verifications.zip4);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpException))]
        public void TestInstanceCreateWithStrictVerifications() {
            address = new Address() {
                company = "Simpler Postage Inc"
            };
            address.Create(null, strictVerifications: new List<string> { "delivery", "zip4" });
        }

        [TestMethod]
        public void TestVerify() {
            Dictionary<string, object> parameters = new Dictionary<string, object>() {
                { "company", "Simpler Postage Inc" },
                { "street1", "164 Townsend Street" },
                { "street2", "Unit 1" },
                { "city", "San Francisco" },
                { "state", "CA" },
                { "country", "US" },
                { "zip", "94107" },
                { "residential", true }
            };
            Address address = Address.Create(null, parameters);
            address.Verify(null);
            Assert.IsNotNull(address.id);
            Assert.AreEqual(address.company, "SIMPLER POSTAGE INC");
            Assert.IsNull(address.name);
            Assert.IsFalse((bool)address.residential);
        }

        [TestMethod]
        public void TestVerifyCarrier() {
            Dictionary<string, object> parameters = new Dictionary<string, object>() {
                { "company", "Simpler Postage Inc" },
                { "street1", "164 Townsend Street" },
                { "street2", "Unit 1" },
                { "city", "San Francisco" },
                { "state", "CA" },
                { "country", "US" },
                { "zip", "94107" },
                { "residential", true }
            };
            Address address = Address.Create(null, parameters);
            address.Verify(null, "usps");
            Assert.IsNotNull(address.id);
            Assert.AreEqual(address.company, "SIMPLER POSTAGE INC");
            Assert.AreEqual(address.street1, "164 TOWNSEND ST UNIT 1");
            Assert.IsNull(address.name);
        }

        [TestMethod]
        public void TestVerifyBeforeCreate() {
            address.Verify(null);
            Assert.IsNotNull(address.id);
        }

        [TestMethod]
        public void TestCreateAndVerify() {
            Dictionary<string, object> parameters = new Dictionary<string, object>() {
                { "company", "Simpler Postage Inc" },
                { "street1", "164 Townsend Street" },
                { "street2", "Unit 1" },
                { "city", "San Francisco" },
                { "state", "CA" },
                { "country", "US" },
                { "zip", "94107" }
            };
            Address address = Address.CreateAndVerify(null, parameters);
            Assert.IsNotNull(address.id);
            Assert.AreEqual(address.company, "SIMPLER POSTAGE INC");
            Assert.IsNull(address.name);
            Assert.AreEqual(address.verifications.delivery.success, true);
        }

        [TestMethod]
        public void TestVerificationFailure() {
            Address address = new Address() {
                company = "Simpler Postage Inc",
                street1 = "1645456 Townsend Street",
                street2 = "Unit 1",
                city = "San Francisco",
                state = "CA",
                country = "US",
                zip = "94107"
            };

            address.Create(null);

            try {
                address.Verify(null);
            }
            catch (HttpException e) {
                Assert.AreEqual("ADDRESS.VERIFY.FAILURE", e.Code);
                Assert.AreEqual("Unable to verify address.", e.Message);
                Assert.AreEqual("E.ADDRESS.NOT_FOUND", e.Errors[0].code);
                Assert.AreEqual("address", e.Errors[0].field);
                Assert.AreEqual("Address not found", e.Errors[0].message);
            }
        }
    }
}
