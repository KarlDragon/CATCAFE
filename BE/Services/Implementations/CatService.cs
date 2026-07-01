namespace BE.Services.Implementations;
using BE.Services.Interfaces;
using BE.Models;
using BE.DTOs;
using BE.Repositories.Interfaces;
using BE.Exceptions;

public class CatService : ICatService
{
    private readonly ICatRepository _catRepository;

    public CatService( ICatRepository catRepository )
    {
        _catRepository = catRepository;
    }

    public async Task<bool> CreateCatAsync( CreateCatDTO createCatDto )
    {
        var newCat = new Cat
        {
            CatName = createCatDto.CatName.Trim(),
            Breed = createCatDto.Breed.Trim(),
            Status = createCatDto.Status.Trim(),
            Description = createCatDto.Description.Trim()
        };
        var duplicateName = await _catRepository.IsDuplicateName(newCat.CatName);
        if (duplicateName)
        {
            throw new DuplicateNameException("Trùng tên mèo");
        }

        return await _catRepository.CreateCatAsync(newCat);
    }

    public 
}