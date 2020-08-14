using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using EasyPost;

namespace EasyPostTest {
    [TestClass]
    public class WebhookTest {
        [TestInitialize]
        public void Initialize() {
            ClientManager.SetCurrent("GxhY479LTioDWsGcEtSAfQ");
        }

        [TestMethod]
        public void TestCRUD() {
            Webhook webhook = Webhook.Create(null, new Dictionary<string, object>() { { "url", "https://www.foobar.com" } });
            Assert.AreEqual(webhook.url, "https://www.foobar.com");

            webhook.Update(null);

            List<Webhook> webhooks = Webhook.List(null);
            CollectionAssert.Contains(webhooks.Select(w => w.id).ToList(), webhook.id);

            webhook.Destroy(null);
            try {
                User.Retrieve(webhook.id);
                Assert.Fail();
            }
            catch (HttpException) { }
        }
    }
}
