using Unit06.Game.Casting;
using System.Collections.Generic;
using System;
namespace Unit06.Game.Scripting

{
    public class MoveBrickAction : Action
    {
        public MoveBrickAction()
        {
        }

        public void Execute(Cast cast, Script script, ActionCallback callback)
        {
            //Brick ball = (Brick)cast.GetFirstActor(Constants.BRICK_GROUP);
            List<Actor> balls = cast.GetActors(Constants.BRICK_GROUP);
            int i = 0;
            foreach (Actor balln in balls){
                Random random = new Random();
                
                List<int> launchableXcods = new List<int>(){470, 670, 870, 1150, 470};
                int random_index = random.Next(0,4);
                Brick ball = (Brick)balln;
                Body body = ball.GetBody();
                Point position = body.GetPosition();
                Point velocity = body.GetVelocity();
                if (position.GetY()< Constants.SCREEN_HEIGHT){
                    position = position.Add(velocity);
                }
                else{
                    position = new Point((launchableXcods[i]), (position.GetY()-random.Next(1000, 2500) ));
                }
                body.SetPosition(position);
                i++;
            }
        }
    }
}