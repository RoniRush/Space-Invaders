namespace SpaceInvaders.GameClasses.Managers
{
     using System.Collections.Generic;
     using global::SpaceInvaders.GameClasses.Sprites;
     using Infrastructure.ObjectModel;
     using Microsoft.Xna.Framework;
     using Microsoft.Xna.Framework.Graphics;

     public class BarrierManager : CompositeDrawableComponent<IGameComponent>
     {
          private const int k_NumOfBarriers = 4;
          private const float k_VelocityFactor = 0.4f;
          private List<Barrier> m_Barriers;
          private float m_BarrierVelocityLevelMultiplier;

          public BarrierManager(Game i_Game, int i_Level)
               : base(i_Game)
          {
               PresetLevel(i_Level);
               BlendState = BlendState.NonPremultiplied;
          }

          public void KillAll()
          {
               foreach(Barrier barrier in m_Barriers)
               {
                    barrier.CommitSuicide();
                    Remove(barrier);
               }
          }

          public void PresetLevel(int i_Level)
          {
               m_Barriers = new List<Barrier>(k_NumOfBarriers);
               if ((i_Level - 1) % 5 == 0)
               {
                    // level 1 and the likes
                    m_BarrierVelocityLevelMultiplier = 0;
               }
               else
               {
                    // level 2-5 and the likes
                    float multiplier = 1f;
                    m_BarrierVelocityLevelMultiplier = multiplier + ((((i_Level - 1) % 5) - 1) * k_VelocityFactor);
               }

               addBarriers();
          }

          private void addBarriers()
          {
               for (int i = 0; i < k_NumOfBarriers; i++)
               {
                    m_Barriers.Add(new Barrier(Game, m_BarrierVelocityLevelMultiplier));
                    Add(m_Barriers[i]);
               }
          }

          public void InitBarriersPositions(float i_ShipYPosition)
          {
               float positionX;
               float positionY;

               float initialPositionX = getFirstBarrierPosition();
               int indexOfBarrier = 0;
               int numOfSpaces = 0;
               foreach (Barrier barrier in m_Barriers)
               {
                    positionY = i_ShipYPosition - (barrier.Height * 2);
                    positionX = ((indexOfBarrier + numOfSpaces) * barrier.Width) + initialPositionX;
                    barrier.Position = new Vector2(positionX, positionY);
                    barrier.InitialPosition = barrier.Position;
                    indexOfBarrier++;
                    numOfSpaces++;
               }
          }

          private float getFirstBarrierPosition()
          {
               float screenWidth = Game.GraphicsDevice.Viewport.Width;
               int numOfSpacesBetweenBarriers = k_NumOfBarriers - 1;
               float lengthOfBarriersIncludeSpaces = (k_NumOfBarriers + numOfSpacesBetweenBarriers) * m_Barriers[0].Width;
               return (screenWidth - lengthOfBarriersIncludeSpaces) / 2;
          }
     }
}
