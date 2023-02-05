using Unit06.Game.Casting;
using Unit06.Game.Services;


namespace Unit06.Game.Scripting
{
    public class ControlBackGroundAction : Action
    {
        private KeyboardService _keyboardService;

        public ControlBackGroundAction(KeyboardService keyboardService)
        {
            this._keyboardService = keyboardService;
        }

        public void Execute(Cast cast, Script script, ActionCallback callback)
        {
            Background racket = (Background)cast.GetFirstActor(Constants.BACKGROUND_GROUP);
            racket.MoveNext();
            racket.SwingRight();
        }

        //     if (_keyboardService.IsKeyDown(Constants.LEFT))
        //     {
        //         racket.SwingLeft();
        //     }
        //     else if (_keyboardService.IsKeyDown(Constants.RIGHT))
        //     {
        //         racket.SwingRight();
        //     }
        //     else
        //     {
        //         racket.StopMoving();
        //     }
        // }
    }
}