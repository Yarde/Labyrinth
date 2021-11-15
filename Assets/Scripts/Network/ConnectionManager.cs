using Cysharp.Threading.Tasks;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Networking;

namespace Network
{
    public class ConnectionManager
    {
        public static ConnectionManager Instance { get; private set; }
        private readonly string _host;

        public ConnectionManager(string host)
        {
            _host = host;
            Instance = this;
        }

        public async UniTask<TResponse> SendMessageAsync<TResponse>(IMessage message, string endpoint)
            where TResponse : IMessage<TResponse>, new()
        {
            Debug.Log($"Sending started {message}");
            
            var request = UnityWebRequest.Put($"{_host}{endpoint}", message.ToByteArray());
            request.method = "POST";
            
            // to use string instead of bytes
            //var request = UnityWebRequest.Post($"{_host}{endpoint}", message.ToString());
            
            request.SetRequestHeader("Content-Type", "application/x-protobuf");
            
            Debug.Log($"Web Request Sent");

            try
            {
                await request.SendWebRequest();
            
                Debug.Log($"Request: {request.result}\nData: {request.downloadHandler.text}");
            
                var response = new MessageParser<TResponse>(() => new TResponse())
                    .ParseFrom(request.downloadHandler.data);
            
                Debug.Log($"Response {response}");
            
                return response;
            }
            catch (UnityWebRequestException exception)
            {
                Debug.LogError(exception);
                return new TResponse();
            }
        }
    }
}
