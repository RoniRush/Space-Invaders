namespace SpaceInvaders.GameClasses.Screens
{
     using Microsoft.Xna.Framework;

     public class MainMenuScreen : PulsingMenuScreen
     {
          public enum eMenuItems
          {
               Players,
               Sound,
               Screen,
               Play,
               Quit
          }

          private const int k_NumOfMenuItems = 5;

          public MainMenuScreen(Game i_Game)
               : base(i_Game, k_NumOfMenuItems, "Main Menu")
          {
          }

          protected override void InitItemNames()
          {
               ItemNames[(int)eMenuItems.Players] = @"Players: One";
               ItemNames[(int)eMenuItems.Sound] = @"Sound Settings";
               ItemNames[(int)eMenuItems.Screen] = @"Screen Settings";
               ItemNames[(int)eMenuItems.Play] = @"Play";
               ItemNames[(int)eMenuItems.Quit] = @"Quit";
          }

          protected override void InitItemCommands()
          {
               m_MenuItems[(int)eMenuItems.Players].Command = changeActivePlayer;
               m_MenuItems[(int)eMenuItems.Players].IsTogglable = true;
               m_MenuItems[(int)eMenuItems.Sound].Command = switchToSoundSettings;
               m_MenuItems[(int)eMenuItems.Sound].IsSelectable = true;
               m_MenuItems[(int)eMenuItems.Screen].Command = switchToScreenSettings;
               m_MenuItems[(int)eMenuItems.Screen].IsSelectable = true;
               m_MenuItems[(int)eMenuItems.Play].Command = play;
               m_MenuItems[(int)eMenuItems.Play].IsSelectable = true;
               m_MenuItems[(int)eMenuItems.Quit].Command = exitGame;
               m_MenuItems[(int)eMenuItems.Quit].IsSelectable = true;
          }

          private void changeActivePlayer()
          {
               if(m_MenuItems[(int)eMenuItems.Players].ItemFont.Message.Equals(@"Players: One"))
               {
                    m_MenuItems[(int)eMenuItems.Players].ItemFont.Message = @"Players: Two";
                    GameSettings.NumOfPlayers = 2;
               }
               else
               {
                    m_MenuItems[(int)eMenuItems.Players].ItemFont.Message = @"Players: One";
                    GameSettings.NumOfPlayers = 1;
               }
          }

          private void play()
          {
               InstructionsScreenProxy previousScreen = PreviousScreen as InstructionsScreenProxy;
               if(previousScreen != null)
               {
                    previousScreen.PlaySelected.Invoke();
                    ExitScreen();
               }
          }

          private void switchToSoundSettings()
          {
               ScreensManager.SetCurrentScreen(new SoundSettingsScreen(Game));
          }

          private void switchToScreenSettings()
          {
               ScreensManager.SetCurrentScreen(new ScreenSettingsScreen(Game));
          }

          private void exitGame()
          {
               Game.Exit();
          }
     }
}
