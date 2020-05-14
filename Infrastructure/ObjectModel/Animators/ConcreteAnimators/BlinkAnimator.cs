using System;

namespace Infrastructure.ObjectModel.Animators.ConcreteAnimators
{
     using Microsoft.Xna.Framework;

     public class BlinkAnimator : SpriteAnimator
     {
          private TimeSpan m_BlinkLength;
          private TimeSpan m_TimeLeftForNextBlink;

          public TimeSpan BlinkLength
          {
               get { return m_BlinkLength; }
               set { m_BlinkLength = value; }
          }

          // CTORs
          public BlinkAnimator(string i_Name, TimeSpan i_BlinkLength, TimeSpan i_AnimationLength)
               : base(i_Name, i_AnimationLength)
          {
               m_BlinkLength = i_BlinkLength;
               m_TimeLeftForNextBlink = i_BlinkLength;
          }

          protected override void DoFrame(GameTime i_GameTime)
          {
               m_TimeLeftForNextBlink -= i_GameTime.ElapsedGameTime;
               if (m_TimeLeftForNextBlink.TotalSeconds < 0)
               {
                    // we have elapsed, so blink
                    BoundSprite.Visible = !BoundSprite.Visible;
                    m_TimeLeftForNextBlink = m_BlinkLength;
               }
          }

          protected override void RevertToOriginal()
          {
               BoundSprite.Visible = m_OriginalSpriteInfo.Visible;
               m_TimeLeftForNextBlink = m_BlinkLength;
          }
     }
}
