using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Remoting.Messaging;

namespace OpenIGTLink {
    public class OpenIGTLinkClient {
        private readonly string host;
        private readonly int port;
        private Socket socket;
        private AsyncCallback connectCallback;

        public OpenIGTLinkClient(string host, int port) {
            this.host = host;
            this.port = port;
        }

        public void BeginConnect(AsyncCallback connectCallback) {
            this.connectCallback = connectCallback;
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp) {
                Blocking = false
            };

            var addresslist = Dns.GetHostAddresses(host);

            socket.BeginConnect(addresslist[0], port, ConnectCallback, socket);
        }

        private void ConnectCallback(IAsyncResult ar) {
            var socket = (Socket) ar.AsyncState;
            socket.EndConnect(ar);

            if (connectCallback != null) {
                connectCallback.Invoke(ar);
            }
        }
    }
}