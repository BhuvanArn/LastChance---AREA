﻿using ActionReactionService;
using AdvancedServices.Request;
using Database.Entities;
using EventBus;
using EventBus.Event;
using Microsoft.AspNetCore.Mvc;
using Action = Database.Entities.Action;

namespace AdvancedServices.Controllers;

[ApiController]
[Route("[controller]")]
public class AreaController : ControllerBase
{
    private readonly IEventBus _eventBus;
    private readonly ILogger<AreaController> _logger;

    public AreaController(IEventBus eventBus, ILogger<AreaController> logger)
    {
        _eventBus = eventBus;
        _logger = logger;
    }
    
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        _logger.LogInformation("Action reaction event triggered.");
        
        var response = await _eventBus.PublishAsync<ActionReactionEvent, (List<Service>, ResultType)>(new ActionReactionEvent());
        
        return response.Item2 == ResultType.Fail ? Unauthorized(response.Item1) : Ok(response.Item1);
    }

    [HttpGet("{user_token}/services")]
    public async Task<IActionResult> GetServices(string user_token)
    {
        _logger.LogInformation("GetServices event triggered.");

        var response = await _eventBus.PublishAsync<GetServiceEvent, (List<Service>, ResultType)>(new GetServiceEvent
        {
            JwtToken = user_token
        });
        
        return response.Item2 == ResultType.Fail ? Unauthorized(response.Item1) : Ok(response.Item1);
    }

    [HttpPost("{user_token}/subscribe_service")]
    public async Task<IActionResult> SubscribeService(string user_token, [FromBody] SubscribeServiceRequest request)
    {
        _logger.LogInformation($"SubscribeService event triggered for token: {user_token} and service: {request.Name}", user_token, request.Name);

        var response = await _eventBus.PublishAsync<SubscribeServiceEvent, (string, ResultType)>(new SubscribeServiceEvent
        {
            JwtToken = user_token,
            Name = request.Name,
            Auth = request.Auth
        });

        return response.Item2 == ResultType.Fail ? Unauthorized(response.Item1) : Ok(response.Item1);
    }
    
    [HttpPost("{user_token}/unsubscribe_service")]
    public async Task<IActionResult> UnsubscribeService(string user_token, [FromBody] UnsubscribeServiceRequest request)
    {
        _logger.LogInformation($"UnsubscribeService event triggered for token: {user_token} and service: {request.Name}", user_token, request.Name);

        var response = await _eventBus.PublishAsync<UnsubscribeServiceEvent, (List<Service>, ResultType)>(new UnsubscribeServiceEvent
        {
            JwtToken = user_token,
            Name = request.Name
        });

        return response.Item2 == ResultType.Fail ? Unauthorized(response.Item1) : Ok(response.Item1);
    }

    [HttpGet("{user_token}/services/{service_name}/actions_reactions")]
    public async Task<IActionResult> GetActionsReactions(string user_token, string service_name)
    {
        _logger.LogInformation($"GetActionsReactions event triggered with token {user_token} and service {service_name}");
        
        var response = await _eventBus.PublishAsync<GetActionsReactionsEvent, (object, ResultType)>(new GetActionsReactionsEvent
        {
            ServiceName = service_name,
            JwtToken = user_token
        });

        return response.Item2 == ResultType.Fail ? Unauthorized(response.Item1) : Ok(response.Item1);
    }

    [HttpGet("{user_token}/services/{service_name}/action")]
    public async Task<IActionResult> GetAction(string user_token, string service_name)
    {
        _logger.LogInformation("GetActions event triggered");
        
        var response = await _eventBus.PublishAsync<GetActionEvent, (List<Action>, ResultType)>(new GetActionEvent
        {
            ServiceName = service_name,
            JwtToken = user_token
        });
        
        return response.Item2 == ResultType.Fail ? Unauthorized(response.Item1) : Ok(response.Item1);
    }
    
    [HttpGet("{user_token}/services/{service_name}/reaction")]
    public async Task<IActionResult> GetReaction(string user_token, string service_name)
    {
        _logger.LogInformation("GetReaction event triggered");
        
        var response = await _eventBus.PublishAsync<GetReactionEvent, (List<Reaction>, ResultType)>(new GetReactionEvent
        {
            ServiceName = service_name,
            JwtToken = user_token
        });
        
        return response.Item2 == ResultType.Fail ? Unauthorized(response.Item1) : Ok(response.Item1);
    }

    [HttpPost("addactions")]
    public async Task<IActionResult> AddAction([FromBody] AddActionRequest request)
    {
        _logger.LogInformation("AddActions event triggered");

        var response = await _eventBus.PublishAsync<AddActionRequest, (string, ResultType)>(new AddActionRequest
        {
            ServiceId = request.ServiceId,
            Name = request.Name,
            TriggerConfig = request.TriggerConfig,
            JwtToken = request.JwtToken
        });
        
        return response.Item2 == ResultType.Fail ? Unauthorized(response.Item1) : Ok(response.Item1);
    }

    [HttpPost("addreactions")]
    public async Task<IActionResult> AddReaction([FromBody] AddReactionRequest request)
    {
        _logger.LogInformation("AddReactions event triggered");

        var response = await _eventBus.PublishAsync<AddReactionEvent, (string, ResultType)>(new AddReactionEvent
        {
            ServiceId = request.ServiceId,
            ActionId = request.ActionId,
            Name = request.Name,
            ExecutionConfig = request.ExecutionConfig,
            JwtToken = request.JwtToken
        });
        
        return response.Item2 == ResultType.Fail ? Unauthorized(response.Item1) : Ok(response.Item1);
    }
}