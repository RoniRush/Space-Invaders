using System;

namespace SpaceInvaders.GameClasses.Sprites
{
     using Infrastructure.ServiceInterfaces;
     using Microsoft.Xna.Framework;

     public delegate void BulletHitDelegate(Bullet i_Bullet);

     public class Bullet : SpriteAdapter, ICollidable2D
     {
          private const int k_MaxValueNumber = 10;
          private const int k_RandomAppearancePartition = 50;
          private const string k_BulletPath = @"Sprites/Bullet";
          private const float k_Velocity = 160;
          private static readonly Random sr_RandomGenerator = new Random();
          private readonly bool r_IsGoingDown;
          private readonly bool r_SubMyHeightFromPosition;
          public BulletHitDelegate m_BulletHitDelegate;

          public int HoldScore { get; set; }

          public Bullet(Game i_Game, Color i_TintColor, Vector2 i_Position, bool i_IsGoingDown, bool i_AddBulletHightToPosition)
               : base(k_BulletPath, i_Game)
          {
               TintColor = TeamColor = i_TintColor;
               Visible = true;
               r_IsGoingDown = i_IsGoingDown;
               updateVelocityByDirection();
               r_SubMyHeightFromPosition = i_AddBulletHightToPosition;
               addPosition(i_Position);
          }

          private void addPosition(Vector2 i_Position)
          {
               Position = r_SubMyHeightFromPosition ? new Vector2(i_Position.X, i_Position.Y - Height) : i_Position;
          }

          private void updateVelocityByDirection()
          {
               Velocity = new Vector2(0, k_Velocity);
               if (!r_IsGoingDown)
               {
                    Velocity = new Vector2(Velocity.X, Velocity.Y * -1);
               }
          }

          public override void Collided(ICollidable i_Collidable)
          {
               Bullet bullet = i_Collidable as Bullet;
               bool dispose = true;
               if (bullet != null)
               {
                    if (TeamColor == Color.Blue)
                    {
                         dispose = randomCalculationIfIShouldDie();
                    }
               }
               else
               {
                    IScorable iScorable = i_Collidable as IScorable;
                    if (iScorable != null)
                    {
                         addScoresToMyShooter(iScorable);
                    }
               }

               if (dispose)
               {
                    IsAlive = false;
                    Visible = false;
               }
          }

          private void addScoresToMyShooter(IScorable i_Scorable)
          {
               HoldScore = i_Scorable.ScoreGiven();
               m_BulletHitDelegate?.Invoke(this);
          }

          private bool randomCalculationIfIShouldDie()
          {
               return !(sr_RandomGenerator.Next(0, k_MaxValueNumber) >= k_RandomAppearancePartition);
          }

          public override void Update(GameTime i_GameTime)
          {
               if (Visible)
               {
                    if ((Position.Y <= 0 - Height && !r_IsGoingDown)
                       || (Position.Y >= Game.GraphicsDevice.Viewport.Height && r_IsGoingDown))
                    {
                         IsAlive = false;
                         Visible = false;
                    }
               }

               base.Update(i_GameTime);
          }
     }
}
