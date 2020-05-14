using System;
using System.Collections.Generic;

namespace SpaceInvaders.GameClasses
{
     using Infrastructure.ObjectModel;
     using Infrastructure.ServiceInterfaces;
     using Microsoft.Xna.Framework;

     public abstract class SpriteAdapter : Sprite
     {
          private readonly Color r_NeutralColor = Color.White;

          public ISoundManager SoundManager { get; private set; }

          public Color NeutralColor
          {
               get
               {
                    return r_NeutralColor;
               }
          }

          public Color[] TexturePixels { get; set; }

          public List<int> CollidedPixels { get; set; }

          public bool CheckPerPixel { get; set; } = true;

          protected SpriteAdapter(string i_AssetName, Game i_Game)
               : base(i_AssetName, i_Game, int.MaxValue)
          {
               CollidedPixels = new List<int>();
               SoundManager = Game.Services.GetService(typeof(ISoundManager)) as ISoundManager;
          }

          public void CommitSuicide()
          {
               IsAlive = false;
               Visible = false;
          }
          
          public bool IsAlive { get; set; } = true;

          public Color TeamColor { get; set; }

          public override bool CheckCollision(ICollidable i_Source)
          {
               bool collided = false;
               ICollidable2D source = i_Source as ICollidable2D;
               if(IsAlive && source != null)
               {
                    SpriteAdapter sprite = i_Source as SpriteAdapter;
                    if(sprite != null)
                    {
                         if(sprite.TeamColor != TeamColor && sprite.IsAlive)
                         {
                              if(sprite.Bounds.Intersects(Bounds))
                              {
                                   collided = !CheckPerPixel || PerPixelCollision(sprite);
                              }
                         }
                    }
               }

               return collided;
          }

          protected override void LoadContent()
          {
               base.LoadContent();
               TexturePixels = new Color[Texture.Width * Texture.Height];
               Texture.GetData(TexturePixels);
          }

          protected bool PerPixelCollision(SpriteAdapter i_Sprite)
          {
               bool isTherePixelCollision = false;
               GetCollidedRectangleBounds(
                    out int positionXStart,
                    out int positionXEnd,
                    out int positionYStart,
                    out int positionYEnd,
                    i_Sprite);
               for(int j = positionYStart; j < positionYEnd; ++j)
               {
                    for(int i = positionXStart; i < positionXEnd; ++i)
                    {
                         Color sourceColor =
                              i_Sprite.TexturePixels[(i - i_Sprite.Bounds.X)
                                                     + ((j - i_Sprite.Bounds.Y) * i_Sprite.Texture.Width)];
                         Color myColor = TexturePixels[(i - Bounds.X) + ((j - Bounds.Y) * Texture.Width)];
                         if(checkIfBothPixelsAreNotTransparent(sourceColor, myColor))
                         {
                              CollidedPixels.Add((i - Bounds.X) + ((j - Bounds.Y) * Texture.Width));
                              isTherePixelCollision = true;
                         }
                    }
               }

               return isTherePixelCollision;
          }

          private bool checkIfBothPixelsAreNotTransparent(Color i_SourceColor, Color i_MyColor)
          {
               return i_SourceColor.A != 0 && i_MyColor.A != 0;
          }

          protected void GetCollidedRectangleBounds(
               out int o_PositionXStart,
               out int o_PositionXEnd,
               out int o_PositionYStart,
               out int o_PositionYEnd,
               SpriteAdapter i_Sprite)
          {
               o_PositionXStart = Math.Max(i_Sprite.Bounds.X, Bounds.X);
               o_PositionXEnd = Math.Min(i_Sprite.Bounds.X + i_Sprite.Bounds.Width, Bounds.X + Bounds.Width);
               o_PositionYStart = Math.Max(i_Sprite.Bounds.Y, Bounds.Y);
               o_PositionYEnd = Math.Min(i_Sprite.Bounds.Y + i_Sprite.Bounds.Height, Bounds.Y + Bounds.Height);
          }
     }
}
