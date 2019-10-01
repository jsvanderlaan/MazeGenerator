using System;

namespace Common
{
    public class Timer
    {
        private int _steps = 0;
        private int _count = 0;
        private int _sections = 0;
        private double _sectionLength; 
        private double _boundary;
        private int _currBoundary = 0;
        private readonly string _name;
        private DateTime _start;
        private DateTime _end;
        private bool _loadingbar;
        private char _topChar = 'v';
        private char _bottomChar = '|';
        private bool _stopped = false;

        public Timer(string name)
        {
            _name = name;
        }

        public void Start()
        {
            _start = DateTime.Now;
            Console.WriteLine($"{_start}: {_name} started");
        }

        public void Start(int numberOfSteps, int loadingbarLength = 100)
        {
            Start();
            _sections = loadingbarLength;
            _loadingbar = true;
            _steps = numberOfSteps;
            _sectionLength = _steps / (double)loadingbarLength;
            _boundary = _sectionLength;
            for(int i = 0; i < loadingbarLength; i++) Console.Write(_topChar);
            Console.WriteLine();
        }

        public void Next()
        {
            if (_loadingbar)
            {
                _count++;
                if (_count >= _steps)
                {
                    _currBoundary++;
                    Stop();
                }
                else if (_count >= _boundary)
                {
                    _boundary += _sectionLength;
                    _currBoundary++;
                    Console.Write(_bottomChar);
                }
            }
        }

        public void Stop()
        {
            if (_stopped) return;
            _stopped = true;
            if (_loadingbar)
            {
                for (int i = 0; i < _sections - _currBoundary; i++) Console.Write(_bottomChar);
                Console.WriteLine();
            }
            _end = DateTime.Now;
            Console.WriteLine($"{_end}: {_name} ended in {(int)_end.Subtract(_start).TotalMilliseconds} milliseconds");
            Console.WriteLine();
        }
    }
}
