namespace Infrastructure.ServiceInterfaces
{
     using System;
     using Infrastructure.Managers;
     using Microsoft.Xna.Framework.Input;

     public interface ISoundManager
     {
          event EventHandler<EventArgs> ToggleSoundSelected;

          float BackgroundMusicVolume { get; set; }

          float SoundEffectsVolume { get; set; }

          bool ToggleSound { get; set; }

          void PlaySound(string i_AssetName);

          void ChangeVolume(float i_Volume, bool i_IsBgMusic);

          void CheckIfUserWantsToToggleSound(InputManager i_InputManager, Keys i_Key);
     }
}
