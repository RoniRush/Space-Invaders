namespace SpaceInvaders.GameClasses.Screens
{
     using System.Collections.Generic;
     using global::SpaceInvaders.GameClasses.Sprites;
     using Infrastructure.Managers;
     using Infrastructure.ObjectModel;
     using Infrastructure.ObjectModel.Screens;
     using Microsoft.Xna.Framework;
     using Microsoft.Xna.Framework.Graphics;
     using Microsoft.Xna.Framework.Input;

     public abstract class MenuScreen : GameScreen
     {
          private readonly Background r_Background;
          private readonly int r_NumOfItems;
          protected List<MenuItem> m_MenuItems;
          protected Color m_ActiveColor = Color.LightGreen;
          protected Color m_InactiveColor = Color.AntiqueWhite;
          protected int m_PreviousItemIndex;
          protected int m_ActiveItemIndex;
          protected Font m_Title;
          protected bool m_IsPageUp;
          protected InputManager m_InputManager;
          private bool m_WasInputManagerInitialized;

          protected string[] ItemNames { get; set; }

          protected MenuScreen(Game i_Game, int i_NumOfItems, string i_Title)
               : base(i_Game)
          {
               r_NumOfItems = i_NumOfItems;
               r_Background = new Background(i_Game);
               m_MenuItems = new List<MenuItem>(r_NumOfItems);
               ItemNames = new string[r_NumOfItems];
               m_ActiveItemIndex = -1;
               BlendState = BlendState.NonPremultiplied;
               m_Title = new Font(i_Game) { Message = i_Title, TintColor = Color.HotPink, Scales = new Vector2(3) };
          }

          public override void Initialize()
          {
               base.Initialize();
               InitItemNames();
               InitFonts();
               initFontsPosition();
               InitItemCommands();
               AddScreenComponents();
               m_InputManager = InputManager as InputManager;
          }

          private void initFontsPosition()
          {
               int xPosition = 100;
               int yPosition = 50;
               m_Title.Position = new Vector2(xPosition, yPosition);
               foreach(MenuItem item in m_MenuItems)
               {
                    yPosition += 90;
                    item.ItemFont.Position = new Vector2(xPosition, yPosition);
                    item.ItemFont.Scales = new Vector2(2, 2);
                    item.ItemFont.SourceRectangle = new Rectangle(xPosition, yPosition, (int)item.ItemFont.Width, (int)item.ItemFont.Height);
               }
          }

          protected virtual void AddScreenComponents()
          {
               Add(r_Background);
               Add(m_Title);
               foreach (MenuItem item in m_MenuItems)
               {
                    Add(item.ItemFont);
               }
          }

          public override void Update(GameTime i_GameTime)
          {
               if(!m_WasInputManagerInitialized)
               {
                    m_InputManager = InputManager as InputManager;

               }

               checkIfFirstUpdateToUpdateActiveItem();
               m_IsPageUp = false;
               if(m_InputManager != null)
               {
                    checkIfUserPressedDownAndAct();
                    checkIfUserPressedUpAndAct();
                    checkIfUserPressedEnterAndAct();
                    checkIfUserMovesBetweenItemOptionsAndAct();
                    checkIfUserPressedOnMouseAndAct();
                    checkIfMouseHoversItemAndAct();
                    SoundManager.CheckIfUserWantsToToggleSound(m_InputManager, Keys.M);
               }

               base.Update(i_GameTime);
          }

          private void checkIfMouseHoversItemAndAct()
          {
               for (int i = 0; i < m_MenuItems.Count; i++)
               {
                    Font font = m_MenuItems[i].ItemFont;
                    if (checkIfMouseIsInFontBound(font))
                    {
                         if (m_ActiveItemIndex != i)
                         {
                              m_PreviousItemIndex = m_ActiveItemIndex;
                              m_ActiveItemIndex = i;
                              updateCurrentAndPrevItems();
                         }
                    }
               }
          }

          private void checkIfUserPressedOnMouseAndAct()
          {
               if (m_InputManager.MouseState.LeftButton == ButtonState.Pressed
                   && m_InputManager.PrevMouseState.LeftButton == ButtonState.Released)
               {
                    for (int i = 0; i < m_MenuItems.Count; i++)
                    {
                         Font font = m_MenuItems[i].ItemFont;
                         if (checkIfMouseIsInFontBound(font))
                         {
                              checkIfItemIsSelectableAndAct();
                         }
                    }
               }
          }

          private bool checkIfMouseIsInFontBound(Font i_Font)
          {
               return m_InputManager.MouseState.X <= i_Font.Bounds.Right
                      && m_InputManager.MouseState.X >= i_Font.Bounds.Left
                      && m_InputManager.MouseState.Y <= i_Font.Bounds.Bottom
                      && m_InputManager.MouseState.Y >= i_Font.Bounds.Top;
          }

          private void checkIfUserMovesBetweenItemOptionsAndAct()
          {
               if ((m_InputManager.KeyboardState.IsKeyDown(Keys.PageUp) && m_InputManager.PrevKeyboardState.IsKeyUp(Keys.PageUp))
                   || (m_InputManager.KeyboardState.IsKeyDown(Keys.PageDown) && m_InputManager.PrevKeyboardState.IsKeyUp(Keys.PageDown))
                   || (m_InputManager.MouseState.ScrollWheelValue != m_InputManager.PrevMouseState.ScrollWheelValue)
                   || (m_InputManager.MouseState.RightButton == ButtonState.Pressed && m_InputManager.PrevMouseState.RightButton == ButtonState.Released))
               {
                    if (m_MenuItems[m_ActiveItemIndex].IsTogglable)
                    {
                         if (m_InputManager.KeyboardState.IsKeyDown(Keys.PageUp) && m_InputManager.PrevKeyboardState.IsKeyUp(Keys.PageUp))
                         {
                              m_IsPageUp = true;
                         }

                         m_MenuItems[m_ActiveItemIndex].Command.Invoke();
                    }
               }
          }

          private void checkIfUserPressedEnterAndAct()
          {
               if (m_InputManager.KeyboardState.IsKeyDown(Keys.Enter) && m_InputManager.PrevKeyboardState.IsKeyUp(Keys.Enter))
               {
                    checkIfItemIsSelectableAndAct();
               }
          }

          private void checkIfItemIsSelectableAndAct()
          {
               if (m_MenuItems[m_ActiveItemIndex].IsSelectable)
               {
                    m_MenuItems[m_ActiveItemIndex].Command.Invoke();
               }
          }

          private void checkIfUserPressedUpAndAct()
          {
               if (m_InputManager.KeyboardState.IsKeyDown(Keys.Up) && m_InputManager.PrevKeyboardState.IsKeyUp(Keys.Up))
               {
                    m_PreviousItemIndex = m_ActiveItemIndex;
                    m_ActiveItemIndex--;
                    if (m_ActiveItemIndex < 0)
                    {
                         m_ActiveItemIndex = r_NumOfItems - 1;
                    }

                    updateCurrentAndPrevItems();
               }
          }

          private void checkIfUserPressedDownAndAct()
          {
               if (m_InputManager.KeyboardState.IsKeyDown(Keys.Down) && m_InputManager.PrevKeyboardState.IsKeyUp(Keys.Down))
               {
                    m_PreviousItemIndex = m_ActiveItemIndex;
                    m_ActiveItemIndex++;
                    if (m_ActiveItemIndex > r_NumOfItems - 1)
                    {
                         m_ActiveItemIndex = 0;
                    }

                    updateCurrentAndPrevItems();
               }
          }

          private void updateCurrentAndPrevItems()
          {
               SoundManager.PlaySound(GameSettings.s_SoundEffectsNames[GameSettings.eSoundEffects.MenuMove]);
               UpdateActiveItem();
               UpdatePreviousItem();
          }

          private void checkIfFirstUpdateToUpdateActiveItem()
          {
               if (m_ActiveItemIndex == -1)
               {
                    m_ActiveItemIndex++;
                    UpdateActiveItem();
               }
          }

          protected virtual void UpdatePreviousItem()
          {
               m_MenuItems[m_PreviousItemIndex].ItemFont.TintColor = m_InactiveColor;
          }

          protected virtual void UpdateActiveItem()
          {
               m_MenuItems[m_ActiveItemIndex].ItemFont.TintColor = m_ActiveColor;
          }

          protected abstract void InitItemNames();

          protected virtual void InitFonts()
          {
               for (int i = 0; i < r_NumOfItems; i++)
               {
                    Font font = new Font(Game) { TintColor = m_InactiveColor };
                    font.Message = ItemNames[i];
                    MenuItem menuItem = new MenuItem() { ItemFont = font };
                    m_MenuItems.Add(menuItem);
               }
          }

          protected void Done()
          {
               ExitScreen();
          }

          protected abstract void InitItemCommands();
     }
}
