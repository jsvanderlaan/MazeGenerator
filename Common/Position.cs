﻿namespace Common
{
    public struct Position
    {
        public int X;
        public int Y;

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj)
        {
            if (obj is Position)
            {
                var pos = (Position)obj;
                return X == pos.X && Y == pos.Y;
            }
            return false;
        }

        public static bool operator ==(Position lhs, Position rhs)
        {
            return Equals(lhs, rhs);
        }

        public static bool operator !=(Position lhs, Position rhs)
        {
            return !Equals(lhs, rhs);
        }

        public override string ToString()
        {
            return $"[{X},{Y}]";
        }

        public override int GetHashCode()
        {
            var hashCode = 1861411795;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            return hashCode;
        }
    }
}
