using System;
using System.ServiceModel;

namespace ConsoleDemo
{
    [ServiceContract]
    public interface IMvpLoginOutService
    {
        [OperationContract(IsOneWay = true)]
        void Login(string userId, string userName, string system_id, string aostarMvpLoacalPath);

        [OperationContract(IsOneWay = true)]
        void LogOut(string userId, string userName, string system_id);
    }

    public static class BHOUserLoginOutDataProvider
    {
        //private static readonly ILogger logger = LoggerFactory.GetLogger("BHOUserLoginOutDataProvider");

        static string serviceAddress = "net.pipe://localhost/BHOUserLoginOutService";

        public static void Login(string userId, string userName, string system_id, string aostarMvpLoacalPath)
        {
            Console.Write("test");
            try
            {
                int sign = 0;
                int num = 10 / sign;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            try
            {
                Console.Write(string.Format("userId:{0} userName:{1} systemId:{2} path:{3}", userId, userName, system_id, aostarMvpLoacalPath));
                ChannelFactory<IMvpLoginOutService> l_factory = new ChannelFactory<IMvpLoginOutService>(new NetNamedPipeBinding(), new EndpointAddress(serviceAddress));
                IMvpLoginOutService iService = l_factory.CreateChannel();
                iService.Login(userId, userName, system_id, aostarMvpLoacalPath);
            }
            catch (EndpointNotFoundException ex)
            {
                Console.Write(ex.Message);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }
        /// <summary>
        /// 通知BHO用户退出
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <param name="system_id"></param>
        public static void LogOut(string userId, string userName, string system_id)
        {
            try
            {
                //logger.Info(string.Format("userId:{0} userName:{1} systemId:{2}", userId, userName, system_id));
                ChannelFactory<IMvpLoginOutService> l_factory = new ChannelFactory<IMvpLoginOutService>(new NetNamedPipeBinding(), new EndpointAddress(serviceAddress));
                IMvpLoginOutService iService = l_factory.CreateChannel();
                iService.LogOut(userId, userName, system_id);
            }
            catch (EndpointNotFoundException)
            {
               // logger.Info("EndpointNotFoundException");
            }
            catch (Exception ex)
            {
               // logger.Info(ex.Message);
            }
        }
    }
}
