using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace CalRemix.Content.Tiles.Plates.Molten
{
    public abstract class BaseMoltenPlatePlaced : ModTile
    {
        public static readonly SoundStyle MinePlatingSound = new("CalamityMod/Sounds/Custom/PlatingMine", 3, SoundType.Sound);

        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            CalamityUtils.MergeWithGeneral(Type);
            HitSound = MinePlatingSound;
            MineResist = 1f;
            DustType = DustID.LavaMoss;
            AddMapEntry(new Color(235, 108, 108), null);
        }
    }

    public class MoltenAeroplatePlaced : BaseMoltenPlatePlaced
    {

    }

    public class MoltenBloodplatePlaced : BaseMoltenPlatePlaced
    {

    }

    public class MoltenOnyxplatePlaced : BaseMoltenPlatePlaced
    {

    }

    public class MoltenCinderplatePlaced : BaseMoltenPlatePlaced
    {

    }

    public class MoltenHavocplatePlaced : BaseMoltenPlatePlaced
    {

    }

    public class MoltenElumplatePlaced : BaseMoltenPlatePlaced
    {

    }

    public class MoltenNavyplatePlaced : BaseMoltenPlatePlaced
    {

    }

    public class MoltenPlagueplatePlaced : BaseMoltenPlatePlaced
    {

    }
}