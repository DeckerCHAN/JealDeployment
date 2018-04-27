using System.ServiceModel;
using System.ServiceModel.Channels;
using JealDeployment.Entites;
using JealDeployment.Services;

namespace JealDeployment.Proxys
{
public class DeployServiceClientProxy : ClientBase<IDeploy>, IDeploy
{

    public DeployServiceClientProxy(Binding binding,
        EndpointAddress remoteAddress) :
        base(binding, remoteAddress)
    {
    }

    public string Calibrate()
    {
        return base.Channel.Calibrate();

        }

    public Consiquence Deploy(Deployment deployment)
    {
        return base.Channel.Deploy(deployment);
    }

    public Consiquence DryDeploy(Deployment deployment)
    {
        return base.Channel.DryDeploy(deployment);
    }
}

}