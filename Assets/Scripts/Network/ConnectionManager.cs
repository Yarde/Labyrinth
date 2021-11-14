using Cysharp.Threading.Tasks;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Networking;

namespace Network
{
    public class ConnectionManager
    {
        public static ConnectionManager Instance { get; private set; }
        
        private string _host;

        public ConnectionManager(string host)
        {
            _host = host;
            Instance = this;
        }

        public async UniTask<TResponse> SendMessageAsync<TResponse>(IMessage message, string endpoint)
            where TResponse : IMessage<TResponse>, new()
        {
            Debug.Log($"Sending started {message}");
            
            UnityWebRequest request = UnityWebRequest.Post($"{_host}{endpoint}", message.ToString());
            //if we want to use byte array instead of string we need to change ".Post" to ".Put" and uncomment line bellow 
            //unityWebRequest.method = "POST";
            
            request.SetRequestHeader("Content-Type", "application/x-protobuf");
            
            Debug.Log($"Web Request Sent");

            try
            {
                await request.SendWebRequest();
            
                Debug.Log($"Request: {request.result}\nData: {request.downloadHandler.text}");
            
                TResponse x = new MessageParser<TResponse>(() => new TResponse())
                    .ParseFrom(request.downloadHandler.data);
            
                Debug.Log($"Response {x}");
            
                return x;
            }
            catch (UnityWebRequestException exception)
            {
                Debug.LogError(exception);
                return new TResponse();
            }
        }
    }
}
