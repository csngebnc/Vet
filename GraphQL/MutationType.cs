using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.GraphQL
{
    public class MutationType : ObjectType<Mutation>
    {
        protected override void Configure(IObjectTypeDescriptor<Mutation> descriptor)
        {
            base.Configure(descriptor);
            descriptor.Authorize();
        }
    }
}
