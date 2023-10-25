// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SignalRIdentityServerShared;

namespace SignalRIdentityServerServer;

[Authorize(JwtBearerDefaults.AuthenticationScheme)]
public class Broadcaster : Hub<IClient>, IHubContract
{
    public async Task Broadcast(string sender, string message)
    {
        await Clients.All.ReceiveMessage(sender, message);
    }
}
