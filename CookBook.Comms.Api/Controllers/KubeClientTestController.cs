using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using k8s;

namespace CookBook.Comms.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KubeClientTestController : ControllerBase
    {

        ILogger<KubeClientTestController> _logger;

        public KubeClientTestController(ILogger<KubeClientTestController> logger)
        {
            _logger = logger;
        }


        [HttpGet("pods")]
        public object GetPods()
        {
            KubernetesClientConfiguration config = null;
            try
            {
                config = KubernetesClientConfiguration.BuildDefaultConfig();
            }
            catch
            {
                config = KubernetesClientConfiguration.InClusterConfig();
            }
            IKubernetes client = new Kubernetes(config);
            Console.WriteLine("Starting Request!");

            var list = client.ListNamespacedPod("cookbook-dev");
            foreach (var item in list.Items)
            {
                Console.WriteLine(item.Metadata.Name);
            }
            if (list.Items.Count == 0)
            {
                Console.WriteLine("Empty!");
            }
            return list.Items;



            //return View();
        }


        [HttpGet("services")]
        public object GetServices()
        {
            KubernetesClientConfiguration config = null;
            try
            {
                config = KubernetesClientConfiguration.BuildDefaultConfig();
            }
            catch
            {
                config = KubernetesClientConfiguration.InClusterConfig();
            }
            IKubernetes client = new Kubernetes(config);
            Console.WriteLine("Starting Request!");

            var list = client.ListNamespacedService("cookbook-dev");
            return list.Items;
        }


        [HttpGet("namespaces")]
        public object GetNamespaces()
        {
            KubernetesClientConfiguration config = null;
            try
            {
                config = KubernetesClientConfiguration.BuildDefaultConfig();
            }
            catch
            {
                config = KubernetesClientConfiguration.InClusterConfig();
            }
            IKubernetes client = new Kubernetes(config);
            Console.WriteLine("Starting Request!");


            var namespaces = client.ListNamespace();
            return namespaces.Items;
        }

    }

}
