﻿using Gamify.Contracts.Requests;

namespace Gamify.Client.Net.Services
{
    public interface IGameSender<TRequest> where TRequest : IRequestObject
    {
        void Send(TRequest request);
    }
}