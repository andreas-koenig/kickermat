using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VideoSource;

namespace webapp.Controllers
{
    [Route("api/parameters")]
    [ApiController]
    public class ParametersController : ControllerBase
    {
        private ILogger<ParametersController> _logger;
        private IServiceProvider _services;

        public ParametersController(ILogger<ParametersController> logger,
            IServiceProvider services)
        {
            _logger = logger;
            _services = services;
        }

        [HttpGet("{component}")]
        public ActionResult<IEnumerable<KickerParameterAttribute>> GetParameters(string component)
        {
            var kickerComponent = GetComponent(component);
            if (kickerComponent == null)
            {
                return NotFound(String.Format("The component {0} was not found", component));
            }

            List<KickerParameterAttribute> attrs = new List<KickerParameterAttribute>();
            foreach (var prop in kickerComponent.GetType().GetProperties())
            {
                var propAttrs = (KickerParameterAttribute[])prop
                    .GetCustomAttributes(typeof(KickerParameterAttribute), false);
                if (propAttrs?.Length == 1)
                {
                    try
                    {
                        propAttrs[0].Value = prop.GetValue(kickerComponent);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("Failed to get parameter: {0}", ex);
                        propAttrs[0].Value = default;
                    }
                    attrs.Add(propAttrs[0]);
                }
            }

            return Ok(attrs);
        }

        [HttpPut("{component}/{parameter}")]
        public IActionResult SetParameter(string component, string parameter,
            [FromBody] object value)
        {
            var kickerComponent = GetComponent(component);

            foreach (var prop in kickerComponent.GetType().GetProperties())
            {
                var propAttrs = (KickerParameterAttribute[])prop
                    .GetCustomAttributes(typeof(KickerParameterAttribute), false);
                if (propAttrs?.Length == 1 && propAttrs[0].Name.Equals(parameter))
                {
                    try
                    {
                        prop.SetValue(kickerComponent, value);
                        return Ok();
                    }
                    catch (KickerParameterException ex)
                    {
                        return BadRequest(ex.Message);
                    }
                    catch (Exception)
                    {
                        return BadRequest("Could not set the parameter");
                    }
                }
            }

            return NotFound(String.Format("The parameter {0} was not found", parameter));
        }

        private object GetComponent(string componentName)
        {
            switch (componentName)
            {
                case "Camera":
                    return _services.GetService(typeof(IVideoSource));
                default:
                    return null;
            }
        }
    }
}
