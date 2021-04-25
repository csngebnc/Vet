using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.GraphQL
{
    public class QueryType : ObjectType<Query>
    {
        protected override void Configure(IObjectTypeDescriptor<Query> descriptor)
        {
            base.Configure(descriptor);
            descriptor.Authorize();
        }
    }
}
