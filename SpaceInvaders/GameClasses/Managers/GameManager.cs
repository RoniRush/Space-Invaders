namespace SpaceInvaders.GameClasses.Managers
{
     using global::SpaceInvaders.GameClasses.Sprites;
     using Infrastructure.ObjectModel;
     using Infrastructure.ServiceInterfaces;
     using Microsoft.Xna.Framework;
     using Microsoft.Xna.Framework.Graphics;

     public class GameManager : CompositeDrawableComponent<IGameComponent>
     {
          private float m_EnemyOriginalHeight;
          private float m_EnemyOriginalWidth;

          public string WinnerName { get; set; }

          public ISoundManager SoundManager { get; private set; }

          public int CurrentTotalScore { get; set; }

          public EnemyMatrixManager EnemyMatrixManager { get; private set; }

          public ShipsManager ShipsManager { get; private set; }

          public MotherShip MotherShip { get; private set; }

          public BarrierManager BarrierManager { get; private set; }

          public int Level { get; set; }

          public GameManager(Game i_Game)
               : base(i_Game)
          {
               Level = 1;
               BarrierManager = new BarrierManager(i_Game, Level);
               EnemyMatrixManager = new EnemyMatrixManager(i_Game, Level);
               ShipsManager = new ShipsManager(i_Game);
               MotherShip = new MotherShip(i_Game);
               CurrentTotalScore = 0;
               SoundManager = Game.Services.GetService(typeof(ISoundManager)) as ISoundManager;
               BlendState = BlendState.NonPremultiplied;
          }

          public override void Initialize()
          {
               Add(BarrierManager);
               Add(EnemyMatrixManager);
               Add(ShipsManager);
               Add(MotherShip);
               base.Initialize();
          }

          protected override void LoadContent()
          {
               base.LoadContent();
               initGameComponentsPositions();
          }

          public void PresetLevel()
          {
               BarrierManager.PresetLevel(Level);
               MotherShip.ResetPosition();
               EnemyMatrixManager.PresetLevel(Level);
               ShipsManager.PresetLevel();
               initGameComponentsPositions();
          }

          private void initGameComponentsPositions()
          {
               EnemyMatrixManager.InitPositions();
               m_EnemyOriginalHeight = EnemyMatrixManager.EnemyMatrix[0][0].Height;
               m_EnemyOriginalWidth = EnemyMatrixManager.EnemyMatrix[0][0].Width;
               ShipsManager.InitShipsPositions();
               BarrierManager.InitBarriersPositions(ShipsManager.ShipsYPosition());
          }

          public bool CheckIfGameIsOver()
          {
               bool isGameOver = false;
               if (EnemyAtBottom() || !ShipsManager.IsThereAShipAlive())
               {
                    SoundManager.PlaySound(GameSettings.s_SoundEffectsNames[GameSettings.eSoundEffects.GameOver]);
                    isGameOver = true;
                    WinnerName = ShipsManager.WhoWon();
               }

               return isGameOver;
          }

          public void KillAll()
          {
               BarrierManager.KillAll();
               EnemyMatrixManager.KillAll();
               ShipsManager.KillAll();
               MotherShip.CommitSuicide();
          }

          public void AnalyzeEnemyBounds()
          {
               if (EnemyMatrixManager.RightBoundEnemy != null)
               {
                    if (EnemyMatrixManager.RightBoundEnemy.Position.X
                        >= Game.GraphicsDevice.Viewport.Width - m_EnemyOriginalWidth)
                    {
                         Enemy.HitRightWall = true;
                    }
               }

               if (EnemyMatrixManager.LeftBoundEnemy != null)
               {
                    if (EnemyMatrixManager.LeftBoundEnemy.Position.X <= 0)
                    {
                         Enemy.HitLeftWall = true;
                    }
               }
          }

          public bool LevelEnded()
          {
               bool levelEnded = EnemyMatrixManager.TotalEnemiesStillBreathing == 0;
               if(levelEnded)
               {
                    SoundManager.PlaySound(GameSettings.s_SoundEffectsNames[GameSettings.eSoundEffects.LevelWin]);
               }

               return levelEnded;
          }

          public bool EnemyAtBottom()
          {
               bool isEnemyAtBottom = false;
               if (EnemyMatrixManager.BottomBoundEnemy != null)
               {
                    isEnemyAtBottom = EnemyMatrixManager.BottomBoundEnemy.Position.Y + m_EnemyOriginalHeight
                                      >= ShipsManager.ShipsYPosition();
               }

               return isEnemyAtBottom;
          }
     }
}
