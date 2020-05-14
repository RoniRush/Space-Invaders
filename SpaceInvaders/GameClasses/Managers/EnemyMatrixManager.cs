namespace SpaceInvaders.GameClasses.Managers
{
     using global::SpaceInvaders.GameClasses.Sprites;
     using Infrastructure.ObjectModel;
     using Microsoft.Xna.Framework;
     using Microsoft.Xna.Framework.Graphics;

     public class EnemyMatrixManager : CompositeDrawableComponent<IGameComponent>
     {
          public enum eEnemies
          {
               Pink,
               Blue,
               Yellow
          }

          private const int k_TotalEnemyRows = 5;
          private const int k_TotalPinkEnemyLines = 1;
          private const int k_TotalBlueEnemyLines = 2;
          private const int k_TotalKindsOfEnemies = 3;
          private const int k_AddedScorePerLevel = 140;
          private const int k_InitialTotalEnemyCols = 9;
          private readonly Color[] r_EnemiesColorByEnum = { Color.LightPink, Color.LightBlue, Color.Yellow };
          private readonly int[] r_InitialEnemiesScoreByEnum = { 250, 150, 100 };
          private int m_TotalEnemyCols;
          private int[] m_EnemiesScoreByEnum;
          private int[] m_EnemiesMaxBulletsByEnum;
          private int m_KillCount;

          public Enemy LeftBoundEnemy { get; private set; }

          public Enemy RightBoundEnemy { get; private set; }

          public Enemy BottomBoundEnemy { get; private set; }

          public Enemy[][] EnemyMatrix { get; private set; }

          public int TotalEnemiesStillBreathing { get; set; }

          public EnemyMatrixManager(Game i_Game, int i_Level)
          : base(i_Game)
          {
               BlendState = BlendState.NonPremultiplied;
               m_EnemiesScoreByEnum = new int[r_InitialEnemiesScoreByEnum.Length];
               m_EnemiesMaxBulletsByEnum = new int[r_EnemiesColorByEnum.Length];
               PresetLevel(i_Level);
          }

          private void initMatrixProperties()
          {
               TotalEnemiesStillBreathing = 0;
               m_KillCount = 0;
               buildMatrix();
               LeftBoundEnemy = EnemyMatrix[0][0];
               RightBoundEnemy = EnemyMatrix[0][m_TotalEnemyCols - 1];
               BottomBoundEnemy = EnemyMatrix[k_TotalEnemyRows - 1][0];
               Enemy.NumOfEnemies = k_TotalKindsOfEnemies;
          }

          private void buildMatrix()
          {
               eEnemies enemyToCreate;

               int numOfImageFromTexture = 0;
               for (int i = 0; i < k_TotalEnemyRows; i++)
               {
                    if (i < k_TotalPinkEnemyLines)
                    {
                         enemyToCreate = eEnemies.Pink;
                    }
                    else if (i <= k_TotalBlueEnemyLines)
                    {
                         enemyToCreate = eEnemies.Blue;
                    }
                    else
                    {
                         enemyToCreate = eEnemies.Yellow;
                    }

                    EnemyMatrix[i] = buildLine(enemyToCreate, numOfImageFromTexture);
                    numOfImageFromTexture++;
                    numOfImageFromTexture %= 2;
               }
          }

          public void PresetLevel(int i_Level)
          {
               EnemyMatrix = new Enemy[k_TotalEnemyRows][];
               if ((i_Level - 1) % 5 == 0)
               {
                    // level 1 and the likes
                    m_TotalEnemyCols = k_InitialTotalEnemyCols;
                    resetEnemyScoreAndMaxBullets();
               }
               else
               {
                    // level 2-5 and the likes
                    m_TotalEnemyCols = ((i_Level - 1) % 5) + k_InitialTotalEnemyCols;
                    updateEnemyMaxBulletPerLevel(i_Level);
                    updateEnemyScorePerLevel();
               }

               initMatrixProperties();
          }

          private void updateEnemyMaxBulletPerLevel(int i_Level)
          {
               m_EnemiesMaxBulletsByEnum[i_Level % 2]++;
          }

          private void resetEnemyScoreAndMaxBullets()
          {
               for (int i = 0; i < r_EnemiesColorByEnum.Length; i++)
               {
                    m_EnemiesScoreByEnum[i] = r_InitialEnemiesScoreByEnum[i];
                    m_EnemiesMaxBulletsByEnum[i] = 1;
               }
          }

          private void updateEnemyScorePerLevel()
          {
               for(int i = 0; i < m_EnemiesScoreByEnum.Length; i++)
               {
                    m_EnemiesScoreByEnum[i] += k_AddedScorePerLevel;
               }
          }

          private Enemy[] buildLine(eEnemies i_EnemyToCreate, int i_NumOfImageFromTexture)
          {
               Enemy[] enemyLine = new Enemy[m_TotalEnemyCols];
               for (int j = 0; j < m_TotalEnemyCols; j++)
               {
                    switch (i_EnemyToCreate)
                    {
                         case eEnemies.Pink:
                              {
                                   enemyLine[j] = new Enemy(Game, (int)eEnemies.Pink, i_NumOfImageFromTexture, m_EnemiesScoreByEnum[(int)eEnemies.Pink], r_EnemiesColorByEnum[(int)eEnemies.Pink], m_EnemiesMaxBulletsByEnum[(int)eEnemies.Pink]);
                                   break;
                              }

                         case eEnemies.Blue:
                              {
                                   enemyLine[j] = new Enemy(Game, (int)eEnemies.Blue, i_NumOfImageFromTexture, m_EnemiesScoreByEnum[(int)eEnemies.Blue], r_EnemiesColorByEnum[(int)eEnemies.Blue], m_EnemiesMaxBulletsByEnum[(int)eEnemies.Blue]);
                                   break;
                              }

                         case eEnemies.Yellow:
                              {
                                   enemyLine[j] = new Enemy(Game, (int)eEnemies.Yellow, i_NumOfImageFromTexture, m_EnemiesScoreByEnum[(int)eEnemies.Yellow], r_EnemiesColorByEnum[(int)eEnemies.Yellow], m_EnemiesMaxBulletsByEnum[(int)eEnemies.Yellow]);
                                   break;
                              }
                    }

                    enemyLine[j].m_EnemyDiedDelegate = calcBoundsPostMortem;
                    addEnemyComponents(enemyLine[j]);
                    TotalEnemiesStillBreathing++;
               }

               return enemyLine;
          }

          private void addEnemyComponents(Enemy i_Enemy)
          {
               Add(i_Enemy);
               foreach (Bullet bullet in i_Enemy.EnemyBullets)
               {
                    Add(bullet);
               }
          }

          public void KillAll()
          {
               foreach(Enemy[] enemyLine in EnemyMatrix)
               {
                    foreach(Enemy enemy in enemyLine)
                    {
                         enemy.Shooter.KillAllBullets();
                         enemy.CommitSuicide();
                         foreach (Bullet bullet in enemy.EnemyBullets)
                         {
                              Remove(bullet);
                         }

                         Remove(enemy);
                    }
               }
          }

          private void calcBoundsPostMortem(Enemy i_KilledEnemy)
          {
               TotalEnemiesStillBreathing--;
               calcVelocityIncEvery5Deaths();
               if (i_KilledEnemy == RightBoundEnemy)
               {
                    RightBoundEnemy = calcNewBounds(i_KilledEnemy, -1, m_TotalEnemyCols, k_TotalEnemyRows);
               }

               if (i_KilledEnemy == LeftBoundEnemy)
               {
                    LeftBoundEnemy = calcNewBounds(i_KilledEnemy, 1, m_TotalEnemyCols, k_TotalEnemyRows);
               }

               if (i_KilledEnemy == BottomBoundEnemy)
               {
                    BottomBoundEnemy = calcNewBounds(i_KilledEnemy, -1, k_TotalEnemyRows, m_TotalEnemyCols);
               }
          }

          private Enemy calcNewBounds(Enemy i_KilledEnemy, int i_Adder, int i_VerticalBound, int i_HorizontalBound)
          {
               Enemy newRep = null;
               findEnemyInMatrix(i_KilledEnemy, out int o_DeadEnemyRow, out int o_DeadEnemyCol);
               if (o_DeadEnemyCol != -1 && o_DeadEnemyRow != -1)
               {
                    int cell = i_VerticalBound == m_TotalEnemyCols ? o_DeadEnemyCol : o_DeadEnemyRow;
                    newRep = getNewBoundsRep(i_Adder, cell, i_VerticalBound, i_HorizontalBound);
               }

               return newRep;
          }

          private Enemy getNewBoundsRep(int i_Adder, int i_Cell, int i_VerticalBound, int i_HorizontalBound)
          {
               Enemy newBoundEnemy = null;
               bool found = false;
               int x = i_Cell;
               while (!found && x > -1 && x < i_VerticalBound)
               {
                    for (int i = 0; i < i_HorizontalBound; i++)
                    {
                         Enemy enemy = i_VerticalBound == m_TotalEnemyCols ? EnemyMatrix[i][x] : EnemyMatrix[x][i];
                         if (enemy.IsAlive)
                         {
                              newBoundEnemy = enemy;
                              found = true;
                         }
                    }

                    x += i_Adder;
               }

               return newBoundEnemy;
          }

          private void calcVelocityIncEvery5Deaths()
          {
               m_KillCount++;
               if (m_KillCount % 5 == 0)
               {
                    for (int i = 0; i < k_TotalEnemyRows; i++)
                    {
                         for (int j = 0; j < m_TotalEnemyCols; j++)
                         {
                              Enemy enemyToUpdate = EnemyMatrix[i][j];
                              enemyToUpdate.Velocity = new Vector2(enemyToUpdate.Velocity.X * enemyToUpdate.FixedIncVelocityPer5Kills, enemyToUpdate.Velocity.Y);
                         }
                    }
               }
          }

          private void findEnemyInMatrix(Enemy i_KilledEnemy, out int o_DeadEnemyRow, out int o_DeadEnemyCol)
          {
               int row = -1;
               int col = -1;
               for (int i = 0; i < k_TotalEnemyRows; i++)
               {
                    for (int j = 0; j < m_TotalEnemyCols; j++)
                    {
                         if (EnemyMatrix[i][j] == i_KilledEnemy)
                         {
                              row = i;
                              col = j;
                              break;
                         }
                    }
               }

               o_DeadEnemyRow = row;
               o_DeadEnemyCol = col;
          }

          public void InitPositions()
          {
               float enemyWidth = EnemyMatrix[0][0].Width;
               float distance = enemyWidth + (enemyWidth * 0.6f);
               EnemyMatrix[0][0].Position = new Vector2(10, enemyWidth * 3);
               Vector2 prevPositionByX = EnemyMatrix[0][0].Position;
               Vector2 prevPositionByY = EnemyMatrix[0][0].Position;
               for (int i = 0; i < k_TotalEnemyRows; i++)
               {
                    if (i != 0)
                    {
                         EnemyMatrix[i][0].Position = new Vector2(10, prevPositionByY.Y + distance);
                         prevPositionByX = EnemyMatrix[i][0].Position;
                         prevPositionByY = EnemyMatrix[i][0].Position;
                    }

                    for (int j = 1; j < m_TotalEnemyCols; j++)
                    {
                         EnemyMatrix[i][j].Position = new Vector2(prevPositionByX.X + distance, prevPositionByX.Y);
                         prevPositionByX = EnemyMatrix[i][j].Position;
                    }
               }
          }
     }
}
