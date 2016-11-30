OSGi.NET Implement by FreezeSoul 2013.12.1

这是在项目中实现的一套基于OSGi规范的C#基础框架-OSGi.NET，并且用Go语言初步实现了插件的管理平台-插件仓库。在几个中小型项目中有所应用（Winform、WPF），主要可以解决多人协作的开发规范与插件的管理问题。 

更多说明: http://www.diginfo.me/osgi-net-implement  

简介：

</h3>
OSGi.NET框架是一个参照了OSGi规范的模块化管理框架。框架为应用程序扩展（组件（bundle））提供了一个标准环境。整个框架可以划分为一些层次：
<ul>
<li>1.运行环境</li>
<li>2.模块（Bundle）</li>
<li>3.生命周期管理</li>
<li>4.服务注册</li>
<li>5.扩展点支持</li>
</ul>
目前OSGi.NET具有如下特色：
<ul>
<li>1.组件的可插拔性：组件可根据业务需要在运行时进行装载、卸载操作</li>
<li>2.组件的动态更新：组件在运行时可更新替换当前版本</li>
<li>3.组件的版本隔离：不同组件引用相同产品的不同版本程序集可以版本隔离</li>
<li>4.组件完整的生命周期：包括已安装、已装载、已激活、启动中、停止中、已卸载</li>
<li>5.组件的加载顺序：组件加载根据业务要求可设置加载级别来控制加载次序</li>
<li>6.组件的通信支持：组件间通过面向服务的编程模型来达到组件间通信、调用的目的</li>
<li>7.组件的扩展支持：组件提供了扩展点及其扩展来满足某个组件的扩展性支持</li>
</ul>
启动一个OSGi.NET应用程序仅需要如下代码
<p style="background-color:#323232">
<pre style="color:#dedede; background-color:#323232; font-size:10pt; font-family:'Courier New';"><span style="color:#f0e68c">using</span> System<span style="color:#dedede">;</span>

<span style="color:#f0e68c">using</span> OSGi<span style="color:#dedede">.</span>NET<span style="color:#dedede">.</span>Core<span style="color:#dedede">.</span>Root<span style="color:#dedede">;</span>

<span style="color:#f0e68c">namespace</span> ConsoleDemo
<span style="color:#dedede">{</span>
    <span style="color:#f0e68c">class</span> Program
    <span style="color:#dedede">{</span>
        <span style="color:#f0e68c">static</span> <span style="color:#bdb66b">void</span> <span style="color:#f25252">Main</span><span style="color:#dedede">(</span><span style="color:#bdb66b">string</span><span style="color:#dedede">[]</span> args<span style="color:#dedede">)</span>
        <span style="color:#dedede">{</span>
            <span style="color:#ffa500">//创建框架工厂</span>
            var frameworkFactory <span style="color:#dedede">=</span> <span style="color:#f0e68c">new</span> <span style="color:#f25252">FrameworkFactory</span><span style="color:#dedede">();</span>
            <span style="color:#ffa500">//创建框架内核</span>
            var framework <span style="color:#dedede">=</span> frameworkFactory<span style="color:#dedede">.</span><span style="color:#f25252">CreateFramework</span><span style="color:#dedede">();</span>
            <span style="color:#ffa500">//初始化框架</span>
            framework<span style="color:#dedede">.</span><span style="color:#f25252">Init</span><span style="color:#dedede">();</span>
            <span style="color:#ffa500">//启动框架</span>
            framework<span style="color:#dedede">.</span><span style="color:#f25252">Start</span><span style="color:#dedede">();</span>

            Console<span style="color:#dedede">.</span><span style="color:#f25252">ReadLine</span><span style="color:#dedede">();</span>
        <span style="color:#dedede">}</span>
    <span style="color:#dedede">}</span>
<span style="color:#dedede">}</span>
</pre>
</p>

<p>
创建一个OSGi.NET项目需要：<br/>
1.引用框架内核程序集OSGi.NET.dll<br/>
2.添加框架内核配置文件OSGi.NET.properties<br/>
3.如需要日志支持，添加log4net.config文件及log4net.dll程序集引用<br/>
</p>

OSGi.NET项目的默认文件目录结构如下<br/>
/程序目录<br/>
/程序目录/Bundles/<br/>
/程序目录/Bundles/模块A/<br/>
/程序目录/Bundles/模块A/Manifest.xml<br/>
/程序目录/Bundles/模块A/模块A.dll<br/>
/程序目录/Bundles/模块A/Libs/<br/>
/程序目录/Bundles/模块A/Libs/* .dll<br/>
/程序目录/Bundles/模块B/<br/>
/程序目录/Bundles/模块C/<br/>
/程序目录/Libs/(可选)<br/>
/程序目录/OSGi.NET.properties<br/>
注：<br/>
程序目录中的Libs文件夹存放个Bundles的共享程序集(也可通过在配置文件中配置共享清单)，如接口契约、公共第三方类库等。<br/>
模块A中的Libs文件夹存放其私有程序集。<br/>
Manifest.xml作为程序清单文件对模块进行自描述。<br/>
OSGi.NET.properties为框架内核配置文件<br/>

关于加载次序：<br/>
由于业务需求，各模块存在依赖关系的可能，所以模块加载也会有加载顺序的要求，此时可以通过清单文件中Manifest.xml，Bundle节点的StartLevel属性对其加载次序进行设置。数值越小，加载越前。
<br/><br/>
关于Bundle引用程序集搜索原则：<br/>
1.根据加载的Bundle引用程序集，依据<b>程序集名称+版本号匹配原则</b>，优先从[/程序目录/Libs/]目录或共享清单中搜索。<br/>
2.如第一步无匹配，则根据<b>程序集名称</b>从[/程序目录/Bundles/模块A/Libs/*.dll]目录搜索，并将搜索到的程序集对应版本关联Bundle。<br/>
3.各Bundle下Libs目录程序集在加载中做了Bundle间的隔离，确保不同的Bundle引用的程序集间不会造成影响。即：如存在共享程序集请放置[/程序目录/Libs/]目录或在共享清单配置。<br/>


使用说明CHM: <a href="https://github.com/FreezeSoul/OSGi.NET/blob/master/OSGi.NET%20Client/Help/Help/Documentation.chm?raw=true">CHM Download</a>

代码中还包含了VS的插件创建模板，插件清单文件的生成工具，还有一些示例插件/项目。
