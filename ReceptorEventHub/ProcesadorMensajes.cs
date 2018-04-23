using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;

namespace ReceptorEventHub
{
    public class ProcesadorMensajes : IEventProcessor
    {
        public Task CloseAsync(PartitionContext context, CloseReason reason)
        {
            Console.WriteLine($"Proceso apagando. Particion "
                                 + context.Lease.PartitionId
                                 + ", Razon: " + reason + ".");
            return Task.CompletedTask;

        }

        public Task OpenAsync(PartitionContext context)
        {
            Console.WriteLine($"Proceso abriendo. Particion "
                + context.Lease.PartitionId);
            return Task.CompletedTask;

        }

        public Task ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
        {
            //RECORREMOS TODOS LOS MENSAJES
            foreach (EventData mensaje in messages)
            {
                //RECUPERAMOS LOS DATOS DEL MENSAJE
                string datos = Encoding.UTF8.GetString(mensaje.GetBytes());
                //PODRIAMOS HACER CUALQUIER CARACTERISTICA
                //COMO POR EJEMPLO, SI NOS HUBIERAN ENVIADO UN 
                //OBJETO EN JSON, DESERIALIZAR EL OBJETO 
                //Y UTILIZARLO
                String msj = string.Format("Mensaje recibido.  Particion: '{0}', Data: '{1}'",
                    context.Lease.PartitionId, datos);
                Console.WriteLine(msj);
            }
            return context.CheckpointAsync();

        }
    }
}
