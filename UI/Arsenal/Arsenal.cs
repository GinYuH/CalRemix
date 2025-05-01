using CalRemix.Content.Items.Potions;
using Humanizer;
using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.UI;
using Terraria.UI;
using Terraria.UI.Chat;

namespace CalRemix.UI.Arsenal;

public class ArsenalUIState : UIState
{
    internal UIPanel background;
    internal UIImage logo;
    internal UIButton<string> closeButton;

    public override void OnInitialize()
    {
        background = new UIPanel();
        background.Width.Set(1000, 0);
        background.Height.Set(600, 0);
        background.BackgroundColor = Color.Blue;
        background.HAlign = 0.5f;
        background.VAlign = 0.5f;

        logo = new UIImage(ModContent.Request<Texture2D>("CalRemix/Assets/Textures/Arsenal/ArsenalLogo"));
        logo.IgnoresMouseInteraction = false;
        logo.OnLeftClick += ArsenalUtils.OpenTimeline;

        logo.Width.Pixels = 156;
        logo.Height.Pixels = 64;
        background.Append(logo);

        closeButton = new("X")
        {
            HAlign = 1f,
            BackgroundColor = Color.DarkRed,
            HoverPanelColor = Color.Red
        };
        closeButton.OnLeftClick += ArsenalUtils.CloseArsenal;
        closeButton.Width.Pixels = 32;
        closeButton.Height.Pixels = 32;

        background.Append(closeButton);

        Append(background);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (background.ContainsPoint(Main.MouseScreen))
            Main.LocalPlayer.mouseInterface = true;
    }
}

public class ArsenalTimelineUI : ArsenalUIState
{
    internal readonly List<Post> posts = [];

    internal UIScrollbar scrollBar;

    internal UIList timeline;

    public override void OnInitialize()
    {
        base.OnInitialize();

        scrollBar = new UIScrollbar
        {
            HAlign = 1f,
            VAlign = 0.5f
        };
        scrollBar.Height.Pixels = 400;

        timeline = [];
        timeline.SetScrollbar(scrollBar);
        timeline.HAlign = 0.5f;
        timeline.VAlign = 1f;
        timeline.Width.Pixels = 500;
        timeline.Height.Pixels = 600;

        background.Append(timeline);
        background.Append(scrollBar);
    }

    public override void OnActivate()
    {
        scrollBar.ViewPosition = 0f;

        posts.Clear();

        foreach (var v in ArsenalSystem.Posts)
        {
            Post newPost;

            if(v.Value.Requirement.Invoke())
            {
                string activeExtension = LanguageManager.Instance.ActiveCulture.Name;
                string path = "UI/Arsenal/" + activeExtension + "/Posts/" + v.Key + ".json";

                // Fall back to english if not found
                if (!CalRemix.instance.FileExists(path))
                    path = "UI/Arsenal/en-US/Posts/" + v.Key + ".json";

                // Throw if we cant find english either
                if (!CalRemix.instance.FileExists(path))
                    throw new FileNotFoundException($"Could not find Post json file {path}.");

                Stream stream = CalRemix.instance.GetFileStream(path);

                newPost = JsonSerializer.Deserialize<Post>(stream);

                stream.Close();

                posts.Add(newPost);
            }
        }

        //Puts posts with higher priority higher on the timeline
        posts.Sort();

        // Only include a maximum of 100 posts on the timeline (subject to change)
        if(posts.Count > 100)
            posts.RemoveRange(100, posts.Count);

        timeline.Clear();

        foreach (var post in posts)
            timeline.Add(ArsenalUtils.SetupPostUIElement(post));
    }

    
}

public class ArsenalProfileUI(string name) : ArsenalUIState
{
    internal readonly List<Post> posts = [];

    internal string ProfileName = name;

    internal UIScrollbar scrollBar;

    internal UIList timeline;

    internal UIList replies;

    internal UIList likes;

    internal UIPanel profileHeader;

    public override void OnInitialize()
    {
        base.OnInitialize();

        scrollBar = new UIScrollbar
        {
            HAlign = 1f,
            VAlign = 0.5f
        };
        scrollBar.Height.Pixels = 300;

        timeline = [];
        timeline.SetScrollbar(scrollBar);
        timeline.HAlign = 0.5f;
        timeline.VAlign = 1f;
        timeline.Width.Pixels = 500;
        timeline.Height.Pixels = 600;

        background.Append(timeline);
        background.Append(scrollBar);
    }

    public override void OnActivate()
    {
        scrollBar.ViewPosition = 0f;

        #region Profile Header Setup
        profileHeader = new()
        {
            BackgroundColor = Color.PaleTurquoise,
            BorderColor = Color.Black,
            HAlign = 0.5f
        };
        profileHeader.Width.Pixels = 500f;
        profileHeader.Height.Pixels = 500f;

        UIPanel banner = new()
        {
            BackgroundColor = Color.Cyan,
            BorderColor = Color.Transparent,
        };
        banner.Width.Pixels = 500f;
        banner.Height.Pixels = 150f;

        profileHeader.Append(banner);
        string name = ProfileName;
        UIImage pfp = new(ArsenalSystem.ProfileData[name].PFP ?? ArsenalSystem.ProfileData["Default"].PFP)
        {
            IgnoresMouseInteraction = true,
        };
        pfp.Top.Pixels = banner.Top.Pixels + banner.Height.Pixels - pfp.Height.Pixels / 2;
        if (pfp.Width.Pixels > 88)
            pfp.Left.Pixels -= (pfp.Width.Pixels - 88) / 2;
        pfp.Left.Pixels += 12;
        if (pfp.Height.Pixels > 88)
            pfp.Top.Pixels -= (pfp.Height.Pixels - 88) / 2;
        pfp.ImageScale = 88f / pfp.Width.Pixels;
        pfp.Width.Pixels = 88f;
        pfp.Height.Pixels = 88f;
        profileHeader.Append(pfp);

        UIText displayName = new(ArsenalSystem.ProfileData[name].Profile.DisplayName.Formatted(name, ArsenalUtils.MemberType.DisplayName))
        {
            ShadowColor = Color.Black,
            TextColor = Color.White,
            IsWrapped = false,
        };

        float yPos = pfp.Top.Pixels + pfp.Height.Pixels;

        displayName.Top.Pixels = yPos;
        profileHeader.Append(displayName);

        UIText accountName = new("@" + ArsenalSystem.ProfileData[name].Profile.AccountName.Formatted(name, ArsenalUtils.MemberType.AccountName))
        {
            ShadowColor = Color.Black,
            TextColor = Color.DarkGray,
            IsWrapped = false,
        };

        yPos += FontAssets.MouseText.Value.MeasureString(displayName.Text).Y;
        accountName.Top.Pixels = yPos;
        profileHeader.Append(accountName);

        UIText bio = new(ArsenalSystem.ProfileData[name].Profile.Description.Formatted(name, ArsenalUtils.MemberType.Bio))
        {
            ShadowColor = Color.Black,
            TextColor = Color.White,
            IsWrapped = true,
        };
        bio.Width.Pixels = 500f;
        bio.Height.Pixels = 50f;

        yPos += FontAssets.MouseText.Value.MeasureString(displayName.Text).Y * 1.5f;
        bio.Top.Pixels = yPos;
        profileHeader.Append(bio);

        UIText location = new("Location: " + ArsenalSystem.ProfileData[name].Profile.Location.Formatted(name, ArsenalUtils.MemberType.Location))
        {
            ShadowColor = Color.Black,
            TextColor = Color.DarkGray,
            IsWrapped = false,
        };

        yPos += FontAssets.MouseText.Value.MeasureString(displayName.Text).Y * 1.5f;
        location.Top.Pixels = yPos;
        profileHeader.Append(location);

        UIText joinDate = new("Joined: " + ArsenalSystem.ProfileData[name].Profile.JoinDate.Formatted(name, ArsenalUtils.MemberType.JoinDate))
        {
            ShadowColor = Color.Black,
            TextColor = Color.DarkGray,
            IsWrapped = false,
        };

        yPos += FontAssets.MouseText.Value.MeasureString(displayName.Text).Y;
        joinDate.Top.Pixels = yPos;
        profileHeader.Append(joinDate);

        UIText followingCount = new(ArsenalSystem.ProfileData[name].Profile.Following.Length + " Following")
        {
            ShadowColor = Color.Black,
            TextColor = Color.DarkGray,
            IsWrapped = false,
        };

        followingCount.Top.Pixels = profileHeader.Height.Pixels - 48;
        profileHeader.Append(followingCount);

        UIText followerCount = new(ArsenalSystem.ProfileData[name].Profile.Followers + " Followers")
        {
            ShadowColor = Color.Black,
            TextColor = Color.DarkGray,
            IsWrapped = false,
        };

        followerCount.Top.Pixels = profileHeader.Height.Pixels - 48;
        followerCount.Left.Pixels += FontAssets.MouseText.Value.MeasureString(displayName.Text).X + 16;
        profileHeader.Append(followerCount);
        #endregion

        posts.Clear();

        foreach (var v in ArsenalSystem.Posts.Where(p => p.Key.StartsWith(ProfileName + "/")))
        {
            Post newPost;

            if (v.Value.Requirement.Invoke())
            {
                string activeExtension = LanguageManager.Instance.ActiveCulture.Name;
                string path = "UI/Arsenal/" + activeExtension + "/Posts/" + v.Key + ".json";

                // Fall back to english if not found
                if (!CalRemix.instance.FileExists(path))
                    path = "UI/Arsenal/en-US/Posts/" + v.Key + ".json";

                // Throw if we cant find english either
                if (!CalRemix.instance.FileExists(path))
                    throw new FileNotFoundException($"Could not find Post json file {path}.");

                Stream stream = CalRemix.instance.GetFileStream(path);

                newPost = JsonSerializer.Deserialize<Post>(stream);

                stream.Close();

                posts.Add(newPost);
            }
        }

        //Puts posts with higher priority higher on the timeline
        posts.Sort();

        // Only include a maximum of 100 posts on the timeline (subject to change)
        if (posts.Count > 100)
            posts.RemoveRange(100, posts.Count);

        timeline.Clear();

        timeline.Add(profileHeader);

        foreach (var post in posts)
            timeline.Add(ArsenalUtils.SetupPostUIElement(post));
    }
}

public class ArsenalPostUI : ArsenalUIState
{

}

public static class ArsenalUtils
{
    internal static UIPanel SetupPostUIElement(Post post)
    {
        UIPanel postArea = new()
        {
            BorderColor = Color.Transparent,
            BackgroundColor = Color.Transparent
        };
        postArea.Width.Pixels = 470f;
        postArea.Height.Pixels = 60f;

        UIPanel postTextbox = new()
        {
            BackgroundColor = Color.CadetBlue,
            BorderColor = Color.Black,
            HAlign = 1f,
            VAlign = 0f
        };
        postTextbox.Width.Pixels = 400f;
        postTextbox.Top.Pixels += 36;

        UIText bodyText = new(post.Body.Formatted(post.Poster, MemberType.PostBody))
        {
            ShadowColor = Color.Black,
            TextColor = Color.White,
            IsWrapped = true,
        };
        Vector2 textSize = ChatManager.GetStringSize(FontAssets.MouseText.Value, bodyText.Text, Vector2.One, 400f);
        bodyText.Width.Pixels = 400f;
        bodyText.Height.Pixels = textSize.Y;
        postTextbox.Height.Pixels = textSize.Y + 16;
        postArea.Height.Pixels += textSize.Y + 16;
        postTextbox.Append(bodyText);

        postArea.Append(postTextbox);

        if(post.Image != null)
        {
            UIImage image = new(ModContent.Request<Texture2D>($"CalRemix/Assets/Textures/Arsenal/Images/{post.Image}"))
            {
                HAlign = 0.5f,
                VAlign = 1f,
                AllowResizingDimensions = true
            };
            float scale = image.Width.Pixels <= 350f ? 1f: 350f / image.Width.Pixels;
            image.Width.Pixels = 350f;
            image.Height.Pixels *= scale;
            image.ImageScale = scale;
            image.Left.Pixels -= image.Width.Pixels * scale;
            image.Top.Pixels -= image.Height.Pixels * scale;


            postTextbox.Height.Pixels += image.Height.Pixels + 16;
            postArea.Height.Pixels += image.Height.Pixels + 16;

            postTextbox.Append(image);
        }

        UIImage pfp = new(ArsenalSystem.ProfileData[post.Poster].PFP ?? ArsenalSystem.ProfileData["Default"].PFP)
        {
            IgnoresMouseInteraction = true
        };
        if (pfp.Width.Pixels > 44)
            pfp.Left.Pixels -= (pfp.Width.Pixels - 44) / 2;
        if (pfp.Height.Pixels > 44)
            pfp.Top.Pixels -= (pfp.Height.Pixels - 44) / 2;
        pfp.ImageScale = 44f / pfp.Width.Pixels;
        pfp.Width.Pixels = 44f;
        pfp.Height.Pixels = 44f;

        UIButton<string> uIButton = new(post.Poster)
        {
            BackgroundColor = Color.Transparent,
            BorderColor = Color.Transparent,
            HoverPanelColor = Color.Transparent,
            HoverBorderColor = Color.Transparent,
            OverflowHidden = true
        };
        uIButton.OnLeftClick += OpenProfile;
        uIButton.Width.Pixels = uIButton.Height.Pixels = 44;
        postArea.Append(uIButton);
        postArea.Append(pfp);

        UIText displayName = new(ArsenalSystem.ProfileData[post.Poster].Profile.DisplayName.Formatted(post.Poster, MemberType.DisplayName))
        {
            ShadowColor = Color.Black,
            TextColor = Color.White,
            IsWrapped = false
        };

        displayName.Left.Pixels = pfp.Width.Pixels + 8;
        displayName.Top.Pixels = pfp.Height.Pixels / 4;
        postArea.Append(displayName);

        UIText accountName = new("@" + ArsenalSystem.ProfileData[post.Poster].Profile.AccountName.Formatted(post.Poster, MemberType.AccountName))
        {
            ShadowColor = Color.Black,
            TextColor = Color.DarkGray,
            IsWrapped = false
        };

        accountName.Left.Pixels = pfp.Width.Pixels + ChatManager.GetStringSize(FontAssets.MouseText.Value, displayName.Text, Vector2.One).X + 16;
        accountName.Top.Pixels = pfp.Height.Pixels / 4;
        postArea.Append(accountName);

        return postArea;
    }

    internal static void OpenProfile(UIMouseEvent evt, UIElement listeningElement)
    {
        ArsenalSystem system = ModContent.GetInstance<ArsenalSystem>();
        system.profileUI.ProfileName = ((UIButton<string>)listeningElement).Text;
        system.userInterface?.SetState(system.profileUI);
    }

    internal static void OpenTimeline(UIMouseEvent evt, UIElement listeningElement)
    {
        ArsenalSystem system = ModContent.GetInstance<ArsenalSystem>();

        if (system.userInterface.CurrentState is not ArsenalTimelineUI)
            system.userInterface?.SetState(system.timelineUI);
    }

    internal static void CloseArsenal(UIMouseEvent evt, UIElement listeningElement)
    {
        ArsenalSystem system = ModContent.GetInstance<ArsenalSystem>();
        system.uiOpen = false;
        system.userInterface?.SetState(null);
    }

    internal enum MemberType
    {
        Location,
        DisplayName,
        AccountName,
        Bio,
        PostBody,
        JoinDate
    }

    internal static string Formatted(this string str, string name, MemberType type)
    {
        switch(name)
        {
            case "Fanny":
                if(type == MemberType.Location)
                    return str.FormatWith(Main.LocalPlayer.name);
                return str;
            case "PlayerHater":
                return str.FormatWith(type == MemberType.AccountName ? Main.LocalPlayer.name.Replace(' ', '-') : Main.LocalPlayer.name);
            default: 
                return str;
        }
    }
}

public class ArsenalSystem : ModSystem
{
    internal static readonly Dictionary<string, (Func<bool> Requirement, bool Notification)> Posts = [];
    internal static readonly Dictionary<string, (Profile Profile, Asset<Texture2D> PFP)> ProfileData = [];

    internal enum CommentPriorityTier
    {
        ExoBot = -1,
        None,
        Low,
        Medium,
        High
    }

    // Should be used as a general reference for where to set a post's priority to ensure posts related to current events are put higher
    internal enum PostPriorityTier
    {
        None = 0, // Post-One Mech is when Arsenal is unlocked
        PostThreeMechs,
        PostPlantera,
        PostGolem,
        PostCultist,
        PostML,
        PostProvidence,
        PostPolterghast,
        PostDog,
        PostYharon,
        PostCalamitas,
        PostDraedon,
        //And beyond...?
    }

    internal ArsenalTimelineUI timelineUI;
    internal ArsenalProfileUI profileUI;
    internal UserInterface userInterface;
    public bool uiOpen = false;

    public override void OnModLoad()
    {
        string activeExtension = LanguageManager.Instance.ActiveCulture.Name;

        ProfileData.Add("Default", (null, ModContent.Request<Texture2D>("CalRemix/Assets/Textures/Arsenal/ProfilePictures/Default", AssetRequestMode.AsyncLoad)));

        foreach (string path in CalRemix.instance.GetFileNames().Where(s => s.EndsWith(".json") && s.StartsWith($"UI/Arsenal/{activeExtension}/Profiles")))
        {
            Stream stream = CalRemix.instance.GetFileStream(path);

            Profile p = JsonSerializer.Deserialize<Profile>(stream);

            bool hasPfP = ModContent.RequestIfExists($"CalRemix/Assets/Textures/Arsenal/ProfilePictures/{p.Name}", out Asset<Texture2D> pfp, AssetRequestMode.AsyncLoad);
            ProfileData.Add(p.Name, (p, hasPfP ? pfp : null));

            stream.Close();
        }
        //Add all posts to Posts
        Posts.Add("Fanny/GoodMorning", (() => true, false));
        Posts.Add("XboxGamer/StupidHelper", (() => true, false));
        Posts.Add("Renault/Advert1", (() => true, false));
        Posts.Add("PlayerHater/BadMorning", (() => true, false));
        Posts.Add("MiracleBoy/Dog", (() => true, false));
        Posts.Add("Fanny/BabilHunting", (() => true, false));
        Posts.Add("MiracleBoy/Cartwheel", (() => true, false));

        if (!Main.dedServ)
        {
            userInterface = new UserInterface();
            timelineUI = new ArsenalTimelineUI();
            profileUI = new ArsenalProfileUI("Fanny");
            timelineUI.Activate();
            profileUI.Activate();
        }
    }

    public void OpenArsenalUI()
    {
        uiOpen = true;
        userInterface?.SetState(timelineUI);
    }

    public void CloseArsenalUI()
    {
        uiOpen = false;
        userInterface?.SetState(null);
    }
    
    public override void UpdateUI(GameTime gameTime)
    {
        if (userInterface?.CurrentState != null)
            userInterface?.Update(gameTime);
    }
    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
        if (mouseTextIndex != -1)
        {
            layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                "CalRemix: Displays the Arsenal Social Media UI",
                delegate
                {
                    userInterface.Draw(Main.spriteBatch, new GameTime());
                    return true;
                },
                InterfaceScaleType.UI)
            );
        }
    }
}

public class Profile
{
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public string AccountName { get; set; }
    public string Description { get; set; } = "";
    public string Location { get; set; } = "";
    public string Birthday { get; set; } = "";
    public string JoinDate { get; set; } = "";
    public int Followers { get; set; } = 0;
    public string[] Following { get; set; } = [];
    public bool Prism { get; set; } = false;
}
public class Post: IComparable
{
    public string Name { get; set; }
    public string Poster { get; set; }
    public string Body { get; set; }
    public string[] Tags { get; set; } = [];
    public float Priority { get; set; } = 0;
    public string Image { get; set; } = null;
    public string Quote { get; set; } = null;
    public string TopReply { get; set; } = null;
    public string Attachment { get; set; } = null;

    public int CompareTo(object obj)
    {
        if (obj == null)
        {
            return 1;
        }

        // NOTE: Cannot use return (_value - value) as this causes a wrap
        // around in cases where _value - value > MaxValue.
        if (obj is Post p)
        {
            if (Priority > p.Priority) return -1;
            if (Priority < p.Priority) return 1;
            return 0;
        }

        return 0;
    }
}

public class Reply
{
    public string Name { get; set; }
    public string Poster { get; set; }
    public string Body { get; set; }
    public string[] Tags { get; set; } = [];
    public float Priority { get; set; } = 0;
    public string Image { get; set; } = null;
    public string Attachment { get; set; } = null;
}

public class Attachment
{
    public string Name { get; set; }
    public string Link { get; set; }
    public string Title { get; set; } = null;
    public string Image { get => $"Assets/Textures/Arsenal/Thumbnails/{Name}"; }
}
