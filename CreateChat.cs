using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Graph.Models.ODataErrors;
using Microsoft.Graph.Models;
using System.Collections.Generic;

namespace appsvc_fnc_createchat_dotnet001
{
    public static class CreateChat
    {
        [FunctionName("CreateChat")]
        public static async Task<string> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("CreateChat received a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            string response = "";

            try
            {
                if (req.Query["userId"] != "")
                {
                    string authorizationHeader = req.Headers["Authorization"];
                    if (authorizationHeader.StartsWith("Bearer"))
                    {
                        authorizationHeader = authorizationHeader.Substring(7);
                    }

                    var graphClient = Auth.graphAuth(authorizationHeader, log);

                    User user = await graphClient.Me.GetAsync();

                    string loggedInUserId = user.Id;
                    string targetUserId = req.Query["userId"];

                    var chatBody = new Chat
                    {
                        ChatType = ChatType.OneOnOne,
                        Members = new List<ConversationMember> {
                            new ConversationMember {
                                OdataType = "#microsoft.graph.aadUserConversationMember",
                                Roles = new List<string> {"owner"},
                                AdditionalData = new Dictionary<string, object> {{"user@odata.bind", $"https://graph.microsoft.com/v1.0/users('{loggedInUserId}')"}}
                            },
                            new ConversationMember {
                                OdataType = "#microsoft.graph.aadUserConversationMember",
                                Roles = new List<string> {"owner"},
                                AdditionalData = new Dictionary<string, object> {{"user@odata.bind", $"https://graph.microsoft.com/v1.0/users('{targetUserId}')"}}
                            }
                        }
                    };

                    var newChat = await graphClient.Chats.PostAsync(chatBody);

                    log.LogInformation($"Logged in userId {loggedInUserId} has created a chat with userId {targetUserId}");

                    response = newChat.WebUrl;
                }
            }
            catch (ODataError odataError)
            {
                log.LogError(odataError.Error.Code);
                log.LogError(odataError.Error.Message);
            }
            catch (Exception e)
            {
                log.LogError($"Message: {e.Message}");
                if (e.InnerException is not null)
                    log.LogError($"InnerException: {e.InnerException.Message}");
                log.LogError($"StackTrace: {e.StackTrace}");
            }

            log.LogInformation("CreateChat processed a request.");

            return JsonConvert.SerializeObject(response);
        }
    }
}