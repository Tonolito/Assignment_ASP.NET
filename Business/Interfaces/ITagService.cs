
namespace Business.Interfaces
{
    public interface ITagService
    {
        Task<List<object>> SearchTagsAsync(string term);
    }
}