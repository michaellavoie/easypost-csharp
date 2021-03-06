using System;

namespace EasyPost {
    /// <summary>
    /// Provides the ability to manage delegated construction of client connections for requests.
    /// </summary>
    public static class ClientManager {
        private static Func<Client> getCurrent;

        internal static Client Build() {
            if (getCurrent == null)
                throw new ClientNotConfigured();
            return getCurrent();
        }

        [Obsolete("SetCurrent() does not support multi-threaded requests")]
        public static void SetCurrent(string apiKey) {
            SetCurrent(() => new Client(new ClientConfiguration(apiKey)));
        }

        [Obsolete("SetCurrent() does not support multi-threaded requests")]
        public static void SetCurrent(Func<Client> getClient) {
            getCurrent = getClient;
        }

        public static void Unconfigure() {
            getCurrent = null;
        }
    }
}