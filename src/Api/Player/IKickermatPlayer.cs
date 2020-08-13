using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Player
{
    public interface IKickermatPlayer : IIdentifiable
    {
        void Start();

        void Stop();

        void Pause();

        void Resume();
    }
}
