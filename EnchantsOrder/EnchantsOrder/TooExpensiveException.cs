using System;

namespace EnchantsOrder
{
    /// <summary>
    /// Values used to indicate the reason of Too Expensive in the <see cref="TooExpensiveException" />.
    /// </summary>
    public enum TooExpensiveReason
    {
        /// <summary>
        /// Final penalty larger than 6.
        /// </summary>
        Penalty,

        /// <summary>
        /// Max xp larger than 39.
        /// </summary>
        Experience,
    }

    /// <summary>
    /// Exception for too expensive errors.
    /// </summary>
    public sealed class TooExpensiveException : Exception
    {
        private static string penaltymessage = $"Cannot enchant: Final penalty larger than {Instance.max_penalty}.";
        private static string experiencemessage = $"Cannot enchant! Max xp larger than {Instance.max_experience}.";

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="reason"></param>
        public TooExpensiveException(TooExpensiveReason reason) : base(GetMessage(reason))
        {
            Reason = reason;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="TooExpensiveException" />.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="reason"></param>
        public TooExpensiveException(string message, TooExpensiveReason reason) : base(message ?? GetMessage(reason))
        {
            Reason = reason;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="TooExpensiveException" />.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        /// <param name="reason"></param>
        public TooExpensiveException(string message, Exception innerException, TooExpensiveReason reason) : base(message ?? GetMessage(reason), innerException)
        {
            Reason = reason;
        }

        private static string GetMessage(TooExpensiveReason reason)
        {
            switch (reason)
            {
                case TooExpensiveReason.Penalty: return penaltymessage;
                case TooExpensiveReason.Experience: return experiencemessage;
                default: return string.Empty;
            }
        }

        /// <summary>
        /// The reason of Too Expensive in the <see cref="TooExpensiveException" />.
        /// </summary>
        public TooExpensiveReason Reason { get; set; }
    }
}
