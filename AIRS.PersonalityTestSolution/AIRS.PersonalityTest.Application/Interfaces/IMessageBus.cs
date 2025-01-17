using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIRS.PersonalityTest.Application.Interfaces;
public interface IMessageBus
{
    Task PublishAsync<T>(string exchange, string routingKey, T message);
}
