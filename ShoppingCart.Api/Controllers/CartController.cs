using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Core.Entities;
using ShoppingCart.Core.Models;
using ShoppingCart.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService cartService;

        public CartController(ICartService cartService)
        {
            this.cartService = cartService;
        }

        /// <summary>
        /// Initializes default carts, items and stock values.
        /// </summary>
        /// <returns>Summary of additions</returns>
        [HttpPost("initialize-default-carts")]
        public async Task<ActionResult<InitializeDefaultCartsResponse>> InitializeDefault()
        {
            var carts = await cartService.InitializeCarts();
            return Ok(carts);
        }

        /// <summary>
        /// Adds an item to given cart by amount.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Updated cart</returns>
        [HttpPost("add-item")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Cart>> AddItemToCart(AddItemToCartRequest request)
        {
            var validationError = CheckRequestValidationErrors(request);

            if (validationError != null)
            {
                return BadRequest(validationError);
            }

            var (cart, error) = await cartService.AddItemToCart(request);

            if (error != null)
            {
                return BadRequest(error);
            }

            return Ok(cart);
        }

        private string CheckRequestValidationErrors(AddItemToCartRequest request)
        {
            if (request.CartId == null || request.CartId.Length != 24)
            {
                return "Cart id not valid";
            }

            if (request.ItemId == null || request.ItemId.Length != 24)
            {
                return "Item id not valid";
            }

            if (request.Amount < 1)
            {
                return "Amount should greater than zero";
            }

            return null;
        }
    }
}
