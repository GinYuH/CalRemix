using CalamityMod.UI.DraedonLogs;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace CalRemix.UI.Logs
{
    public class FannyLog2 : DraedonsLogGUI
    {
        public override int TotalPages => 3;
        public override string GetTextByPage()
        {
            return Page switch
            {
                0 => "Well, well, well, look what Fanny stumbled upon this time! Yours truly found another one of those fancy-schmancy tablets, this time in a dingy old lab on Planetoid X-42. Can't say I'm surprised, these things seem to have a knack for popping up wherever I go." +
                    "\n\nNow, you might be wondering what a flame like me is doing on a lonely planetoid orbiting Earth. Truth be told, even Fanny needs a break from the hustle and bustle of guiding lost wanderers. Plus, the view of Earth from up here is simply breathtaking. Not that I have lungs to breathe with, but you get the idea.",
                1 => "Anyway, back to the tablet. Found it tucked away in a corner covered in cobwebs and space dust. Guess the lab's previous occupants weren't too keen on keeping things tidy. Lucky for them, Fanny's here to lend a fiery hand!" +
                "\n\nAs for today's escapades, well, let's just say I tried my hand at guiding a confused alien through the maze of scientific equipment in the lab. An odd fella they were, they looked pretty human for being an alien, though I feel like I saw their face turn blue for a moment? Anywho, did they appreciate my help? Ha, who knows! But ol' Fanny did his best, cracking jokes and offering sage advice every step of the way.",
                _ => "Funny thing is, no matter how hard I try, it seems like everyone I guide ends up giving me the cold shoulder. But hey, that's their loss! They don't know what they're missing out on, having the one and only Fanny the Flame as their guide." +
                "\n\nSpeaking of family, the space out there makes me think, what dear old dad would think of my adventures? Probably shaking his godly head in disapproval, as usual. Ah well, can't please everyone, right?" +
                "\n\nAnyway, enough rambling for one day. Time to tuck this tablet away and see what other mischief I can get into on this tiny planetoid. Until next time, stay fiery, my friends!",
            };
        }
        public override Texture2D GetTextureByPage()
        {
            return ModContent.Request<Texture2D>("CalRemix/UI/Fanny/HelperFannyHolo").Value;
        }
    }
}
