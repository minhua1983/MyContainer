# 自定IOC义容器
这是一个简单的自定义容器demo，一般平时使用autofac来做ioc，慢慢有了容器的概念，想自己也写一个试试，所以有了这个demo。

## 实现了3种简单生命周期级别的类型注册
* IContainerBuilder.Register<T>().AsInstancePerDependency<T>()，可以按每次都单独以T类型实例化
* IContainerBuilder.Register<T>().AsSingleInstance<T>()，可以按整个Container实例中T类型只有一个实例，如果用于web项目中，请保证Container实例的唯一性。
* IContainerBuilder.Register<T>().AsPerLifetimeScope<T>()，每个ILifetimeScope代码段中T类型只有一个实例

## 调用方式以LifetimeScope级别注册为例子
<pre>
//生成容器创建器
ContainerBuilder containerBuilder = new ContainerBuilder();
//注册所需类型
containerBuilder.Register&lt;Audit, IAudit&gt;().AsPerLifetimeScope&lt;IAudit&gt;();
//创建容器
IContainer container = containerBuilder.Build&lt;Container&gt;();
//调用
using(ILifetimeScope scope = container.BeginLifetimeScope())
{
    IAudit audit = scope.Resove&lt;IAudit&gt;();
    //用audit实例处理
}
</pre>
