

using System.Collections.Generic;
using Unit06.Game.Casting;
using Unit06.Game.Services;
namespace Unit06.Game.Scripting
{
    public class DrawSidesAction : Action
    {
        private VideoService _videoService;
        
        public DrawSidesAction(VideoService videoService)
        {
            this._videoService = videoService;
        }
        public void Execute(Cast cast, Script script, ActionCallback callback)
        {       


    List<Actor> sides = cast.GetActors(Constants.BACKGROUND_GROUP);
            foreach (Actor actor in sides)
            {
                Background side = (Background)actor;
                Body body = side.GetBody();

                if (side.IsDebug())
                {
                    Rectangle rectangle = body.GetRectangle();
                    Point size = rectangle.GetSize();
                    Point pos = rectangle.GetPosition();
                    _videoService.DrawRectangle(size, pos, Constants.PURPLE, false);
                }
                //Animation animation = side.GetAnimation();
                Image image = side.GetImage();
                Point position = body.GetPosition();
                _videoService.DrawImage(image, position);
            }
        }
    }
}

