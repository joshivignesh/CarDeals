﻿using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Controllers;

[ApiController]
[Route("api/auctions")]
public class AuctionsController : ControllerBase
{
    private readonly AuctionDbContext _context;

    private readonly IMapper _mapper;

    
    public AuctionsController(AuctionDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<List<AuctionDto>>> GetAllAuctions()
    {
        var auctions = await _context.Auctions
                .Include(x=> x.Item)
                .OrderBy(x=> x.Item.Make)
                .ToListAsync();

        return _mapper.Map<List<AuctionDto>>(auctions);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AuctionDto>> GetAuctionsById(Guid id)
    {
            var auction = await _context.Auctions
                    .Include(x=>x.Item)
                    .FirstOrDefaultAsync(x=> x.Id == id);
                    
            if(auction == null) 
            {
                NotFound();
            }

            return _mapper.Map<AuctionDto>(auction); 
    }

    [HttpPost]
    public async Task<ActionResult<AuctionDto>> CreateAuction(CreateAuctionDto auctionDto)
    {
        var auction = _mapper.Map<Auction>(auctionDto);
        auction.Seller = "test";
 
        _context.Auctions.Add(auction);

        var result = await _context.SaveChangesAsync() > 0;
        if(!result)
        {
            return BadRequest("Could not save changes to the DB");
        }

        return CreatedAtAction(nameof(GetAuctionsById),
        new {auction.Id},_mapper.Map<AuctionDto>(auction));
    }

    [HttpPut("{Id}")]
    public async Task<ActionResult> UpdateAuction(Guid id, UpdateAuctionDto updateAuctionDto)
    {
        var auction = await _context.Auctions.Include(x=> x.Item)
                .FirstOrDefaultAsync(x=> x.Id == id);

        if(auction == null) return NotFound();

        auction.Item.Make = updateAuctionDto.Make ?? auction.Item.Make;
        auction.Item.Model = updateAuctionDto.Model ?? auction.Item.Model;
        auction.Item.Color = updateAuctionDto.Color ?? auction.Item.Color;
        auction.Item.Mileage = updateAuctionDto.Mileage ?? auction.Item.Mileage;
        auction.Item.Year = updateAuctionDto.Year ?? auction.Item.Year;
        auction.Item.Year = updateAuctionDto.Year ?? auction.Item.Year;


        var result = await _context.SaveChangesAsync() > 0;
        if(result) return Ok();
        return BadRequest("Problem saving changes");
    }

    [HttpDelete("{Id}")]
    public async Task<ActionResult> DeleteAuction(Guid id)
    {
        var auction = await _context.Auctions.FindAsync(id);
        if (auction == null) return NotFound();
        
        //TODO : Check Seller = userName

        _context.Auctions.Remove(auction);
        var results = _context.SaveChanges() > 0;

        if(!results) return BadRequest("COuld not update DB");

        return Ok();
    }
}
