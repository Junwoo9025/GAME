using System.Collections.Generic;
using Unit06.Game.Casting;
using Unit06.Game.Services;


namespace Unit06.Game.Scripting
{
    public class CollideBrickAction : Action
    {
        private AudioService _audioService;
        private PhysicsService _physicsService;
        
        public CollideBrickAction(PhysicsService physicsService, AudioService audioService)
        {
            this._physicsService = physicsService;
            this._audioService = audioService;
        }

        public void Execute(Cast cast, Script script, ActionCallback callback)
        {
            Racket ball = (Racket)cast.GetFirstActor(Constants.RACKET_GROUP);
            List<Actor> bricks = cast.GetActors(Constants.BRICK_GROUP);
            Stats stats = (Stats)cast.GetFirstActor(Constants.STATS_GROUP);
            Sound overSound = new Sound(Constants.OVER_SOUND);
            
            foreach (Actor actor in bricks)
            {
                Brick brick = (Brick)actor;
                
                Body brickBody = brick.GetBody();
                Body ballBody = ball.GetBody();

                if (_physicsService.HasCollided(brickBody, ballBody))
                {
                    cast.RemoveActor(Constants.RACKET_GROUP, ball);
                    cast.RemoveActor(Constants.BRICK_GROUP, brick);
                    //cast.AddActor(Constants.BRICK_GROUP, brick);
                
                //     Sound sound = new Sound(Constants.BOUNCE_SOUND);
                //     _audioService.PlaySound(sound);
                //     int points = brick.GetPoints();
                //     stats.AddPoints(points);
                //     cast.RemoveActor(Constants.BRICK_GROUP, brick);
                // }
                //Stats stats = (Stats)cast.GetFirstActor(Constants.STATS_GROUP);
                stats.RemoveLife();

                if (stats.GetLives() > 0)
                {
                    callback.OnNext(Constants.TRY_AGAIN);
                }
                else
                {
                    callback.OnNext(Constants.GAME_OVER);
                    _audioService.PlaySound(overSound);
                }
                }
            }
        }
    }
}