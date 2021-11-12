using Cysharp.Threading.Tasks;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Networking;
namespace Utils
{
    public class ConnectionManager
    {
        public static ConnectionManager Instance { get; private set; }
        
        private const string URI = "http://zpi2021.westeurope.cloudapp.azure.com/api/";

        public ConnectionManager() => Instance = this;

        public async UniTask<TResponse> GetMessageAsync<TResponse>(IMessage message, string endpoint)
            where TResponse : IMessage<TResponse>, new()
        {
            Debug.Log($"Sending started {message}");
            UnityWebRequest request = UnityWebRequest.Post($"{URI}{endpoint}", message.ToString());
            //unityWebRequest.method = "POST";
            request.SetRequestHeader("Content-Type", "application/x-protobuf");
            
            Debug.Log($"Header Set for Request");

            await request.SendWebRequest();
            
            Debug.Log($"Request sent {request.result} {request.downloadHandler.text}");
            
            TResponse x = new MessageParser<TResponse>(() => new TResponse())
                .ParseFrom(request.downloadHandler.data);
            
            Debug.Log($"Response {x}");
            
            return x;
        }
    }
}
