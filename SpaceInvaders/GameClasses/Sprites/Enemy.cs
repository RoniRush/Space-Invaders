using System;

namespace SpaceInvaders.GameClasses.Sprites
{
     using System.Collections.Generic;
     using Infrastructure.ObjectModel.Animators.ConcreteAnimators;
     using Infrastructure.ServiceInterfaces;
     using Microsoft.Xna.Framework;

     public delegate void EnemyDiedDelegate(Enemy i_KilledEnemy);

     public class Enemy : SpriteAdapter, ICollidable2D, IScorable
     {
          private const int k_MaxValueNumber = 10000;
          private const float k_AnimationDuration = 1.2f;
          private const int k_RandomAppearancePartition = 9995;
          private const float k_CellTime = 0.5f;
          private const float k_FixedIncVelocity = 1.05f;
          private const float k_AngularVelocity = MathHelper.TwoPi * 6;
          private const int k_NumOfCells = 2;
          private const string k_EnemiesPath = @"Sprites/Enemies";
          private const float k_FixedIncVelocityPer5Kills = 1.03f;
          private static readonly Random sr_RandomGenerator = new Random();
          private static readonly int r_EnemyInitialVelocity = 50;
          private readonly Color r_TeamColor = Color.Blue;
          private readonly Shooter r_Shooter;
          private readonly int r_Score;
          private readonly Color r_BulletColor = Color.Blue;
          private readonly int r_NumOfTexture;
          private readonly int r_StartIndexInTexture;
          private int m_MaxNumOfBullets;
          private bool m_AreMovingDown;
          public EnemyDiedDelegate m_EnemyDiedDelegate;

          public static int NumOfEnemies { get; set; } = 0;

          public static bool HitRightWall { get; set; }

          public static bool HitLeftWall { get; set; }

          public int ScoreGiven()
          {
               return r_Score;
          }

          public Shooter Shooter
          {
               get
               {
                    return r_Shooter;
               }
          }

          public List<Bullet> EnemyBullets
          {
               get
               {
                    return r_Shooter.BulletList;
               }
          }

          public float FixedIncVelocityPer5Kills
          {
               get
               {
                    return k_FixedIncVelocityPer5Kills;
               }
          }

          public Enemy(Game i_Game, int i_NumOfTexture, int i_NumOfImageFromTexture, int i_Score, Color i_TintColor, int i_MaxNumOfBullets)
          : base(k_EnemiesPath, i_Game)
          {
               m_MaxNumOfBullets = i_MaxNumOfBullets;
               r_NumOfTexture = i_NumOfTexture;
               Velocity = new Vector2(r_EnemyInitialVelocity, 0);
               m_AreMovingDown = false;
               r_StartIndexInTexture = i_NumOfImageFromTexture;
               TintColor = i_TintColor;
               TeamColor = r_TeamColor;
               IsAlive = true;
               r_Score = i_Score;
               r_Shooter = new Shooter(m_MaxNumOfBullets, Game, r_BulletColor, true, false);
          }

          public override void Initialize()
          {
               base.Initialize();
               initAnimations();
          }

          private void initAnimations()
          {
               CellAnimator cellAnimator = new CellAnimator("CellAnimation", TimeSpan.FromSeconds(k_CellTime), k_NumOfCells, TimeSpan.Zero, this.r_StartIndexInTexture);
               RotateAnimator rotateAnimation = new RotateAnimator("Rotate", k_AngularVelocity, TimeSpan.FromSeconds(k_AnimationDuration));
               ChangeScaleAnimator changeScaleAnimation = new ChangeScaleAnimator("ChangeScale", TimeSpan.FromSeconds(k_AnimationDuration), Scales);
               Animations.Add(rotateAnimation);
               Animations.Add(cellAnimator);
               Animations.Add(changeScaleAnimation);
               Animations.Resume();
               Animations["CellAnimation"].Resume();
               changeScaleAnimation.Finished += changeScaleAnimation_Finished;
          }

          private void changeScaleAnimation_Finished(object sender, EventArgs e)
          {
               Animations["Rotate"].Pause();
               Animations["ChangeScale"].Pause();
               Animations["Rotate"].Reset();
               Animations["ChangeScale"].Reset();
               Visible = false;
          }

          protected override void InitSourceRectangle()
          {
               base.InitSourceRectangle();
               WidthBeforeScale /= k_NumOfCells;
               HeightBeforeScale /= NumOfEnemies;
               SourceRectangle = new Rectangle(
                    (int)(Width * r_StartIndexInTexture), (int)(Height * r_NumOfTexture), (int)Width, (int)Height);
               RotationOrigin = SourceRectangleCenter;
          }

          public override void Update(GameTime i_GameTime)
          {
               if (IsAlive)
               {
                    if (sr_RandomGenerator.Next(0, k_MaxValueNumber) >= k_RandomAppearancePartition && Visible)
                    {
                         r_Shooter.Shoot(new Vector2(Position.X + (0.5f * Width), Position.Y + Height));
                         SoundManager.PlaySound(GameSettings.s_SoundEffectsNames[GameSettings.eSoundEffects.EnemyShot]);
                    }
               }

               manageMovement();
               base.Update(i_GameTime);
          }

          private void manageMovement()
          {
               if (HitRightWall)
               {
                    m_AreMovingDown = true;
               }
               else if (HitLeftWall)
               {
                    m_AreMovingDown = true;
               }

               if (m_AreMovingDown)
               {
                    m_AreMovingDown = false;
                    Position = new Vector2(Position.X, Position.Y + (Width / 2f));
                    Velocity = new Vector2((Velocity.X * k_FixedIncVelocity) * -1, Velocity.Y);
               }
          }

          public override void Collided(ICollidable i_Collidable)
          {
               if (IsAlive)
               {
                    SoundManager.PlaySound(GameSettings.s_SoundEffectsNames[GameSettings.eSoundEffects.EnemyKilled]);
                    SpriteAdapter spriteAdapter = i_Collidable as SpriteAdapter;
                    if (spriteAdapter != null)
                    {
                         if (spriteAdapter.TeamColor != NeutralColor)
                         {
                              IsAlive = false;
                              m_EnemyDiedDelegate?.Invoke(this);
                              Animations.Resume();
                              Animations["ChangeScale"].Resume();
                              Animations["Rotate"].Resume();
                         }
                    }
               }
          }
     }
}
