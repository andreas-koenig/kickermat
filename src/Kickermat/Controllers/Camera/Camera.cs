using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Periphery;

namespace Kickermat.Controllers.Camera
{
    public class Camera
    {
        public Camera(string name, PeripheralState state, Guid id)
        {
            Name = name;
            State = state;
            Id = id;
        }

        public string Name { get; }

        public PeripheralState State { get; }

        public Guid Id { get; }
    }
}
