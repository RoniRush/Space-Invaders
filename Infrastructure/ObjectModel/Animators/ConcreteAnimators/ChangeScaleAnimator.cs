using System;

namespace Infrastructure.ObjectModel.Animators.ConcreteAnimators
{
     using Microsoft.Xna.Framework;

     public class ChangeScaleAnimator : SpriteAnimator
     {
          private readonly Vector2 r_ScaleFactor;

          public ChangeScaleAnimator(string i_Name, TimeSpan i_AnimationLength, Vector2 i_CurrentScale)
               : base(i_Name, i_AnimationLength)
          {
               r_ScaleFactor = i_CurrentScale / (float)i_AnimationLength.TotalSeconds;
          }

          protected override void RevertToOriginal()
          {
               BoundSprite.Scales = m_OriginalSpriteInfo.Scales;
          }

          protected override void DoFrame(GameTime i_GameTime)
          {
               BoundSprite.Scales -= new Vector2(r_ScaleFactor.X * (float)i_GameTime.ElapsedGameTime.TotalSeconds, r_ScaleFactor.Y * (float)i_GameTime.ElapsedGameTime.TotalSeconds);
          }
     }
}
