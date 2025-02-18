﻿using Serilog.Events;
using System;

namespace ToDoList.Common.Exceptions.Logging
{
    public class LogMessage
    {
        public int UserId { get; set; } = 0;
        public string UserEmail { get; set; } = "";
        public string Message { get; set; } = "";
        public DateTime CreatedOn { get; set; }=DateTime.UtcNow;
        public string ApplicationName { get; set; } = "";
        public string RequestPath { get; set; } = "";
        public LogEventLevel LogLevel { get; set; }
    }
}
