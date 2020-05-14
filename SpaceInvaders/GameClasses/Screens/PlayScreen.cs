namespace SpaceInvaders.GameClasses.Screens
{
     using global::SpaceInvaders.GameClasses.Managers;
     using global::SpaceInvaders.GameClasses.Sprites;
     using Infrastructure.Managers;
     using Infrastructure.ObjectModel.Screens;
     using Microsoft.Xna.Framework;
     using Microsoft.Xna.Framework.Graphics;
     using Microsoft.Xna.Framework.Input;

     public class PlayScreen : GameScreen
     {
          private readonly Background r_Background;
          private GameManager m_GameManager;
          private bool m_IsGameInitialized;
          private int m_Level;

          public PlayScreen(Game i_Game)
               : base(i_Game)
          {
               m_Level = 1;
               r_Background = new Background(i_Game);
               BlendState = BlendState.NonPremultiplied;
          }

          public override void Initialize()
          {
               Add(r_Background);
               base.Initialize();
          }

          public override void Update(GameTime i_GameTime)
          {
               if(!m_IsGameInitialized)
               {
                    m_IsGameInitialized = true;
                    initializeGameManager();
               }

               if(m_Level != m_GameManager.Level)
               {
                    m_Level = m_GameManager.Level;
                    m_GameManager.PresetLevel();
               }

               SoundManager.CheckIfUserWantsToToggleSound(InputManager as InputManager, Keys.M);
               Enemy.HitRightWall = false;
               Enemy.HitLeftWall = false;
               m_GameManager.AnalyzeEnemyBounds();
               if(InputManager.KeyboardState.IsKeyDown(Keys.P))
               {
                    ScreensManager.SetCurrentScreen(new PauseScreen(Game));
               }

               checkIfGameIsOver();
               checkIfLevelEnded();
               base.Update(i_GameTime);
          }

          private void checkIfLevelEnded()
          {
               if (m_GameManager.LevelEnded())
               {
                    killAll();
                    m_GameManager.Level++;
                    ScreensManager.SetCurrentScreen(new TransitionScreen(Game, m_GameManager.Level));
               }
          }

          private void checkIfGameIsOver()
          {
               if(m_GameManager.CheckIfGameIsOver())
               {
                    int player2Score = -1;
                    string winnerName = string.Empty;
                    if(m_GameManager.ShipsManager.Ships.Count > 1)
                    {
                         player2Score = m_GameManager.ShipsManager.Ships[m_GameManager.ShipsManager.Ships.Count - 1]
                              .Score;
                         winnerName = m_GameManager.WinnerName;
                    }

                    GameOverScreen gameOverScreen = PreviousScreen as GameOverScreen;
                    if(gameOverScreen != null)
                    {
                         gameOverScreen.Player1Score = m_GameManager.ShipsManager.Ships[0].Score;
                         gameOverScreen.Player2Score = player2Score;
                         gameOverScreen.Winner = winnerName;
                    }

                    killAll();
                    ExitScreen();
               }
          }

          private void initializeGameManager()
          {
               m_GameManager = new GameManager(Game);
               Add(m_GameManager);
          }

          private void killAll()
          {
               m_GameManager.KillAll();
          }
     }
}
