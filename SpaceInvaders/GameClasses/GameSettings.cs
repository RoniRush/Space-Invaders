namespace SpaceInvaders.GameClasses
{
     using System.Collections.Generic;

     public static class GameSettings
     {
          public enum eSoundEffects
          {
               ShipShot,
               EnemyShot,
               EnemyKilled,
               MotherShipKilled,
               BarrierHit,
               GameOver,
               LevelWin,
               LifeDie,
               MenuMove
          }

          public static readonly int sr_DefaultScreenWidth = 1000;
          public static readonly int sr_DefaultScreenHeight = 660;
          public static readonly float sr_DefaultBGVolume = 0.3f;
          public static readonly float sr_DefaultSoundEffectVolume = 0.8f;


          public static int NumOfPlayers { get; set; } = 1;

          public static Dictionary<eSoundEffects, string> s_SoundEffectsNames = new Dictionary<eSoundEffects, string>()
               {
                    { eSoundEffects.ShipShot, "SSGunShot" },
                    { eSoundEffects.EnemyShot, "EnemyGunShot" },
                    { eSoundEffects.EnemyKilled, "EnemyKill" },
                    { eSoundEffects.MotherShipKilled, "MotherShipKill" },
                    { eSoundEffects.BarrierHit, "BarrierHit" },
                    { eSoundEffects.GameOver, "GameOver" },
                    { eSoundEffects.LevelWin, "LevelWin" },
                    { eSoundEffects.LifeDie, "LifeDie" },
                    { eSoundEffects.MenuMove, "MenuMove" }
               };

          public static string s_BackgroundSoundName = "BGMusic";
     }
}
