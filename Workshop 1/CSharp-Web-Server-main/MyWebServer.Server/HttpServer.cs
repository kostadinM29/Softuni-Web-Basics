﻿namespace MyWebServer.Server
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;
    using MyWebServer.Server.Http;

    public class HttpServer
    {
        private readonly IPAddress _ipAddress;
        private readonly int _port;
        private readonly TcpListener _listener;

        public HttpServer(string ipAddress, int port)
        {
            this._ipAddress = IPAddress.Parse(ipAddress);
            this._port = port;

            _listener = new TcpListener(this._ipAddress, port);
        }

        public async Task Start()
        {
            this._listener.Start();

            Console.WriteLine($"Server started on port {_port}...");
            Console.WriteLine("Listening for requests...");

            while (true)
            {
                var connection = await this._listener.AcceptTcpClientAsync();

                var networkStream = connection.GetStream();

                var requestText = await ReadRequest(networkStream);

                Console.WriteLine(requestText);

                var request = HttpRequest.Parse(requestText);

                await WriteResponse(networkStream);

                connection.Close();
            }
        }

        private static async Task<string> ReadRequest(NetworkStream networkStream)
        {
            const int bufferLength = 1024;
            var buffer = new byte[bufferLength];

            var totalBytes = 0;

            var requestBuilder = new StringBuilder();

            while (networkStream.DataAvailable)
            {
                var bytesRead = await networkStream.ReadAsync(buffer, 0, bufferLength);

                totalBytes += bytesRead;

                if (totalBytes > 10 * 1024)
                {
                    throw new InvalidOperationException("Request is too large.");
                }

                requestBuilder.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));
            }

            return requestBuilder.ToString();
        }

        private static async Task WriteResponse(NetworkStream networkStream)
        {
            var content = @"
<html>
    <head>
        <link rel=""icon"" href=""data:,"">
    </head>
    <body>
        Hello from my server!
    </body>
</html>";
            var contentLength = Encoding.UTF8.GetByteCount(content);

            var response = $@"
HTTP/1.1 200 OK
Server: My Web Server
Date: {DateTime.UtcNow:r}
Content-Length: {contentLength}
Content-Type: text/html; charset=UTF-8

{content}";

            var responseBytes = Encoding.UTF8.GetBytes(response);

            await networkStream.WriteAsync(responseBytes);
        }
    }
}
