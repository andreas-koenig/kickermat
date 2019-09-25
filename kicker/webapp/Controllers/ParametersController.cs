﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Configuration;
using ImageProcessing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VideoSource;

namespace webapp.Controllers
{
    [Route("api/parameters")]
    [ApiController]
    public class ParametersController : ControllerBase
    {
        private readonly ILogger<ParametersController> _logger;
        private readonly IServiceProvider _services;

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

            IWritableOptions options;
            try
            {
                options = GetWritableOptions(kickerComponent);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get configurable options: {0}", ex);
                return BadRequest();
            }

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

            IWritableOptions options;
            try
            {
                options = GetWritableOptions(kickerComponent);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get configurable options: {0}", ex);
                return BadRequest();
            }

            var optionsProperties = options.ValueObject.GetType().GetProperties();

            foreach (var prop in optionsProperties)
            {
                var kickerAttrs = prop.GetCustomAttributes(typeof(KickerParameterAttribute), true);
                foreach (var attr in kickerAttrs)
                {
                    if (!((KickerParameterAttribute)attr).Name.Equals(parameter))
                    {
                        continue;
                    }

                    try
                    {
                        options.Update(changes =>
                        {
                            prop.SetValue(changes, Convert.ChangeType(value, prop.PropertyType));
                        });
                    }
                    catch (Exception ex)
                    {
                        var msg = string.Format("Failed to set parameter {0} to {1}",
                            parameter, value);
                        _logger.LogError(msg + ": {0}", ex);
                        return BadRequest(msg);
                    }

                    return Ok();
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

        private IWritableOptions GetWritableOptions(object component)
        {
            var attrs = component.GetType()
                .GetCustomAttributes(typeof(ConfigurableOptionsAttribute), false);

            if (attrs?.Length == 0)
            {
                var msg = string.Format("Component {0} has no configurable options",
                    component.GetType().FullName);
                throw new Exception(msg);
            }

            var optionsType = ((ConfigurableOptionsAttribute)attrs[0]).OptionsType;
            var writableOptionsType = typeof(IWritableOptions<>)
                .MakeGenericType(new Type[] { optionsType });

            return (IWritableOptions)_services.GetService(writableOptionsType);
        }
    }
}