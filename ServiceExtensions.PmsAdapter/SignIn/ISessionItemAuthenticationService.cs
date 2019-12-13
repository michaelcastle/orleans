﻿using ServiceExtensions.PmsAdapter.ClientChannel;
using ServiceExtensions.PmsAdapter.Connected_Services.PmsProcessor;

namespace ServiceExtensions.PmsAdapter.SignIn
{
    public interface ISessionItemAuthenticationService
    {
        SessionItem SignIn(IClientChannelFactory<IPMSInterfaceContractChannel> _clientFactory, string username, string password);
    }
}
