namespace SpaceInvaders.GameClasses.Sprites
{
     using Infrastructure.ObjectModel;
     using Microsoft.Xna.Framework;

     public class Background : Sprite
     {
          private const string k_SpaceBackgroundPath = @"Sprites/BG_Space01_1024x768";

          public Background(Game i_Game)
               : base(k_SpaceBackgroundPath, i_Game)
          {
               setBackgroundTint();
               Visible = true;
          }

          protected override void InitBounds()
          {
               base.InitBounds();
               DrawOrder = int.MinValue;
          }

          private void setBackgroundTint()
          {
               Vector4 bgTint = Vector4.One;
               bgTint.W = 0.4f;
               TintColor = new Color(bgTint);
          }

          public override void Draw(GameTime i_GameTime)
          {
               SpriteBatch.Draw(Texture, GraphicsDevice.Viewport.Bounds, TintColor);
          }
     }
}
