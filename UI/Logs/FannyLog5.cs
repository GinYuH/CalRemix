using CalamityMod.Items.DraedonMisc;
using CalamityMod.UI.DraedonLogs;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;
using Terraria.Graphics;
using Terraria.ModLoader;

namespace CalRemix.UI
{
    public class FannyLog5 : DraedonsLogGUI
    {
        public override int TotalPages => 3;
        public override string GetTextByPage()
        {
            return Page switch
            {
                0 => "Well, well, well, looks like Fanny's fiery adventures have taken a chilly turn! Yours truly found himself in the depths of an underground ice cavern, where the air was as cold as a polar bear's nose and the ground was slick with frost."+
                "\n\nNow, you might be wondering how a flame like me ended up in such a frosty predicament. Truth be told, even Fanny needs a change of scenery every now and then. Plus, who can resist the allure of exploring a frozen labyrinth filled with mysteries?",
                1 => "So, there I was, tiptoeing (well, flame-floating, if you want to get technical) through the icy cavern, marveling at the shimmering walls of ice and the eerie silence that hung in the air like a thick blanket." +
                "\n\nAnd wouldn't you know it, nestled deep within the heart of the cavern, I stumbled upon yet another lab. This one was a stark contrast to the fiery labs of the underworld, with its icy corridors and frost-covered equipment." +
                "\n\nBut hey, a lab's a lab, right? And where there's a lab, there's bound to be another one of those futuristic tablets just waiting to be discovered.",
                _ => "Sure enough, tucked away in a corner of the lab, half-buried beneath a mound of snow and ice, was the object of my fiery curiosity. Now, did I encounter any lost souls in this frozen wasteland? Well, let's just say the only entities I found were those of the frosty critters scurrying about in the shadows. Not exactly the chatty types, if you catch my drift." +
                "\n\nBut for now, it's time for Fanny to bid adieu to the ice cavern and see what other frosty adventures await. Until next time, stay fiery, my friends!"

            };
        }
        public override Texture2D GetTextureByPage()
        {
            return ModContent.Request<Texture2D>("CalRemix/UI/Fanny/FannyHolo").Value;
        }
    }
}
