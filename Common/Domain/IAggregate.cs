using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Domain
{
    public interface IAggregateRoot : IAggregateMember
    {
    }

    public interface IAggregateMember
    {
    }
}
