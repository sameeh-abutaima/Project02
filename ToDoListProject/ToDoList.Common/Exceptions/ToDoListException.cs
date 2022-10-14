using System;

namespace ToDoList.Common.Exceptions
{
    public class ToDoListException : Exception
    {
        #region Props
        public int StatusCode { get; set; }
        #endregion Props

        #region Ctors

        public ToDoListException() : base("ToDoList Exception")
        {
        }
        public ToDoListException(string msg) : base(msg)
        {
        }
        public ToDoListException(string msg, Exception innerexception) : base(msg, innerexception)
        {
        }
        public ToDoListException(int statusCode, string msg) : base(msg)
        {
            StatusCode = statusCode;
        }
        
        public ToDoListException(int statusCode,string msg,Exception innerexception):base(msg,innerexception)
        {
            StatusCode = statusCode;
        }
        #endregion Ctors
    }
}
