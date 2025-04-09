
using Business.Interfaces;
using Data.Interfaces;

namespace Business.Services
{
    public class TagService(ITagRepository tagRepository) : ITagService
    {
        private readonly ITagRepository _tagRepository = tagRepository;

        public async Task<List<object>> SearchTagsAsync(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                return new List<object>();
            }
            var result = await _tagRepository.GetAllAsync(where: (x => x.TagName.Contains(term)));
            if (!result.Succeeded)
            {
                return new List<object>();
            }

            

            return result.Result!.Select(tag => new { tag.Id, tag.TagName }).ToList<object>();
        }
    }
}
