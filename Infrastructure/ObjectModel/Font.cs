namespace Infrastructure.ObjectModel
{
     using System.Text;
     using Infrastructure.ObjectModel.Animators;
     using Microsoft.Xna.Framework;
     using Microsoft.Xna.Framework.Graphics;

     public class Font : Sprite
     {
          private const string k_FontPath = @"Fonts/Comic Sans MS";
          private SpriteFont m_ComicFont;
          public string m_Message;
          private string m_OriginalMessage;
          private bool m_MessageInitialized;

          public bool HasAnimation { get; set; }

          public bool AnimationFinished { get; set; }

          public string Message
          {
               get
               {
                    return m_Message;
               }

               set
               {
                    if(!m_MessageInitialized)
                    {
                         m_MessageInitialized = true;
                         m_Message = m_OriginalMessage = value;
                    }
                    else
                    {
                         m_Message = value;
                    }

                    changeWidthAndHeight();
               }
          }

          protected override void InitBounds()
          {
               changeWidthAndHeight();
          }

          private void changeWidthAndHeight()
          {
               if(m_ComicFont != null)
               {
                    m_WidthBeforeScale = m_ComicFont.MeasureString(new StringBuilder(m_Message)).X;
                    m_HeightBeforeScale = m_ComicFont.MeasureString(new StringBuilder(m_Message)).Y;
               }
          }

          public Font(Game i_Game)
               : base(k_FontPath, i_Game, int.MaxValue)
          {
               Scales = new Vector2(1);
          }

          public void InitAnimations(params SpriteAnimator[] i_SpriteAnimators)
          {
               foreach(SpriteAnimator spriteAnimator in i_SpriteAnimators)
               {
                    HasAnimation = true;
                    Animations.Add(spriteAnimator);
                    Animations.Enabled = false;
                    Animations[spriteAnimator.Name].Enabled = false;
               }
          }

          public void UpdateMessage(string i_Message)
          {
               m_Message = string.Format(@"{0} {1}", m_OriginalMessage, i_Message);
          }

          protected override void DrawBoundingBox()
          {
          }

          public override void Draw(GameTime i_GameTime)
          {
               m_SpriteBatch.DrawString(
                    m_ComicFont,
                    m_Message,
                    Position,
                    TintColor,
                    0,
                    Vector2.Zero,
                    Scales,
                    SpriteEffects.None,
                    0);
          }

          protected override void LoadContent()
          {
               m_ComicFont = Game.Content.Load<SpriteFont>(m_AssetName);
               if (m_SpriteBatch == null)
               {
                    m_SpriteBatch =
                         Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;

                    if (m_SpriteBatch == null)
                    {
                         m_SpriteBatch = new SpriteBatch(Game.GraphicsDevice);
                    }
               }
          }
     }
}
