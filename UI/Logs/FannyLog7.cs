using CalamityMod.Items.DraedonMisc;
using CalamityMod.UI.DraedonLogs;
using CalRemix.UI.ElementalSystem;
using Humanizer;
using Microsoft.Build.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;
using Terraria;
using Terraria.Graphics;
using Terraria.ModLoader;
using static System.Net.Mime.MediaTypeNames;

namespace CalRemix.UI
{
    public class FannyLog7 : TerminalUI
    {
        public override int TotalPages => 3;
        public override string GetTextByPage()
        {
            return Page switch
            {
                0 => "Well, well, well, looks like Fanny's fiery adventures have taken a dive, quite literally! Yours truly found himself plunging into the depths of the abyss, an underwater trench where the darkness seems to swallow everything whole." +
                "\n\nBut fear not, for where others see darkness, I see opportunity! And what do you know, nestled at the bottom of the abyss, in a chest covered in barnacles and seaweed, was a sight to behold: my dear old dad's prized artifact, a slab made of moonstone with a fiery red eye staring back at me.",
                1 => "Now, you might be wondering what a flame like me is doing swimming around in the murky depths of the ocean. Truth be told, even Fanny needs a change of scenery every now and then, and where better to find adventure than in the watery depths?" +
                "\n\nWith a flicker of excitement, I reached out and claimed the artifact, feeling its power thrumming beneath my fiery touch. And as I held it aloft, I couldn't help but feel a surge of pride. For though my dear old dad may not see eye to eye with me, I've proven time and time again that I'm more than just a flame in the wind.",
                _ => "Now, as for scribbling an entry on this moonstone slab, well, why not? After all, it's not every day you find yourself at the bottom of the abyss, holding a prized artifact in your fiery hands." +
                "\n\nSo here's to another chapter in the ongoing adventures of Fanny the Flame. Who knows what mysteries await us in the depths of the ocean? One thing's for sure, with me around, things are always bound to heat up!" +
                "\n\nUntil next time, stay fiery, my friends, and never stop diving into the unknown!"
            };
    }
    }
}
