using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Products.Application.Contracts;
using Products.Application.Services;
using Products.Domain.Models;

namespace Products.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductAppService productAppService;

        public ProductsController(ProductAppService productAppService)
        {
            this.productAppService = productAppService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProductContract>> GetAll()
        {
            return StatusCode((int) HttpStatusCode.OK, this.productAppService.All());
        }

        [HttpGet("pages/{page}")]
        public ActionResult<PageResult<ProductContract>> GetAll(int page)
        {
            return StatusCode((int) HttpStatusCode.OK, this.productAppService.GetPage(page));
        }

        [HttpGet("{id}")]
        public ActionResult<ProductContract> Get(int id)
        {
            return StatusCode((int) HttpStatusCode.OK, this.productAppService.GetById(id));
        }

        [HttpPost]
        public ActionResult<ProductContract> Post([FromBody] ProductContract contract)
        {
            return StatusCode((int) HttpStatusCode.Created, this.productAppService.Add(contract));
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] ProductContract contract)
        {
            this.productAppService.Edit(id, contract);
            return StatusCode((int) HttpStatusCode.OK);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            this.productAppService.Delete(id);
            return StatusCode((int) HttpStatusCode.NoContent);
        }
    }
}
