using Cysharp.Threading.Tasks;
using Google.Protobuf;
using UI;
using UnityEngine;
using UnityEngine.Networking;

namespace Network
{
    public static class Endpoints
    {
        public static string StartGame = "start-game";
        public static string EndGame = "endgame";
        public static string Question = "next-question";
        public static string Answer = "answer";
    }
    
    public class ConnectionManager
    {
        public static ConnectionManager Instance { get; private set; }
        private readonly string _host;
        private UserInterface _ui;

        public ConnectionManager(string host, UserInterface ui)
        {
            _host = host;
            _ui = ui;
            Instance = this;
        }

        public async UniTask<TResponse> SendMessageAsync<TResponse>(IMessage message, string endpoint, bool wait = false)
            where TResponse : IMessage<TResponse>, new()
        {
            Debug.Log($"Sending started {message} to endpoint {endpoint}, waiting for answer? {wait}");
            
            var request = UnityWebRequest.Put($"{_host}{endpoint}", message.ToByteArray());
            request.method = "POST";
            // to use string instead of bytes
            //var request = UnityWebRequest.Post($"{_host}{endpoint}", message.ToString());
            request.SetRequestHeader("Content-Type", "application/x-protobuf");
            try
            {
                if (wait)
                {
                    _ui.SetLoadingActive(true);
                }
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
            finally
            {
                if (wait)
                {
                    _ui.SetLoadingActive(false);
                }
            }
        }
    }
}
