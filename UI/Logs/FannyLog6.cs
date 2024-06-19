using CalamityMod.Items.DraedonMisc;
using CalamityMod.UI.DraedonLogs;
using CalRemix.UI.ElementalSystem;
using Humanizer;
using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;
using System.Collections.Generic;
using Terraria.Graphics;
using Terraria.ModLoader;

namespace CalRemix.UI
{
    public class FannyLog6 : DraedonsLogGUI
    {
        public override int TotalPages => 4;
        public override string GetTextByPage()
        {
            return Page switch
            {
                0 => "Well, well, well, looks like Fanny's fiery adventures have brought me to the grand finale! Yours truly found himself in the depths of an abandoned mining facility, where the air was heavy with dust and the echoes of forgotten pickaxes still rang in the tunnels.\n\n" +
                "Now, you might be wondering what led me to such a desolate place. Truth be told, even Fanny needs a little excitement now and then, and what's more exciting than exploring an abandoned mine, right?",
                1 => "So, there I was, weaving my way through the labyrinthine tunnels of the mining facility, marveling at the rusty machinery and discarded tools that littered the ground. And wouldn't you know it, nestled deep within the heart of the mine, I stumbled upon yet another lab." +
                "\n\nBut this lab was different.It was bigger, grander, and more ominous than any I had encountered before.The walls were lined with strange symbols, and the air hummed with an otherworldly energy.I could feel it in my fiery bones: this was no ordinary lab.",
                2 => "Undeterred by the creeping sense of unease, I pressed on, my flames burning brighter than ever. And there, in the heart of the lab, surrounded by flickering lights and the steady drip of water, I found it: the final piece of the puzzle, the last of the futuristic tablets.\n\n" +
                "But as I reached out to claim my prize, a voice echoed through the chamber, sending shivers down my fiery spine. \"Stop right there, flame,\" it boomed, and I turned to see a figure emerging from the shadows." +
                "\n\nIt was none other than the creator of the lab, a tall robotic scientist with stick legs! And he wasn't too pleased to find me rummaging through his secrets.",
                _ => "A battle ensued, flames clashing against the darkness as we fought for control of the lab. It was a showdown for the ages, a clash of fire and fury that shook the very foundations of the mine." +
                "\n\nBut in the end, it was yours truly who emerged victorious. That robot guy dismantled and dead on the floor." +
                "\n\nAnd as I made my fiery exit from the abandoned mining facility, I couldn't help but smile. For though the journey had been perilous and the dangers many, Fanny the Flame had triumphed once again." +
                "\n\nUntil next time, my friends, stay fiery and never let the flames die out!"
            };
    }
    public override Texture2D GetTextureByPage()
    {
            return ModContent.Request<Texture2D>("CalRemix/UI/Fanny/HelperFannyHolo").Value;
        }
    }
}
