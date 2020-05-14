namespace SpaceInvaders.GameClasses.Screens
{
     using System;
     using Infrastructure.ObjectModel.Animators.ConcreteAnimators;
     using Microsoft.Xna.Framework;

     public abstract class PulsingMenuScreen : MenuScreen
     {
          protected PulsingMenuScreen(Game i_Game, int i_NumOfItems, string i_Title)
               : base(i_Game, i_NumOfItems, i_Title)
          {
          }

          protected abstract override void InitItemNames();

          protected abstract override void InitItemCommands();

          public override void Initialize()
          {
               base.Initialize();
               initAnimation();
          }

          private void initAnimation()
          {
               foreach (MenuItem menuItem in m_MenuItems)
               {
                    menuItem.ItemFont.InitAnimations(new PulseAnimator("Pulse", TimeSpan.Zero, 2.5f, 1));
               }
          }

          protected override void UpdatePreviousItem()
          {
               m_MenuItems[m_PreviousItemIndex].ItemFont.Animations.Enabled = false;
               m_MenuItems[m_PreviousItemIndex].ItemFont.Animations["Pulse"].Enabled = false;
               m_MenuItems[m_PreviousItemIndex].ItemFont.Animations["Pulse"].Reset();
               base.UpdatePreviousItem();
          }

          protected override void UpdateActiveItem()
          {
               m_MenuItems[m_ActiveItemIndex].ItemFont.Animations.Enabled = true;
               m_MenuItems[m_ActiveItemIndex].ItemFont.Animations["Pulse"].Enabled = true;
               base.UpdateActiveItem();
          }
     }
}
