using JealDeployment.Services;

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
public class HelloWorldServiceClient : System.ServiceModel.ClientBase<IHelloWorld>, IHelloWorld
{
    public HelloWorldServiceClient()
    {
    }

    public HelloWorldServiceClient(string endpointConfigurationName) :
        base(endpointConfigurationName)
    {
    }

    public HelloWorldServiceClient(string endpointConfigurationName, string remoteAddress) :
        base(endpointConfigurationName, remoteAddress)
    {
    }

    public HelloWorldServiceClient(string endpointConfigurationName,
        System.ServiceModel.EndpointAddress remoteAddress) :
        base(endpointConfigurationName, remoteAddress)
    {
    }

    public HelloWorldServiceClient(System.ServiceModel.Channels.Binding binding,
        System.ServiceModel.EndpointAddress remoteAddress) :
        base(binding, remoteAddress)
    {
    }

    public string GetHelloWorld()
    {
        return base.Channel.GetHelloWorld();
    }
}