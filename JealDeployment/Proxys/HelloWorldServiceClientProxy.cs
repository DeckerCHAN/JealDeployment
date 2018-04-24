using System.ServiceModel;
using System.ServiceModel.Channels;
using JealDeployment.Services;

namespace JealDeployment.Proxys
{
public class HelloWorldServiceClientProxy : ClientBase<IHelloWorld>, IHelloWorld
{

    public HelloWorldServiceClientProxy(Binding binding,
        EndpointAddress remoteAddress) :
        base(binding, remoteAddress)
    {
    }

    public string GetHelloWorld()
    {
        return base.Channel.GetHelloWorld();
    }
}

}