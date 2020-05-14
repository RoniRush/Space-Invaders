using System;

namespace Infrastructure.ObjectModel.Animators.ConcreteAnimators
{
     using Microsoft.Xna.Framework;

     public class CellAnimator : SpriteAnimator
     {
          private readonly int r_NumOfCells;
          private TimeSpan m_CellTime;
          private TimeSpan m_TimeLeftForCell;
          private bool m_Loop;
          private int m_CurrCellIdx;

          // CTORs
          public CellAnimator(string i_Name, TimeSpan i_CellTime, int i_NumOfCells, TimeSpan i_AnimationLength, int i_CurrentIndex)
              : base(i_Name, i_AnimationLength)
          {
               m_CellTime = i_CellTime;
               m_TimeLeftForCell = i_CellTime;
               r_NumOfCells = i_NumOfCells;
               m_CurrCellIdx = i_CurrentIndex;
               m_Loop = i_AnimationLength == TimeSpan.Zero;
          }

          private void goToNextFrame()
          {
               m_CurrCellIdx++;
               if (m_CurrCellIdx >= r_NumOfCells)
               {
                    if (m_Loop)
                    {
                         m_CurrCellIdx = 0;
                    }
                    else
                    {
                         m_CurrCellIdx = r_NumOfCells - 1;
                         IsFinished = true;
                    }
               }
          }

          protected override void RevertToOriginal()
          {
               BoundSprite.SourceRectangle = m_OriginalSpriteInfo.SourceRectangle;
          }

          protected override void DoFrame(GameTime i_GameTime)
          {
               if (m_CellTime != TimeSpan.Zero)
               {
                    m_TimeLeftForCell -= i_GameTime.ElapsedGameTime;
                    if (m_TimeLeftForCell.TotalSeconds <= 0)
                    {
                         goToNextFrame();
                         m_TimeLeftForCell = m_CellTime;
                    }
               }

               BoundSprite.SourceRectangle = new Rectangle(
                   m_CurrCellIdx * BoundSprite.SourceRectangle.Width,
                   BoundSprite.SourceRectangle.Top,
                   BoundSprite.SourceRectangle.Width,
                   BoundSprite.SourceRectangle.Height);
          }
     }
}
