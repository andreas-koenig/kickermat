namespace Logging.log4net.Appenders
{
    using System;
    using System.Windows.Forms;
    using global::log4net;
    using global::log4net.Appender;
    using global::log4net.Core;

    /// <summary>
    /// Appender for log4net which writes messages to a list box.
    /// </summary>
    public class ListBoxAppender : AppenderSkeleton
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListBoxAppender"/> class.
        /// </summary>
        /// <remarks>Empty default constructor</remarks>
        public ListBoxAppender()
        {
            this.SettingsUserControl = new ListBoxAppenderUserControl();
            this.LoggingListBox = this.SettingsUserControl.LoggingListBox;
        }

        /// <summary>
        /// Gets the settings user control.
        /// </summary>
        /// <value>The settings user control.</value>
        public ListBoxAppenderUserControl SettingsUserControl { get; private set; }

        /// <summary>
        /// Gets or sets the logging list box.
        /// </summary>
        /// <value>The logging list box.</value>
        public ListBox LoggingListBox { get; set; }        

        /// <summary>
        /// Gets a list box appender.
        /// </summary>
        /// <param name="type">The type for which the appender displays messages.</param>
        /// <returns>A ListBoxAppender which displays messages for the type, null if no appender was found.</returns>
        public static ListBoxAppender GetAppender(Type type)
        {
            global::log4net.Repository.Hierarchy.Hierarchy h = (global::log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository();            
            
            // First step: Check in logger hierarchy if append is available and return if found
            foreach (global::log4net.Repository.Hierarchy.Logger logger in h.GetCurrentLoggers())
            {
                if (type.FullName.StartsWith(logger.Name))
                {
                    foreach (IAppender appender in logger.Appenders)
                    {
                        ListBoxAppender listBoxAppender = appender as ListBoxAppender;
                        if (listBoxAppender != null)
                        {
                            return listBoxAppender;
                        }
                    }
                }
            }

            // Second step: If no appender was found in logger hierarchy, 
            // check if root loogger contains an appender and return it.
            foreach (IAppender appender in h.Root.Appenders)
            {
                ListBoxAppender listBoxAppender = appender as ListBoxAppender;
                if (listBoxAppender != null)
                {
                    return listBoxAppender;
                }
            }

            // Third step: no logger found, return null
            return null;
        }    

        /// <summary>
        /// Subclasses of <see cref="T:log4net.Appender.AppenderSkeleton"/> should implement this method
        /// to perform actual logging.
        /// </summary>
        /// <param name="loggingEvent">The event to append.</param>
        /// <remarks>
        /// <para>
        /// A subclass must implement this method to perform
        /// logging of the <paramref name="loggingEvent"/>.
        /// </para>
        /// <para>This method will be called by <see cref="M:log4net.Appender.AppenderSkeleton.DoAppend(log4net.Core.LoggingEvent)"/>
        /// if all the conditions listed for that method are met.
        /// </para>
        /// <para>
        /// To restrict the logging of events in the appender
        /// override the <see cref="M:log4net.Appender.AppenderSkeleton.PreAppendCheck"/> method.
        /// </para>
        /// </remarks>
        protected override void Append(LoggingEvent loggingEvent)
        {            
            if (this.LoggingListBox != null)
            {
                if (this.LoggingListBox.InvokeRequired == true)
                {
                    this.LoggingListBox.Invoke(new Action<LoggingEvent>(this.Append), loggingEvent);
                }
                else
                {
                    string[] messageParts = RenderLoggingEvent(loggingEvent).Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                    foreach (string message in messageParts)
                    {
                        this.LoggingListBox.Items.Add(message);
                    }
                }
            }
        }
    }
}
