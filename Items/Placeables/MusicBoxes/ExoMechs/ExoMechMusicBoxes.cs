using Terraria;
using Terraria.ModLoader;
using Terraria.Utilities;
using static Terraria.ModLoader.ModContent;

namespace CalRemix.Items.Placeables.MusicBoxes.ExoMechs
{
    public class ApingasLarryMusicBox : RemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Exos/ApingasLarry"), Type, TileType<Tiles.MusicBoxes.ExoMechs.ApingasLarryMusicBox>());
        }
        public override void SetDefaults()
        {
            Item.createTile = ModContent.TileType<Tiles.MusicBoxes.ExoMechs.ApingasLarryMusicBox>();
            base.SetDefaults();
        }
        public override bool? PrefixChance(int pre, UnifiedRandom rand) => base.PrefixChance(pre, rand);
    }
    public class ApingasMusicBox : RemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Exos/Apingas"), Type, TileType<Tiles.MusicBoxes.ExoMechs.ApingasMusicBox>());
        }
        public override void SetDefaults()
        {
            Item.createTile = ModContent.TileType<Tiles.MusicBoxes.ExoMechs.ApingasMusicBox>();
            base.SetDefaults();
        }
        public override bool? PrefixChance(int pre, UnifiedRandom rand) => base.PrefixChance(pre, rand);
    }
    public class ApingasThanosMusicBox : RemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Exos/ApingasThanos"), Type, TileType<Tiles.MusicBoxes.ExoMechs.ApingasThanosMusicBox>());
        }
        public override void SetDefaults()
        {
            Item.createTile = ModContent.TileType<Tiles.MusicBoxes.ExoMechs.ApingasThanosMusicBox>();
            base.SetDefaults();
        }
        public override bool? PrefixChance(int pre, UnifiedRandom rand) => base.PrefixChance(pre, rand);
    }
    public class LarryMusicBox : RemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Exos/Larry"), Type, TileType<Tiles.MusicBoxes.ExoMechs.LarryMusicBox>());
        }
        public override void SetDefaults()
        {
            Item.createTile = ModContent.TileType<Tiles.MusicBoxes.ExoMechs.LarryMusicBox>();
            base.SetDefaults();
        }
        public override bool? PrefixChance(int pre, UnifiedRandom rand) => base.PrefixChance(pre, rand);
    }
    public class ThanosLarryMusicBox : RemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Exos/ThanosLarry"), Type, TileType<Tiles.MusicBoxes.ExoMechs.ThanosLarryMusicBox>());
        }
        public override void SetDefaults()
        {
            Item.createTile = ModContent.TileType<Tiles.MusicBoxes.ExoMechs.ThanosLarryMusicBox>();
            base.SetDefaults();
        }
        public override bool? PrefixChance(int pre, UnifiedRandom rand) => base.PrefixChance(pre, rand);
    }
    public class ThanosMusicBox : RemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Exos/Thanos"), Type, TileType<Tiles.MusicBoxes.ExoMechs.ThanosMusicBox>());
        }
        public override void SetDefaults()
        {
            Item.createTile = ModContent.TileType<Tiles.MusicBoxes.ExoMechs.ThanosMusicBox>();
            base.SetDefaults();
        }
        public override bool? PrefixChance(int pre, UnifiedRandom rand) => base.PrefixChance(pre, rand);
    }
    public class XOMusicBox : RemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Exos/XO"), Type, TileType<Tiles.MusicBoxes.ExoMechs.XOMusicBox>());
        }
        public override void SetDefaults()
        {
            Item.createTile = ModContent.TileType<Tiles.MusicBoxes.ExoMechs.XOMusicBox>();
            base.SetDefaults();
        }
        public override bool? PrefixChance(int pre, UnifiedRandom rand) => base.PrefixChance(pre, rand);
    }
}
