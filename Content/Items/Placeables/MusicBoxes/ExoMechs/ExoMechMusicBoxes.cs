using Terraria;
using Terraria.ModLoader;
using Terraria.Utilities;
using static Terraria.ModLoader.ModContent;

namespace CalRemix.Content.Items.Placeables.MusicBoxes.ExoMechs
{
    public class ApingasLarryMusicBox : RemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            MusicLoader.AddMusicBox(Mod, CalRemixMusic.ExoTwinsAres, Type, TileType<Tiles.MusicBoxes.ExoMechs.ApingasLarryMusicBox>());
        }
        public override void SetDefaults()
        {
            Item.createTile = TileType<Tiles.MusicBoxes.ExoMechs.ApingasLarryMusicBox>();
            base.SetDefaults();
        }
        public override bool? PrefixChance(int pre, UnifiedRandom rand) => base.PrefixChance(pre, rand);
    }
    public class ApingasMusicBox : RemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            MusicLoader.AddMusicBox(Mod, CalRemixMusic.ExoTwins, Type, TileType<Tiles.MusicBoxes.ExoMechs.ApingasMusicBox>());
        }
        public override void SetDefaults()
        {
            Item.createTile = TileType<Tiles.MusicBoxes.ExoMechs.ApingasMusicBox>();
            base.SetDefaults();
        }
        public override bool? PrefixChance(int pre, UnifiedRandom rand) => base.PrefixChance(pre, rand);
    }
    public class ApingasThanosMusicBox : RemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            MusicLoader.AddMusicBox(Mod, CalRemixMusic.ExoTwinsThanatos, Type, TileType<Tiles.MusicBoxes.ExoMechs.ApingasThanosMusicBox>());
        }
        public override void SetDefaults()
        {
            Item.createTile = TileType<Tiles.MusicBoxes.ExoMechs.ApingasThanosMusicBox>();
            base.SetDefaults();
        }
        public override bool? PrefixChance(int pre, UnifiedRandom rand) => base.PrefixChance(pre, rand);
    }
    public class LarryMusicBox : RemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            MusicLoader.AddMusicBox(Mod, CalRemixMusic.Ares, Type, TileType<Tiles.MusicBoxes.ExoMechs.LarryMusicBox>());
        }
        public override void SetDefaults()
        {
            Item.createTile = TileType<Tiles.MusicBoxes.ExoMechs.LarryMusicBox>();
            base.SetDefaults();
        }
        public override bool? PrefixChance(int pre, UnifiedRandom rand) => base.PrefixChance(pre, rand);
    }
    public class ThanosLarryMusicBox : RemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            MusicLoader.AddMusicBox(Mod, CalRemixMusic.ThanatosAres, Type, TileType<Tiles.MusicBoxes.ExoMechs.ThanosLarryMusicBox>());
        }
        public override void SetDefaults()
        {
            Item.createTile = TileType<Tiles.MusicBoxes.ExoMechs.ThanosLarryMusicBox>();
            base.SetDefaults();
        }
        public override bool? PrefixChance(int pre, UnifiedRandom rand) => base.PrefixChance(pre, rand);
    }
    public class ThanosMusicBox : RemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            MusicLoader.AddMusicBox(Mod, CalRemixMusic.Thanatos, Type, TileType<Tiles.MusicBoxes.ExoMechs.ThanosMusicBox>());
        }
        public override void SetDefaults()
        {
            Item.createTile = TileType<Tiles.MusicBoxes.ExoMechs.ThanosMusicBox>();
            base.SetDefaults();
        }
        public override bool? PrefixChance(int pre, UnifiedRandom rand) => base.PrefixChance(pre, rand);
    }
    public class XOMusicBox : RemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            MusicLoader.AddMusicBox(Mod, CalRemixMusic.ExoMechs, Type, TileType<Tiles.MusicBoxes.ExoMechs.XOMusicBox>());
        }
        public override void SetDefaults()
        {
            Item.createTile = TileType<Tiles.MusicBoxes.ExoMechs.XOMusicBox>();
            base.SetDefaults();
        }
        public override bool? PrefixChance(int pre, UnifiedRandom rand) => base.PrefixChance(pre, rand);
    }
}
