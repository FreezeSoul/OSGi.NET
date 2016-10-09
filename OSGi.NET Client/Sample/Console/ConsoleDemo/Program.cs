using System;

using OSGi.NET.Core.Root;

namespace ConsoleDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //创建框架工厂
            var frameworkFactory = new FrameworkFactory();
            //创建框架内核
            var framework = frameworkFactory.CreateFramework();
            //初始化框架
            framework.Init();
            //启动框架
            framework.Start();

            Console.ReadLine();
        }
    }
}
