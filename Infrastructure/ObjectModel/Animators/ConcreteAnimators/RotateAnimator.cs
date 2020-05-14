using System;

namespace Infrastructure.ObjectModel.Animators.ConcreteAnimators
{
     using Microsoft.Xna.Framework;

     public class RotateAnimator : SpriteAnimator
     {
          private readonly float r_AngularVelocity;

          // CTORs
          public RotateAnimator(string i_Name, float i_AngularVelocity, TimeSpan i_AnimationLength)
               : base(i_Name, i_AnimationLength)
          {
               r_AngularVelocity = i_AngularVelocity;
          }

          protected override void DoFrame(GameTime i_GameTime)
          {
               BoundSprite.AngularVelocity = r_AngularVelocity;
          }

          protected override void RevertToOriginal()
          {
               BoundSprite.AngularVelocity = m_OriginalSpriteInfo.AngularVelocity;
          }
     }
}
