using Common.Enums;
using System;

namespace Common
{
    public class Timer
    {
        public Timer(TimerCategory timerCategory, TimerTask timerTask) : this(timerCategory, timerTask, TimerAction.None) { }
        public Timer(TimerCategory timerCategory, TimerTask timerTask, TimerAction timerAction)
        {
            TimerCategory = timerCategory;
            TimerTask = timerTask;
            TimerAction = timerAction;
        }
        public string Id { get; set; }
        public TimerCategory TimerCategory { get; }
        public TimerTask TimerTask { get; }
        public TimerAction TimerAction { get; }
        public DateTime StartTime { get; set; }
        public DateTime StopTime { get; set; }
        public double ElapsedMilliseconds { get => ElapsedTimeSpan.TotalMilliseconds; }
        public double ElapsedSeconds { get => ElapsedTimeSpan.TotalSeconds; }
        public double ElapsedMinutes { get => ElapsedTimeSpan.TotalMinutes; }
        public DateTime Start()
        {
            if (StartTime != DateTime.MinValue) throw new InvalidOperationException("Timer already started");
            StartTime = DateTime.Now;
            return StartTime;
        }
        public DateTime Stop()
        {
            if (StopTime != DateTime.MinValue) throw new InvalidOperationException("Timer already stopped");
            StopTime = DateTime.Now;
            return StopTime;
        }
        private TimeSpan ElapsedTimeSpan
        {
            get
            {
                if (StartTime == DateTime.MinValue) return TimeSpan.Zero;
                if (StopTime == DateTime.MinValue) return DateTime.Now.Subtract(StartTime);
                return StopTime.Subtract(StartTime);
            }
        }
    }
}
