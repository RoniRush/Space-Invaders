namespace SpaceInvaders.GameClasses
{
     using System.Collections.Generic;
     using global::SpaceInvaders.GameClasses.Sprites;
     using Microsoft.Xna.Framework;

     public class Shooter
     {
          private readonly int r_MaxNumOfBullets;
          private List<Bullet> m_BulletList;

          public Shooter(int i_MaxNumOfBullets, Game i_Game, Color i_BulletColor, bool i_IsGoingDown, bool i_NeedToSubBulletHeight)
          {
               r_MaxNumOfBullets = i_MaxNumOfBullets;
               m_BulletList = new List<Bullet>(i_MaxNumOfBullets);
               for(int i = 0; i < r_MaxNumOfBullets; i++)
               {
                    Bullet bullet = new Bullet(
                         i_Game,
                         i_BulletColor,
                         Vector2.Zero,
                         i_IsGoingDown,
                         i_NeedToSubBulletHeight);
                    bullet.IsAlive = false;
                    bullet.Visible = false;
                    m_BulletList.Add(bullet);
               }
          }

          public List<Bullet> BulletList
          {
               get
               {
                    return m_BulletList;
               }
          }

          public Bullet Shoot(Vector2 i_Position)
          {
               Bullet bullet = null;
               int index = getIndexOfAvailableBulletOrMinusOne();
               if(index != -1)
               {
                    m_BulletList[index].Position = i_Position;
                    m_BulletList[index].IsAlive = true;
                    m_BulletList[index].Visible = true;
                    bullet = m_BulletList[index];
               }

               return bullet;
          }

          private int getIndexOfAvailableBulletOrMinusOne()
          {
               int i = 0;
               bool foundFreeBullet = false;
               while (i < m_BulletList.Count && !foundFreeBullet)
               {
                    if (!m_BulletList[i].Visible)
                    {
                         foundFreeBullet = true;
                    }
                    else
                    {
                         i++;
                    }
               }

               if (i == m_BulletList.Count)
               {
                    i = -1;
               }

               return i;
          }

          public void KillAllBullets()
          {
               foreach(Bullet bullet in BulletList)
               {
                    bullet.CommitSuicide();
               }
          }
     }
}
