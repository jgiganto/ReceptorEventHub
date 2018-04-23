using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceptorEventHub
{
    class Program
    {
        private static async Task ProcesarMensajesEventHub(string[] args)
        {
            //RECUPERAMOS LA CADENA EVENTHUB
            //Y STORAGE DEL CONFIG
            String cadenaeventhub =
                ConfigurationManager.AppSettings["eventhub"];
            String cadenastorage =
                ConfigurationManager.AppSettings["cuentastorage"];
            Console.WriteLine("Comenzando el procesado de mensajes");
            //CREAMOS EL PROCESADOR DE MENSAJES
            EventProcessorHost procesadormensajes =
                 new EventProcessorHost(
                     "PROCESADOR CONSOLA",
                     "centromensajestajamar",
                     EventHubConsumerGroup.DefaultGroupName,
                     cadenaeventhub, cadenastorage);
            //CREAMOS OPCIONES PARA EL PROCESO
            EventProcessorOptions opcionesproceso = new EventProcessorOptions()
            {
                MaxBatchSize = 100,
                PrefetchCount = 1,
                ReceiveTimeOut = TimeSpan.FromSeconds(20)
            };

            //REGISTRAMOS EL PROCESO CON LA CLASE QUE
            //ADMINISTRARA LA RECEPCION DE MENSAJES
            await procesadormensajes.RegisterEventProcessorAsync<ProcesadorMensajes>();

            Console.WriteLine("Recibiendo. Pulse ENTER cuando quiera finalizar.");
            Console.ReadLine();

            //FINALIZAMOS LA LECTURA DE MENSAJES
            await procesadormensajes.UnregisterEventProcessorAsync();
        }

        static void Main(string[] args)
        {
            //REALIZAMOS LA LLAMADA PARA PROCESAR
            //LOS MENSAJES
            ProcesarMensajesEventHub(args).GetAwaiter().GetResult();
        }

    }
}
