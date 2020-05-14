namespace SpaceInvaders.GameClasses.Screens
{
     using Infrastructure.Managers;
     using Microsoft.Xna.Framework;
     using Microsoft.Xna.Framework.Graphics;

     public class ScreenSettingsScreen : PulsingMenuScreen
     {
          public enum eScreenMenuItems
          {
               FullScreen,
               MouseVisible,
               WindowResize,
               Done
          }

          private const int k_NumOfMenuItems = 4;
          private bool m_IsFullScreen;

          public ScreenSettingsScreen(Game i_Game)
               : base(i_Game, k_NumOfMenuItems, "Screen Settings")
          {
          }

          protected override void InitItemNames()
          {
               ItemNames[(int)eScreenMenuItems.FullScreen] = @"Full Screen Mode: Off";
               ItemNames[(int)eScreenMenuItems.MouseVisible] = @"Mouse Visibility: Visible";
               ItemNames[(int)eScreenMenuItems.WindowResize] = @"Allow Window Resizing: Off";
               ItemNames[(int)eScreenMenuItems.Done] = @"Done";
          }

          protected override void InitItemCommands()
          {
               m_MenuItems[(int)eScreenMenuItems.FullScreen].Command = changeFullScreenOption;
               m_MenuItems[(int)eScreenMenuItems.FullScreen].IsTogglable = true;
               m_MenuItems[(int)eScreenMenuItems.MouseVisible].Command = toggleMouseVisible;
               m_MenuItems[(int)eScreenMenuItems.MouseVisible].IsTogglable = true;
               m_MenuItems[(int)eScreenMenuItems.WindowResize].Command = toggleWindowResizable;
               m_MenuItems[(int)eScreenMenuItems.WindowResize].IsTogglable = true;
               m_MenuItems[(int)eScreenMenuItems.Done].Command = Done;
               m_MenuItems[(int)eScreenMenuItems.Done].IsSelectable = true;
          }

          private void changeFullScreenOption()
          {
               if(m_MenuItems[(int)eScreenMenuItems.FullScreen].ItemFont.Message.Equals(@"Full Screen Mode: Off"))
               {
                    m_MenuItems[(int)eScreenMenuItems.FullScreen].ItemFont.Message = @"Full Screen Mode: On";
                    m_IsFullScreen = true;
               }
               else
               {
                    m_MenuItems[(int)eScreenMenuItems.FullScreen].ItemFont.Message = @"Full Screen Mode: Off";
                    m_IsFullScreen = false;
               }

               toggleFullScreen(GameSettings.sr_DefaultScreenHeight, GameSettings.sr_DefaultScreenWidth);
          }

          private void toggleFullScreen(int i_PreferredHeight, int i_PreferredWidth)
          {
               ScreensManager screensManager = ScreensManager as ScreensManager;
               if (screensManager != null)
               {
                    screensManager.GraphicsDeviceManager.ToggleFullScreen();
                    if (m_IsFullScreen)
                    {
                         screensManager.GraphicsDeviceManager.PreferredBackBufferHeight =
                              GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                         screensManager.GraphicsDeviceManager.PreferredBackBufferWidth =
                              GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                    }
                    else
                    {
                         screensManager.GraphicsDeviceManager.PreferredBackBufferHeight =
                              i_PreferredHeight;
                         screensManager.GraphicsDeviceManager.PreferredBackBufferWidth =
                              i_PreferredWidth;
                    }

                    screensManager.GraphicsDeviceManager.IsFullScreen = m_IsFullScreen;
                    screensManager.GraphicsDeviceManager.ApplyChanges();
               }
          }

          private void toggleMouseVisible()
          {
               if (m_MenuItems[(int)eScreenMenuItems.MouseVisible].ItemFont.Message.Equals(@"Mouse Visibility: Invisible"))
               {
                    m_MenuItems[(int)eScreenMenuItems.MouseVisible].ItemFont.Message = @"Mouse Visibility: Visible";
                    Game.IsMouseVisible = true;
               }
               else
               {
                    m_MenuItems[(int)eScreenMenuItems.MouseVisible].ItemFont.Message = @"Mouse Visibility: Invisible";
                    Game.IsMouseVisible = false;
               }
          }

          private void toggleWindowResizable()
          {
               if (m_MenuItems[(int)eScreenMenuItems.WindowResize].ItemFont.Message.Equals(@"Allow Window Resizing: Off"))
               {
                    m_MenuItems[(int)eScreenMenuItems.WindowResize].ItemFont.Message = @"Allow Window Resizing: On";
                    Game.Window.AllowUserResizing = true;
               }
               else
               {
                    m_MenuItems[(int)eScreenMenuItems.WindowResize].ItemFont.Message = @"Allow Window Resizing: Off";
                    Game.Window.AllowUserResizing = false;
               }
          }
     }
}
