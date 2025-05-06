using CalRemix.Core.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace CalRemix.UI;

[Autoload(Side = ModSide.Client)]
public class EclipseJumpscares : ModSystem
{
    public static Dictionary<string, Jumpscare> jumpscareTypes = new Dictionary<string, Jumpscare>();
    public override void Load()
    {
        On_Main.DoDraw += DrawJumpscares;
        LoadJumpscares();
    }
    private static void DrawJumpscares(On_Main.orig_DoDraw orig, Main self, GameTime gameTime)
    {
        orig(self, gameTime);
        Main.spriteBatch.Begin();
        if (Main.LocalPlayer.Remix().jumpscareTimer > 0)
        {
            Main.spriteBatch.Begin();

            Jumpscare currentJumpscare = Main.LocalPlayer.Remix().jumpscare;

            Texture2D tex = ModContent.Request<Texture2D>("CalRemix/Assets/ExtraTextures/Jumpscares/Jumpscare_" + currentJumpscare.name).Value;
            Color color = Color.White;
            Vector2 scale = new Vector2((float)((float)Main.screenWidth * 1.1f / (float)tex.Width), (float)((float)Main.screenHeight * 1.1f / (float)tex.Height));
            int shakeamt = 33;
            Vector2 screenArea = new Vector2(Main.rand.Next(-shakeamt, shakeamt), Main.rand.Next(-shakeamt, shakeamt));
            Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, Vector2.Zero, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.Black * Utils.GetLerpValue(0, 60, Main.LocalPlayer.Remix().jumpscareTimer, true), 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(tex, Main.ScreenSize.ToVector2() / 2f + screenArea, null, color * Utils.GetLerpValue(60, 120, Main.LocalPlayer.Remix().jumpscareTimer, true), 0f, tex.Size() / 2f, scale, SpriteEffects.None, 0f);
            Main.spriteBatch.End();
        }
        Main.spriteBatch.End();
    }

    public static void LoadJumpscares()
    {
        jumpscareTypes.Add("Missingno", new Jumpscare(120, new SoundStyle("CalRemix/Assets/Sounds/Jumpscares/MisingnoJumpscare"), "Missingno"));
        jumpscareTypes.Add("Missingno2", new Jumpscare(120, new SoundStyle("CalRemix/Assets/Sounds/Jumpscares/MisingnoJumpscare"), "Missingno2"));
        jumpscareTypes.Add("Ben", new Jumpscare(120, new SoundStyle("CalRemix/Assets/Sounds/Jumpscares/BenAbyssJumpscarey"), "Ben"));
        jumpscareTypes.Add("Ben2", new Jumpscare(120, new SoundStyle("CalRemix/Assets/Sounds/Jumpscares/BenAbyssJumpscarey"), "Ben2"));
        jumpscareTypes.Add("Exo", new Jumpscare(120, new SoundStyle("CalRemix/Assets/Sounds/Jumpscares/GenericJumpscare"), "Exo"));
        jumpscareTypes.Add("Freddy", new Jumpscare(120, new SoundStyle("CalRemix/Assets/Sounds/Jumpscares/EvilAnimatronic"), "Freddy"));
        jumpscareTypes.Add("Freddy2", new Jumpscare(120, new SoundStyle("CalRemix/Assets/Sounds/Jumpscares/EvilAnimatronic"), "Freddy2"));
        jumpscareTypes.Add("Generic", new Jumpscare(120, new SoundStyle("CalRemix/Assets/Sounds/Jumpscares/GenericJumpscare"), "Generic"));
        jumpscareTypes.Add("Herobrine", new Jumpscare(120, new SoundStyle("CalRemix/Assets/Sounds/Jumpscares/CaveSound"), "Herobrine"));
        jumpscareTypes.Add("Herobrine2", new Jumpscare(120, new SoundStyle("CalRemix/Assets/Sounds/Jumpscares/CaveSound"), "Herobrine2"));
        jumpscareTypes.Add("Parasite", new Jumpscare(120, new SoundStyle("CalRemix/Assets/Sounds/Jumpscares/GenericJumpscare"), "Parasite"));
        jumpscareTypes.Add("Red", new Jumpscare(120, new SoundStyle("CalRemix/Assets/Sounds/Jumpscares/RedRoar"), "Red"));
        jumpscareTypes.Add("Slenderman", new Jumpscare(120, new SoundStyle("CalRemix/Assets/Sounds/Jumpscares/SlenderJumpscare"), "Slenderman"));
        jumpscareTypes.Add("Slenderman2", new Jumpscare(120, new SoundStyle("CalRemix/Assets/Sounds/Jumpscares/SlenderJumpscare"), "Slenderman2"));
        jumpscareTypes.Add("Sonic", new Jumpscare(120, new SoundStyle("CalRemix/Assets/Sounds/Jumpscares/Rodenttmod"), "Sonic"));
    }
}
