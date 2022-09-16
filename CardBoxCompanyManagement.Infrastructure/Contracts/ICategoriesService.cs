using CardBox.ApiClient.Models;

namespace CardBox.ApiClient.Services;

public interface ICategoriesService
{
    public List<Category> GetCategories();
}