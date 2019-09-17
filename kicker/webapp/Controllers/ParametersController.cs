using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Configuration;
using ImageProcessing.BarSearch;
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

            if (!IsIConfigurable(kickerComponent))
            {
                return Ok(); // component does not implement IConfigurable<SettingsType>
            }

            var options = GetWritableOptions(kickerComponent);
            var optionsProperties = options.ValueObject.GetType().GetProperties();
            var attrs = new List<KickerParameterAttribute>();
            foreach (var opt in optionsProperties)
            {
                var kickerAttrs = opt.GetCustomAttributes(typeof(KickerParameterAttribute), true);
                foreach (var attr in kickerAttrs as KickerParameterAttribute[])
                {
                    attr.Value = opt.GetValue(options.ValueObject);
                    attrs.Add(attr);
                }
            }

            return Ok(attrs);
        }

        [HttpPut("{component}/{parameter}")]
        public IActionResult SetParameter(string component, string parameter,
            [FromBody] object value)
        {
            var kickerComponent = GetComponent(component);
            if (kickerComponent == null)
            {
                return NotFound(String.Format("The component {0} was not found", component));
            }

            var options = GetWritableOptions(kickerComponent);
            var optionsProperties = options.ValueObject.GetType().GetProperties();

            foreach (var prop in optionsProperties)
            {
                var kickerAttrs = prop.GetCustomAttributes(typeof(KickerParameterAttribute), true);
                foreach (var attr in kickerAttrs)
                {
                    if (((KickerParameterAttribute)attr).Name.Equals(parameter))
                    {
                        options.Update(changes =>
                        {
                            prop.SetValue(changes, Convert.ChangeType(value, prop.PropertyType));
                        });

                        try
                        {
                            ((IConfigurable)kickerComponent).ApplyOptions();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError("Failed to apply options: {0}", ex);
                            var msg = string.Format("Failed to set parameter {0} to {1}",
                                parameter, value);
                            return BadRequest(msg);
                        }

                        return Ok();
                    }
                }
            }

            return NotFound(String.Format("The parameter {0} was not found", parameter));
        }

        [HttpPut("{component}/parameters/save")]
        public IActionResult SaveParameters(string component)
        {
            var kickerComponent = GetComponent(component);
            if (kickerComponent == null)
            {
                return NotFound(String.Format("The component {0} was not found", component));
            }

            if (!IsIConfigurable(kickerComponent))
            {
                var msg = String.Format("The component {0} has not parameters to save", component);
                return BadRequest(msg);
            }

            return Ok();
        }

        private object GetComponent(string componentName)
        {
            switch (componentName)
            {
                case "Camera":
                    return _services.GetService(typeof(IVideoSource));
                case "BarSearch":
                    return _services.GetService(typeof(IBarSearch));
                default:
                    return null;
            }
        }

        private IWritableOptions GetWritableOptions(object component)
        {
            return (IWritableOptions)component
                .GetType()
                .GetProperty("Options")
                .GetValue(component);
        }

        private bool IsIConfigurable(object component)
        {
            foreach (var i in component.GetType().GetInterfaces())
            {
                if (i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IConfigurable<>))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
