namespace Infrastructure.ObjectModel.Animators.ConcreteAnimators
{
     using System;
     using Microsoft.Xna.Framework;

     public class CountDownAnimator : SpriteAnimator
     {
          private TimeSpan m_CountDownLength;
          private TimeSpan m_TimeLeftForNextCountDown;
          private int m_Count;

          public CountDownAnimator(string i_Name, TimeSpan i_AnimationLength, TimeSpan i_CountDownLength, int i_Count)
               : base(i_Name, i_AnimationLength)
          {
               m_CountDownLength = i_CountDownLength;
               m_TimeLeftForNextCountDown = m_CountDownLength;
               m_Count = i_Count;
          }

          public override void Initialize()
          {
               base.Initialize();
               updateFontMessage();
          }

          private void updateFontMessage()
          {
               Font font = BoundSprite as Font;
               font?.UpdateMessage(m_Count.ToString());
          }

          protected override void RevertToOriginal()
          {
          }

          protected override void DoFrame(GameTime i_GameTime)
          {
               m_TimeLeftForNextCountDown -= i_GameTime.ElapsedGameTime;
               if (m_TimeLeftForNextCountDown.TotalSeconds <= 0)
               {
                    if(m_Count > 0)
                    {
                         m_Count--;
                         updateFontMessage();
                    }

                    m_TimeLeftForNextCountDown = m_CountDownLength;
               }
          }
     }
}
