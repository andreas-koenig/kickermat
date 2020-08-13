using System;
using System.Collections.Generic;
using System.Text;

namespace Api
{
    public interface IIdentifiable
    {
        public Guid Id { get; }
    }
}

