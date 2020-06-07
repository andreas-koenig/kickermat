using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Configuration;
using ImageProcessing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using VideoSource;
using Webapp.Services;

namespace Webapp.Controllers
{
    [Route("api/parameters")]
    [ApiController]
    public class ParametersController : ControllerBase
    {
        private readonly ILogger<ParametersController> _logger;
        private readonly ParameterService _parameterService;
        private readonly IServiceProvider _services;

        public ParametersController(
            ILogger<ParametersController> logger, ParameterService paramService,
            IServiceProvider services)
        {
            _logger = logger;
            _parameterService = paramService;
            _services = services;
        }

        [HttpGet("{component}")]
        public ActionResult<IEnumerable<BaseParameterAttribute>> GetParameters(string component)
        {
            var kickerComponent = GetComponent(component);
            if (kickerComponent == null)
            {
                return NotFound(string.Format("The component {0} was not found", component));
            }

            IWriteable options;
            try
            {
                options = _parameterService.KickerOptions[kickerComponent.GetType()];
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get configurable options: {0}", ex);
                return BadRequest();
            }

            var optionsProperties = options.ValueObject.GetType().GetProperties();
            var attrs = new List<BaseParameterAttribute>();
            foreach (var opt in optionsProperties)
            {
                var kickerAttrs = opt.GetCustomAttributes(typeof(BaseParameterAttribute), true);
                foreach (var attr in kickerAttrs as BaseParameterAttribute[])
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
                return NotFound(string.Format("The component {0} was not found", component));
            }

            IWriteable options;
            try
            {
                options = _parameterService.KickerOptions[kickerComponent.GetType()];
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get configurable options: {0}", ex);
                return BadRequest();
            }

            var optionsProperties = options.ValueObject.GetType().GetProperties();

            foreach (var prop in optionsProperties)
            {
                var kickerAttrs = prop.GetCustomAttributes(typeof(BaseParameterAttribute), true);
                foreach (var attr in kickerAttrs)
                {
                    if (!((BaseParameterAttribute)attr).Name.Equals(parameter))
                    {
                        continue;
                    }

                    try
                    {
                        var newVal = JsonConvert.DeserializeObject(value.ToString(), prop.PropertyType);
                        options.Update(changes =>
                        {
                            // TODO: Check for changes
                            prop.SetValue(changes, newVal);
                        });
                    }
                    catch (Exception ex)
                    {
                        var msg = string.Format(
                            "Failed to set parameter {0} to {1}", parameter, value);
                        _logger.LogError(msg + ": {0}", ex);
                        return BadRequest(msg);
                    }

                    return Ok();
                }
            }

            return NotFound(string.Format("The parameter {0} was not found", parameter));
        }

        [HttpPut("{component}/parameters/save")]
        public IActionResult SaveParameters(string component)
        {
            var kickerComponent = GetComponent(component);
            if (kickerComponent == null)
            {
                return NotFound(string.Format("The component {0} was not found", component));
            }

            // TODO: Implement save parameters functionality
            return Ok();
        }

        private object GetComponent(string componentName)
        {
            switch (componentName)
            {
                case "Camera":
                    return _services.GetService(typeof(IVideoSource));
                case "ImageProcessor":
                    return _services.GetService(typeof(IImageProcessor));
                default:
                    return null;
            }
        }
    }
}
