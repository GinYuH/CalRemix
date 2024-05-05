using CalamityMod.Items.DraedonMisc;
using CalamityMod.UI.DraedonLogs;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;
using Terraria.ModLoader;

namespace CalRemix.UI
{
    public class FannyLog1 : DraedonsLogGUI
    {
        public override int TotalPages => 3;
        public override string GetTextByPage()
        {
            return Page switch
            {
                0 => "Well, well, well, what do we have here? It seems ol' Fanny stumbled upon a shiny new toy today! Yep, yours truly found a futuristic tablet lying around, probably dropped by some forgetful time traveler. Can't say I'm complaining though, I've always wanted to be a tech-savvy flame.\n" +
                                        "\nSo, here I am, tapping away on this sleek piece of future gadgetry. Gotta admit, it's a bit tricky to type with these fiery fingers, but practice makes perfect, right?",
                1 => "Now, you might be wondering, why would a flame like me bother with keeping a journal? Well, let me tell ya, being a helpful but perpetually wisecracking guide to lost souls ain't easy. Sometimes, even Fanny needs to vent, and what better way to do it than in a digital diary?" +
                                        "\n\nToday's been a typical day in the life of yours truly. Guided a lost traveler through the twists and turns of a labyrinth, cracked a few fiery puns along the way, and accidentally set a bush or two on fire. Hey, nobody's perfect!",
                _ => "But hey, it's all in a day's work for Fanny the Flame, the hottest guide this side of wherever we are. Who knows what adventures tomorrow will bring? One thing's for sure, with me around, things are always bound to heat up!",
            };
        }
        public override Texture2D GetTextureByPage()
        {
            return ModContent.Request<Texture2D>("CalRemix/UI/Fanny/FannyHolo").Value;
        }
    }
}
