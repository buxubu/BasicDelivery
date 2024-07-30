using Microsoft.AspNetCore.SignalR;

namespace BasicDelivery.HubConfig
{
    public class MyHub : Hub
    {
        public async Task askServer(string someTextFromClient)
        {
            string tempString = string.Empty;

            if(someTextFromClient == "hey")
            {
                tempString = "message was 'hey'";
            }
            else
            {
                tempString = "message was something else";
            }
            await Clients.Clients(this.Context.ConnectionId).SendAsync("askServerReponse",tempString);
        }
    }
}
