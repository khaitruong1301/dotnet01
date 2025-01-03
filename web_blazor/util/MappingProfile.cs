using AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Ánh xạ từ User sang UserDTO
        CreateMap<ProductCarVM, CartItemVM>();
        CreateMap<CartItemVM, ProductCarVM>();

            // .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName));
        // Ánh xạ ngược lại từ UserDTO về User
        // CreateMap<UserDTO, User>()
        //     .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name));
    }
}
