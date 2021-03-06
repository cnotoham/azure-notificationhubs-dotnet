//-----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved. 
// Licensed under the MIT License. See License.txt in the project root for 
// license information.
//-----------------------------------------------------------------------------

namespace Microsoft.Azure.NotificationHubs.Messaging
{
    using System;
    using System.Collections;
    using System.Runtime.Serialization;

    /// <summary> Exception for signalling messaging errors. </summary>
    /// Any class that derives from this should be added to Microsoft.Notifications.Messaging.MessagingExceptionHelper.ErrorCodes
    [Serializable]
    public class MessagingException : Exception
    {
        // TODO 228780 Remove MessagingException constructor that does not take an exception detail and context information


        /// <summary>
        /// Initializes a new instance of the <see cref="MessagingException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public MessagingException(string message) :
            base(message)
        {
            MessagingExceptionDetail detail = MessagingExceptionDetail.UnknownDetail(message);
            this.Initialize(detail, DateTime.UtcNow);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessagingException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        public MessagingException(string message, Exception innerException) :
            base(message, innerException)
        {
            MessagingExceptionDetail detail = MessagingExceptionDetail.UnknownDetail(message);
            this.Initialize(detail, DateTime.UtcNow);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessagingException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="isTransientError">If set to <c>true</c>, indicates it is a transient error.</param>
        /// <param name="innerException">The inner exception.</param>
        public MessagingException(string message, bool isTransientError, Exception innerException) :
            base(message, innerException)
        {
            MessagingExceptionDetail detail = MessagingExceptionDetail.UnknownDetail(message);
            this.Initialize(detail, DateTime.UtcNow);
            this.IsTransient = isTransientError;
        }

        /// <summary> Constructor. </summary>
        /// <param name="detail"> </param>
        internal MessagingException(MessagingExceptionDetail detail) :
            base(detail.Message)
        {
            this.Initialize(detail, DateTime.UtcNow);
        }

        /// <summary> Constructor. </summary>
        /// <param name="detail"> Detail about the cause of the exception. </param>
        /// <param name="innerException"> The inner exception. </param>
        internal MessagingException(MessagingExceptionDetail detail, Exception innerException) :
            base(detail.Message, innerException)
        {
            this.Initialize(detail, DateTime.UtcNow);
        }

        /// <summary> Constructor. </summary>
        /// <param name="info">    The information. </param>
        /// <param name="context"> The context. </param>
        protected MessagingException(SerializationInfo info, StreamingContext context) :
            base(info, context)
        {
            this.Initialize(
                (MessagingExceptionDetail)info.GetValue("Detail", typeof(MessagingExceptionDetail)),
                (DateTime)info.GetValue("Timestamp", typeof(DateTime)));
        }

        /// <summary>
        /// Details about the cause of the exception
        /// </summary>
        public MessagingExceptionDetail Detail { get; private set; }

        /// <summary>
        /// UTC timestamp of the exception
        /// </summary>
        public DateTime Timestamp { get; private set; }

        /// <summary>
        /// A boolean indicating if the exception is a transient error or not.
        /// getting a true from this property implies that user can retry the operation that 
        /// generated the exception without additional intervention. 
        /// </summary>
        public bool IsTransient { get; protected set; }

        /// <summary>
        /// Sets the <see cref="T:System.Runtime.Serialization.SerializationInfo"/> with information about the exception.
        /// </summary>
        /// <param name="info">The serialization information.</param>
        /// <param name="context">The streaming context.</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("Detail", this.Detail);
            info.AddValue("Timestamp", this.Timestamp.ToString());
        }

        /// <summary>
        /// Gets a collection of key/value pairs that provide additional user-defined information about the exception.
        /// </summary>
        public sealed override IDictionary Data
        {
            get
            {
                return base.Data;
            }
        }

        void Initialize(MessagingExceptionDetail detail, DateTime timestamp)
        {
            this.IsTransient = true;
            this.Detail = detail;
            this.Timestamp = timestamp;
        }
    }
}
