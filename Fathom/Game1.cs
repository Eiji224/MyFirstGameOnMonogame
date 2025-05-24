using Fathom.Controllers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Fathom;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    
    private LevelModel _level;
    
    private LevelView _levelView;
    private PlayerView _playerView;

    private PlayerController _playerController;
    private Camera _camera;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _level = new LevelModel();
        _level.Initialize();
        _levelView = new LevelView(_level);
        _playerView = new PlayerView(_level);

        _playerController = new PlayerController(_level.Player, _level);
        
        _camera = new Camera(
            new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight),
            new Vector2(1920f, 1080f) // размеры уровня (ширина, высота)
        );

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _levelView.LoadContent(Content);
        _playerView.LoadContent(Content);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        
        _playerController.Update(gameTime);
        _playerView.Update(gameTime);

        _camera.Follow(_level.Player);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin(transformMatrix: _camera.TransformMatrix);
        
        _levelView.Draw(_spriteBatch, _camera);
        _playerView.Draw(_spriteBatch, _camera);
        
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}