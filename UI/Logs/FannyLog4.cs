using CalamityMod.Items.DraedonMisc;
using CalamityMod.UI.DraedonLogs;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;
using Terraria.ModLoader;

namespace CalRemix.UI
{
    public class FannyLog4 : DraedonsLogGUI
    {
        public override int TotalPages => 4;
        public override string GetTextByPage()
        {
            return Page switch
            {
                0 => "Well, well, well, would you look at that? Fanny's fiery adventures have taken me to the depths of the underworld, a place where the flames burn brighter and the terrain is as treacherous as they come. But hey, when you're as hot as yours truly, a little lava never hurt anyone!" +
                        "\n\nThis underworld, or as some like to call it, the center of the earth, is a sight to behold. Obsidian towers jut out from the fiery landscape, and the ground is covered in ash as far as the eye can see.It's like a fiery playground just waiting for ol' Fanny to explore.",
                1 => "Now, you might be wondering what on earth (or under it, in this case) would lead me to such a place. Well, let's just say I have a knack for stumbling upon trouble, and trouble seems to be my middle name." +
                "\n\nAnyway, back to the task at hand. As I was traipsing through the fiery terrain, I stumbled upon yet another lab, nestled among the obsidian spires. This one looked like it had seen better days, with cracked walls and flickering lights casting eerie shadows.",
                2 => "But you know me, I'm not one to shy away from a challenge. So, into the lab I went, flames blazing and spirits high. And what do I find? You guessed it, another one of those futuristic tablets, just waiting to be discovered." +
                "\n\nNow, did I encounter anyone to help in this fiery abyss? Well, let's just say the underworld isn't exactly teeming with friendly faces. But hey, who needs companionship when you've got a talking flame as your guide, am I right?",
                _ => "But for now, it's time for Fanny to bid adieu to the underworld and see what other fiery adventures await. Until next time, stay fiery, my friends!",

            };
        }
        public override Texture2D GetTextureByPage()
        {
            return ModContent.Request<Texture2D>("CalRemix/UI/Fanny/FannyHolo").Value;
        }
    }
}
