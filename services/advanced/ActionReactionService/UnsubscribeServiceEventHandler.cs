﻿using ActionReactionService.AboutParser;
using Database;
using Database.Entities;
using EventBus;
using EventBus.Event;
using Extension;
using Extension.Socket;

namespace ActionReactionService;

public class UnsubscribeServiceEventHandler : IIntegrationEventHandler<UnsubscribeServiceEvent, (List<Service>, ResultType)>
{
    private readonly IDatabaseHandler _dbHandler;
    private readonly IAboutParserService _aboutParserService;
    private readonly ISocketService _socketService;

    public UnsubscribeServiceEventHandler(IDatabaseHandler dbHandler, IAboutParserService aboutParserService, ISocketService socketService)
    {
        _dbHandler = dbHandler;
        _aboutParserService = aboutParserService;
        _socketService = socketService;
    }
    
    public async Task<(List<Service>, ResultType)> HandleAsync(UnsubscribeServiceEvent @event)
    {
        string id = @event.JwtToken.GetJwtSubClaim();

        if (!Guid.TryParse(id, out Guid userId))
        {
            return (new(), ResultType.Fail);
        }

        var services = _aboutParserService.GetServices();

        if (services.All(s => s.Name != @event.Name))
        {
            return (new(), ResultType.Fail);
        }
        
        var existingService = (await _dbHandler.GetAsync<Service>(s => s.Name == @event.Name && s.UserId == userId)).FirstOrDefault();

        if (existingService is null)
        {
            return (new(), ResultType.Fail);
        }
        
        await _dbHandler.DeleteAsync(existingService);
        
        _socketService.OpenSocket();
        _socketService.SendHandshake();
        _socketService.NotifyChange();
        _socketService.CloseSocket();
        
        return (await _dbHandler.GetAllAsync<Service>(), ResultType.Success);
    }
}