using System;
using System.Collections.Generic;
using System.Text;

namespace asynchronous_programming_async_await
{
    public class Metrics
    {
        public Metrics()
        {
            AwaitTasksEfficiently = new List<double>();
            CompositionWithTasks = new List<double>();
            NonAsynchronous = new List<double>();
            NonBlocking = new List<double>();
            StartTasksConcurrently = new List<double>();
            StartTasksConcurrentlyButMoveOrder = new List<double>();
        }
        public List<double> AwaitTasksEfficiently { get; set; }
        public List<double> CompositionWithTasks { get; set; }
        public List<double> NonAsynchronous { get; set; }
        public List<double> NonBlocking { get; set; }
        public List<double> StartTasksConcurrently { get; set; }
        public List<double> StartTasksConcurrentlyButMoveOrder { get; set; }
    }
}
