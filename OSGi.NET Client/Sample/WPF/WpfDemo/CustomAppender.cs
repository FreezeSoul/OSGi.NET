using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using log4net.Appender;

namespace WpfDemo
{
    public class CustomAppender : AppenderSkeleton
    {
        public static SplashWindow Context { get; set; }

        protected override void Append(log4net.Core.LoggingEvent loggingEvent)
        {
            if (Context != null)
            {
                var message = RenderLoggingEvent(loggingEvent);
                lock (SplashWindow.lockObj)
                {
                    Context.MessageStack.Enqueue(message);
                }
            }
        }


        protected override bool RequiresLayout
        {
            get { return true; }
        }
    }
}
