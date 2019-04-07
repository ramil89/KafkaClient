using System;
using System.Collections.Generic;
using System.Text;

namespace KafkaClient.Exceptions
{
    public class KafkaConsumeStartedException: Exception
    {
        public KafkaConsumeStartedException()
            : base()
        {

        }

        public KafkaConsumeStartedException(string message)
            : base(message)
        {

        }
    }
}
