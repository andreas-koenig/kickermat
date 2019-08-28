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

            Type settingsType;
            try
            {
                settingsType = GetSettingsType(kickerComponent);
            }
            catch (Exception)
            {
                return Ok(); // component does not implement IConfigurable<SettingsType>
            }

            // Get configurable from all parameters
            List<KickerParameterAttribute> attrs = new List<KickerParameterAttribute>();
            foreach (var prop in settingsType.GetProperties())
            {
                var propAttrs = (KickerParameterAttribute[])prop
                    .GetCustomAttributes(typeof(KickerParameterAttribute), false);
                if (propAttrs?.Length == 1)
                {
                    try
                    {
                        // TODO: Get Value for the property
                        //propAttrs[0].Value = default;
                        //propAttrs[0].Value = prop.GetValue(kickerComponent);
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
            if (kickerComponent == null)
            {
                return NotFound(String.Format("The component {0} was not found", component));
            }

            var options = (IWritableOptions)kickerComponent
                .GetType()
                .GetProperty("Options")
                .GetValue(kickerComponent);

            var optionsProperties = options.ValueObject.GetType().GetProperties();

            foreach (var prop in optionsProperties)
            {
                var kickerAttrs = prop.GetCustomAttributes(typeof(KickerParameterAttribute), true);
                foreach (var attr in kickerAttrs)
                {
                    if (((KickerParameterAttribute)attr).Name.Equals(parameter))
                    {
                        prop.SetValue(options.ValueObject, Convert.ChangeType(value, prop.PropertyType)); 
                        options.Update(changes =>
                        {
                            changes = options;
                        });

                        return Ok();
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

        private Type GetSettingsType(object component)
        {
            foreach (var i in component.GetType().GetInterfaces())
            {
                if (i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IConfigurable<>))
                {
                    return i.GetGenericArguments()[0];
                }
            }

            throw new Exception();
        }
    }
}
