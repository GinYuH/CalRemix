using CalamityMod.Items.DraedonMisc;
using CalamityMod.UI.DraedonLogs;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Security.Cryptography;
using Terraria.ModLoader;

namespace CalRemix.UI
{
    public class FannyLog3 : DraedonsLogGUI
    {
        public override int TotalPages => 4;
        public override string GetTextByPage()
        {
            return Page switch
            {
                0 => "Well, well, well, what do we have here? Fanny's fiery adventures have led me deep into the heart of a jungle, where I stumbled upon yet another lab. And this one, my friends, is not for the faint of heart."+
                "\n\nThe moment I set foot inside, I could tell something was amiss. The air was thick with the scent of danger, and the lab itself looked like it had seen better days. But hey, when has a little danger ever stopped ol' Fanny from poking his fiery nose where it doesn't belong?",
                1 => "Now, about this lab. It was filled with all sorts of contraptions and gizmos, but the real kicker was the gaping hole in the ceiling, as if something had busted out in a big hurry. And what do you know, the lab was chock-full of experiments on some nasty business called 'plague', a green nanovirus that sounded like trouble with a capital T."+
                "\n\nYou'd think a flame like me would steer clear of such toxic stuff, but where's the fun in that? Besides, someone's gotta make sure these pesky nanoviruses don't wreak havoc on unsuspecting jungle critters.",
                2 => "So, there I was, poking around the lab, trying to make sense of all the bubbling vials and ominous-looking machines, when I stumbled upon yet another one of those futuristic tablets. Seems like they're becoming a regular part of my fiery adventures, huh?"+
                "\n\nNow, did I guide anyone through this treacherous jungle lab? Well, let's just say I tried. Found a cute little roomba, but then it was so happy that it killed itself the moment it caught sight of me! Can't say I blame 'em, though. Who wouldn't want the guidance of a talking flame?",
                _ => "\n\nBut for now, it's time for Fanny to bid adieu to this jungle lab and see what other fiery adventures await. Until next time, stay fiery, my friends!",
            };
        }
        public override Texture2D GetTextureByPage()
        {
            return ModContent.Request<Texture2D>("CalRemix/UI/Fanny/FannyHolo").Value;
        }
    }
}
