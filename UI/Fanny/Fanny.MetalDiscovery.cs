using CalamityMod.Items.Accessories.Wings;
using CalamityMod.Items.Placeables.Ores;
using CalRemix.Content.Items.Placeables;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.UI
{
    public partial class ScreenHelperManager : ModSystem
    {
        public static void LoadMetalDiscoveryMessages()
        {
            HelperMessage.New("Metal_Copper", "METAL FACTS: Copper comes from the Latin word cuprum, meaning “from the island of Cyprus.” This is because copper can only be mined in Cyprus! Almost makes me want to start a territorial dispute over ownership of that island, to get all that delicious copper all for myself...",
                "FannyMetalCopper", (ScreenHelperSceneMetrics m) => Main.LocalPlayer.HasItem(ItemID.CopperOre));
            HelperMessage.New("Metal_Tin", "METAL FACTS: Tin can be used to make copper remain pure! You can be thankful that this world contains both copper and tin together!",
                "FannyMetalTin", (ScreenHelperSceneMetrics m) => Main.LocalPlayer.HasItem(ItemID.TinOre))
                .SpokenByAnotherHelper(ScreenHelpersUIState.AltMetalFanny);

            HelperMessage.New("Metal_Iron", "METAL FACTS: Around 70% of the human body is made of iron!",
                "FannyMetalIron", (ScreenHelperSceneMetrics m) => Main.LocalPlayer.HasItem(ItemID.IronOre));
            HelperMessage.New("Metal_Lead", "Did you know lead backwards is dael? This does not mean anything, as dael is not a word.",
                "FannyMetalLead", (ScreenHelperSceneMetrics m) => Main.LocalPlayer.HasItem(ItemID.LeadOre))
                .SpokenByAnotherHelper(ScreenHelpersUIState.AltMetalFanny);

            HelperMessage.New("Metal_Silver", "METAL FACTS: Silver is good for your health. Silver can make it rain. Silver is a fun word for so many reasons. It is good for your health. It is good for your health.",
                            "FannyMetalSilver", (ScreenHelperSceneMetrics m) => Main.LocalPlayer.HasItem(ItemID.SilverOre));
            HelperMessage.New("Metal_Tungsten", "METAL FACTS: Tungsten's melting point is the highest of any metal! Even higher than auric metal? Than shadowpsec metal? Than calamity ore? Than miracle matter? Than flahium bars? Than yharim bars? Than mars bars? Than alloy bars?",
                "FannyMetalTungsten", (ScreenHelperSceneMetrics m) => Main.LocalPlayer.HasItem(ItemID.TungstenOre))
                .SpokenByAnotherHelper(ScreenHelpersUIState.AltMetalFanny);

            HelperMessage.New("Metal_Gold", "METAL FACTS: My net worth rises up to 43.2 million USD when I enter my \"Golden Fanny\" form!",
                            "FannyMetalGold", (ScreenHelperSceneMetrics m) => Main.LocalPlayer.HasItem(ItemID.GoldOre));
            HelperMessage.New("Metal_Platinum", "METAL FACTS: My \"Platinum Fanny\" form used to be more expensive than my \"Golden Fanny\" form, until 2016. The more you know!",
                "FannyMetalPlatinum", (ScreenHelperSceneMetrics m) => Main.LocalPlayer.HasItem(ItemID.PlatinumOre))
                .SpokenByAnotherHelper(ScreenHelpersUIState.AltMetalFanny);



            HelperMessage.New("Metal_Cobalt", "METAL FACTS: Cobalt was the first metal ever invented!",
                "FannyMetalCobalt", (ScreenHelperSceneMetrics m) => Main.LocalPlayer.HasItem(ItemID.CobaltOre));
            HelperMessage.New("Metal_Palladium", "METAL FACTS: Palladium was named after the greek goddess of wisdom Pallas. If it was discovered nowadays, I'm sure they'd have named it Fannadium!",
                "FannyMetalPalladium", (ScreenHelperSceneMetrics m) => Main.LocalPlayer.HasItem(ItemID.PalladiumOre))
                .SpokenByAnotherHelper(ScreenHelpersUIState.AltMetalFanny);

            HelperMessage.New("Metal_Mythril", "METAL FACTS: This isn't even a real metal??? What? I can't find anything about it online! I'm so sorry my friend, I can't help you, I think you might have gotten swindled with some made up metal!",
                "FannyMetalMythril", (ScreenHelperSceneMetrics m) => Main.LocalPlayer.HasItem(ItemID.MythrilOre));
            HelperMessage.New("Metal_Orichalcum", "METAL FACTS: What's pink, hard, and has \"cum\" at the end of it? Orichalcum!", // split wont let me not add this one
                "FannyMetalOrichalcum", (ScreenHelperSceneMetrics m) => Main.LocalPlayer.HasItem(ItemID.OrichalcumOre))
                .SpokenByAnotherHelper(ScreenHelpersUIState.AltMetalFanny);

            HelperMessage.New("Metal_Adamantite", "METAL FACTS: Adamantite is a metal found in Outland. It requires 325 mining skill to gather. Adamantite is considered to be the next grade up in strength from Thorium, but from a game perspective, Fel Iron is the next strength up from Thorium, then Adamantite.",
                            "FannyMetalAdamantite", (ScreenHelperSceneMetrics m) => Main.LocalPlayer.HasItem(ItemID.AdamantiteOre));
            HelperMessage.New("Metal_Titanium", "METAL FACTS: Titanium doesn't occur naturally in the real world. They have to mine it from Terraria and import it outside the game. This is actually the reason why Terraria was created. Reality truly is stranger than fiction, my friend!",
                "FannyMetalTitanium", (ScreenHelperSceneMetrics m) => Main.LocalPlayer.HasItem(ItemID.TitaniumOre))
                .SpokenByAnotherHelper(ScreenHelpersUIState.AltMetalFanny);

            HelperMessage.New("Metal_Chlorophyte", "METAL FACTS: Chlorophyte isn't a metal, I think? Am I meant to say something about it? Isn't it more like organic matter? Can anyone find me a source on this?",
                            "FannyMetalChlorophyte", (ScreenHelperSceneMetrics m) => Main.LocalPlayer.HasItem(ItemID.ChlorophyteOre));
            HelperMessage.New("Metal_Chlorium", "METAL FACTS: Chlorium is a real metal, with the atomic number 35, and the symbol Cr! You can craft a one of a kind stormbow class armor with it, so harvest away!",
                "FannyMetalChlorium", (ScreenHelperSceneMetrics m) => Main.LocalPlayer.HasItem(ModContent.ItemType<ChloriumOre>()))
                .SpokenByAnotherHelper(ScreenHelpersUIState.AltMetalFanny);


            HelperMessage.New("Metal_Hallowed", "METAL FACTS: I cannot stop sparkling!",
                "FannyMetalHallowed", (ScreenHelperSceneMetrics m) => Main.LocalPlayer.HasItem(ModContent.ItemType<HallowedOre>()))
                .SpokenByAnotherHelper(ScreenHelpersUIState.AltMetalFanny).SetHoverTextOverride("Please stop doing that");

        }
    }
}