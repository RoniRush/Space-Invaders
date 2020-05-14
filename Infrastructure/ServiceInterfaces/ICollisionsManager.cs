using System;

namespace Infrastructure.ServiceInterfaces
{
     using Microsoft.Xna.Framework;

     public delegate void PositionChangedEventHandler(object i_Collidable);

     public interface ICollidable
     {
          event PositionChangedEventHandler PositionChanged;

          event EventHandler<EventArgs> Disposed;

          bool Visible { get; }

          bool CheckCollision(ICollidable i_Source);

          void Collided(ICollidable i_Collidable);
     }

     public interface ICollidable2D : ICollidable
     {
          Rectangle Bounds { get; }

          Vector2 Velocity { get; }
     }

     public interface ICollidable3D : ICollidable
     {
          BoundingBox Bounds { get; }

          Vector3 Velocity { get; }
     }

     public interface ICollisionsManager
     {
          void AddObjectToMonitor(ICollidable i_Collidable);
     }
}
