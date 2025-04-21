using System.Net.Sockets;
using System.Runtime.Serialization;

namespace Sobee.Messaging
{
    public class SocketHandle : SocketWrapper
    {
        private static MemoryStream UpdateMessageReadingStream = new MemoryStream(SocketHandle.MaxMessageSize);
        private MessageHelper? MessageHelper;
        private DispatchHelper? Dispatcher;

        public SocketHandle(Socket Socket) : base(Socket)
        {

        }

        public virtual void Update()
        {
            lock (this)
            {
                if (this.GetIsConnected())
                {
                    Message gclass = null;
                    try
                    {
                        int num = 0;
                        int num2 = 0;
                        byte[] array;
                        while ((array = this.DequeueMessage()) != null)
                        {
                            gclass = null;
                            UpdateMessageReadingStream.Position = 0L;
                            UpdateMessageReadingStream.SetLength(0L);
                            UpdateMessageReadingStream.Write(array, 0, array.Length);
                            UpdateMessageReadingStream.Position = 0L;
                            gclass = (Message)this.MessageHelper.ReadMessage();
                            gclass.int_0 = array.Length;
                            num2 += gclass.int_0;
                            num++;
                            this.vmethod_11(new GEventArgs23(Dispatcher.GetDispatchOwner(), new GEventArgs9(this, gclass)));
                        }
                    }
                    catch (SerializationException ex)
                    {
                        //this.vmethod_10(ex);
                        //GClass149.gclass149_0.vmethod_3(ex.ToString() + " fromx : " + this.GetLocalEndPoint().Address.ToString(), LogLevel.Exception);
                        //this.vmethod_2();
                    }
                    catch (Exception ex2)
                    {
                        //string text = (gclass != null) ? gclass.GetType().ToString() : "<no message>";
                        //GClass149.gclass149_0.vmethod_3(string.Concat(new string[]
                        //{
                        //    text,
                        //    " - ",
                        //    ex2.ToString(),
                        //    " from : ",
                        //    this.gclass297_0.method_8().Address.ToString()
                        //}), LogLevel.Exception);
                        //this.vmethod_2();
                    }
                }
                this.QueueBufferedMessage();
            }
        }

        protected virtual void vmethod_11(GEventArgs23 gclass175_0)
        {
            Dispatcher.DispatchToMessageEvent(gclass175_0.method_1().method_1().GetType(), gclass175_0);
            //Dispatcher.DispatchToMessageEvent(gclass175_0.int_0, gclass175_0)
            //if (this.eventHandler_6 != null)
            //{
            //    this.eventHandler_6(this, new GEventArgs9(this, gclass175_0));
            //}
        }

        public void SetDispatchSource(DispatchHelper dispatcher)
        {
            this.Dispatcher = dispatcher;
            this.MessageHelper = new MessageHelper(UpdateMessageReadingStream, this.Dispatcher.GetDispatcher(), this.Dispatcher.GetMessageTypeToIdDelegate());
        }
    }
}
