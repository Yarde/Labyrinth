using Cysharp.Threading.Tasks;
using Gameplay;
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
            Debug.Log($"Sending {message} to endpoint {endpoint} started, waiting for answer? {wait}");
            
            var request = UnityWebRequest.Put($"{_host}{endpoint}", message.ToByteArray());
            request.method = "POST";
            request.SetRequestHeader("Content-Type", "application/x-protobuf");
            
            if (wait)
            {
                _ui.SetLoadingActive(true);
            }
            
            try
            {
                if (typeof(TResponse) == typeof(Empty))
                {
                    Debug.Log($"TResponse is Empty, sending and Forget");
                    request.SendWebRequest();
                    return new TResponse();
                }

                await request.SendWebRequest();

                Debug.Log($"Response result: {request.result}\nData: {request.downloadHandler.text}");

                var parsedResponse = new MessageParser<TResponse>(() => new TResponse())
                    .ParseFrom(request.downloadHandler.data);

                Debug.Log($"Parsed Response {parsedResponse}");

                return parsedResponse;
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
