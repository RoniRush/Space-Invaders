namespace SpaceInvaders.GameClasses.Sprites
{
     using Microsoft.Xna.Framework;

     public class RemainingLife : SpriteAdapter
     {
          public RemainingLife(Game i_Game, string i_ShipPath)
               : base(i_ShipPath, i_Game)
          {
          }

          protected override void InitBounds()
          {
               base.InitBounds();
               Scales = new Vector2(0.5f, 0.5f);
               Opacity = 0.5f;
          }

          public void RemoveLife()
          {
               Visible = false;
               IsAlive = false;
          }
     }
}
