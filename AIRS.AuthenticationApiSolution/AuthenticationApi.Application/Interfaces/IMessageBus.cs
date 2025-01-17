using AuthenticationApi.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationApi.Application.Interfaces;
public interface IMessageBus
{
    void Dispose();
    Task PublishAsync<T>(string exchange, string routingKey, T message);
}
