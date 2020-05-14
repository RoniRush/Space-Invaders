namespace SpaceInvaders.GameClasses.Screens
{
     using System;
     using Microsoft.Xna.Framework;

     public class SoundSettingsScreen : PulsingMenuScreen
     {
          public enum eSoundMenuItems
          {
               BackgroundMusic,
               SoundEffects,
               ToggleSound,
               Done
          }

          private const int k_NumOfMenuItems = 4;
          private int m_SoundEffectsVolume;
          private int m_BackgroundMusicVolume;
          private bool m_IsSoundActive;

          public SoundSettingsScreen(Game i_Game)
               : base(i_Game, k_NumOfMenuItems, "Sound Settings")
          {
               m_BackgroundMusicVolume = 30;
               m_SoundEffectsVolume = 80;
          }

          public override void Initialize()
          {
               base.Initialize();
               m_IsSoundActive = SoundManager.ToggleSound;
               SoundManager.ToggleSoundSelected += SoundManager_OnToggleSoundSelected;
          }

          private void SoundManager_OnToggleSoundSelected(object sender, EventArgs e)
          {
               updateToggleSoundScreenSetting();
          }

          private void updateToggleSoundMessage()
          {
               m_MenuItems[(int)eSoundMenuItems.ToggleSound].ItemFont.Message =
                    m_IsSoundActive ? @"Toggle Sound: On" : @"Toggle Sound: Off";
          }

          protected override void InitItemNames()
          {
               ItemNames[(int)eSoundMenuItems.BackgroundMusic] = @"BackGround Music Volume: 30";
               ItemNames[(int)eSoundMenuItems.SoundEffects] = @"Sound Effects Volume: 80";
               ItemNames[(int)eSoundMenuItems.ToggleSound] =
                    SoundManager.ToggleSound ? @"Toggle Sound: On" : @"Toggle Sound: Off";
               ItemNames[(int)eSoundMenuItems.Done] = @"Done";
          }

          protected override void InitItemCommands()
          {
               m_MenuItems[(int)eSoundMenuItems.BackgroundMusic].Command = changeBackgroundMusicVolume;
               m_MenuItems[(int)eSoundMenuItems.BackgroundMusic].IsTogglable = true;
               m_MenuItems[(int)eSoundMenuItems.SoundEffects].Command = changeSoundEffectsVolume;
               m_MenuItems[(int)eSoundMenuItems.SoundEffects].IsTogglable = true;
               m_MenuItems[(int)eSoundMenuItems.ToggleSound].Command = toggleSound;
               m_MenuItems[(int)eSoundMenuItems.ToggleSound].IsTogglable = true;
               m_MenuItems[(int)eSoundMenuItems.Done].Command = Done;
               m_MenuItems[(int)eSoundMenuItems.Done].IsSelectable = true;
          }

          private void changeBackgroundMusicVolume()
          {
               m_BackgroundMusicVolume = changeVolume(m_BackgroundMusicVolume);
               SoundManager.ChangeVolume(m_BackgroundMusicVolume / 100f, true);
               m_MenuItems[(int)eSoundMenuItems.BackgroundMusic].ItemFont.Message = string.Format(
                    @"BackGround Music Volume: {0}",
                    m_BackgroundMusicVolume);
          }

          private void changeSoundEffectsVolume()
          {
               m_SoundEffectsVolume = changeVolume(m_SoundEffectsVolume);
               SoundManager.ChangeVolume(m_SoundEffectsVolume / 100f, false);
               m_MenuItems[(int)eSoundMenuItems.SoundEffects].ItemFont.Message = string.Format(
                    @"Sound Effects Volume: {0}",
                    m_SoundEffectsVolume);
          }

          private int changeVolume(int i_Volume)
          {
               i_Volume = m_IsPageUp ? (i_Volume + 10) % 110 : (i_Volume - 10) % 110;
               if (i_Volume < 0)
               {
                    i_Volume = 100;
               }

               return i_Volume;
          }

          private void toggleSound()
          {
               updateToggleSoundScreenSetting();
               SoundManager.ToggleSound = m_IsSoundActive;
          }

          private void updateToggleSoundScreenSetting()
          {
               m_IsSoundActive = !m_IsSoundActive;
               updateToggleSoundMessage();
          }
     }
}
