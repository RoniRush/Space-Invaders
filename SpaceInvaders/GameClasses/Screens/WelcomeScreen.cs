namespace SpaceInvaders.GameClasses.Screens
{
     using global::SpaceInvaders.GameClasses.Sprites;
     using Infrastructure.Managers;
     using Microsoft.Xna.Framework;
     using Microsoft.Xna.Framework.Input;

     public class WelcomeScreen : InstructionsScreenProxy
     {
          private Background m_background;

          public WelcomeScreen(Game i_Game)
          : base(i_Game)
          {
               m_background = new Background(i_Game);
          }

          protected override void AddScreenComponents()
          {
               Add(m_background);
               base.AddScreenComponents();
          }

          public override void Update(GameTime i_GameTime)
          {
               if (WasPlaySelected)
               {
                    WasPlaySelected = false;
                    ExitScreen();
               }

               if (InputManager != null)
               {
                    if(InputManager.KeyboardState.IsKeyDown(Keys.Enter))
                    {
                         ExitScreen();
                    }

                    if(InputManager.KeyboardState.IsKeyDown(Keys.Escape))
                    {
                         Game.Exit();
                    }

                    InputManager inputManager = InputManager as InputManager;
                    if(inputManager != null)
                    {
                         if(InputManager.KeyboardState.IsKeyDown(Keys.M))
                         {
                              inputManager.Update(i_GameTime);
                              ScreensManager.SetCurrentScreen(new MainMenuScreen(Game));
                         }
                    }

                    base.Update(i_GameTime);
               }
          }

          protected override string GetInstructions()
          {
               return @"   
               Welcome

     Press Enter to Start Playing

     Press Esc to Exit

     Press M to Access Main Menu ";
          }
     }
}
