using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Periphery;

namespace Kickermat.Controllers.Camera
{
    public class Camera : IEntity
    {
        public Camera(string id, string name, PeripheralState state)
        {
            Id = id;
            Name = name;
            PeripheralState = state;
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public PeripheralState PeripheralState { get; set; }
    }
}
