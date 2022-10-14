using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Common.Exceptions
{
    public class ServiceValidationException:ToDoListException
    {
        #region Ctors
        public ServiceValidationException():base("Service Validation Exception")
        {

        }
        public ServiceValidationException(string msg):base(msg)
        {

        }
        public ServiceValidationException(string msg,Exception ex):base(msg,ex)
        {

        }
        public ServiceValidationException(int statusCode,string msg):base(statusCode,msg)
        {

        }
        public ServiceValidationException(int statusCode,string msg,Exception ex):base (statusCode,msg,ex)
        {

        }
        #endregion Ctors
    }
}
