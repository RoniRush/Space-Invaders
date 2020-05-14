namespace SpaceInvaders.GameClasses.Managers
{
     using System.Collections.Generic;
     using global::SpaceInvaders.GameClasses.Sprites;
     using Infrastructure.ObjectModel;
     using Microsoft.Xna.Framework;
     using Microsoft.Xna.Framework.Graphics;
     using Microsoft.Xna.Framework.Input;

     public class ShipsManager : CompositeDrawableComponent<IGameComponent>
     {
          private const string k_FirstShipPath = @"Sprites/Ship01_32x32";
          private const string k_SecondShipPath = @"Sprites/Ship02_32x32";
          private List<Ship> m_Ships;

          public ShipsManager(Game i_Game)
          : base(i_Game)
          {
               m_Ships = new List<Ship>(GameSettings.NumOfPlayers);
               addShips(i_Game);
               BlendState = BlendState.NonPremultiplied;
          }

          public List<Ship> Ships
          {
               get
               {
                    return m_Ships;
               }
          }

          public void PresetLevel()
          {
               foreach (Ship ship in Ships)
               {
                    if(ship.RemainingLives > 0)
                    {
                         ship.IsAlive = true;
                         ship.Visible = true;
                    }
               }
          }

          public void KillAll()
          {
               foreach (Ship ship in Ships)
               {
                    ship.Shooter.KillAllBullets();
                    ship.CommitSuicide();
               }
          }

          private void addShips(Game i_Game)
          {
               Ship playerShip = new Ship(i_Game, k_FirstShipPath);
               playerShip.SetKeyboardControls(Keys.K, Keys.H, Keys.U);
               playerShip.CanUseTheMouse = true;
               playerShip.PlayerColor = Color.Blue;
               playerShip.FontForScore =
                    new Font(i_Game) { TintColor = playerShip.PlayerColor, Message = "P1 Score: " };
               playerShip.FontForScore.UpdateMessage(0.ToString());
               addShipComponents(playerShip);
               if(GameSettings.NumOfPlayers == 2)
               {
                    playerShip = new Ship(i_Game, k_SecondShipPath);
                    playerShip.SetKeyboardControls(Keys.D, Keys.A, Keys.W);
                    playerShip.CanUseTheMouse = false;
                    playerShip.PlayerColor = Color.Green;
                    playerShip.FontForScore = new Font(i_Game) { TintColor = playerShip.PlayerColor, Message = "P2 Score: " };
                    playerShip.FontForScore.UpdateMessage(0.ToString());
                    addShipComponents(playerShip);
               }
          }

          private void addShipComponents(Ship i_Ship)
          {
               m_Ships.Add(i_Ship);
               Add(i_Ship);
               Add(i_Ship.FontForScore);
               foreach(Bullet bullet in i_Ship.ShipBullets)
               {
                    Add(bullet);
               }

               foreach(RemainingLife remainingLife in i_Ship.RemainingLivesTextures)
               {
                  Add(remainingLife);  
               }
          }

          public void InitShipsPositions()
          {
               int positionX, positionY, lastShipEndPositionX;

               lastShipEndPositionX = 5;
               Mouse.SetPosition(0, Mouse.GetState().Y);
               foreach (Ship ship in m_Ships)
               {
                    positionX = lastShipEndPositionX;
                    positionY = ship.Game.GraphicsDevice.Viewport.Height - ship.Texture.Height - 20;
                    ship.Position = new Vector2(positionX, positionY);
                    lastShipEndPositionX += (int)ship.Width;
                    ship.InitRemainingLifePosition();
                    ship.InitScorePosition();
               }
          }

          public bool IsThereAShipAlive()
          {
               bool isThereAnyShipAlive = false;
               int i = 0;
               while (i < m_Ships.Count && !isThereAnyShipAlive)
               {
                    if (m_Ships[i].RemainingLives > 0)
                    {
                         isThereAnyShipAlive = true;
                    }
                    else
                    {
                         i++;
                    }
               }

               return isThereAnyShipAlive;
          }

          public float ShipsYPosition()
          {
               return m_Ships[m_Ships.Count - 1].Position.Y;
          }

          public string WhoWon()
          {
               string whoWon = string.Empty;
               int currentMaxScore = -1;
               int index = 1;
               foreach (Ship ship in m_Ships)
               {
                    if (ship.Score > currentMaxScore)
                    {
                         currentMaxScore = ship.Score;
                         whoWon = string.Format(@"Player {0}", index);
                    }
                    else if (ship.Score == currentMaxScore)
                    {
                         whoWon = whoWon.Equals(string.Empty) ? string.Format(@"Player {0}", index) : "It's a tie!";
                    }

                    index++;
               }

               return whoWon;
          }
     }
}
