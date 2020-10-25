using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Player
{
    /// <summary>
    /// Implement this interface for your computer player.
    /// 
    /// This only fulfills the technical side of the contract. To register your player with
    /// the Kickermat framework annotate the class with the <see cref="KickermatPlayerAttribute"/>.
    /// </summary>
    public interface IKickermatPlayer : INamed
    {
        /// <summary>
        /// A short description for your player. You should mention basic information about the
        /// internal technologies and everything else you regard as useful for the human operator.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// The names of the player's developers.
        /// </summary>
        string[] Authors { get; }

        /// <summary>
        /// A unicode emoji that is displayed in the frontend.
        /// </summary>
        Rune Emoji { get; }

        /// <summary>
        /// Start the game for the player.
        /// In this method you can initialize the various components you need for your game, you
        /// can for example start with the image acquisition of the camera.
        /// </summary>
        void Start();

        /// <summary>
        /// Stop the game for the player.
        /// Used to clean up the resources and for example stop the camera acquisition.
        /// </summary>
        void Stop();
    }
}
