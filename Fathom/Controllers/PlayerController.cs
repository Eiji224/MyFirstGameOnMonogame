using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Fathom.Controllers;

public class PlayerController(Player player)
{
    private readonly Player _player = player;

    public void Update()
    {
        HandleInput();
        _player.MoveByGravitation(); // Прописать взаимодействие с платформой Velocity = 0
    }

    private void HandleInput()
    {
        var keyboard = Keyboard.GetState();

        if (keyboard.IsKeyDown(Keys.W))
        {
            _player.Move(new Vector2(0, -2.5f));
        }
        if (keyboard.IsKeyDown(Keys.A))
        {
            _player.Move(new Vector2(-2.5f, 0));
        }
        if (keyboard.IsKeyDown(Keys.S))
        {
            _player.Move(new Vector2(0, 2.5f));
        }
        if (keyboard.IsKeyDown(Keys.D))
        {
            _player.Move(new Vector2(2.5f, 0));
        }
    }
}