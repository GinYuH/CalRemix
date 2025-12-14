using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace CalRemix.UI.VideoPlayer;

public class ExampleVideoPlayerUI : UIState
{
    internal DraggableUIPanel _mainPanel;
    internal VideoPlayerUIElement _videoPlayer;
    private UIPanel _controlPanel;
    private UIPanel _urlPanel;
    private UIPanel _timelinePanel;

    // URL input
    private UIText _urlLabel;
    internal CustomTextInput _urlInput;
    private UITextPanel<string> _loadButton;

    // Control buttons
    private UITextPanel<string> _playButton;
    private UITextPanel<string> _pauseButton;
    private UITextPanel<string> _stopButton;
    private UITextPanel<string> _closeButton;

    // Timeline elements
    private TimelineBar _timelineBar;
    private TimelineProgress _timelineProgress;
    private DraggableTimelineScrubber _timelineScrubber;
    private UIText _currentTimeText;
    private UIText _totalTimeText;

    // Resize handle
    private ResizeHandle _resizeHandle;

    // Dimensions
    internal float _currentWidth = 800f;
    internal float _currentHeight = 620f;
    private const float MIN_WIDTH = 600f;
    private const float MIN_HEIGHT = 400f;
    private const float MAX_WIDTH = 1600f;
    private const float MAX_HEIGHT = 1200f;

    private bool _isInitialized = false;

    public override void OnInitialize()
    {
        if (_isInitialized) return;

        // Create draggable main container
        _mainPanel = new DraggableUIPanel();
        _mainPanel.Width.Set(_currentWidth, 0f);
        _mainPanel.Height.Set(_currentHeight, 0f);
        _mainPanel.HAlign = 0.5f;
        _mainPanel.VAlign = 0.5f;
        _mainPanel.BackgroundColor = new Color(33, 43, 79) * 0.9f;
        Append(_mainPanel);

        SetupURLPanel();
        SetupVideoPlayer();
        SetupTimelinePanel();
        SetupControlPanel();
        SetupResizeHandle();

        _isInitialized = true;
    }

    private void SetupURLPanel()
    {
        _urlPanel = new UIPanel();
        _urlPanel.Width.Set(-20, 1f);
        _urlPanel.Height.Set(60, 0f);
        _urlPanel.HAlign = 0.5f;
        _urlPanel.Top.Set(10, 0f);
        _urlPanel.BackgroundColor = new Color(25, 33, 63);
        _mainPanel.Append(_urlPanel);

        _urlLabel = new UIText("URL:", 0.9f);
        _urlLabel.Left.Set(10, 0f);
        _urlLabel.Top.Set(15, 0f);
        _urlPanel.Append(_urlLabel);

        _urlInput = new CustomTextInput(500);
        _urlInput.Width.Set(-130, 1f);
        _urlInput.Height.Set(30, 0f);
        _urlInput.Left.Set(50, 0f);
        _urlInput.Top.Set(10, 0f);
        _urlPanel.Append(_urlInput);

        _loadButton = new UITextPanel<string>("Load");
        _loadButton.Width.Set(60, 0f);
        _loadButton.Height.Set(30, 0f);
        _loadButton.Left.Set(-70, 1f);
        _loadButton.Top.Set(5, 0f);
        _loadButton.OnLeftClick += OnLoadClicked;
        _urlPanel.Append(_loadButton);
    }

    private void SetupVideoPlayer()
    {
        float videoWidth = _currentWidth - 100;
        float videoHeight = _currentHeight - 240;

        _videoPlayer = new VideoPlayerUIElement((int)videoWidth, (int)videoHeight, 1280, 720);
        _videoPlayer.HAlign = 0.5f;
        _videoPlayer.Top.Set(80, 0f);
        _mainPanel.Append(_videoPlayer);
    }

    private void SetupTimelinePanel()
    {
        _timelinePanel = new UIPanel();
        _timelinePanel.Width.Set(-20, 1f);
        _timelinePanel.Height.Set(50, 0f);
        _timelinePanel.HAlign = 0.5f;
        _timelinePanel.Top.Set(-130, 1f);
        _timelinePanel.BackgroundColor = new Color(25, 33, 63);
        _mainPanel.Append(_timelinePanel);

        // Current time
        _currentTimeText = new UIText("0:00", 0.8f);
        _currentTimeText.Left.Set(10, 0f);
        _currentTimeText.VAlign = 0.5f;
        _timelinePanel.Append(_currentTimeText);

        // Timeline bar
        _timelineBar = new TimelineBar();
        _timelineBar.Width.Set(-120, 1f);
        _timelineBar.Height.Set(10, 0f);
        _timelineBar.Left.Set(60, 0f);
        _timelineBar.VAlign = 0.5f;
        _timelineBar.OnLeftClick += OnTimelineClicked;
        _timelinePanel.Append(_timelineBar);

        // Progress bar - using custom element
        _timelineProgress = new TimelineProgress();
        _timelineProgress.Width.Set(0, 0f);
        _timelineProgress.Height.Set(0, 1f);
        _timelineBar.Append(_timelineProgress);

        // Scrubber
        _timelineScrubber = new DraggableTimelineScrubber(_videoPlayer);
        _timelineScrubber.Width.Set(12, 0f);
        _timelineScrubber.Height.Set(20, 0f);
        _timelineScrubber.Left.Set(-6, 0f);
        _timelineScrubber.VAlign = 0.5f;
        _timelineBar.Append(_timelineScrubber);

        // Total time
        _totalTimeText = new UIText("0:00", 0.8f);
        _totalTimeText.Left.Set(-50, 1f);
        _totalTimeText.VAlign = 0.5f;
        _timelinePanel.Append(_totalTimeText);
    }

    private void OnTimelineClicked(UIMouseEvent evt, UIElement listeningElement)
    {
        // Calculate click position on timeline
        CalculatedStyle dims = _timelineBar.GetDimensions();
        float relativeX = evt.MousePosition.X - dims.X;
        float percentage = Math.Clamp(relativeX / dims.Width, 0f, 1f);

        _videoPlayer.Seek(percentage);
    }

    private void SetupControlPanel()
    {
        _controlPanel = new UIPanel();
        _controlPanel.Width.Set(-20, 1f);
        _controlPanel.Height.Set(60, 0f);
        _controlPanel.HAlign = 0.5f;
        _controlPanel.Top.Set(-70, 1f);
        _controlPanel.BackgroundColor = new Color(25, 33, 63);
        _mainPanel.Append(_controlPanel);

        float buttonSpacing = 10f;
        float buttonWidth = 100f;
        float panelWidth = _currentWidth - 40;
        float startX = (panelWidth - (buttonWidth * 4 + buttonSpacing * 3)) / 2;

        _playButton = new UITextPanel<string>("Play");
        _playButton.Width.Set(buttonWidth, 0f);
        _playButton.Height.Set(40, 0f);
        _playButton.Left.Set(startX, 0f);
        _playButton.VAlign = 0.5f;
        _playButton.OnLeftClick += OnPlayClicked;
        _controlPanel.Append(_playButton);

        _pauseButton = new UITextPanel<string>("Pause");
        _pauseButton.Width.Set(buttonWidth, 0f);
        _pauseButton.Height.Set(40, 0f);
        _pauseButton.Left.Set(startX + buttonWidth + buttonSpacing, 0f);
        _pauseButton.VAlign = 0.5f;
        _pauseButton.OnLeftClick += OnPauseClicked;
        _controlPanel.Append(_pauseButton);

        _stopButton = new UITextPanel<string>("Stop");
        _stopButton.Width.Set(buttonWidth, 0f);
        _stopButton.Height.Set(40, 0f);
        _stopButton.Left.Set(startX + (buttonWidth + buttonSpacing) * 2, 0f);
        _stopButton.VAlign = 0.5f;
        _stopButton.OnLeftClick += OnStopClicked;
        _controlPanel.Append(_stopButton);

        _closeButton = new UITextPanel<string>("Close");
        _closeButton.Width.Set(buttonWidth, 0f);
        _closeButton.Height.Set(40, 0f);
        _closeButton.Left.Set(startX + (buttonWidth + buttonSpacing) * 3, 0f);
        _closeButton.VAlign = 0.5f;
        _closeButton.OnLeftClick += OnCloseClicked;
        _closeButton.BackgroundColor = new Color(100, 50, 50);
        _controlPanel.Append(_closeButton);
    }

    private void SetupResizeHandle()
    {
        _resizeHandle = new ResizeHandle(this);
        _resizeHandle.Width.Set(20, 0f);
        _resizeHandle.Height.Set(20, 0f);
        _resizeHandle.Left.Set(-10, 1f);
        _resizeHandle.Top.Set(-10, 1f);
        _mainPanel.Append(_resizeHandle);
    }

    private void OnLoadClicked(UIMouseEvent evt, UIElement listeningElement)
    {
        string url = _urlInput.Text;
        if (string.IsNullOrWhiteSpace(url))
        {
            Main.NewText("Please enter a URL!", Color.Orange);
            return;
        }

        if (url.Contains("youtube.com") || url.Contains("youtu.be"))
        {
            _videoPlayer.PlayUrl(url);
        }
        else
        {
            _videoPlayer.PlayVideo(url);
        }
    }

    private void OnPlayClicked(UIMouseEvent evt, UIElement listeningElement)
    {
        if (_videoPlayer.IsPaused)
        {
            _videoPlayer.Resume();
        }
        else if (!_videoPlayer.IsPlaying)
        {
            string url = _urlInput.Text;
            if (!string.IsNullOrWhiteSpace(url))
            {
                if (url.Contains("youtube.com") || url.Contains("youtu.be"))
                {
                    _videoPlayer.PlayUrl(url);
                }
                else
                {
                    _videoPlayer.PlayVideo(url);
                }
            }
            else
            {
                Main.NewText("Please enter a URL first!", Color.Orange);
            }
        }
    }

    private void OnPauseClicked(UIMouseEvent evt, UIElement listeningElement)
    {
        _videoPlayer.Pause();
    }

    private void OnStopClicked(UIMouseEvent evt, UIElement listeningElement)
    {
        _videoPlayer.Stop();
    }

    private void OnCloseClicked(UIMouseEvent evt, UIElement listeningElement)
    {
        ModContent.GetInstance<ExampleVideoUISystem>().HideUI();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (IsMouseHovering)
            Main.LocalPlayer.mouseInterface = true;

        // Show/hide timeline based on video state
        bool hasVideo = _videoPlayer != null && (_videoPlayer.IsPlaying || _videoPlayer.IsPaused);
        if (hasVideo && _timelinePanel.Parent == null)
        {
            _mainPanel.Append(_timelinePanel);
            _mainPanel.Recalculate();
        }
        else if (!hasVideo && _timelinePanel.Parent != null)
        {
            _mainPanel.RemoveChild(_timelinePanel);
            _mainPanel.Recalculate();
        }

        // Deactivate text input when clicking outside
        bool urlDeactivated = false;
        if (_urlInput != null && _urlInput._active)
        {
            if ((Main.mouseLeft && !_urlInput.IsMouseHovering) ||
                (Main.keyState.IsKeyDown(Keys.Escape) && !Main.oldKeyState.IsKeyDown(Keys.Escape)))
            {
                _urlInput.Deselect();
                urlDeactivated = true;
            }
        }

        // Update timeline
        if (_videoPlayer != null && (_videoPlayer.IsPlaying || _videoPlayer.IsPaused))
        {
            float position = _videoPlayer.GetPosition();
            long duration = _videoPlayer.GetDuration();

            // Update progress bar
            _timelineProgress.Width.Set(position, 1f);
            _timelineProgress.Recalculate();

            // Update scrubber position
            float timelineWidth = _timelineBar.GetDimensions().Width;
            _timelineScrubber.Left.Set(position * timelineWidth - 6, 0f);
            _timelineScrubber.Recalculate();

            // Update time text
            long currentMs = (long)(position * duration);
            _currentTimeText.SetText(FormatTime(currentMs));
            _totalTimeText.SetText(FormatTime(duration));
        }

        // ESC to close (only if text input is not active)
        if (Main.keyState.IsKeyDown(Keys.Escape) && !Main.oldKeyState.IsKeyDown(Keys.Escape))
        {
            if (_urlInput == null || !urlDeactivated)
            {
                ModContent.GetInstance<ExampleVideoUISystem>().HideUI();
            }
        }
    }

    private static string FormatTime(long milliseconds)
    {
        if (milliseconds <= 0) return "0:00";

        TimeSpan time = TimeSpan.FromMilliseconds(milliseconds);
        if (time.TotalHours >= 1)
        {
            return $"{(int)time.TotalHours}:{time.Minutes:D2}:{time.Seconds:D2}";
        }
        return $"{time.Minutes}:{time.Seconds:D2}";
    }

    public void ResizePlayer(float newWidth, float newHeight)
    {
        _currentWidth = Math.Clamp(newWidth, MIN_WIDTH, MAX_WIDTH);
        _currentHeight = Math.Clamp(newHeight, MIN_HEIGHT, MAX_HEIGHT);

        _mainPanel.Width.Set(_currentWidth, 0f);
        _mainPanel.Height.Set(_currentHeight, 0f);

        // Resize video player
        float videoWidth = _currentWidth - 100;
        float videoHeight = _currentHeight - 240;
        _videoPlayer.Width.Set(videoWidth, 0f);
        _videoPlayer.Height.Set(videoHeight, 0f);
        _videoPlayer.Recalculate();
    }

    public void OnClose()
    {
        if (_videoPlayer != null && !_videoPlayer._isDisposed)
        {
            _videoPlayer.Stop();
            _videoPlayer.Dispose();
            CalRemix.instance.Logger.Info("Player disposed in OnClose");
        }

        _urlInput?.Deselect();

        if (_urlInput != null)
            _urlInput.Text = "";
    }
}

public class DraggableUIPanel : UIPanel
{
    private Vector2 offset;
    private bool dragging;

    public override void LeftMouseDown(UIMouseEvent evt)
    {
        base.LeftMouseDown(evt);
        DragStart(evt);
    }

    public override void LeftMouseUp(UIMouseEvent evt)
    {
        base.LeftMouseUp(evt);
        DragEnd(evt);
    }

    private void DragStart(UIMouseEvent evt)
    {
        offset = new Vector2(evt.MousePosition.X - Left.Pixels, evt.MousePosition.Y - Top.Pixels);
        dragging = true;
    }

    private void DragEnd(UIMouseEvent evt)
    {
        Vector2 endMousePosition = evt.MousePosition;
        dragging = false;

        Left.Set(endMousePosition.X - offset.X, 0f);
        Top.Set(endMousePosition.Y - offset.Y, 0f);

        Recalculate();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (IsMouseHovering)
        {
            Main.LocalPlayer.mouseInterface = true;
        }

        if (dragging)
        {
            Left.Set(Main.mouseX - offset.X, 0f);
            Top.Set(Main.mouseY - offset.Y, 0f);
            Recalculate();
        }

        var parentSpace = Parent.GetDimensions().ToRectangle();
        if (!GetDimensions().ToRectangle().Intersects(parentSpace))
        {
            Left.Pixels = Utils.Clamp(Left.Pixels, 0, parentSpace.Right - Width.Pixels);
            Top.Pixels = Utils.Clamp(Top.Pixels, 0, parentSpace.Bottom - Height.Pixels);
            Recalculate();
        }
    }
}

public class ResizeHandle : UIPanel
{
    private ExampleVideoPlayerUI _parent;
    private bool _resizing;
    private Vector2 _startMousePos;
    private Vector2 _startSize;

    public ResizeHandle(ExampleVideoPlayerUI parent)
    {
        _parent = parent;
        BackgroundColor = Color.White * 0.5f;
    }

    public override void LeftMouseDown(UIMouseEvent evt)
    {
        base.LeftMouseDown(evt);
        _resizing = true;
        _startMousePos = new Vector2(Main.mouseX, Main.mouseY);
        _startSize = new Vector2(_parent._currentWidth, _parent._currentHeight);
    }

    public override void LeftMouseUp(UIMouseEvent evt)
    {
        base.LeftMouseUp(evt);
        _resizing = false;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (_resizing)
        {
            Vector2 delta = new Vector2(Main.mouseX, Main.mouseY) - _startMousePos;
            float newWidth = _startSize.X + delta.X;
            float newHeight = _startSize.Y + delta.Y;
            _parent.ResizePlayer(newWidth, newHeight);
        }

        // Change cursor when hovering
        if (IsMouseHovering)
        {
            Main.LocalPlayer.mouseInterface = true;
        }
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        base.DrawSelf(spriteBatch);

        // Draw resize icon (diagonal lines)
        CalculatedStyle dimensions = GetDimensions();
        Vector2 pos = dimensions.Position();

        for (int i = 0; i < 3; i++)
        {
            Vector2 start = pos + new Vector2(15 - i * 5, 5);
            Vector2 end = pos + new Vector2(5, 15 - i * 5);
            // Simple line representation
            spriteBatch.Draw(ExampleVideoUISystem.Background.Value,
                new Rectangle((int)start.X, (int)start.Y, (int)(end.X - start.X), 2),
                Color.Gray);
        }
    }
}

public class DraggableTimelineScrubber(VideoPlayerUIElement player) : UIElement
{
    private bool _dragging;

    public override void LeftMouseDown(UIMouseEvent evt)
    {
        base.LeftMouseDown(evt);
        _dragging = true;
    }

    public override void LeftMouseUp(UIMouseEvent evt)
    {
        base.LeftMouseUp(evt);
        _dragging = false;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (_dragging && Parent != null)
        {
            CalculatedStyle parentDims = Parent.GetDimensions();
            float relativeX = Main.mouseX - parentDims.X;
            float percentage = Math.Clamp(relativeX / parentDims.Width, 0f, 1f);

            player.Seek(percentage);
        }

        if (IsMouseHovering)
        {
            Main.LocalPlayer.mouseInterface = true;
        }
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        CalculatedStyle dimensions = GetDimensions();
        spriteBatch.Draw(ExampleVideoUISystem.Background.Value, dimensions.ToRectangle(), Color.White);
    }
}

public class TimelineBar : UIElement
{
    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        CalculatedStyle dimensions = GetDimensions();
        spriteBatch.Draw(ExampleVideoUISystem.Background.Value, dimensions.ToRectangle(), Color.Gray * 0.5f);
    }
}

public class TimelineProgress : UIElement
{
    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        CalculatedStyle dimensions = GetDimensions();
        if (dimensions.Width > 0 && dimensions.Height > 0)
        {
            spriteBatch.Draw(ExampleVideoUISystem.Background.Value, dimensions.ToRectangle(), Color.Blue);
        }
    }
}

/// <summary>
/// Custom text input element based on UITextPrompt pattern
/// </summary>
public class CustomTextInput : UIPanel
{
    public string Text = "";
    internal bool _active = false;
    private readonly int _maxLength = 100;
    private readonly string _placeholder = "Enter URL...";

    public CustomTextInput(int maxLength = 100, string placeholder = "Enter URL...")
    {
        _maxLength = maxLength;
        _placeholder = placeholder;
        BackgroundColor = new Color(33, 43, 79) * 0.8f;
    }

    public override void LeftMouseDown(UIMouseEvent evt)
    {
        base.LeftMouseDown(evt);
        _active = true;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (IsMouseHovering)
        {
            Main.LocalPlayer.mouseInterface = true;
        }
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        base.DrawSelf(spriteBatch);

        if (_active)
        {
            PlayerInput.WritingText = true;
            Main.instance.HandleIME();

            string newInput = Main.GetInputText(Text);
            if (newInput != Text && newInput.Length <= _maxLength)
            {
                Text = newInput;
            }
        }

        // Draw text
        CalculatedStyle dimensions = GetDimensions();
        Vector2 position = dimensions.Position() + new Vector2(8, 8);

        string displayText = string.IsNullOrEmpty(Text) && !_active ? _placeholder : Text;
        Color textColor = string.IsNullOrEmpty(Text) && !_active ? Color.Gray : Color.White;

        // Add cursor when active
        if (_active && (int)(Main.GlobalTimeWrappedHourly * 2) % 2 == 0)
        {
            displayText += "|";
        }

        Utils.DrawBorderString(spriteBatch, displayText, position, textColor, 0.9f);
    }

    public void Deselect()
    {
        _active = false;
    }
}

/// <summary>
/// System to manage the video player UI state.
/// </summary>
public class ExampleVideoUISystem : ModSystem
{
    internal static Asset<Texture2D> Background;

    private UserInterface _videoUserInterface;
    internal ExampleVideoPlayerUI _videoUI;

    public override void Load()
    {
        if (Main.dedServ) return;

        Background = ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/Pixel");

        // Initialize UI components
        _videoUI = new ExampleVideoPlayerUI();
        _videoUI.Activate();
        _videoUserInterface = new UserInterface();
    }

    public override void Unload()
    {
        HideUI();
        Background = null;
    }

    public void ShowUI()
    {
        // Clear cache when showing UI
        VideoUrlHelper.ClearCache();

        // Always create a fresh UI instance
        _videoUI = new ExampleVideoPlayerUI();
        _videoUI.Activate();

        _videoUserInterface?.SetState(_videoUI);
    }

    public void HideUI()
    {
        if (_videoUI != null)
        {
            _videoUI.OnClose();
        }

        _videoUserInterface?.SetState(null);
    }

    public void ToggleUI()
    {
        if (_videoUserInterface?.CurrentState != null)
        {
            HideUI();
        }
        else
        {
            ShowUI();
        }
    }

    public override void UpdateUI(GameTime gameTime)
    {
        _videoUserInterface?.Update(gameTime);
    }

    public override void ModifyInterfaceLayers(System.Collections.Generic.List<GameInterfaceLayer> layers)
    {
        int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
        if (mouseTextIndex != -1)
        {
            layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                "CalRemix: Video Player UI",
                delegate
                {
                    if (_videoUserInterface?.CurrentState != null)
                    {
                        _videoUserInterface.Draw(Main.spriteBatch, new GameTime());
                    }
                    return true;
                },
                InterfaceScaleType.UI)
            );
        }
    }
}

/// <summary>
/// Example item that opens the video player UI.
/// </summary>
public class VideoPlayerOpenerItem : ModItem
{
    public override string Texture => "CalamityMod/Projectiles/Boss/SignusScythe";

    public override void SetDefaults()
    {
        Item.width = 32;
        Item.height = 32;
        Item.useStyle = ItemUseStyleID.HoldUp;
        Item.useTime = 30;
        Item.useAnimation = 30;
        Item.rare = ItemRarityID.Blue;
    }

    public override bool? UseItem(Player player)
    {
        if (Main.myPlayer == player.whoAmI)
        {
            ModContent.GetInstance<ExampleVideoUISystem>().ShowUI();
        }
        return true;
    }
}