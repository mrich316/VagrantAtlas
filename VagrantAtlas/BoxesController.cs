﻿using System;
using System.Collections.Generic;
using System.Web.Http;

namespace VagrantAtlas
{
    public class BoxesController : ApiController
    {
        private readonly IBoxRepository _boxRepository;

        public BoxesController(IBoxRepository boxRepository)
        {
            if (boxRepository == null) throw new ArgumentNullException("boxRepository");

            _boxRepository = boxRepository;
        }

        [HttpHead, HttpGet]
        public IHttpActionResult Get(string user, string name)
        {
            var id = string.Format("{0}/{1}", user, name);
            var atlasBox = _boxRepository.Get(id);

            return (atlasBox == null)
                ? (IHttpActionResult)NotFound()
                : Ok(atlasBox);
        }

        [HttpPut]
        public IHttpActionResult Put(string user, string name, string version, [FromBody] BoxProvider provider)
        {
            // When BoxReference and BoxProvider are in the PUT signature call, we have the following ASP.NET error:
            // "Can't bind multiple parameters ('boxReference' and 'provider') to the request's content."
            // We create the object and validate it manually to bypass this.
            var boxReference = new BoxReference
            {
                User = user,
                Name = name,
                Version = version
            };

            Validate(boxReference);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var box = new Box
            {
                Name = boxReference.Name,
                User = boxReference.User,
                Versions = new List<BoxVersion>
                {
                    new BoxVersion
                    {
                        Version = boxReference.Version,
                        Providers = new List<BoxProvider>
                        {
                            provider
                        }
                    }
                }
            };

            var newBox = _boxRepository.AddOrUpdate(box);
            return Ok(newBox);
        }
    }
}