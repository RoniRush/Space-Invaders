namespace SpaceInvaders.GameClasses.Sprites
{
     using Infrastructure.ServiceInterfaces;
     using Microsoft.Xna.Framework;
     using Microsoft.Xna.Framework.Graphics;

     public class Barrier : SpriteAdapter, ICollidable2D
     {
          private const string k_BarrierPath = @"Sprites/Barrier_44x32";
          private int k_Velocity = 45;

          public Vector2 InitialPosition { get; set; }

          public Barrier(Game i_Game, float i_VelocityLevelMultiplier)
               : base(k_BarrierPath, i_Game)
          {
               TeamColor = Color.White;
               Velocity = new Vector2(k_Velocity * i_VelocityLevelMultiplier, 0);
          }

          protected override void LoadContent()
          {
               base.LoadContent();
               Texture2D newTexture = new Texture2D(GraphicsDevice, Texture.Width, Texture.Height);
               newTexture.SetData(TexturePixels);
               Texture = newTexture;
          }

          public override void Update(GameTime i_GameTime)
          {
               float minXPositionVal = InitialPosition.X - (Width / 2);
               float maxYPositionVal = InitialPosition.X + Width + (Width / 2);
               Position = new Vector2(MathHelper.Clamp(Position.X, minXPositionVal, maxYPositionVal), Position.Y);
               if (needToChangeDirection())
               {
                    Velocity *= -1;
               }

               base.Update(i_GameTime);
          }

          private bool needToChangeDirection()
          {
               bool getToLeftSide = Position.X.Equals(InitialPosition.X - (Width / 2));
               bool getToRightSide = Position.X.Equals(InitialPosition.X + Width + (Width / 2));
               return getToRightSide || getToLeftSide;
          }

          public override void Collided(ICollidable i_Collidable)
          {
               Enemy enemy = i_Collidable as Enemy;
               Bullet bullet = i_Collidable as Bullet;
               if (enemy != null)
               {
                    foreach (int pixel in CollidedPixels)
                    {
                         TexturePixels[pixel] = new Color(0, 0, 0, 0);
                    }
               }

               if (bullet != null)
               {
                    perPixelCollision(bullet);
               }

               Texture.SetData(TexturePixels);
          }

          private void perPixelCollision(Bullet i_Bullet)
          {
               GetCollidedRectangleBounds(out int positionXStart, out int positionXEnd, out int positionYStart, out int positionYEnd, i_Bullet);
               if (i_Bullet.Velocity.Y < 0)
               {
                    for (int i = positionYStart; i > positionYStart - (int)(i_Bullet.Height * 0.7f) && i >= Bounds.Y; i--)
                    {
                         biteTexture(positionXStart, positionXEnd, i);
                    }
               }
               else
               {
                    for (int i = positionYStart; i < positionYEnd + (int)(i_Bullet.Height * 0.7f) && i < this.Bounds.Bottom; i++)
                    {
                         biteTexture(positionXStart, positionXEnd, i);
                    }
               }
          }

          private void biteTexture(int i_PositionXStart, int i_PositionXEnd, int i_IndexByY)
          {
               SoundManager.PlaySound(GameSettings.s_SoundEffectsNames[GameSettings.eSoundEffects.BarrierHit]);
               for (int j = i_PositionXStart; j < i_PositionXEnd; j++)
               {
                    int indexPotentialCollisionPixelBarrier =
                         (j - Bounds.X) + ((i_IndexByY - Bounds.Y) * Texture.Width);
                    TexturePixels[indexPotentialCollisionPixelBarrier].A = 0;
               }
          }
     }
}
