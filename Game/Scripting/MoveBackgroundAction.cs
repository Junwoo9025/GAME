
//This may not be needed. But in order to move the sides I might need to run it through this class. This resembles the MoveBallClass
//using Unit06.Game.Casting;
using Unit06.Game.Casting;
// using Unit06.Game.Services;
namespace Unit06.Game.Scripting
{
 public class MoveSidesAction : Action
    {
        public void MoveSideAction()
        {
        }

        public void Execute(Cast cast, Script script, ActionCallback callback)
        {
            foreach (Background side in cast.GetActors(Constants.BACKGROUND_GROUP))
            {
                Body body = side.GetBody();
                Point position = body.GetPosition();
                Point velocity = body.GetVelocity();
                if (position.GetY() >= Constants.SCREEN_HEIGHT-5)
                {
              position = new Point(0, -Constants.SCREEN_HEIGHT);
                }
                else
                {
                    position = position.Add(velocity);
                }
                body.SetPosition(position);
            }

        }
    }
}

