namespace SpaceInvaders.GameClasses.Screens
{
     using global::SpaceInvaders.GameClasses.Sprites;
     using Infrastructure.Managers;
     using Microsoft.Xna.Framework;
     using Microsoft.Xna.Framework.Input;

     public class GameOverScreen : InstructionsScreenProxy
     {
          private readonly Background r_Background;
          private bool m_WasThereAPropertyChange;
          private int m_Player1Score;
          private int m_Player2Score;
          private string m_Winner;

          public int Player1Score
          {
               get
               {
                    return m_Player1Score;
               }

               set
               {
                    m_Player1Score = value;
                    m_WasThereAPropertyChange = true;
               }
          }

          public int Player2Score
          {
               get
               {
                    return m_Player2Score;
               }

               set
               {
                    m_Player2Score = value;
                    m_WasThereAPropertyChange = true;
               }
          }

          public string Winner
          {
               get
               {
                    return m_Winner;
               }

               set
               {
                    m_Winner = value;
                    m_WasThereAPropertyChange = true;
               }
          }

          public GameOverScreen(Game i_Game) 
               : base(i_Game)
          {
               r_Background = new Background(i_Game);
               m_Instructions.TintColor = Color.Red;
          }

          public override void Update(GameTime i_GameTime)
          {
               if (WasPlaySelected)
               {
                    WasPlaySelected = false;
                    startNewGame();
               }

               checkInput(i_GameTime);
               if (m_WasThereAPropertyChange)
               {
                    m_WasThereAPropertyChange = false;
                    m_Instructions.Message = GetInstructions();
               }

               base.Update(i_GameTime);
          }

          private void checkInput(GameTime i_GameTime)
          {
               if(InputManager != null)
               {
                    if(InputManager.KeyboardState.IsKeyDown(Keys.Enter))
                    {
                         startNewGame();
                    }

                    if(InputManager.KeyboardState.IsKeyDown(Keys.Escape))
                    {
                         Game.Exit();
                    }

                    if(InputManager.KeyboardState.IsKeyDown(Keys.M))
                    {
                         InputManager inputManager = InputManager as InputManager;
                         if(inputManager != null)
                         {
                              inputManager.Update(i_GameTime);
                              ScreensManager.SetCurrentScreen(new MainMenuScreen(Game));
                         }
                    }
               }
          }

          private void startNewGame()
          {
               ScreensManager.SetCurrentScreen(new PlayScreen(Game));
          }

          protected override void AddScreenComponents()
          {
               Add(r_Background);
               base.AddScreenComponents();
          }

          protected override string GetInstructions()
          {
               string winnerAnnouncement = string.Empty;
               string scoreTally = string.Format(
                    @"
     Player 1 Score: {0}",
                    Player1Score);

               if (Player2Score > -1)
               {
                    winnerAnnouncement = @"The Winner is: ";
                    scoreTally += string.Format(
                         @"
     Player 2 Score: {0}",
                         Player2Score);
               }
                
               return string.Format(
                    @"
           Game Over
          {0}
     {1} {2}

     Press Enter to Start a New Game

     Press Esc to Exit

     Press M to Access Main Menu",
                    scoreTally,
                    winnerAnnouncement,
                    Winner);
          }

          protected override void InitPositions()
          {
               m_Instructions.Position = new Vector2(100, 20);
          }
     }
}
