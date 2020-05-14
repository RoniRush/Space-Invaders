namespace SpaceInvaders.GameClasses.Sprites
{
     using System;
     using Infrastructure.ObjectModel.Animators.ConcreteAnimators;
     using Infrastructure.ServiceInterfaces;
     using Microsoft.Xna.Framework;

     public class MotherShip : SpriteAdapter, ICollidable2D, IScorable
     {
          private const string k_MotherShipPath = @"Sprites/MotherShip_32x120";
          private const int k_MaxValueNumber = 10000;
          private const float k_BlinkLength = 0.1f;
          private const float k_AnimationDuration = 2.2f;
          private const int k_RandomAppearancePartition = 9990;
          private const int k_ScoreGiven = 800;
          private const float k_Velocity = 100;
          private readonly Color r_TeamColor = Color.Blue;
          private Random m_RandomGenerator;
          private bool m_InAnimation;

          public MotherShip(Game i_Game)
          : base(k_MotherShipPath, i_Game)
          {
               Velocity = new Vector2(k_Velocity, 0);
               TintColor = Color.Red;
               Visible = false;
               IsAlive = false;
               m_RandomGenerator = new Random();
               m_InAnimation = false;
               TeamColor = r_TeamColor;
          }

          protected override void InitBounds()
          {
               base.InitBounds();
               ResetPosition();
          }

          public void ResetPosition()
          {
               Position = new Vector2(0 - Width, Height);
          }

          public int ScoreGiven()
          {
               return k_ScoreGiven;
          }

          public override void Initialize()
          {
               base.Initialize();
               initAnimations();
          }

          public override void Update(GameTime i_GameTime)
          {
               if (!m_InAnimation)
               {
                    if (!Visible)
                    {
                         if (m_RandomGenerator.Next(0, k_MaxValueNumber) >= k_RandomAppearancePartition)
                         {
                              Position = new Vector2(-Width, Position.Y);
                              Visible = true;
                              IsAlive = true;
                         }
                    }

                    if (Position.X >= Game.GraphicsDevice.Viewport.Width && IsAlive)
                    {
                         Visible = false;
                    }
               }

               base.Update(i_GameTime);
          }

          public override void Collided(ICollidable i_Collidable)
          {
               SoundManager.PlaySound(GameSettings.s_SoundEffectsNames[GameSettings.eSoundEffects.MotherShipKilled]);
               IsAlive = false;
               m_InAnimation = true;
               Animations.Resume();
               Animations["Blink"].Resume();
               Animations["Fade"].Resume();
               Animations["ChangeScale"].Resume();
          }

          private void initAnimations()
          {
               Animations.Add(new BlinkAnimator(
                    "Blink",
                    TimeSpan.FromSeconds(k_BlinkLength),
                    TimeSpan.FromSeconds(k_AnimationDuration)));
               Animations.Add(new FadeAnimator("Fade", TimeSpan.FromSeconds(k_AnimationDuration), Opacity));
               Animations.Add(new ChangeScaleAnimator("ChangeScale", TimeSpan.FromSeconds(k_AnimationDuration), Scales));
               Animations.Enabled = false;
               Animations["ChangeScale"].Finished += changeScaleAnimation_Finished;
          }

          private void changeScaleAnimation_Finished(object sender, EventArgs e)
          {
               Animations.Pause();
               Animations["Blink"].Pause();
               Animations["Fade"].Pause();
               Animations["ChangeScale"].Pause();
               m_InAnimation = false;
               Animations.Reset();
               Visible = false;
          }
     }
}
