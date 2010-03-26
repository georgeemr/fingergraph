using System;


namespace FingerGraphDB
{
    class DbException : ApplicationException
    {
        protected DbException(string str)
            : base(str)
        {
        }
    }


    class CannotConnectToDatabaseException : DbException
    {
        public CannotConnectToDatabaseException()
            : base("Can't connect to Database!")
        {
        }
    }

    class InvalidQueryException : DbException
    {
        public InvalidQueryException()
            : base("Invalid query!")
        {
        }
    }

}
