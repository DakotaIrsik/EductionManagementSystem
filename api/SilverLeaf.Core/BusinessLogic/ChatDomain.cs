using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;
using SilverLeaf.Common;
using SilverLeaf.Common.Constants;
using SilverLeaf.Common.Services;
using SilverLeaf.Entities.DTOs;
using SilverLeaf.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using SilverLeaf.common.Services;

namespace SilverLeaf.Core.BusinessLogic
{

    //public interface IChatDomain
    //{
    //    void Get(string userId);
    //}
    //public class ChatDomain : BaseDomain, IChatDomain
    //{
    //    private IHubContext<ChatHub> _chatHub;
    //    public ChatDomain(EMSContext context,
    //        IMapper mapper,
    //        IElasticSearchService elastic,
    //        ILogger logger,
    //        IHttpContextAccessor httpContextAccessor,
    //        IOptions<AppSettings> settings,
    //        IHubContext<ChatHub> chatHub,
    //        ICacheService cache) : base(context, mapper, elastic, logger, httpContextAccessor, settings, cache)
    //    {
    //        _chatHub = chatHub;
    //    }

    //    public void Get(string userId)
    //    {
    //        var timerManager = new TimerService(() => _chatHub.Clients.All.SendAsync("transferchartdata", WithUser(userId)));
    //    }

    //    public ChatDTO WithUser(string userId)
    //    {
    //        Thread.Sleep(3000);
    //        var chat = GenerateSampleChat(userId);
    //        return _mapper.Map<ChatDTO>(chat);
    //    }

    //    public Chat GenerateSampleChat(string userId)
    //    {
    //        var optionsBuilder = new DbContextOptionsBuilder<EMSContext>();
    //        optionsBuilder.UseSqlServer(_settings.Value.ConnectionStrings.MSSQL);

    //        var x2 = UserId;
    //        using (var context = new EMSContext(optionsBuilder.Options, _http, _elastic, _settings, _mapper))
    //        {
    //            var demoChat = new Chat()
    //            {
    //                ToUserId = context.User.OrderBy(x => new Random().Next(1, 1000)).FirstOrDefault().IdentityUserId,
    //                Message = Strings.LoremIpsum(3, 20, 1, 3, 1, false),
    //                UserId = userId
    //            };

    //            context.Add(demoChat);
    //            context.SaveChanges();
    //            return demoChat;
    //        }
    //    }
    //}
}
