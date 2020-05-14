using System;
using System.Collections.Generic;

namespace Infrastructure.Managers
{
     using Infrastructure.ObjectModel;
     using Infrastructure.ServiceInterfaces;
     using Microsoft.Xna.Framework;

     public class CollisionsManager : GameService, ICollisionsManager
     {
          protected readonly List<ICollidable> r_Collidables = new List<ICollidable>();

          public CollisionsManager(Game i_Game)
               : base(i_Game, int.MaxValue)
          {
          }

          protected override void RegisterAsService()
          {
               this.Game.Services.AddService(typeof(ICollisionsManager), this);
          }

          public void AddObjectToMonitor(ICollidable i_Collidable)
          {
               if (!this.r_Collidables.Contains(i_Collidable))
               {
                    this.r_Collidables.Add(i_Collidable);
                    i_Collidable.PositionChanged += collidable_PositionChanged;
                    i_Collidable.Disposed += collidable_Disposed;
               }
          }

          private void collidable_PositionChanged(object i_Collidable)
          {
               if (i_Collidable is ICollidable)
               {
                    checkCollision(i_Collidable as ICollidable);
               }
          }

          private void checkCollision(ICollidable i_Source)
          {
               List<ICollidable> collidedComponents = new List<ICollidable>();

               // finding who collided with i_Source:
               foreach (ICollidable target in this.r_Collidables)
               {
                    if (i_Source.Visible && i_Source != target && target.Visible)
                    {
                         if (target.CheckCollision(i_Source))
                         {
                              collidedComponents.Add(target);
                         }
                    }
               }

               // Informing i_Source and all the collided targets about the collision:
               foreach (ICollidable target in collidedComponents)
               {
                    target.Collided(i_Source);
                    i_Source.Collided(target);
               }
          }

          private void collidable_Disposed(object sender, EventArgs e)
          {
               ICollidable collidable = sender as ICollidable;

               if (collidable != null
                   &&
                   this.r_Collidables.Contains(collidable))
               {
                    collidable.PositionChanged -= collidable_PositionChanged;
                    collidable.Disposed -= collidable_Disposed;
                    this.r_Collidables.Remove(collidable);
               }
          }
     }
}
