using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvaders
{
     using System.Linq;

     using global::SpaceInvaders.GameClasses;
     using global::SpaceInvaders.GameClasses.Screens;
     using Infrastructure.Managers;

     /// <summary>
     /// This is the main type for your game.
     /// </summary>
     public class SpaceInvaders : Game
     {
          private GraphicsDeviceManager m_Graphics;
          private ScreensManager m_ScreensMananger;

          public SpaceInvaders()
          {
               m_Graphics = new GraphicsDeviceManager(this);
               m_Graphics.PreferredBackBufferWidth = GameSettings.sr_DefaultScreenWidth;
               m_Graphics.PreferredBackBufferHeight = GameSettings.sr_DefaultScreenHeight;
               Content.RootDirectory = "Content";
               m_ScreensMananger = new ScreensManager(this);
               m_ScreensMananger.GraphicsDeviceManager = m_Graphics;
               m_ScreensMananger.Push(new GameOverScreen(this));
               m_ScreensMananger.Push(new PlayScreen(this));
               m_ScreensMananger.Push(new TransitionScreen(this, 1));
               m_ScreensMananger.SetCurrentScreen(new WelcomeScreen(this));
               InputManager inputManager = new InputManager(this);
               CollisionsManager collisionManager = new CollisionsManager(this);
               SoundManager soundManager = new SoundManager(this);
               soundManager.InitSoundEffects(GameSettings.s_SoundEffectsNames.Values.ToList(), GameSettings.s_BackgroundSoundName);
               soundManager.InitDefaultVolumes(GameSettings.sr_DefaultBGVolume, GameSettings.sr_DefaultSoundEffectVolume);
               IsMouseVisible = true;
          }

          protected override void Update(GameTime i_GameTime)
          {
               if (Keyboard.GetState().IsKeyDown(Keys.Escape))
               {
                    Exit();
               }

               base.Update(i_GameTime);
          }

          protected override void Draw(GameTime i_GameTime)
          {
               GraphicsDevice.Clear(Color.Black);
               base.Draw(i_GameTime);
          }
     }
}