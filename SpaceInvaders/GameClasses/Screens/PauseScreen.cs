namespace SpaceInvaders.GameClasses.Screens
{
     using Infrastructure.Managers;
     using Microsoft.Xna.Framework;
     using Microsoft.Xna.Framework.Input;

     public class PauseScreen : InstructionsScreenProxy
     {
          public PauseScreen(Game i_Game)
               : base(i_Game)
          {
               UseGradientBackground = true;
               IsOverlayed = true;
               BlackTintAlpha = 0.55f;
          }

          protected override string GetInstructions()
          {
               return string.Format(@"
     PAUSE

Press R to Resume");
          }

          protected override void InitPositions()
          {
               m_Instructions.Position = new Vector2(200, 200);
          }

          public override void Update(GameTime i_GameTime)
          {
               if (InputManager != null)
               {
                    SoundManager.CheckIfUserWantsToToggleSound(InputManager as InputManager, Keys.M);
                    if (InputManager.KeyboardState.IsKeyDown(Keys.R))
                    {
                         ExitScreen();
                    }
               }

               base.Update(i_GameTime);
          }
     }
}
