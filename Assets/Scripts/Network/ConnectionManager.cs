using Cysharp.Threading.Tasks;
using Gameplay;
using Google.Protobuf;
using UI;
using UnityEngine;
using UnityEngine.Networking;
using Logger = Utils.Logger;

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
        private string _host;
        private UserInterface _ui;

        public ConnectionManager(string host, UserInterface ui)
        {
			ChangeHost(host);
            _ui = ui;
            Instance = this;
        }

        public void ChangeHost(string host)
        {
            _host = host;
			Debug.Log($"Host set to: {host}");
        }

        public async UniTask<TResponse> SendMessageAsync<TResponse>(IMessage message, string endpoint, bool wait = false)
            where TResponse : IMessage<TResponse>, new()
        {
            Logger.Log($"Sending {message} to endpoint {endpoint} started, waiting for answer? {wait}");
            
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
                    Logger.Log($"TResponse is Empty, sending and Forget");
                    request.SendWebRequest();
                    return new TResponse();
                }

                await request.SendWebRequest();

                Logger.Log($"Response result: {request.result}\nData: {request.downloadHandler.text}");

                var parsedResponse = new MessageParser<TResponse>(() => new TResponse())
                    .ParseFrom(request.downloadHandler.data);

                Logger.Log($"Parsed Response {parsedResponse}");

                return parsedResponse;
            }
            catch (UnityWebRequestException exception)
            {
                Logger.LogError(exception.ToString());
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
