namespace SpaceInvaders.GameClasses.Screens
{
     using System;
     using global::SpaceInvaders.GameClasses.Sprites;
     using Infrastructure.Managers;
     using Infrastructure.ObjectModel;
     using Infrastructure.ObjectModel.Animators.ConcreteAnimators;
     using Microsoft.Xna.Framework;
     using Microsoft.Xna.Framework.Input;

     public class TransitionScreen : InstructionsScreenProxy
     {
          private readonly Background r_Background;

          public int Level { get; set; }

          public int CountDown { get; set; }

          public TransitionScreen(Game i_Game, int i_Level)
               : base(i_Game)
          {
               r_Background = new Background(i_Game);
               Level = i_Level;
               m_Instructions = new Font(i_Game) { TintColor = Color.AntiqueWhite, Scales = new Vector2(3, 3) };
               CountDown = 3; 
          }

          public override void Initialize()
          {
               base.Initialize();
               initAnimation();
          }

          private void initAnimation()
          {
               m_Instructions.InitAnimations(new CountDownAnimator("CountDown", TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(1), 3));
               m_Instructions.Animations.Enabled = true;
               m_Instructions.Animations["CountDown"].Enabled = true;
               m_Instructions.Animations["CountDown"].Finished += countDownAnimator_Finished;
          }

          private void countDownAnimator_Finished(object sender, EventArgs e)
          {
               m_Instructions.AnimationFinished = true;
          }

          public override void Update(GameTime i_GameTime)
          {
               SoundManager.CheckIfUserWantsToToggleSound(InputManager as InputManager, Keys.M);
               if (m_Instructions.HasAnimation)
               {
                    if(m_Instructions.AnimationFinished)
                    {
                         ExitScreen();
                    }
               }

               base.Update(i_GameTime);
          }
          
          protected override void InitPositions()
          {
               m_Instructions.Position = new Vector2(180, 100);
          }

          protected override string GetInstructions()
          {
               return string.Format(
                    @"
               Level {0}

                    ",
                    Level);
          }

          protected override void AddScreenComponents()
          {
               Add(r_Background);
               base.AddScreenComponents();
          }
     }
}
