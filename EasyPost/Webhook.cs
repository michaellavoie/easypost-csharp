using System;
using System.Collections.Generic;

using RestSharp;

namespace EasyPost {
    public class Webhook : Resource {
#pragma warning disable IDE1006 // Naming Styles
        public string id { get; set; }
        public string mode { get; set; }
        public string url { get; set; }
        public DateTime? disabled_at { get; set; }
#pragma warning restore IDE1006 // Naming Styles

        /// <summary>
        /// Get a list of scan forms.
        /// </summary>
        /// <returns>List of EasyPost.Webhook insteances.</returns>
        public static List<Webhook> List(Client client, Dictionary<string, object> parameters = null) {
            Request request = new Request("v2/webhooks");

            WebhookList webhookList = request.Execute<WebhookList>(client);
            return webhookList.webhooks;
        }

        /// <summary>
        /// Retrieve a Webhook from its id.
        /// </summary>
        /// <param name="id">String representing a webhook. Starts with "hook_".</param>
        /// <returns>EasyPost.User instance.</returns>
        public static Webhook Retrieve(Client client, string id) {
            Request request = new Request("v2/webhooks/{id}");
            request.AddUrlSegment("id", id);

            return request.Execute<Webhook>(client);
        }

        /// <summary>
        /// Create a Webhook.
        /// </summary>
        /// <param name="parameters">
        /// Dictionary containing parameters to create the carrier account with. Valid pairs:
        ///   * { "url", string } Url of the webhook that events will be sent to.
        /// All invalid keys will be ignored.
        /// </param>
        /// <returns>EasyPost.Webhook instance.</returns>
        public static Webhook Create(Client client, Dictionary<string, object> parameters) {
            Request request = new Request("v2/webhooks", Method.POST);
            request.AddBody(new Dictionary<string, object>() { { "webhook", parameters } });

            return request.Execute<Webhook>(client);
        }

        /// <summary>
        /// Enable a Webhook that has been disabled previously.
        /// </summary>
        public void Update(Client client) {
            Request request = new Request("v2/webhooks/{id}", Method.PUT);
            request.AddUrlSegment("id", id);

            Merge(request.Execute<Webhook>(client));
        }

        public void Destroy(Client client) {
            Request request = new Request("v2/webhooks/{id}", Method.DELETE);
            request.AddUrlSegment("id", id);
            request.Execute(client);
        }
    }
}
