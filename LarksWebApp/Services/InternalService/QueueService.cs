using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;

namespace LarksWebApp.Services.InternalService
{
    public class QueueService
    {
        IConnection connection;
        IModel channel;
        ConnectionFactory connectionFactory = new ConnectionFactory();

        private readonly string EXCHANGE = "ex.Police_Station";
     

        public QueueService()
        {
            connectionFactory.HostName = "kangaroo-01.rmq.cloudamqp.com";
            connectionFactory.VirtualHost = "ztzwccte";
            connectionFactory.Port = 5672;

            connectionFactory.UserName = "ztzwccte";
            connectionFactory.Password = "sopr25Gx3CW0IhMp7plbb3KuLOpcWvfP";
        }

        /// <summary>
        ///  A service responsible for sending Messages 
        /// </summary>
        /// <param name="payload"></param>
        /// <param name="routingKey"></param>
        /// <param name="queueName"></param>
        public void SendService(string payload, string routingKey, string queueName)
        {

            try
            {
                connection = connectionFactory.CreateConnection();

                channel = connection.CreateModel();

                channel.ExchangeDeclare(EXCHANGE, "direct", true, false, null);

                channel.QueueDeclare(queueName, true, false, false, null);

                //Bind the Queue to the Exchange and with a specific Routing Key
                channel.QueueBind(queueName, EXCHANGE, routingKey);

                channel.BasicPublish(EXCHANGE, routingKey, null, Encoding.UTF8.GetBytes(payload));

                channel.Close();

            }
            catch (Exception ex)
            {

                throw;
            }

        }

        /// <summary>
        ///  This Method is responsible for Receiving Messages 
        /// </summary>
        /// <param name="queueName"></param>
        /// <returns></returns>
      //  public string CollectFromStationQueue()
        public List<string> CollectFromStationQueue(string queueName)
        {
            try
            {
                //string queueName = "q.createPoliceStations";


                connection = connectionFactory.CreateConnection();
                channel = connection.CreateModel();
                channel.QueueDeclare(queueName, true, false, false, null);
                channel.BasicQos(0,1, true);

                //Event Consumer
                var consumer = new EventingBasicConsumer(channel);

                var msgsRecievedGate = new ManualResetEventSlim(false);
                var response = channel.QueueDeclarePassive(queueName);
                var msgCount = response.MessageCount;
                var msgRecieved = 0;
               // string retrievedMessage = "";
               var retrievedMessage = new List<string>();


                consumer.Received += (model, ea) =>
                {
                    msgRecieved++;
                    var body = ea.Body.Span;
                    var message = Encoding.UTF8.GetString(body);

                    //retrievedMessage = message;
                    retrievedMessage.Add(message);



                    if (msgRecieved == msgCount)
                    {

                        msgsRecievedGate.Set();
                        channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);

                        //retrievedMessage.Add(message);

                        return;
                    }
                };

               
               channel.BasicConsume(queueName, true, consumer);


               //msgsRecievedGate.Wait();
               msgsRecievedGate.Wait(5000);


                // return retrievedMessage;


                channel.Close();
                return retrievedMessage;

            }
            catch (Exception ex)
            {

                throw;
            }


        }

    }
}
