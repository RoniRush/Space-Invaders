using System;

namespace Infrastructure.ObjectModel.Animators.ConcreteAnimators
{
     using Microsoft.Xna.Framework;

     public class FadeAnimator : SpriteAnimator
     {
          private readonly float r_OpacityFactor;

          public FadeAnimator(string i_Name, TimeSpan i_AnimationLength, float i_InitialOpacity)
               : base(i_Name, i_AnimationLength)
          {
               r_OpacityFactor = i_InitialOpacity / (float)AnimationLength.TotalSeconds;
          }

          protected override void RevertToOriginal()
          {
               BoundSprite.Opacity = m_OriginalSpriteInfo.Opacity;
          }

          protected override void DoFrame(GameTime i_GameTime)
          {
               if (BoundSprite.Opacity > 0)
               {
                    BoundSprite.Opacity -= r_OpacityFactor * (float)i_GameTime.ElapsedGameTime.TotalSeconds;
               }
          }
     }
}
