using BasicCaching.Models;

namespace BasicCaching.Services;

public interface ICategoryService
{
	Task<IEnumerable<Category>> GetCategoriesAsync();
	Task<Category?>             GetCategoryAsync(int         id);
	Task<Category>              AddCategoryAsync(Category    category);
	Task<Category?>             UpdateCategoryAsync(Category category);
	Task<bool>                  DeleteCategoryAsync(int      id);

	Task<IEnumerable<Category>?> GetFavoritesCategoriesAsync(int userId);
}