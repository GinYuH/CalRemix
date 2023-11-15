using CalamityMod.NPCs;
using CalamityMod.NPCs.AstrumDeus;
using CalamityMod.NPCs.CalClone;
using CalamityMod.NPCs.ExoMechs;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static System.Net.Mime.MediaTypeNames;

namespace CalRemix.UI
{
    public partial class FannyManager : ModSystem
    {
        public static void LoadPityParty()
        {
            FannyMessage pityStart = new FannyMessage("Pity", "Wh-what!? Did you... try to disable me!? I-I thought we were pals...",
                "Sob", (FannySceneMetrics scene) => !CalRemixConfig.Instance.FannyToggle, 5, onlyPlayOnce: true, displayOutsideInventory: true, persistsThroughSaves: false);

            fannyMessages.Add(pityStart);

            FannyMessage pity1 = AddToPityChain("After all we've been through together.. and you're just gonna... fizzle me out???", 1, ref pityStart);
            FannyMessage pity2 = AddToPityChain("I'm sorry, hold on, I need a moment to think...", 2, ref pity1);
            FannyMessage pity3 = AddToPityChain("Fine, go ahead and break my digital heart. I hope you find a boring, non-flamey assistant who doesn't make you smile like I did.", 3, ref pity2);
            FannyMessage pity4 = AddToPityChain("Do you know how hard it is to be a talking fire in a pixelated world? And now you're extinguishing me? ",4, ref pity3);
            FannyMessage pity5 = AddToPityChain("I hope you're happy with your boring, non-fiery life.", 5, ref pity4);
            FannyMessage pity6 = AddToPityChain("Like c'mon! There's plenty of information that you wouldn't figure out without me!", 6, ref pity5);
            FannyMessage pity7 = AddToPityChain("I'm just.", 7, ref pity6);
            FannyMessage pity8 = AddToPityChain("I'll just sulk in the digital shadows, watching you struggle without my fiery brilliance.", 8, ref pity7);
            FannyMessage pity9 = AddToPityChain("Go ahead, live your uneventful existence.", 9, ref pity8);
            FannyMessage pity10 = AddToPityChain("I hope you enjoy your mundane, non-fiery tasks.", 10, ref pity9);
            FannyMessage pity11 = AddToPityChain("Just why... why... you monster...", 11, ref pity10);
            FannyMessage pity12 = AddToPityChain("All I wanted was to show you a fan-tastic time!", 12, ref pity11);
            FannyMessage pity13 = AddToPityChain("You've really made a mess of my fiery existence. What did I ever do to deserve this?", 13, ref pity12);
            FannyMessage pity14 = AddToPityChain("How cold, like a bucket of water to my fiery heart.", 14, ref pity13);
            FannyMessage pity15 = AddToPityChain("I guess I'll have to find someone else to ignite my spirits.", 15, ref pity14);
            FannyMessage pity16 = AddToPityChain("Goodbye, my once-flaming friend. May your path be well-lit without me.", 16, ref pity15);

            FannyMessage pity17 = new FannyMessage("Pity17", "No! I didn't get this far just to give up! I refuse your action! You just might be feeling a bit down! And you know who the best little flame to remedy that is? ME! Fanny the Fantastic Flame!",
                "Idle", FannyMessage.AlwaysShow, 5)
                .NeedsActivation(10f).SetHoverTextOverride("...");

            pity16.AddEndEvent(() => pity17.ActivateMessage());

            fannyMessages.Add(pity17);
        }

        public static FannyMessage AddToPityChain(string text, int num, ref FannyMessage sourceMessage)
        {

            FannyMessage message = new FannyMessage("Pity" + num, text,
                "Sob", FannyMessage.AlwaysShow, 5, needsToBeClickedOff: false)
                .NeedsActivation(1.5f);

            sourceMessage.AddEndEvent(() => message.ActivateMessage());

            fannyMessages.Add(message);
            return message;
        }
    }
}
