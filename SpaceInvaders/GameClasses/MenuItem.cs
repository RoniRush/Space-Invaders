namespace SpaceInvaders.GameClasses
{
     using System;
     using Infrastructure.ObjectModel;

     public class MenuItem
     {
          public Font ItemFont { get; set; }

          public Action Command { get; set; }

          public bool IsTogglable { get; set; }

          public bool IsSelectable { get; set; }
     }
}
