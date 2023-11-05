using AuctionService.DTOs;
using AutoMapper;

namespace AuctionService.RequestHelpers;

public class MappingProfiles : Profile
{
    protected MappingProfiles()
    {
        CreateMap<Auction, AuctionDto>().IncludeMembers(x=>x.Item);
        CreateMap<Item, AuctionDto>();
        CreateMap<CreateAuctionDto, Auction>();
        CreateMap<UpdateAuctionDto, Auction>().ForMember(d=>d.Item, o=>o.MapFrom(s=>s));
        CreateMap<CreateAuctionDto, Item>();
    }

    protected internal MappingProfiles(string profileName) : base(profileName)
    {
    }
}
