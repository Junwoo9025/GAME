using System;
using System.Collections.Generic;


namespace Unit06.Game.Casting
{
    /// <summary>
    /// An animation.
    /// </summary>
    public class Animation
    {
        private TimeSpan _delay;
        private List<string> _images;
        private int _rate;
        private int _index;
        private int _frame;
        private DateTime _startTime;
        private Point _position= new Point(0,0);
        private Point _velocity= new Point(0,0);


        /// <summary>
        /// Constructs a new instance of Animation.
        /// </summary>
        public Animation(List<string> images, int rate, int delay,Point velocity, Point position)
        {
            this._images = images;
            this._rate = rate;
            this._delay = new TimeSpan(0, 0, delay);
            this._index = 0;
            this._frame = 0;
            this._startTime = DateTime.Now;
            this._velocity = velocity;
            this._position = position;
        }

        /// <summary>
        /// Gets the delay between animation cycles.
        /// </summary>
        /// <returns>The delay in seconds.</returns>
        public int GetDelay()
        {
            return _delay.Seconds;
        }

        /// <summary>
        /// Gets the images used in the animation.
        /// </summary>
        /// <returns>A list of filenames.</returns>
        public List<string> GetImages()
        {
            return _images;
        }

        /// <summary>
        /// Gets the animation rate or frames between images.
        /// </summary>
        /// <returns>The animation rate.</returns>
        public int GetRate()
        {
            return _rate;
        }

        /// <summary>
        /// Gets the next image in the animation to display.
        /// </summary>
        /// <returns>The next image.</returns>
        public Image NextImage()
        {
            string filename = _images[_index];
            Image image = new Image(filename);
            //Point position1 = new Point(0,0);
            //position1 = _position;
             
            DateTime currentTime = DateTime.Now;
            TimeSpan elapsedTime = currentTime.Subtract(_startTime);

            if (elapsedTime > _delay)
            {
                //Point newPosition = _position.Add(_velocity);
                _frame++; 
                if (_frame >= _rate)
                {
                    _index = (_index + 1) % _images.Count;
                    _frame = 0;
                } 
            //     if (position1.GetY() < Constants.SCREEN_HEIGHT){

            //     filename = _images[_index];
            //     image = new Image(filename);
            // }
                

                if (_index >= _images.Count - 1)
                {
                    _startTime = currentTime;
                }
            }

            return image; 
        }
        
        public Image NextImageForCars(){
            Random random = new Random();
            string filename = _images[_index];
            Image image = new Image(filename);
            // //Point newPosition = _position.Add(_velocity);
            //  Point newVelocity = new Point(0,0);
            //  newVelocity = _position.Add(_velocity);
            // _body.SetVelocity(newVelocity);
            
            return image;

        }
        public Point MoveNext_falling()
        {   Point position2 = new Point(0,0);
            position2 = _position;
            int x = position2.GetX();
            //int x = ((_position.GetX() + _velocity.GetX()) + maxX) % maxX;
            int y = ((position2.GetY() + _velocity.GetY()) + Constants.Background_Height) % Constants.Background_width;
            Point point = new Point(x,y);
            return point;
        }
    }
}       