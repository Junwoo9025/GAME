using System;
using System.Collections.Generic;
using System.IO;
using Unit06.Game.Casting;
using Unit06.Game.Scripting;
using Unit06.Game.Services;


namespace Unit06.Game.Directing
{
    public class SceneManager
    {
        public static AudioService AudioService = new RaylibAudioService();
        public static KeyboardService KeyboardService = new RaylibKeyboardService();
        public static MouseService MouseService = new RaylibMouseService();
        public static PhysicsService PhysicsService = new RaylibPhysicsService();
        public static VideoService VideoService = new RaylibVideoService(Constants.GAME_NAME,
            Constants.SCREEN_WIDTH, Constants.SCREEN_HEIGHT, Constants.BLACK);

        public SceneManager()
        {
        }

        
        public void PrepareScene(string scene, Cast cast, Script script)
        {
            if (scene == Constants.NEW_GAME)
            {
                PrepareNewGame(cast, script);
            }
            else if (scene == Constants.NEXT_LEVEL)
            {
                PrepareNextLevel(cast, script);
            }
            else if (scene == Constants.TRY_AGAIN)
            {
                PrepareTryAgain(cast, script);
            }
            else if (scene == Constants.IN_PLAY)
            {
                PrepareInPlay(cast, script);
            }
            else if (scene == Constants.GAME_OVER)
            {
                PrepareGameOver(cast, script);
            }
        }

        private void PrepareNewGame(Cast cast, Script script)
        {   //AddBackground2(cast);
            AddBackground(cast);
            AddTime(cast);
            AddStats(cast);
            AddLevel(cast);
            AddScore(cast);
            AddLives(cast);
            AddBall(cast);
            AddBricks(cast);
            AddRacket(cast);
            AddDialog(cast, Constants.ENTER_TO_START);

            script.ClearAllActions();
            AddInitActions(script);
            AddLoadActions(script);

            ChangeSceneAction a = new ChangeSceneAction(KeyboardService, Constants.NEXT_LEVEL);
            script.AddAction(Constants.INPUT, a);
            
            AddOutputActions(script);
            AddUnloadActions(script);
            AddReleaseActions(script);
            PlaySoundAction sa = new PlaySoundAction(AudioService, Constants.START_GAME);
            script.AddAction(Constants.OUTPUT, sa);
        }

        private void ActivateBall(Cast cast)
        {
            Ball ball = (Ball)cast.GetFirstActor(Constants.BALL_GROUP);
            ball.Release();
        }

        private void PrepareNextLevel(Cast cast, Script script)
        {
            AddBall(cast);
            AddBackground(cast);
            //AddBackground2(cast);
            AddBricks(cast);
            AddRacket(cast);
            AddDialog(cast, Constants.PREP_TO_LAUNCH);

            script.ClearAllActions();

            TimedChangeSceneAction ta = new TimedChangeSceneAction(Constants.IN_PLAY, 2, DateTime.Now);
            script.AddAction(Constants.INPUT, ta);

            AddOutputActions(script);

            PlaySoundAction sa = new PlaySoundAction(AudioService, Constants.START_GAME);
            script.AddAction(Constants.OUTPUT, sa);
        }

        private void PrepareTryAgain(Cast cast, Script script)
        {
            AddBall(cast);
            AddBackground(cast);
            //AddBackground2(cast);
            AddRacket(cast);
            AddDialog(cast, Constants.PREP_TO_LAUNCH);

            script.ClearAllActions();
            
            TimedChangeSceneAction ta = new TimedChangeSceneAction(Constants.IN_PLAY, 2, DateTime.Now);
            script.AddAction(Constants.INPUT, ta);
            
            AddUpdateActions(script);
            AddOutputActions(script);
        }

        private void PrepareInPlay(Cast cast, Script script)
        {   
            PlaySoundAction sa = new PlaySoundAction(AudioService, Constants.START_GAME);
            ActivateBall(cast);
            script.AddAction(Constants.OUTPUT, sa);
            cast.ClearActors(Constants.DIALOG_GROUP);

            script.ClearAllActions();

            ControlRacketAction action = new ControlRacketAction(KeyboardService);
            script.AddAction(Constants.INPUT, action);

            AddUpdateActions(script);    
            AddOutputActions(script);
            
        
        }

        private void PrepareGameOver(Cast cast, Script script)
        {
            AddBall(cast);
            AddBackground(cast);
            //AddBackground2(cast);
            AddRacket(cast);
            AddDialog(cast, Constants.WAS_GOOD_GAME);

            script.ClearAllActions();

            TimedChangeSceneAction ta = new TimedChangeSceneAction(Constants.NEW_GAME, 5, DateTime.Now);
            script.AddAction(Constants.INPUT, ta);

            AddOutputActions(script);
        }

        // -----------------------------------------------------------------------------------------
        // casting methods
        // -----------------------------------------------------------------------------------------

        private void AddBall(Cast cast)
        {
            cast.ClearActors(Constants.BALL_GROUP);
        
            int x = Constants.CENTER_X - Constants.BALL_WIDTH / 2;
            int y = Constants.SCREEN_HEIGHT - Constants.RACKET_HEIGHT - Constants.BALL_HEIGHT;
        
            Point position = new Point(x, y);
            Point size = new Point(Constants.BALL_WIDTH, Constants.BALL_HEIGHT);
            Point velocity = new Point(0, 0);
        
            Body body = new Body(position, size, velocity);
            Image image = new Image(Constants.BALL_IMAGE);
            Ball ball = new Ball(body, image, false);
        
            cast.AddActor(Constants.BALL_GROUP, ball);
        }
        private void AddTime(Cast cast){
            cast.ClearActors(Constants.TIME_GROUP);
            Text text = new Text(Constants.TIME_FORMAT, Constants.FONT_FILE, Constants.FONT_SIZE, 9+ Constants.ALIGN_CENTER, Constants.WHITE);
            Point position = new Point(Constants.CENTER_X, Constants.HUD_MARGIN);
            Label label = new Label(text, position);
            cast.AddActor(Constants.TIME_GROUP, label);
        }
        private void AddBricks(Cast cast)
        {
            cast.ClearActors(Constants.BRICK_GROUP);

            


            Stats stats = (Stats)cast.GetFirstActor(Constants.STATS_GROUP);
            int level = stats.GetLevel() % Constants.BASE_LEVELS;
            string filename = string.Format(Constants.LEVEL_FILE, level);
            List<List<string>> rows = LoadLevel(filename);
            
            Random _random = new Random();
            
            List<string> images = Constants.Car_Images["c"].GetRange(1,5);
            foreach (string image in images){
            int location = _random.Next(0,4);
            int x = Constants.FIELD_LEFT + 470 + _random.Next(0, 8)*(Constants.BRICK_WIDTH+20);
            int y = Constants.FIELD_TOP + _random.Next(0,2)*Constants.BRICK_HEIGHT-_random.Next(0, 100);
            Point position = new Point(x, y);
            
            Point size = new Point(Constants.BRICK_WIDTH, Constants.BRICK_HEIGHT);
            Point velocity = new Point(0, 3);

            Body body = new Body(position, size, velocity);
            Animation animation = new Animation(images, Constants.BRICK_RATE, 1, null , null);
            position = body.GetPosition();
            int points = Constants.BRICK_POINTS;
            Brick brick = new Brick(body, animation, points, velocity, false);
            cast.AddActor(Constants.BRICK_GROUP, brick);}
            // for (int r = 0; r < rows.Count; r++)
            // {
            //     for (int c = 0; c < rows[r].Count; c++)
            //     {   
            //         int x = Constants.FIELD_RIGHT/3 +20+ c * Constants.BRICK_WIDTH;
            //         int y = Constants.FIELD_TOP + r * Constants.BRICK_HEIGHT;2

            //         string color = rows[r][c][0].ToString();
            //         int frames = (int)Char.GetNumericValue(rows[r][c][1]);
            //         int points = Constants.BRICK_POINTS;

            //         Point position = new Point(x, y);
            //         Point size = new Point(Constants.BRICK_WIDTH, Constants.BRICK_HEIGHT);
         //         Point velocity = new Point(0, 0);
        //        List<string> images = Constants.Car_Images[color].GetRange(1, frames);

            //         Body body = new Body(position, size, velocity);
            //         Animation animation = new Animation(images, Constants.BRICK_RATE, 1, null, null);
                    
            //         Brick brick = new Brick(body, animation, points, false);
            //         cast.AddActor(Constants.BRICK_GROUP, brick);
            //     }
            // }
        }
        // private void AddBackground(Cast cast){
        //     cast.ClearActors(Constants.BACKGROUND_GROUP);
        
        //     int x = 0;
        //     int y = 0;
        
        //     Point position = new Point(x, y);
        //     Point size = new Point(Constants.Background_Height, Constants.Background_width);
        //     Point velocity = new Point(0, 5);
        
        //     Body body = new Body(position, size, velocity);
        //     Animation animation = new Animation(Constants.BACKGROUND_IMAGES, Constants.RACKET_RATE, 1, velocity, position);
        //     Background background = new Background(body, animation, false);
        
        //     cast.AddActor(Constants.BACKGROUND_GROUP, background);
            

        // }
         private void AddBackground(Cast cast)

        {

            for (int i = 0; i < Constants.NUMBER_SIDE_SLIDES; i++)

            {

                int x = 0;//Constants.CENTER_X - Constants.SIDE_WIDTH / 2;

                int y = i * -Constants.SCREEN_HEIGHT;//Constants.SCREEN_HEIGHT - Constants.SIDE_HEIGHT - Constants.SIDE_HEIGHT; //maybe +1 in y



                Point position = new Point(x, y);

                Point size = new Point(Constants.SCREEN_WIDTH, Constants.SCREEN_HEIGHT);

                Point velocity = new Point(0, 5);



                Body body = new Body(position, size, velocity);

                Image image = new Image(Constants.SIDES_IMAGE);

                Background side = new Background(body, image, false);



                cast.AddActor(Constants.BACKGROUND_GROUP, side);

            }






        }
        //  private void AddBackground2(Cast cast){
        //     cast.ClearActors(Constants.BACKGROUND_GROUP);
        
        //     int x = 0;
        //     int y = 500;
        
        //     Point position = new Point(x, y);
        //     Point size = new Point(Constants.Background_Height, Constants.Background_width);
        //     Point velocity = new Point(0, 5);
        
        //     Body body = new Body(position, size, velocity);
        //     Animation animation = new Animation(Constants.BACKGROUND_IMAGES, Constants.RACKET_RATE, 1, velocity, position);
        //     Background background = new Background(body, animation, false);
        
        //     cast.AddActor(Constants.BACKGROUND_GROUP, background);

        // }
        private void AddDialog(Cast cast, string message)
        {
            cast.ClearActors(Constants.DIALOG_GROUP);

            Text text = new Text(message, Constants.FONT_FILE, Constants.FONT_SIZE, 
                Constants.ALIGN_CENTER, Constants.WHITE);
            Point position = new Point(Constants.CENTER_X, Constants.CENTER_Y);

            Label label = new Label(text, position);
            cast.AddActor(Constants.DIALOG_GROUP, label);   
        }

        private void AddLevel(Cast cast)
        {
            cast.ClearActors(Constants.LEVEL_GROUP);

            Text text = new Text(Constants.LEVEL_FORMAT, Constants.FONT_FILE, Constants.FONT_SIZE, 
                Constants.ALIGN_LEFT, Constants.WHITE);
            Point position = new Point(Constants.HUD_MARGIN, Constants.HUD_MARGIN);

            Label label = new Label(text, position);
            cast.AddActor(Constants.LEVEL_GROUP, label);
        }

        private void AddLives(Cast cast)
        {
            cast.ClearActors(Constants.LIVES_GROUP);

            Text text = new Text(Constants.LIVES_FORMAT, Constants.FONT_FILE, Constants.FONT_SIZE, 
                Constants.ALIGN_RIGHT, Constants.WHITE);
            Point position = new Point(Constants.SCREEN_WIDTH - Constants.HUD_MARGIN, 
                Constants.HUD_MARGIN);

            Label label = new Label(text, position);
            cast.AddActor(Constants.LIVES_GROUP, label);   
        }

        private void AddRacket(Cast cast)
        {
            cast.ClearActors(Constants.RACKET_GROUP);
        
            int x = Constants.CENTER_X - Constants.RACKET_WIDTH / 2;
            int y = Constants.SCREEN_HEIGHT - Constants.RACKET_HEIGHT;
        
            Point position = new Point(x, y);
            Point size = new Point(Constants.RACKET_WIDTH, Constants.RACKET_HEIGHT);
            Point velocity = new Point(0, 0);
        
            Body body = new Body(position, size, velocity);
            Animation animation = new Animation(Constants.RACKET_IMAGES, Constants.RACKET_RATE, 0, null, null);
            Racket racket = new Racket(body, animation, false);
        
            cast.AddActor(Constants.RACKET_GROUP, racket);
        }

        private void AddScore(Cast cast)
        {
            cast.ClearActors(Constants.SCORE_GROUP);

            Text text = new Text(Constants.SCORE_FORMAT, Constants.FONT_FILE, Constants.FONT_SIZE, 
                Constants.ALIGN_CENTER, Constants.WHITE);
            Point position = new Point(Constants.CENTER_X, Constants.HUD_MARGIN);
            
            Label label = new Label(text, position);
            cast.AddActor(Constants.SCORE_GROUP, label);   
        }

        private void AddStats(Cast cast)
        {
            cast.ClearActors(Constants.STATS_GROUP);
            Stats stats = new Stats();
            cast.AddActor(Constants.STATS_GROUP, stats);
        }

        private List<List<string>> LoadLevel(string filename)
        {
            List<List<string>> data = new List<List<string>>();
            using(StreamReader reader = new StreamReader(filename))
            {
                while (!reader.EndOfStream)
                {
                    string row = reader.ReadLine();
                    List<string> columns = new List<string>(row.Split(',', StringSplitOptions.TrimEntries));
                    data.Add(columns);

                }
            }
            
             
            return data;
        }

        // -----------------------------------------------------------------------------------------
        // scriptig methods
        // -----------------------------------------------------------------------------------------

        private void AddInitActions(Script script)
        {
            script.AddAction(Constants.INITIALIZE, new InitializeDevicesAction(AudioService, 
                VideoService));
        }

        private void AddLoadActions(Script script)
        {
            script.AddAction(Constants.LOAD, new LoadAssetsAction(AudioService, VideoService));
        }

        private void AddOutputActions(Script script)
        {   script.AddAction(Constants.OUTPUT, new DrawSidesAction(VideoService));
            script.AddAction(Constants.OUTPUT, new StartDrawingAction(VideoService));
            script.AddAction(Constants.OUTPUT, new DrawRacketAction(VideoService));
            script.AddAction(Constants.OUTPUT, new DrawHudAction(VideoService));
            script.AddAction(Constants.OUTPUT, new DrawBallAction(VideoService));
            script.AddAction(Constants.OUTPUT, new DrawBricksAction(VideoService));
            
            
            script.AddAction(Constants.OUTPUT, new DrawDialogAction(VideoService));
            script.AddAction(Constants.OUTPUT, new EndDrawingAction(VideoService));
        }

        private void AddUnloadActions(Script script)
        {
            script.AddAction(Constants.UNLOAD, new UnloadAssetsAction(AudioService, VideoService));
        }

        private void AddReleaseActions(Script script)
        {
            script.AddAction(Constants.RELEASE, new ReleaseDevicesAction(AudioService, 
                VideoService));
        }

        private void AddUpdateActions(Script script)
        {   script.AddAction(Constants.UPDATE, new MoveSidesAction());
            script.AddAction(Constants.UPDATE, new MoveBallAction());
            script.AddAction(Constants.UPDATE, new MoveBrickAction());
            script.AddAction(Constants.UPDATE, new MoveRacketAction());
           
            script.AddAction(Constants.UPDATE, new CollideBordersAction(PhysicsService, AudioService));
            script.AddAction(Constants.UPDATE, new CollideBrickAction(PhysicsService, AudioService));
            script.AddAction(Constants.UPDATE, new CollideRacketAction(PhysicsService, AudioService));
            script.AddAction(Constants.UPDATE, new CheckOverAction());     
        }
    }
}