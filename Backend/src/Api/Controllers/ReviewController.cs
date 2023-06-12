﻿using Ecommerce.Application.Features.Reviews.Commands.CreateReview;
using Ecommerce.Application.Features.Reviews.Commands.DeleteReview;
using Ecommerce.Application.Features.Reviews.Queries.PaginationReviews;
using Ecommerce.Application.Features.Reviews.Queries.ViewModels;
using Ecommerce.Application.Features.Shared.Queries;
using Ecommerce.Application.Models.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ecommerce.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ReviewController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReviewController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost(Name = "CreateReview")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult<ReviewViewModel>> CreateReview([FromBody] CreateReviewCommand request)
    {
        return await _mediator.Send(request);
    }

    [Authorize(Roles = Role.ADMIN)]
    [HttpDelete("{id}", Name = "DeleteReview")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult<Unit>> DeleteReview(int id)
    {
        var request = new DeleteReviewCommand(id);

        return await _mediator.Send(request);
    }

    [Authorize(Roles = Role.ADMIN)]
    [HttpGet("paginationReviews", Name = "PaginationReview")]
    [ProducesResponseType(typeof(PaginationViewModel<ReviewViewModel>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Unit>> PaginationReview([FromQuery] PaginationReviewsQuery request)
    {
        var paginationReview = await _mediator.Send(request);

        return Ok(paginationReview);
    }
}