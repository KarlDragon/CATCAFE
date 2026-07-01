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

    public async Task RemoveCatAsync( int catId)
    {
        var result = await _catRepository.RemoveCatAsync(catId);
        if ( !result )
        {
            throw new NotFoundException($"Can't remove cat {catId}");
        }
    }

    public async Task<bool> UpdateCatAsync( UpdateCatDTO updateCatDTO)
    {
        if (updateCatDTO.CatName != null) updateCatDTO.CatName = updateCatDTO.CatName?.Trim();
        if (updateCatDTO.Breed != null) updateCatDTO.Breed = updateCatDTO.Breed?.Trim();
        if (updateCatDTO.Status != null) updateCatDTO.Status = updateCatDTO.Status?.Trim();
        if (updateCatDTO.Description != null) updateCatDTO.Description = updateCatDTO.Description?.Trim();
        return await _catRepository.UpdateCatAsync(updateCatDTO);
    }

    public async Task<IEnumerable<Cat>> GetAllCatsAsync(CancellationToken cancellationToken)
    {
        return await _catRepository.GetAllCatsAsync(cancellationToken);
    }
}