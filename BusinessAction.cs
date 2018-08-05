using System;

namespace MHC.Architecture.Flux
{
    public class BusinessAction
    {
        public string Name { get; set; }

        public object Data { get; set; }

        public Guid MessageOwnerId { get; set; }
    }
}
