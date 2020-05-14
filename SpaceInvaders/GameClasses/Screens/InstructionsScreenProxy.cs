namespace SpaceInvaders.GameClasses.Screens
{
     using Infrastructure.ObjectModel;
     using Infrastructure.ObjectModel.Screens;
     using Microsoft.Xna.Framework;
     using Microsoft.Xna.Framework.Graphics;

     public delegate void PlaySelected();

     public class InstructionsScreenProxy : GameScreen
     {
          protected Font m_Instructions;

          public PlaySelected PlaySelected { get; private set; }

          public bool WasPlaySelected { get; set; }

          public InstructionsScreenProxy(Game i_Game)
               : base(i_Game)
          {
               m_Instructions = new Font(i_Game) { TintColor = Color.AntiqueWhite, Scales = new Vector2(2, 2) };
               PlaySelected = onPlaySelected;
               BlendState = BlendState.NonPremultiplied;
          }

          private void onPlaySelected()
          {
               WasPlaySelected = true;
          }

          public override void Initialize()
          {
               base.Initialize();
               m_Instructions.Message = GetInstructions();
               InitPositions();
               AddScreenComponents();
          }

          protected virtual void AddScreenComponents()
          {
               Add(m_Instructions);
          }

          protected virtual void InitPositions()
          {
               m_Instructions.Position = new Vector2(100, 100);
          }

          protected virtual string GetInstructions()
          {
               return string.Empty;
          }
     }
}
