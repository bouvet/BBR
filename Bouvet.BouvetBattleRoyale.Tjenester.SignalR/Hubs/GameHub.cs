namespace Bouvet.BouvetBattleRoyale.Tjenester.SignalR.Hubs
{
    using Bouvet.BouvetBattleRoyale.Tjenester.Interfaces.SignalR.Hubs;

    using Microsoft.AspNet.SignalR;

    public class GameHub : Hub<IGameHub>
    {
    }
}          