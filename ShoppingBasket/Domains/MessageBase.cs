namespace ShoppingBasket.Domains
{
    public abstract class MessageBase
    {
        /// <summary>
        /// Gets the message identifier. Used for debug purposes
        /// </summary>
        /// <value>
        /// The message identifier.
        /// </value>
        public ulong CorrelationId { get; }

        protected MessageBase(ulong correlationId)
        {
            CorrelationId = correlationId;
        }
    }
}