namespace Infrastructure.Managers
{
     using System;
     using System.Collections.Generic;
     using Infrastructure.ObjectModel;
     using Infrastructure.ServiceInterfaces;
     using Microsoft.Xna.Framework;
     using Microsoft.Xna.Framework.Audio;
     using Microsoft.Xna.Framework.Input;

     public class SoundManager : GameService, ISoundManager
     {
          private Dictionary<string, SoundEffectInstance> m_SoundEffectsInstancesDict;
          private Dictionary<string, SoundEffect> m_SoundEffectsDict;
          private List<SoundEffectInstance> m_SoundEffectsList;
          private SoundEffectInstance m_BackgroundMusic;
          private bool m_IsSoundActive;

          public SoundManager(Game i_Game)
               : base(i_Game)
          {
               m_SoundEffectsList = new List<SoundEffectInstance>();
               m_SoundEffectsInstancesDict = new Dictionary<string, SoundEffectInstance>();
               m_SoundEffectsDict = new Dictionary<string, SoundEffect>();
               m_IsSoundActive = true;
          }

          protected override void RegisterAsService()
          {
               Game.Services.AddService(typeof(ISoundManager), this);
          }

          public void InitSoundEffects(List<string> i_SoundEffectsNames, string i_BgMusicName)
          {
               SoundEffect soundEffect;
               
               foreach (string soundEffectName in i_SoundEffectsNames)
               {
                    soundEffect = Game.Content.Load<SoundEffect>(@"Sounds\" + soundEffectName);
                    m_SoundEffectsDict.Add(soundEffectName, soundEffect);
                    SoundEffectInstance soundEffectInstance = soundEffect.CreateInstance();
                    m_SoundEffectsList.Add(soundEffectInstance);
                    m_SoundEffectsInstancesDict.Add(soundEffectName, soundEffectInstance);
               }

               soundEffect = Game.Content.Load<SoundEffect>(@"Sounds\" + i_BgMusicName);
               m_BackgroundMusic = soundEffect.CreateInstance();
               m_BackgroundMusic.IsLooped = true;
               m_BackgroundMusic.Play();
          }

          public void InitDefaultVolumes(float i_BackgroundVolume, float i_SoundEffectVolume)
          {
               BackgroundMusicVolume = i_BackgroundVolume;
               m_BackgroundMusic.Volume = BackgroundMusicVolume;
               SoundEffectsVolume = i_SoundEffectVolume;
               changeSoundEffectsVolume(SoundEffectsVolume);
          }

          public event EventHandler<EventArgs> ToggleSoundSelected;

          public float BackgroundMusicVolume { get; set; }

          public float SoundEffectsVolume { get; set; }

          public bool ToggleSound
          {
               get
               {
                    return m_IsSoundActive;
               }

               set
               {
                    m_IsSoundActive = value;
                    if(!m_IsSoundActive)
                    {
                         changeSoundEffectsVolume(0f);
                         m_BackgroundMusic.Volume = 0f;
                    }
                    else
                    {
                         changeSoundEffectsVolume(SoundEffectsVolume);
                         m_BackgroundMusic.Volume = BackgroundMusicVolume;
                    }
               }
          }

          public void PlaySound(string i_AssetName)
          {
               if(m_SoundEffectsInstancesDict[i_AssetName].State == SoundState.Playing)
               {
                    SoundEffectInstance tempSoundEffectInstance = m_SoundEffectsDict[i_AssetName].CreateInstance();
                    tempSoundEffectInstance.Volume = !m_IsSoundActive ? 0 : SoundEffectsVolume;
                    tempSoundEffectInstance.Play();
               }
               else
               {
                    m_SoundEffectsInstancesDict[i_AssetName].Play();
               }
          }

          public void ChangeVolume(float i_Volume, bool i_IsBgMusic)
          {
               if(i_IsBgMusic)
               {
                    BackgroundMusicVolume = i_Volume;
                    m_BackgroundMusic.Volume = i_Volume;
               }
               else
               {
                    SoundEffectsVolume = i_Volume;
                    changeSoundEffectsVolume(SoundEffectsVolume);
               }
          }

          private void changeSoundEffectsVolume(float i_Volume)
          {
               foreach (SoundEffectInstance soundEffectInstance in m_SoundEffectsList)
               {
                    soundEffectInstance.Volume = i_Volume;
               }
          }

          public void CheckIfUserWantsToToggleSound(InputManager i_InputManager, Keys i_Key)
          {
               if (i_InputManager.KeyboardState.IsKeyDown(i_Key) && i_InputManager.PrevKeyboardState.IsKeyUp(i_Key))
               {
                    ToggleSound = !ToggleSound;
                    ToggleSoundSelected?.Invoke(this, EventArgs.Empty);
               }
          }
     }
}
