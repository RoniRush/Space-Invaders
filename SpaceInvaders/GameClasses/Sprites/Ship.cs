using System.Collections.Generic;

namespace SpaceInvaders.GameClasses.Sprites
{
     using System;
     using Infrastructure.Managers;
     using Infrastructure.ObjectModel;
     using Infrastructure.ObjectModel.Animators.ConcreteAnimators;
     using Infrastructure.ServiceInterfaces;
     using Microsoft.Xna.Framework;
     using Microsoft.Xna.Framework.Input;

     public class Ship : SpriteAdapter, ICollidable2D
     {
          private const int k_MaximumLives = 3;
          private const float k_AngularVelocity = MathHelper.TwoPi * 4;
          private const float k_AnimationTime = 2.5f;
          private const float k_ShipVelocityByKeyboard = 145;
          private const float k_BlinkLength = (float)(0.5 / 3);
          private const int k_LifeLostPenalty = -1200;
          private const int k_MaxActiveBullets = 2;
          private readonly Color r_TeamColor = Color.Red;
          private readonly Color r_BulletColor = Color.Red;
          private readonly Shooter r_Shooter;
          private Keys m_RightMovementKey;
          private Keys m_LeftMovementKey;
          private Keys m_ShootKey;
          private InputManager m_InputManager;
          private bool m_WasScoreUpdated;
          private List<RemainingLife> m_RemainingLivesTextures;

          public Color PlayerColor { get; set; }

          public List<Bullet> ShipBullets
          {
               get
               {
                    return r_Shooter.BulletList;
               }
          }

          public Shooter Shooter
          {
               get
               {
                    return r_Shooter;
               }
          }

          public int Score { get; set; }

          public bool CanUseTheMouse { get; set; }

          public int RemainingLives { get; set; }

          public Font FontForScore { get; set; }

          public List<RemainingLife> RemainingLivesTextures
          {
               get
               {
                    return m_RemainingLivesTextures;
               }
          }

          public Ship(Game i_Game, string i_ShipPath) : base(i_ShipPath, i_Game)
          {
               RemainingLives = k_MaximumLives;
               Visible = true;
               TeamColor = r_TeamColor;
               Score = 0;
               m_RemainingLivesTextures = new List<RemainingLife>(k_MaximumLives);
               addRemainingLifeTexture();
               r_Shooter = new Shooter(k_MaxActiveBullets, Game, r_BulletColor, false, true);
          }

          private void addRemainingLifeTexture()
          {
               for (int i = 0; i < k_MaximumLives; i++)
               {
                    m_RemainingLivesTextures.Add(new RemainingLife(Game, AssetName));
               }
          }

          private void initAnimations()
          {
               Animations.Add(
                    new BlinkAnimator("Blink", TimeSpan.FromSeconds(k_BlinkLength), TimeSpan.FromSeconds(k_AnimationTime))
                    {
                         ResetAfterFinish = false
                    });
               Animations.Add(new RotateAnimator("Rotate", k_AngularVelocity, TimeSpan.FromSeconds(k_AnimationTime)) { ResetAfterFinish = false });
               Animations.Add(new FadeAnimator("Fade", TimeSpan.FromSeconds(k_AnimationTime), Opacity));
               Animations.Enabled = false;
               Animations["Blink"].Finished += blinkAnimator_Finished;
               Animations["Fade"].Finished += fadeAnimation_Finished;
          }

          private void blinkAnimator_Finished(object sender, EventArgs e)
          {
               Animations["Blink"].Reset();
               Animations["Blink"].Enabled = false;
               Animations.Enabled = false;
               IsAlive = true;
               Position = new Vector2(10, Game.GraphicsDevice.Viewport.Height - Texture.Height - 20);
          }

          private void fadeAnimation_Finished(object sender, EventArgs e)
          {
               Visible = false;
               Animations.Enabled = false;
               Animations["Fade"].Reset();
               Animations["Rotate"].Reset();
          }

          public void InitRemainingLifePosition()
          {
               int positionX = Game.GraphicsDevice.Viewport.Width - 5;
               int positionY = (int)Position.X;
               int index = 1;
               foreach (RemainingLife remainLive in m_RemainingLivesTextures)
               {
                    positionX -= index * (int)remainLive.Width;
                    remainLive.Position = new Vector2(positionX, positionY);
                    positionX -= 5;
               }
          }

          public void InitScorePosition()
          {
               FontForScore.Position = new Vector2(5, (int)Position.X);
          }

          protected override void InitSourceRectangle()
          {
               base.InitSourceRectangle();
               RotationOrigin = SourceRectangleCenter;
          }

          public override void Initialize()
          {
               base.Initialize();
               m_InputManager = (Game.Services.GetService(typeof(IInputManager)) as IInputManager) as InputManager;
               initAnimations();
          }

          public void SetKeyboardControls(Keys i_Right, Keys i_Left, Keys i_Shoot)
          {
               m_RightMovementKey = i_Right;
               m_LeftMovementKey = i_Left;
               m_ShootKey = i_Shoot;
          }

          public override void Update(GameTime i_GameTime)
          {
               if (m_InputManager != null)
               {
                    if (IsAlive && Visible)
                    {
                         manageShooting(m_InputManager);
                    }

                    checkMovement();
               }

               if(m_WasScoreUpdated)
               {
                    FontForScore.UpdateMessage(Score.ToString());
                    m_WasScoreUpdated = false;
               }

               base.Update(i_GameTime);
          }

          public override void Collided(ICollidable i_Collidable)
          {
               RemainingLives--;
               removeLifeTexture();
               IsAlive = false;
               Animations.Enabled = true;
               m_WasScoreUpdated = true;
               SoundManager.PlaySound(GameSettings.s_SoundEffectsNames[GameSettings.eSoundEffects.LifeDie]);
               if (Score + k_LifeLostPenalty < 0)
               {
                    Score = 0;
               }
               else
               {
                    Score += k_LifeLostPenalty;
               }

               if (RemainingLives == 0)
               {
                    Animations["Rotate"].Enabled = true;
                    Animations["Fade"].Enabled = true;
               }
               else
               {
                    Animations["Blink"].Enabled = true;
               }
          }

          private void removeLifeTexture()
          {
               m_RemainingLivesTextures[m_RemainingLivesTextures.Count - 1].RemoveLife();
               m_RemainingLivesTextures.Remove(m_RemainingLivesTextures[m_RemainingLivesTextures.Count - 1]);
          }

          private void manageShooting(InputManager i_InputManager)
          {
               if ((i_InputManager.KeyboardState.IsKeyDown(m_ShootKey) && i_InputManager.PrevKeyboardState.IsKeyUp(m_ShootKey))
                   || (i_InputManager.MouseState.LeftButton == ButtonState.Pressed && i_InputManager.PrevMouseState.LeftButton == ButtonState.Released && CanUseTheMouse))
               {
                    Bullet bullet = r_Shooter.Shoot(new Vector2(Position.X + (0.5f * Width), Position.Y));
                    if(bullet != null)
                    {
                         bullet.m_BulletHitDelegate = addScoreToShip;
                         SoundManager.PlaySound(GameSettings.s_SoundEffectsNames[GameSettings.eSoundEffects.ShipShot]);
                    }
               }
          }

          private void checkMovement()
          {
               if (m_InputManager.KeyboardState.IsKeyDown(m_RightMovementKey))
               {
                    Velocity = new Vector2(k_ShipVelocityByKeyboard, Velocity.Y);
               }

               if (m_InputManager.KeyboardState.IsKeyDown(m_LeftMovementKey))
               {
                    Velocity = new Vector2(-k_ShipVelocityByKeyboard, Velocity.Y);
               }

               if (m_InputManager.KeyboardState.IsKeyUp(m_RightMovementKey)
                  && m_InputManager.KeyboardState.IsKeyUp(m_LeftMovementKey))
               {
                    Velocity = new Vector2(0, Velocity.Y);
               }

               if (CanUseTheMouse)
               {
                    Vector2 mousePositionDelta = m_InputManager.MousePositionDelta;
                    Position = new Vector2(Position.X + mousePositionDelta.X, Position.Y);
               }

               Position = new Vector2(MathHelper.Clamp(Position.X, 0, GraphicsDevice.Viewport.Width - Width), Position.Y);
          }

          private void addScoreToShip(Bullet i_Bullet)
          {
               Score += i_Bullet.HoldScore;
               FontForScore.UpdateMessage(Score.ToString());
          }
     }
}
