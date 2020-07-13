using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using k8s;

namespace CookBook.HealthCheck.Api.Controllers
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

        public object Get()
        {

            var config = KubernetesClientConfiguration.BuildDefaultConfig();
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
    }
}
